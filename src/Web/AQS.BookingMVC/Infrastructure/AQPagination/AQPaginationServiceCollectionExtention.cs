using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.AQPagination
{
    public static class AQPaginationServiceCollectionExtention
    {
        public static void AddAQPagination(this IServiceCollection services)
        {
            services.AddScoped<IServicesNode, ServicesNode>();
            services.AddScoped<IServicesNextPage, ServicesNextPage>();
            services.AddScoped<IServicesPrevPage, ServicesPrevPage>();
            services.AddScoped<ILastPageInCollectionService, LastPageInCollectionService>();
            services.AddScoped<IPaginatedService, PaginatedService>();
        }
    }
}
