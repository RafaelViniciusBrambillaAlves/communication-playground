using GrpcService.Application.Dtos;
using GrpcService.Application.Interfaces;
using GrpcService.Application.Mappers;
using GrpcService.Domain.Exceptions;

namespace GrpcService.Application.UseCases.UpdateUser;

public sealed class UpdateUserUseCase(IUserRepository repository)
{
    public async Task<UserDto> ExecuteAsync(
        UpdateUserDto dto, 
        CancellationToken cancellationToken = default
    )
    {
        var user = await repository.UpdateAsync(
            dto.Id, dto.Name, dto.Email, dto.Age, cancellationToken);

        if (user is null)
            throw new UserNotFoundException(dto.Id);

        return user.ToDto();
    }
}