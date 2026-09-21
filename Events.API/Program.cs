using Events.API.Extensions;
using Events.Application.Extensions;
using Events.Infrastructure.Extensions;
using Events.Persistence.Extensions;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services
    .AddApplication()
    .AddPersistence(builder.Configuration)
    .AddInfrastructure()
    .AddApiServices();

var app = builder.Build();

app.UseApiPipeline();

app.Run();
