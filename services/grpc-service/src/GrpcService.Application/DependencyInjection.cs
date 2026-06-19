using GrpcService.Application.UseCases.CreateUser;
using GrpcService.Application.UseCases.DeleteUser;
using GrpcService.Application.UseCases.GetAllUsers;
using GrpcService.Application.UseCases.GetUserById;
using GrpcService.Application.UseCases.UpdateUser;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<DeleteUserUseCase>();
        services.AddScoped<GetAllUsersUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        services.AddScoped<UpdateUserUseCase>();

        return services;
    }
}