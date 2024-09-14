using GatewayEntity.DTO.Req.Api;
using GatewayLogic.MicroserviceClient.Interface;
using LibMessage;
using System.Collections.Generic;
using System.Linq;
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
            var response = await microserviceClient.SendRequestAsync(model);
            return new ReqFromClient { Data = _messageService.RetrieveMessage() };
        }

        return null;
    }
}