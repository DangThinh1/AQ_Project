using AQS.BookingAdmin.Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Implements.Common
{
    public class CacheManager : ICacheManager
    {
        static Dictionary<string, object> _caches=new Dictionary<string,object>();
        private readonly object _lock = new object();
        public T Get<T>(string key, Func<T> acquire)
        {
            
            if (_caches.ContainsKey(key) && _caches[key] != null)
                return (T)_caches[key];
            lock (_lock)
            {
                var data = acquire();
                if (!_caches.ContainsKey(key))
                    _caches.Add(key, data);
                else
                    _caches[key] = data;

                return data;
            }
               
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> acquire)
        {

            if (_caches.ContainsKey(key) && _caches[key] != null)
                return  (T)_caches[key];
            lock (_lock)
            {
                var data =  acquire().Result;
                if (!_caches.ContainsKey(key))
                    _caches.Add(key, data);
                else
                    _caches[key] = data;

                return data;
            }
        }

        public void Remove(string key)
        {
            if (_caches.ContainsKey(key))
                _caches.Remove(key);
        }
    }
}
