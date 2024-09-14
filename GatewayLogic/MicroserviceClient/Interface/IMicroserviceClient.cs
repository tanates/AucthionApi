using GatewayEntity.DTO.Req.Api;

namespace GatewayLogic.MicroserviceClient.Interface
{
    public interface IMicroserviceClient
    {
        Task<string> SendRequestAsync<T>(T message);
    }

}