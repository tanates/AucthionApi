using AucthionApi.EndpointExtensions;
using AuctionEntity.Interface;
using AuctionEntity.Model.Context;
using AuctionEntity.Model.DTO;
using AuctionLogic;
using AuctionLogic.Servisec;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

using Serilog;



var builder = WebApplication.CreateBuilder(args);

Log.Information("Starting API");
var services = builder.Services;



builder.Services.AddOptions();
builder.Logging.AddConsole();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

services.AddControllers();

services.AddDbContext<AuctionDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));



services.AddEndpointsApiExplorer();

services.AddScoped<IAucSet, MainAuct>();


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

