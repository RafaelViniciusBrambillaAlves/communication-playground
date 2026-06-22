using Microsoft.EntityFrameworkCore;
using SoapCore;
using SoapService.Contracts;
using SoapService.Data;
using SoapService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer"),
        sql => sql.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(10),
        errorNumbersToAdd: null)));

builder.Services.AddSoapCore();
builder.Services.AddScoped<IUserSoapService, UserSoapService>();

var app = builder.Build();

await ApplyMigrationsAsync(app);

((IApplicationBuilder)app).UseSoapEndpoint<IUserSoapService>(
    "/UserService.asmx",
    new SoapEncoderOptions(),
    SoapSerializer.XmlSerializer
);

app.MapGet("/", () => "SOAP service is runnign. SWDL available at /UserService.asmx?wsdl");

await app.RunAsync();

// Helpers

static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

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
        logger.LogError(ex, "An error occurred while applying migrations.");
        throw;
    }
}
