
using Grpc.Core;
using GrpcService.api;
using GrpcService.Application.Dtos;
using GrpcService.Application.UseCases.CreateUser;
using GrpcService.Application.UseCases.DeleteUser;
using GrpcService.Application.UseCases.GetAllUsers;
using GrpcService.Application.UseCases.GetUserById;
using GrpcService.Application.UseCases.UpdateUser;

namespace GrpcService.Api.Services;

public sealed class UserGrpcService(
    CreateUserUseCase createUser,
    GetUserByIdUseCase getUserById,
    GetAllUsersUseCase getAllUsers,
    UpdateUserUseCase updateUser,
    DeleteUserUseCase deleteUser
) : UserService.UserServiceBase
{
    public override async Task<UserResponse> CreateUser(
        CreateUserRequest request,
        ServerCallContext context)
    {
        var dto = new CreateUseDto(request.Name, request.Email, request.Age);

        var user = await createUser.ExecuteAsync(dto, context.CancellationToken);

        return MapToResponse(user);
    }

    public override async Task<UserResponse> GetUser(
        GetUserRequest request, 
        ServerCallContext context)
    {
        var id = Guid.Parse(request.Id);

        var user = await getUserById.ExecuteAsync(id, context.CancellationToken);

        return MapToResponse(user);
    }

    public override async Task<GetAllUsersResponse> GetAllUsers(
        GetAllUsersRequest request, 
        ServerCallContext context)
    {
        var users = await getAllUsers.ExecuteAsync(context.CancellationToken);

        var response = new GetAllUsersResponse();

        response.Users.AddRange(users.Select(MapToResponse));

        return response;
    }

    public override async Task<UserResponse> UpdateUser(
        UpdateUserRequest request,
        ServerCallContext context)
    {
        var dto = new UpdateUserDto(
            Id: Guid.Parse(request.Id),
            Name: request.HasName ? request.Name : null,
            Email: request.HasEmail ? request.Email : null,
            Age: request.HasAge ? request.Age : null
        );

        var user = await updateUser.ExecuteAsync(dto, context.CancellationToken);

        return MapToResponse(user);
    }

    public override async Task<DeleteUserResponse> DeleteUser(
        DeleteUserRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.Id);

        await deleteUser.ExecuteAsync(id, context.CancellationToken);

        return new DeleteUserResponse { Success = true };
    }


    private static UserResponse MapToResponse(UserDto dto)
    {
        var response = new UserResponse
        {
            Id = dto.Id.ToString(),
            Name = dto.Name,
            Email = dto.Email, 
            Age = dto.Age,
            CreatedAt = dto.CreatedAt.ToString("O")
        };

        if (dto.UpdatedAt.HasValue)
            response.UpdatedAt = dto.UpdatedAt.Value.ToString("O");
        
        return response;    
    }
}
