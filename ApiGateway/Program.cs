using Api.Masstransit.Extensions;
using GatewayLogic.MicroserviceClient.Interface;
using GatewayLogic.MicroserviceClient.Microservice;
using MassTransit;
using Serilog;

try
    {


    var builder = WebApplication.CreateBuilder(args);
    builder.AddSerilog("API MassTransit");
    Log.Information("Starting API");
    var appSetting = new AppSettings();
    builder.Configuration.Bind(appSetting);

    var services = builder.Services;

    services.AddRouting(options => options.LowercaseUrls = true);

    services.AddControllers();
    services.AddOpenTelemetry(appSetting);

    

    services.AddMassTransitExtension(builder.Configuration);
    services.AddSingleton<IMicroserviceClient, AuctionServicesClient>();



    services.AddSwaggerGen();


    services.AddSingleton<Gateway>();




    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
    


    app.UseSwagger();
    app.UseSwaggerUI();
    
    }




    app.MapControllers();
    await app.RunAsync();
    }
    catch (Exception ex )
    {

    Log.Fatal(ex, "Host terminated unexpectedly");
    }
    finally
    {
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
    }