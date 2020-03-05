using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager
{
   public class ConnectionFactory
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static ConnectionFactory()
        {
            try
            {
                var builder = new ConfigurationBuilder()
               .SetBasePath(System.IO.Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");

                IConfigurationRoot config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();

                var configurationOptions = new ConfigurationOptions
                {
                    EndPoints = { config.GetSection("RedisConnecton:EndPoint") .Value},
                    AllowAdmin = true,
                    ConnectRetry = 10,
                    AbortOnConnectFail = false


                };

                LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static IDatabase RedisCache => Connection.GetDatabase();
    }
}
