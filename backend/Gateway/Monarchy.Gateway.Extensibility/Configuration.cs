using CoreConfiguration = Monarchy.Core.Extensibility.Configuration;

namespace Monarchy.Gateway.Extensibility
{
    public class Configuration : CoreConfiguration
    {
        public string Secret { get; set; }
        public int TokenExpirySecs { get; set; }
    }
}
