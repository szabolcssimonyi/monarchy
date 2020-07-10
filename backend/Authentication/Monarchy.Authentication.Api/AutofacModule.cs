using Autofac;
using Microsoft.Extensions.Hosting;
using Monarchy.Authentication.Domain.Seeder;
using Monarchy.Authentication.Extensibility.Interface;
using Monarchy.Authentication.Service;
using Monarchy.Core.Extensibility.Interface;
using Monarchy.Core.Service;

namespace Monarchy.Authentication.Api
{
    public class AutofacModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            RegisterSeeders(builder);
            RegisterServices(builder);
        }

        private void RegisterSeeders(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationDomainSeeder>().As<IDomainSeeder>();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<MessageBusHostedService>().As<IHostedService>();
            builder.RegisterType<UserService>().As<IUserService>();
        }
    }
}
