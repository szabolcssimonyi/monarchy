using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Monarchy.Core.Service
{
    public class MessageBusHostedService : IHostedService
    {
        private readonly IBusControl busControl;

        public MessageBusHostedService(IBusControl busControl)
        {
            this.busControl = busControl;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return busControl.StopAsync(cancellationToken);
        }
    }
}
