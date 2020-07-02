using Autofac;
using GreenPipes;
using MassTransit;
using System;
using System.Reflection;

namespace Monarchy.Core.Extensibility.Extension
{
    public static class MassTransitExtension
    {
        public static void InitializeMassTransit(this ContainerBuilder builder, Assembly assembly, Configuration.MessageBusConfiguration configuration)
        {
            builder.AddMassTransit(x =>
            {
                x.AddConsumers(assembly);
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(configuration.Host, h =>
                    {
                        h.Username(configuration.UserName);
                        h.Password(configuration.Password);
                    });
                    cfg.ReceiveEndpoint(configuration.Path, e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(10, TimeSpan.FromMilliseconds(200));
                        });
                        e.ConfigureConsumers(provider); // configures registered consumers on given endpoint
                    });
                    //cfg.ConfigureEndpoints(provider); // configures registered consumers on all endpoints, cannot make it work...
                }));
            });
        }
    }
}
