using Microsoft.Extensions.DependencyInjection;

using RabbitMQService.Application.UseCases.HandleUserCreated;
using RabbitMQService.Application.UseCases.HandleUserUpdated;
using RabbitMQService.Application.UseCases.HandleUserDeleted;
using RabbitMQService.Application.UseCases.GetAllUsers;
using RabbitMQService.Application.UseCases.GetUserById;

namespace RabbitMQService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {   
        // Event handlers
        services.AddScoped<HandleUserCreatedUseCase>();
        services.AddScoped<HandleUserUpdatedUseCase>();
        services.AddScoped<HandleUserDeletedUseCase>();

        // Query use cases
        services.AddScoped<GetAllUsersUseCase>();
        services.AddScoped<GetUserByIdUseCase>();

        return services;
    }
}