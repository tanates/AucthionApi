using Api.Masstransit.Event;
using GatewayEntity.DTO.Req.Api;

namespace GatewayLogic.MicroserviceClient.Interface
{
    public interface IMicroserviceClient
    {
        Task<ResponseGateway> SendRequestAsync<T>(T message);
    }

}