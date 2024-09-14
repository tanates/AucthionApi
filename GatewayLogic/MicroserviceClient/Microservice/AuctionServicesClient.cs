using GatewayEntity.DTO.Req.Api;
using GatewayLogic.MicroserviceClient.Interface;
using LibMessage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32.SafeHandles;
using MQConnection;
using Newtonsoft.Json;

namespace GatewayLogic.MicroserviceClient.Microservice
{
    public class AuctionServicesClient : IMicroserviceClient
    {
        //private readonly RabbitMQClient _rabbitMQClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        public AuctionServicesClient(IServiceScopeFactory scopeFactory , IConfiguration configuration )
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        public async Task<string> SendRequestAsync<T>(T model)
        {
            if (model is AuctionReq message)
            {
                using (var scope = _scopeFactory.CreateScope())
                {

                    Console.WriteLine("Message published to request queue");
                    var mProducer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();
                    var config = _configuration.GetSection("AuctionRabbitSettings").GetChildren();


                    var iMessage = scope.ServiceProvider.GetRequiredService<IMessageService>();
                    var r = JsonConvert.SerializeObject(message);


                    iMessage.StoreMessage(r);
                    var resSend = await mProducer.Send(config, iMessage.RetrieveMessage());
                    var res = iMessage.RetrieveMessage();
                    if (res == null)
                    {

                    }
                    return res ?? "No response from service";
                }
            }
            return null;
                
        }
    }
}
