using Microsoft.Extensions.DependencyInjection;

namespace AQBooking.FileStream.Core.Helpers
{
    public class DependencyInjectionHelper
    {
        private static IServiceCollection _serviceCollection;

        public static void Init(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public static T GetService<T>() where T : class
        {
            if (_serviceCollection != null)
                return _serviceCollection.BuildServiceProvider().GetService<T>();
            return null;
        }
    }
}
