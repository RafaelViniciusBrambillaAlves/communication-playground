using GrpcService.Application.Dtos;
using GrpcService.Application.Interfaces;
using GrpcService.Application.Mappers;

namespace GrpcService.Application.UseCases.GetAllUsers;

public sealed class GetAllUsersUseCase(IUserRepository repository)
{
    public async Task<IReadOnlyList<UserDto>> ExecuteAsync(
        CancellationToken cancellationToken = default
    )
    {
        var users = await repository.GetAllAsync(cancellationToken);

        return users.Select(u => u.ToDto()).ToList();
    }
}