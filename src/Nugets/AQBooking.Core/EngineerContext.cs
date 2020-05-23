using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
namespace AQBooking.Core
{
     public class EngineerContext
    {
        static IServiceProvider _serviceProvider;
        public static void Init(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public static T GetService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
} 