using GrpcService.Application.Interfaces;
using GrpcService.Domain.Exceptions;

namespace GrpcService.Application.UseCases.DeleteUser;

public sealed class DeleteUserUseCase(IUserRepository repository)
{
    public async Task ExecuteAsync(
        Guid id, 
        CancellationToken cancellationToken = default
    )
    {
        var deleted = await repository.DeleteAsync(id, cancellationToken);

        if (!deleted)
            throw new UserNotFoundException(id);
    }
}