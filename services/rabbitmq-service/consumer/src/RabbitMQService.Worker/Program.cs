using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

using RabbitMQService.Application;
using RabbitMQService.Infrastructure;
using RabbitMQService.Infrastructure.Data;
using RabbitMQService.Infrastructure.Messaging;
using RabbitMQService.Infrastructure.Settings;
using RabbitMQService.Worker.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMQ")
);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<RabbitMQConsumerService>();

var app = builder.Build();

await ApplyMigrationsAsync(app);

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/", () => "RabbitMQ Consumer Service is running.");

await app.RunAsync();

// ── Helpers ───────────────────────────────────────────────────────────────────

static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope  = app.Services.CreateScope();
    var db     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Applying database migrations…");
        await db.Database.MigrateAsync();
        logger.LogInformation("Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying migrations.");
        throw;
    }
}