using Api.Masstransit.Event;
using Api.Masstransit.Extensions;
using GatewayLogic.MicroserviceClient.Interface;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        public async Task<string> SendRequestAsync<T>([FromBody]T model)
        {
            if (model is StartAuction message)
            {
               
                   using  (var scope = _scopeFactory.CreateScope())
                {
                    var _publisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                    var _reseive = scope.ServiceProvider.GetRequiredService<ReceiveObserverExtensions>();
                    var _logger = scope.ServiceProvider.GetRequiredService<ILogger<AuctionServicesClient>>();

                    try
                    { 
                        await _publisher.Publish(message);
                        _logger.LogInformation($"Send client inserted: {message.Id} - {message.Name}");
                        
                        return $"{message.Id}" ?? "No response from service";
                    }
                    catch (Exception ex)
                    {

                        _logger.LogInformation($"Send client inserted: {message.Id} - {message.Name}");
                        return $"" ?? $"{ex.Message}";
                    }

                }


                
            }
            return null;
                
        }
    }
}
