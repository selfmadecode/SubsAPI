using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Services
{
    public interface ICacheService
    {
        T GetData<T>(string key);

        bool SetData<T>(string key, T data, DateTime expirationTime);

        object RemoveData(string key);
    }
}
