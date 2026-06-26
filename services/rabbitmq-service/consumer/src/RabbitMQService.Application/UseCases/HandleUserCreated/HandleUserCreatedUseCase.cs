using RabbitMQService.Application.DTOs;
using RabbitMQService.Application.Interfaces;
using RabbitMQService.Domain.Entities;
using RabbitMQService.Domain.Exceptions;

namespace RabbitMQService.Application.UseCases.HandleUserCreated;

public sealed class HandleUserCreatedUseCase(IUserRepository repository)
{
    public async Task ExecuteAsync(
        UserCreatedPayload payload,
        CancellationToken cancellationToken = default)
    {
        if (await repository.ExistsWithEmailAsync(payload.Email, cancellationToken))
            throw new UserAlreadyExistsException(payload.Email);

        var user = User.Create(payload.UserId, payload.Name, payload.Email, payload.Age);
        
        await repository.CreateAsync(user, cancellationToken);
    }
}
