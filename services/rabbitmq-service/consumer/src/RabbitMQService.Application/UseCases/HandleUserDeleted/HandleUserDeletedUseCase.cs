using RabbitMQService.Application.DTOs;
using RabbitMQService.Application.Interfaces;
using RabbitMQService.Domain.Exceptions;

namespace RabbitMQService.Application.UseCases.HandleUserDeleted;

public sealed class HandleUserDeletedUseCase(IUserRepository repository)
{
    public async Task ExecuteAsync(
        UserDeletedPayload payload,
        CancellationToken cancellationToken = default)
    {
        var deleted = await repository.DeleteAsync(payload.UserId, cancellationToken);

        if (!deleted)
            throw new UserNotFoundException(payload.UserId);
    }
}