using RabbitMQService.Application.DTOs;
using RabbitMQService.Application.Interfaces;
using RabbitMQService.Application.Mappers;
using RabbitMQService.Domain.Entities;

namespace RabbitMQService.Application.UseCases.GetAllUsers;

public sealed class GetAllUsersUseCase(IUserRepository repository)
{
    public async Task<IReadOnlyList<UserDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var users = await repository.GetAllAsync(cancellationToken);

        return users.Select(u => u.ToDto()).ToList();
    }
}