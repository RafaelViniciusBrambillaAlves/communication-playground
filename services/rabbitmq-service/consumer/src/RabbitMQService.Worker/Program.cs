using Microsoft.EntityFrameworkCore;
using RabbitMQService.Application;
using RabbitMQService.Infrastructure;
using RabbitMQService.Infrastructure.Data;
using RabbitMQService.Infrastructure.Messaging;
using RabbitMQService.Infrastructure.Settings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMQ")
);

builder.Services.AddAplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<RabbitMQConsumerService>();

var host = builder.Build();

await ApplyMigrationsAsync(host);
await host.RunAsync();

// Helpers
static async Task ApplyMigrationsAsync(IHost host)
{
    using var scope = host.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Applying database migrations...");
        await db.Database.MigrateAsync();
        logger.LogInformation("Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying migrations");
        throw;
    }
}