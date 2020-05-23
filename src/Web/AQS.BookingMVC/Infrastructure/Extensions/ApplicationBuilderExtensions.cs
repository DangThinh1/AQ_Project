using AQS.BookingMVC.Infrastructure.Helpers;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
       public static void UseDependencyHelper(this IApplicationBuilder builder)
        {
            DependencyInjectionHelper.Init(builder.ApplicationServices);
        }
    }
}
