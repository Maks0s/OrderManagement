using NotificationService.Application;
using NotificationService.Infrastructure;
using NotificationService.Host;
using Serilog;

try
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateBootstrapLogger();

    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Started app building");

    builder.Host.UseSerilog((_, loggerConfig) =>
    {
        var jsonConfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true, reloadOnChange: true)
            .Build();

        loggerConfig
            .ReadFrom.Configuration(jsonConfig);
    });

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddPresentation();

    var app = builder.Build();

    app.Run();

}
catch (Exception ex)
{
    Log.Error(ex, "Crushed during the building");
}
finally
{
    Log.CloseAndFlush();
}