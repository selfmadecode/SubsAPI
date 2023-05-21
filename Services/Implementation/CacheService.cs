using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace SubsAPI.Services
{
    public class CacheService : ICacheService
    {
        ObjectCache _memoryCache = MemoryCache.Default;

        public T GetData<T>(string key)
        {
            return (T) _memoryCache.Get(key);
        }

        public object RemoveData(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return _memoryCache.Remove(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T data, DateTime expirationTime)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _memoryCache.Set(key, data, expirationTime);
                return true;
            }
            return false;
        }
    }
}
