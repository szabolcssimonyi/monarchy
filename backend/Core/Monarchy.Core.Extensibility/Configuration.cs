using System.Collections.Generic;

namespace Monarchy.Core.Extensibility
{
    public class Configuration
    {
        public List<string> Permissions => new List<string>
        {
            "monarchy.user.view",
            "monarchy.user.create",
            "monarchy.user.edit",
            "monarchy.user.delete",
            "monarchy.me.view",
            "monarchy.role.view",
            "monarchy.role.create",
            "monarchy.role.edit",
            "monarchy.role.delete",
        };

        public class MessageBusConfiguration
        {
            public string Path { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Host { get; set; }
            public string Type { get; set; }
            public int TimeoutSecs { get; set; }
            public string Url => $"{Host}/{Path}";
        }

        public class Microservices
        {
            public const string Gateway = "Gateway";
            public const string Authentication = "Authentication";
        }

        public MessageBusConfiguration GatewayBus
        {
            get => MessageBusConfigurations.ContainsKey(Microservices.Gateway) ? MessageBusConfigurations[Microservices.Gateway] : null;
            set => MessageBusConfigurations[Microservices.Gateway] = value;
        }

        public MessageBusConfiguration AuthenticationBus
        {
            get => MessageBusConfigurations.ContainsKey(Microservices.Authentication) ? MessageBusConfigurations[Microservices.Authentication] : null;
            set => MessageBusConfigurations[Microservices.Authentication] = value;
        }

        public string Microservice { get; set; }

        public MessageBusConfiguration CurrentBus => MessageBusConfigurations[Microservice];

        private readonly Dictionary<string, MessageBusConfiguration> MessageBusConfigurations =
            new Dictionary<string, MessageBusConfiguration>();

        public string LoggerUrl { get; set; }
        public Configuration()
        {
        }
    }
}
