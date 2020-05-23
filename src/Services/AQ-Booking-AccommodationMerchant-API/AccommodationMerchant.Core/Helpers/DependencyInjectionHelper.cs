using Microsoft.Extensions.DependencyInjection;

namespace AccommodationMerchant.Core.Helpers
{
    public static class DependencyInjectionHelper
    {
        private static IServiceCollection _serviceCollection;

        public static void TryInit(ref IServiceCollection serviceCollection)
        {
            if (_serviceCollection == null)
                _serviceCollection = serviceCollection;
        }

        public static T GetService<T>() where T : class
        {
            if(_serviceCollection != null)
                return _serviceCollection.BuildServiceProvider().GetRequiredService<T>();
            return null;
        }

    }
}