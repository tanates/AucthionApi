using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Masstransit.Extensions
{
    public static class MasstransitExtension
    {
        public static void AddMassTransitExtension(
            this IServiceCollection service ,
            IConfiguration configuration)
        {
            service.AddMassTransit(x =>
            {
                x.AddDelayedMessageScheduler();
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("RabbitMq"));
                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));
                    cfg.UseMessageRetry(retry => { retry.Interval(3, TimeSpan.FromSeconds(3)); });
                });

            });
        }
         
    }
}
