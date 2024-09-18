using Api.Masstransit.Event;
using Api.Masstransit.Extensions;
using AuctionEntity.Interface;
using AuctionEntity.Model.Context;
using AuctioneWorker.Consumer;
using AuctionLogic;
using AuctionLogic.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);


    builder.AddSerilog("Worker MassTransit");

    var services = builder.Services;



    builder.AddSerilog("Worker MassTransit");
    Log.Information("Starting Worker");

    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog(Log.Logger)
        .ConfigureServices((context, collection) =>
        { // need scoped for imicroservices
            var appSettings = new AppSettings();
            context.Configuration.Bind(appSettings);
            collection.AddScoped<IAuctionRepository, AuctionRepository>();
            collection.AddScoped<IAcuctioneServices, AuctionServices>();
            collection.AddDbContext<AuctionDbContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("SQL")));
            collection.AddScoped<IAucSet, MainAuct>();
            collection.AddOpenTelemetry(appSettings);
            collection.AddHttpContextAccessor();
            collection.AddMassTransit(x =>
            {

                x.AddDelayedMessageScheduler();
                x.AddConsumer<QueueAuctionStartedConsumer>(typeof(QueueAuctionConsumerDefinition));
                x.AddConsumer<>// deleted this string 
                x.AddRequestClient<StartAuction >();
                x.AddRequestClient<CreatLote>();
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(context.Configuration.GetConnectionString("RabbitMq"));
                    cfg.UseDelayedMessageScheduler();
                    cfg.ServiceInstance(instance =>
                    {
                        instance.ConfigureJobServiceEndpoints();
                        instance.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));
                    });
                });
            });
        }).Build();
    await host.RunAsync();  
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}