using Autofac;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Monarchy.Authentication.Extensibility.Model;
using Monarchy.Core.Service;
using Monarchy.Gateway.Extensibility.Interface;
using Monarchy.Gateway.Service;
using System;
using GatewayConfiguration = Monarchy.Gateway.Extensibility.Configuration;

namespace Monarchy.Gateway.Api
{
    public class AutofacModule : Module
    {
        private readonly Uri authenticationUrl;
        private readonly TimeSpan authenticationTimeout;

        public AutofacModule(GatewayConfiguration configuration)
        {
            authenticationUrl = new Uri(configuration.AuthenticationBus.Url);
            authenticationTimeout = TimeSpan.FromSeconds(configuration.AuthenticationBus.TimeoutSecs);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            RegisterServices(builder);
            RegisterAuthenticationEvents(builder);
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<GatewayTokenService>().As<ITokenService>();
            builder.RegisterType<MessageBusHostedService>().As<IHostedService>();
        }

        private void RegisterAuthenticationEvents(ContainerBuilder builder)
        {
            var resolver = new ResolveRequestClient(authenticationUrl, authenticationTimeout);
            builder.Register(c => resolver.Resolve<LoginModel>(c));
        }

        public class ResolveRequestClient
        {
            private readonly Uri path;
            private readonly TimeSpan timeout;

            public ResolveRequestClient(Uri path, TimeSpan timeout)
            {
                this.path = path;
                this.timeout = timeout;
            }

            public IRequestClient<T> Resolve<T>(IComponentContext context) where T : class
            {
                var factory = context.Resolve<IClientFactory>();
                return factory.CreateRequestClient<T>(path, timeout);
            }

        }
    }
}
