using AuctionEntity.DTO.Req;
using GatewayEntity.DTO.Req.Api;
using GatewayLogic.MicroserviceClient.Interface;
using LibMessage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

public class Gateway
{
    private readonly Dictionary<string, IMicroserviceClient> _microservices;
    private readonly IMessageService _messageService;
    public Gateway(IEnumerable<IMicroserviceClient> microserviceClients , IMessageService messageService)
    {
        _microservices = microserviceClients.ToDictionary(client => client.GetType().Name);
        _messageService = messageService;
    }

    public async Task<ReqFromClient> ProcessRequest(ReqFromClient model)
    {
        if (_microservices.TryGetValue(model.ServisecName, out var microserviceClient))
        {
            var auctionDTO = JsonConvert.DeserializeObject<AuctionDTO>(model.Data.ToString());
             var response = await microserviceClient.SendRequestAsync(auctionDTO);
     
            return new ReqFromClient { Data = response };
        }

        return null;
    }
}


