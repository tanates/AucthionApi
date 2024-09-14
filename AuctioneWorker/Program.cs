using AuctionEntity.Model.DTO;
using AuctioneWorker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQConnection;
using RabbitMQConnection;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
services.AddOptions();
builder.Logging.AddConsole();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


builder.Services.AddHostedService<RabbitMQServisec>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {

        services.AddSingleton<IServiceScopeFactory>();
        services.AddHostedService<RabbitMQServisec>();
        
    })
    .Build();

host.Run();