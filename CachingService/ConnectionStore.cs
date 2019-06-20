using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CachingService
{
   public class ConnectionStore 
    {

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static ConnectionStore()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { config["redis.connection"] }
            };

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static IDatabase dbCache => Connection.GetDatabase();


    }
}
