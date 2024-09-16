using Api.Masstransit.Event;
using Api.Masstransit.Extensions;
using GatewayEntity.DTO.Req.Api;
using GatewayLogic.MicroserviceClient.Interface;
using Jaeger.Thrift.Agent.Zipkin;
using MassTransit;
using MassTransit.Clients;
using MassTransit.Internals;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GatewayLogic.MicroserviceClient.Microservice
{
    public class AuctionServicesClient : IMicroserviceClient
    {
        //private readonly RabbitMQClient _rabbitMQClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public AuctionServicesClient(IServiceScopeFactory scopeFactory)
        {

            _scopeFactory = scopeFactory;
        }

        public async Task<ResponseGateway> SendRequestAsync<T>([FromBody]T model)
        {
            if (model is StartAuction message)
            {
               
                   using  (var scope = _scopeFactory.CreateScope())
                {
                    
                    var _publisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                    var _logger = scope.ServiceProvider.GetRequiredService<ILogger<AuctionServicesClient>>();
                    var _requestClient = scope.ServiceProvider.GetRequiredService<IRequestClient< StartAuction>>();

                    try
                    {

                       //  var r = await _publisher.Publish<StartAuction>(message);
                       if(message.LotID ==null)
                        {

                        }
                       var response = await _requestClient.GetResponse<StartAuction >(message);
                        _logger.LogInformation($"Send client inserted: {message.Id} - {message.Name}");
                        
                        return response.Message.ResponseGateway;
                    }
                    catch (Exception ex)
                    {

                        _logger.LogInformation($"Send client inserted: {message.Id} - {message.Name}");
                        return  new ResponseGateway { Error = ex.Message , Data = "" , IsSuccess = false };
                    }

                }


                
            }
            return null;
                
        }
    }
}
