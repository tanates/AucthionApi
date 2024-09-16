using Api.Masstransit.Event;
using AuctionEntity.DTO.Req;
using AuctionEntity.Interface;
using AuctionLogic;
using AuctionLogic.Services;
using MassTransit;
using MassTransit.Metadata;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctioneWorker.Consumer
{
    public class QueueAuctionStartedConsumer : IConsumer<StartAuction>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public QueueAuctionStartedConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory=scopeFactory;
        }

        public async Task Consume(ConsumeContext<StartAuction> context)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var timer = Stopwatch.StartNew();
                try
                {
                    scope.ServiceProvider.GetRequiredService<IAcuctioneServices>();
                    scope.ServiceProvider.GetRequiredService<IAuctionRepository>();
                    var _aucServisec = scope.ServiceProvider.GetRequiredService<IAucSet>();
                    JsonConvert.DefaultSettings()
                    var msg = JsonConvert.SerializeObject(context.Message);
                    var aucDto = JsonConvert.DeserializeObject<AuctionDTO>(msg);
                    
                    
                    var _logger = scope.ServiceProvider.GetRequiredService<ILogger<QueueAuctionStartedConsumer>>();
                    var res = await  _aucServisec.startAuction(aucDto);


                    var responserGateway = new StartAuction
                    {
                        ResponseGateway =  StartAuction.SetGateway(res.IsSuccess, res.Data),
                    };

                  
                    
                    await context.RespondAsync(responserGateway);
                        

                    await context.NotifyConsumed(timer.Elapsed, TypeMetadataCache<StartAuction >.ShortName);
                }
                catch (Exception ex)
                {
                    await context.NotifyFaulted(timer.Elapsed, TypeMetadataCache<StartAuction>.ShortName, ex);
                }
            }
            
        }
    }

    public class QueueAuctionConsumerDefinition : ConsumerDefinition<QueueAuctionStartedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueAuctionStartedConsumer> consumerConfigurator, IRegistrationContext context)
        {
            consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(20)));

        }

    }
}


