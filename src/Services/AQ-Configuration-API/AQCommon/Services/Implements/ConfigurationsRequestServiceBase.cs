using APIHelpers.ServerHost;
using Identity.Core.Services.Base;

namespace AQConfigurations.Core.Services.Implements
{
    public class ConfigurationsRequestServiceBase : RequestServiceBase
    {
        protected string _configurationsHost;

        public ConfigurationsRequestServiceBase() : base()
        {
            UseNonAuthorization();
            _configurationsHost = ServerHostHelper.GetServerHostByName(ServerHostNameConts.ConfigurationApi);
        }
    }
}