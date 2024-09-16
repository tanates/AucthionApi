using GatewayEntity.DTO.Req.Api;
using GatewayLogic;
using GatewayLogic.MicroserviceClient.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MbController : ControllerBase
    {
        private readonly Gateway _gateway;

        public MbController(Gateway gateway)
        {
            _gateway = gateway;
        }

        [HttpPost]
        public async Task<ActionResult> SendToApi([FromBody]ReqFromClient model)
        {
            if (model == null)
            {
                return BadRequest("Model is null");
            }

                var res = await _gateway.ProcessRequest(model);
            if (res?.Data == null)
            {
                return BadRequest("ProcessRequest error");
            }

            return Ok(res);
        }
    }
}
