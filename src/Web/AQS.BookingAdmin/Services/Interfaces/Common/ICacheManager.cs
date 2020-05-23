using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Interfaces.Common
{
    public interface ICacheManager
    {
        T Get<T>(string key, Func<T> acquire);
        Task<T> GetAsync<T>(string key, Func<Task<T>> acquire);
        void Remove(string key);
    }
}
