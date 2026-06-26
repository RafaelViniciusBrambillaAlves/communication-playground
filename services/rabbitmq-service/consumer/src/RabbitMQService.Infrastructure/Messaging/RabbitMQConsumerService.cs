using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using RabbitMQService.Application.DTOs;
using RabbitMQService.Application.UseCases.HandleUserCreated;
using RabbitMQService.Application.UseCases.HandleUserUpdated;
using RabbitMQService.Application.UseCases.HandleUserDeleted;
using RabbitMQService.Infrastructure.Settings;
using RabbitMQService.Domain.Exceptions;
using Azure.Core;


namespace RabbitMQService.Infrastructure.Messaging;

public sealed class RabbitMQConsumerService(
    IServiceScopeFactory scopeFactory,
    IOptions<RabbitMQSettings> options,
    ILogger<RabbitMQConsumerService> logger
) : BackgroundService
{
    private readonly RabbitMQSettings _settings = options.Value;


    // desserializar mensagens: snake_case do Python -> PascalCase do C#
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // consumer ativo durante todo o ciclo de vida da aplicação
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConsumeAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RabbitMQ connection faile. Retrying in 5s...");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }


    private async Task ConsumeAsync(CancellationToken cancellationToken)
    {
         // Cria uma nova conexão e canal com o broker.
        var factory = new ConnectionFactory { Uri = new Uri(_settings.Url) };

        await using var connection = await factory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await DeclareTopologyAsync(channel, cancellationToken);

        // Limita a quantidade de mensagens não confirmadas (ACK)
        // para evitar sobrecarga do consumidor
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (_, ea) => OnMessageReceivedAsync(channel, ea);

        // Mesmo consumer escutando múltiplas filas
        await channel.BasicConsumeAsync(_settings.CreatedQueue , autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
        await channel.BasicConsumeAsync(_settings.UpdatedQueue , autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
        await channel.BasicConsumeAsync(_settings.DeletedQueue , autoAck: false, consumer: consumer, cancellationToken: cancellationToken);

        logger.LogInformation("RabbitMQ consumer started. Listening on [{Created}, {Updated}, {Deleted}].",
            _settings.CreatedQueue, _settings.UpdatedQueue, _settings.DeletedQueue);
        
        // Mantém o canal aberto até que a aplicação seja encerrada
        await Task.Delay(Timeout.Infinite, cancellationToken);
    }


    private async Task DeclareTopologyAsync(IChannel channel, CancellationToken cancellationToken)
    {
        var dlxName = $"{_settings.Exchange}.dlx";

        // Exchange principal responsável pelo roteamento dos eventos
        await channel.ExchangeDeclareAsync(_settings.Exchange, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);

        // Dead Letter Exchange utilizada para armazenar mensagens
        // que falharam no processamento
        await channel.ExchangeDeclareAsync(dlxName, ExchangeType.Direct, durable: true, cancellationToken: cancellationToken);

        // Filas principais + DLQs
        var queues = new[]
        {
            (_settings.CreatedQueue, "user.created"),
            (_settings.UpdatedQueue, "user.updated"),
            (_settings.DeletedQueue, "user.deleted")
        };

        foreach (var (queue, routingKey) in queues)
        {
            var dlqName = $"{queue}.dlq";

            // Cada fila possui sua própria DLQ para facilitar
            // monitoramento e reprocessamento de mensagens
            await channel.QueueDeclareAsync(dlqName, durable: true, exclusive: false, cancellationToken: cancellationToken);
            await channel.QueueBindAsync(dlqName, dlxName, routingKey, cancellationToken: cancellationToken);

            var args = new Dictionary<string, object?>
            {
                ["x-dead-letter-exchange"] = dlxName,
                ["x-dead-letter-routing-key"] = routingKey
            };
            
            await channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false, arguments: args, cancellationToken: cancellationToken);
            await channel.QueueBindAsync(queue, _settings.Exchange, routingKey, cancellationToken: cancellationToken);
        }
    }

    private async Task OnMessageReceivedAsync(IChannel channel, BasicDeliverEventArgs ea)
    {   
        var ct = CancellationToken.None;
        var body = Encoding.UTF8.GetString(ea.Body.ToArray());

        logger.LogInformation("Message received - routing key: {RoutingKey}", ea.RoutingKey);

        try
        {
            await DispatchAsync(ea.RoutingKey, body, ct);
            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, ct);
        }
        catch (UserAlreadyExistsException ex)
        {
            logger.LogWarning(
                "Duplicate e-mail — routing to DLQ. Routing key: {Key} | {Message}",
                ea.RoutingKey, ex.Message);

            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, ct);
        }
        catch (UserNotFoundException ex)
        {
            logger.LogWarning(
                "User not found — routing to DLQ. Routing key: {Key} | {Message}",
                ea.RoutingKey, ex.Message);

            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, ct);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(
                "Validation error — routing to DLQ. Routing key: {Key} | {Message}",
                ea.RoutingKey, ex.Message);

            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, ct);
        }
        catch (FormatException ex)
        {
            logger.LogWarning(
                "Invalid ID format — routing to DLQ. Routing key: {Key} | {Message}",
                ea.RoutingKey, ex.Message);

            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, ct);
        }
        catch (JsonException ex)
        {
            logger.LogWarning(
                "Malformed JSON — routing to DLQ. Routing key: {Key} | {Message}",
                ea.RoutingKey, ex.Message);

            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, 
                "Unexpected error — routing to DLQ. Routing key: {Key}",
                ea.RoutingKey);

            // requeue: false → mensagem vai para a DLQ configurada
            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, ea.CancellationToken );
        }
    }

    private async Task DispatchAsync(string routingKey, string body, CancellationToken cancellationToken)
    {
        var envelope = JsonSerializer.Deserialize<UserEventMessage>(body, _jsonOptions)
            ?? throw new InvalidOperationException("Failed to deserialize event envelope.");

        using var scope = scopeFactory.CreateScope();

        switch (routingKey)
        {
            case "user.created":
            {
                var payload = envelope.Payload.Deserialize<UserCreatedPayload>(_jsonOptions)
                    ?? throw new JsonException("Failed to deserialize UserCreatedPayload.");

                var useCase = scope.ServiceProvider.GetRequiredService<HandleUserCreatedUseCase>();
                
                await useCase.ExecuteAsync(payload, cancellationToken);
                
                logger.LogInformation(
                    "User created - id: {UserId} | email: {Email}", 
                    payload.UserId, payload.Email);
                break;
            }
            case "user.updated":
            {
                var payload = envelope.Payload.Deserialize<UserUpdatedPayload>(_jsonOptions)
                    ?? throw new JsonException("Failed to deserialize UserUpdatedPayload.");

                var useCase = scope.ServiceProvider.GetRequiredService<HandleUserUpdatedUseCase>();

                await useCase.ExecuteAsync(payload, cancellationToken);

                logger.LogInformation("User Updated - id {UserId}", payload.UserId);
                break;
            }
            case "user.deleted":
            {
                var payload = envelope.Payload.Deserialize<UserDeletedPayload>(_jsonOptions)
                    ?? throw new JsonException("Failed to deserialize UserDeletedPayload.");

                var useCase = scope.ServiceProvider.GetRequiredService<HandleUserDeletedUseCase>();

                await useCase.ExecuteAsync(payload, cancellationToken);

                logger.LogInformation("User Deleted - id {UserId}", payload.UserId);
                break;
            }
            default:
                logger.LogInformation("Unknowm routing key: {RoutingKey}.Discarding message.", routingKey);
                break;
        }
    }
}