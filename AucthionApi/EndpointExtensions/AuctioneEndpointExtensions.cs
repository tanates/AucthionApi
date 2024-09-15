using AuctionEntity.DTO.Req;
using AuctionLogic;
using LibMessage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AucthionApi.EndpointExtensions
{
    public static class AuctionEndpointExtensions
    {
       
        public static IEndpointRouteBuilder MapAuctionRouting
            (this IEndpointRouteBuilder app )
        {
          

            //app.MapDelete("delete" ,Delete );
             app.MapGet("/creat", Creat);
            //app.MapGet("getAll", GetAll); 
            //app.MapPost("get{id}", GetByID);
            return app;
        }

        private static async Task<IResult> Creat(IServiceScopeFactory scopeFactory)
        {
            
            if (scopeFactory == null)
            {
                return Results.BadRequest();
            }
            using var scope = scopeFactory.CreateScope();
            var aucMessange = scope.ServiceProvider.GetRequiredService<IMessageService> ();  
            var allMessage = aucMessange.RetrieveMessage();
            var model = AuctionDTO.convertJson(allMessage);
            var aucServisec = scope.ServiceProvider.GetRequiredService<IAucSet>();
            var res = await aucServisec.startAuction( new ReqFromGateway { AuctionDTO = model });

            return Results.Ok(res.Data);
        }
    }
}
