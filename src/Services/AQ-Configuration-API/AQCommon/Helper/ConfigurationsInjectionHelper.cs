using Microsoft.Extensions.DependencyInjection;

namespace AQConfigurations.Core.Helper
{
    public class ConfigurationsInjectionHelper
    {
        public static IServiceCollection _serviceCollection { get; private set; }
        public static bool IsConfigured => _serviceCollection != null;
        public static void TryInit(ref IServiceCollection serviceCollection)
        {
            if (!IsConfigured)
                _serviceCollection = serviceCollection;
        }
        public static T GetRequiredService<T>() where T : class
            => _serviceCollection?.BuildServiceProvider().GetRequiredService<T>();
    }
}