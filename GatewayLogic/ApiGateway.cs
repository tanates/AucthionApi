using Api.Masstransit.Event;
using GatewayEntity.DTO.Req.Api;
using GatewayLogic.MicroserviceClient.Interface;

public class Gateway
{
    private readonly Dictionary<string, IMicroserviceClient> _microservices;

    public Gateway(IEnumerable<IMicroserviceClient> microserviceClients )
    {
        _microservices = microserviceClients.ToDictionary(client => client.GetType().Name);

    }

    public async Task<ResponseGateway> ProcessRequest(ReqFromClient model)
    {
        if (_microservices.TryGetValue(model.ServisecName, out var microserviceClient))
        {
             var auctionStart = StartAuction.Start(model.Data);
             var response = await microserviceClient.SendRequestAsync(auctionStart);
     
            return response;
        }

        return null;
    }
}


