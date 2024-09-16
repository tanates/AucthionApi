using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Masstransit.Extensions
{
    public static  class OpenTelemetryExtension
    {
        public static void AddOpenTelemetry(this IServiceCollection services , AppSettings appSettings)
        {
            services.AddOpenTelemetry().WithTracing(telemetry =>
            {

                var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector()
                .AddService(appSettings?.DistributedTracing?.Jaeger?.ServiceName??"Service");

                telemetry.AddSource("MassTransit").AddMassTransitInstrumentation()
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .SetSampler(new AlwaysOnSampler())
                .AddJaegerExporter(jaegerOptions =>
                {
                    jaegerOptions.AgentHost =appSettings?.DistributedTracing?.Jaeger?.Host;
                    jaegerOptions.AgentPort =appSettings?.DistributedTracing?.Jaeger?.Port ?? 0;
                });

            });
        }
    }
}
