using RabbitMQService.Application.DTOs;
using RabbitMQService.Application.Interfaces;
using RabbitMQService.Domain.Exceptions;

namespace RabbitMQService.Application.UseCases.HandleUserUpdated;

public sealed class HandleUserUpdatedUseCase(IUserRepository repository)
{
    public async Task ExecuteAsync(
        UserUpdatedPayload payload,
        CancellationToken cancellationToken = default)
    {
        var user = await repository.UpdateAsync(
            payload.UserId, payload.Name, payload.Email, payload.Age, cancellationToken);

        if (user is null)
            throw new UserNotFoundException(payload.UserId);
    }
}