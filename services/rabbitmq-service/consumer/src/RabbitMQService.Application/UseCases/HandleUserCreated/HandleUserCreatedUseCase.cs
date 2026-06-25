using RabbitMQService.Application.DTOs;
using RabbitMQService.Application.Interfaces;
using RabbitMQService.Domain.Entities;

namespace RabbitMQService.Application.UseCases.HandleUserCreated;

public sealed class HandleUserCreatedUseCase(IUserRepository repository)
{
    public async Task ExecuteAsync(
        UserCreatedPayload payload,
        CancellationToken cancellationToken = default)
    {
        var user = User.Create(payload.UserId, payload.Name, payload.Email, payload.Age);
        
        await repository.CreateAsync(user, cancellationToken);
    }
}
