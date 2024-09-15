using AucthionApi.EndpointExtensions;
using AuctionEntity.Interface;
using AuctionEntity.Model.Context;
using AuctionEntity.Model.DTO;
using AuctionLogic;
using AuctionLogic.Servisec;
using LibMessage;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MQConnection;
using MQConnection.DTO;
using RabbitMQConnection;



var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;



builder.Services.AddOptions();
builder.Logging.AddConsole();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

services.AddControllers();

services.AddDbContext<AuctionDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddSingleton<IRabbitConnection>(new RabbitConnection());
services.AddScoped<IMessageProducer, RabbitProducer>();
services.AddSingleton<IConfigServisec>(new ConnectionSettingsDTO(builder.Configuration.GetSection("StartRabbitSettings")));

services.AddEndpointsApiExplorer();
services.AddScoped<IAuctionRepository, AuctionRepository>();
services.AddScoped<IAcuctioneServisec, AuctionServisec>();
services.AddScoped<IAucSet, MainAuct>();

services.AddHostedService<RabbitMQServisec>();
var messenge = services.AddTransient<IMessageService, MessageService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    

}

app.MapAuctionRouting().CreateApplicationBuilder();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();

