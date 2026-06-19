using GrpcService.Api.Interceptors;
using GrpcService.Api.Services;
using GrpcService.Application;
using GrpcService.Infrastructure;
using GrpcService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>();
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

await ApplyMigrationsAsync(app);

app.MapGrpcService<UserGrpcService>();

app.MapGet("/", () => "gRPC service is running. Use a gRPC client to communicate.");

await app.RunAsync();

// ── Helpers ───────────────────────────────────────────────────────────────────

static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var db     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Applying database migrations...");
        await db.Database.MigrateAsync();
        logger.LogInformation("Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying migrations.");
        throw;
    }
}