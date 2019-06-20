using System;
using StackExchange.Redis;

namespace CachingService
{
    public interface IConnectionStore
    {
        ConnectionMultiplexer Connection { get;  }
        //Lazy<ConnectionMultiplexer> LazyConnection { get; set; }
       // IDatabase RedisCache { get; set; }

        bool GetConnection(ConfigurationOptions RedisConfiguration);
    }
}