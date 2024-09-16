using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Masstransit.Extensions
{
    public class ReceiveObserverExtensions : IReceiveObserver
    {
        public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception) where T : class
        {
            return Task.FromResult(context);
        }

        public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType) where T : class
        {
            return Task.FromResult(context);
        }

        public Task PostReceive(ReceiveContext context)
        {
            return  Task.FromResult(context);
        }

        public Task PreReceive(ReceiveContext context)
        {
            return Task.FromResult(context);
        }

        public Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            return  Task.FromResult(new{ Context =context , Exception =exception});
        }
    }
}
