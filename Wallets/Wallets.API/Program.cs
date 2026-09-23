using NLog.Web;
using Wallets.API.Extensions;
using Wallets.Application.Extensions;
using Wallets.Infrastructure.Extensions;
using Wallets.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services
    .AddApplication()
    .AddPersistence(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseApiPipeline();

app.Run();
