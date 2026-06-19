

using GrpcService.Application.Dtos;
using GrpcService.Application.Interfaces;
using GrpcService.Application.Mappers;
using GrpcService.Domain.Entities;

namespace GrpcService.Application.UseCases.CreateUser;

public sealed class CreateUserUseCase(IUserRepository repository)
{
    public async Task<UserDto> ExecuteAsync(
        CreateUseDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var user = User.Create(dto.Name, dto.Email, dto.Age);

        var created = await repository.CreateAsync(user, cancellationToken);

        return created.ToDto();
    }
}