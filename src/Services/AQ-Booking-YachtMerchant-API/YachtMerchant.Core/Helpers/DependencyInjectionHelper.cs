﻿using Microsoft.Extensions.DependencyInjection;

namespace YachtMerchant.Core.Helpers
{
    public static class DependencyInjectionHelper
    {
        private static IServiceCollection _serviceCollection;

        public static void Init(ref IServiceCollection serviceCollection)
        {
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
