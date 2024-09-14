using AuctionEntity.DTO.Req;
using AuctionEntity.Model.DTO;
using AuctionLogic;
using LibMessage;
using Microsoft.AspNetCore.Mvc;
using MQConnection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AuctionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IServiceScopeFactory _scopeFactory;
        string res = string.Empty;
        public ValuesController(IServiceScopeFactory serviceScopeFactory)
        {
          _scopeFactory = serviceScopeFactory;
             
        }



        [HttpPost("start")]
        public async Task<ActionResult> Start()
        {
            using (var scope = _scopeFactory.CreateScope()) 
            {
                var req = scope.ServiceProvider.GetRequiredService<IMessageService>();
                var _aucSet = scope.ServiceProvider.GetRequiredService<IAucSet>();

                try
                {




                    var reqFromGateway = JsonConvert.DeserializeObject<ReqFromGateway>(req.RetrieveMessage());
                    var result = await _aucSet.startAuction(reqFromGateway);
                    var resp = JsonConvert.SerializeObject(result);
                    req.StoreMessage(resp);


                    return Ok(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing request: {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            
            }
        }



    
    }
}