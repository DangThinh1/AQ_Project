using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.Helpers
{
    public  class DependencyInjectionHelper
    {
        internal static IServiceProvider _serviceProvider;
        public static void Init(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        }
        public static T GetService<T>() where T:class
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
