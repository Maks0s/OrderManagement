using NotificationService.Application;
using NotificationService.Infrastructure;
using NotificationService.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();

var app = builder.Build();

app.Run();