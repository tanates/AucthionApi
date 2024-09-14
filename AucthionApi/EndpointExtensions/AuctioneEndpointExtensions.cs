namespace AucthionApi.EndpointExtensions
{
    public static class AuctionEndpointExtensions
    {
        public static IEndpointRouteBuilder MapAuctionRouting
            (this IEndpointRouteBuilder app)
        {
            app.MapPost('')
            
            return app;
        }
    }
}
