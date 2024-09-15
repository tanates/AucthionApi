using AuctionEntity.DTO.Req;
using GatewayEntity.DTO.Req.Api;
using GatewayLogic.MicroserviceClient.Interface;
using LibMessage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using MQConnection;
using Newtonsoft.Json;
using RabbitMQConnection;
using System.Net.Http;
using System.Net.Http.Json;

namespace GatewayLogic.MicroserviceClient.Microservice
{
    public class AuctionServicesClient : IMicroserviceClient
    {
        //private readonly RabbitMQClient _rabbitMQClient;
        private readonly IServiceScopeFactory _scopeFactory;
   
        public AuctionServicesClient(IServiceScopeFactory scopeFactory )
        {
          
            _scopeFactory = scopeFactory;
        }

        public async Task<string> SendRequestAsync<T>(T model)
        {
            if (model is AuctionDTO message)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var ILoggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                    
                    var _logger = ILoggerFactory.CreateLogger<AuctionServicesClient>();
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    Console.WriteLine("Message published to request queue");
                    var mProducer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();
                    var config = configuration.GetSection("StartRabbitSettings");
                     

                    var iMessage = scope.ServiceProvider.GetRequiredService<IMessageService>();
                    var r = JsonConvert.SerializeObject(message);

                    var con =scope.ServiceProvider.GetRequiredService<IRabbitConnection>();
                    iMessage.StoreMessage(r);
                    var resSend = await mProducer.Send(config, r, con);
                    
                   
                    
                    _logger.LogInformation(resSend.Message , resSend.IsSuccses);
            
                    return resSend.Message ?? "No response from service";
                }
            }
            return null;
                
        }
    }
}
