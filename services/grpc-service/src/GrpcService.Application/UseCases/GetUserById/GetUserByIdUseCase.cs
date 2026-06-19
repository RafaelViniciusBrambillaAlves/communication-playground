using GrpcService.Application.Dtos;
using GrpcService.Application.Interfaces;
using GrpcService.Application.Mappers;
using GrpcService.Domain.Exceptions;

namespace GrpcService.Application.UseCases.GetUserById;

public sealed class GetUserByIdUseCase(IUserRepository repository)
{
    public async Task<UserDto> ExecuteAsync(
        Guid id, 
        CancellationToken cancellationToken = default
    )
    {
        var user = await repository.GetByIdAsync(id, cancellationToken);

        if (user is null)
            throw new UserNotFoundException(id);

        return user.ToDto();
    }
}