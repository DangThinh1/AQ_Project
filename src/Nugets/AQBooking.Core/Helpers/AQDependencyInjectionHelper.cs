using Microsoft.Extensions.DependencyInjection;

namespace AQBooking.Core.Helpers
{
    public class AQ_DI : AQDependencyInjectionHelper { }

    public class AQDependencyInjectionHelper
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