using RabbitMQService.Application.DTOs;
using RabbitMQService.Application.Interfaces;
using RabbitMQService.Application.Mappers;
using RabbitMQService.Domain.Exceptions;

namespace RabbitMQService.Application.UseCases.GetUserById;

public sealed class GetUserByIdUseCase(IUserRepository repository)
{
    public async Task<UserDto> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await repository.GetByIdAsync(id, cancellationToken);


        if (user is null)
            throw new UserNotFoundException(id);
        
        return user.ToDto();
    }
}