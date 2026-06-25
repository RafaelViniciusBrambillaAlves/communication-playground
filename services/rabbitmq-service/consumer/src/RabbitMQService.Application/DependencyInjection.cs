using Microsoft.Extensions.DependencyInjection;

using RabbitMQService.Application.UseCases.HandleUserCreated;
using RabbitMQService.Application.UseCases.HandleUserUpdated;
using RabbitMQService.Application.UseCases.HandleUserDeleted;

namespace RabbitMQService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services.AddScoped<HandleUserCreatedUseCase>();
        services.AddScoped<HandleUserUpdatedUseCase>();
        services.AddScoped<HandleUserDeletedUseCase>();

        return services;
    }
}