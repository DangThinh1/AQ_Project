﻿using System;
using Microsoft.Extensions.DependencyInjection;
namespace AQBooking.Core.Helpers
{
    public class ServiceHelper
    {
        private static IServiceProvider _serviceProvider;
        private static dynamic _dbContext;
        public static void Init(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public static void SetDbContext(dynamic dbContext)
        {
            _dbContext = dbContext;
        }

        public static T GetService<T>() => _serviceProvider.GetRequiredService<T>();
        public static dynamic GetDbContext() => _dbContext;
    }
}
