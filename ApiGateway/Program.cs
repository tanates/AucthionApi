using GatewayLogic;
using GatewayLogic.MicroserviceClient.Interface;
using GatewayLogic.MicroserviceClient.Microservice;
using LibMessage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using MQConnection;
using MQConnection.DTO;
using RabbitMQConnection;


var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;



builder.Services.AddOptions();
builder.Logging.AddConsole();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSingleton<IRabbitConnection>(new RabbitConnection());
services.AddSingleton<IMessageProducer, RabbitProducer>();
services.AddSingleton<IConfigServisec>(new ConnectionSettingsDTO(builder.Configuration.GetSection("StartRabbitSettings")));

services.AddSingleton<IMicroserviceClient, AuctionServicesClient>();
services.AddSingleton<Gateway>();
services.AddSingleton<RabbitMQServisec>();
services.AddHostedService<RabbitMQServisec>();
services.AddTransient<IMessageService, MessageService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
