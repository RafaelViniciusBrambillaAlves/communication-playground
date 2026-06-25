using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQService.Application.Interfaces;
using RabbitMQService.Infrastructure.Data;
using RabbitMQService.Infrastructure.Repositories;

namespace RabbitMQService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(Options => 
            Options.UseSqlServer(
                configuration.GetConnectionString("SqlServer"),
                sql => sql.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}