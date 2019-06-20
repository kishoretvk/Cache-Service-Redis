using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CachingService
{
    public class CacheDataAccess
    {
        private IConnectionStore  _ionnectionStore;
        private IConfigurationRoot _config;
        private IDatabase cacheDB;

        public CacheDataAccess()
        {
            cacheDB = ConnectionStore.dbCache;
        }

        public string AddKeyVAueFirstTime(string cacheKey, string value, TimeSpan timetoLIve)
        {
            try
            {
                var result = cacheDB.StringSet(cacheKey, value, timetoLIve, When.NotExists, CommandFlags.FireAndForget);
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }

            return cacheKey;
        }

        public string AddKeyVAueAlwaysOverride(string cacheKey, string value, TimeSpan timetoLIve)
        {
            try
            {
                var result = cacheDB.StringSet(cacheKey, value, timetoLIve, When.Always, CommandFlags.FireAndForget);
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }

            return cacheKey;
        }

        public List<string> AddKeysListBatchOverride(Dictionary<string, string> keysvalues, TimeSpan timetolive)
        {
            var keys = new List<string>();
            try
            {
                var keysBatch = cacheDB.CreateBatch();
                
                foreach (var keyvalue in keysvalues)
                {
                    var storekeys = keysBatch.StringSetAsync(keyvalue.Key, keyvalue.Value, timetolive, When.Always, CommandFlags.FireAndForget);
                    keys.Add(keyvalue.Key);
                }
                keysBatch.Execute();

            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }

            return keys;
        }

        public List<string> AddKeysListBatchOnlyAddNew(Dictionary<string, string> keysvalues, TimeSpan timetolive)
        {
            var keys = new List<string>();
            try
            {
                var keysBatch = cacheDB.CreateBatch();

                foreach (var keyvalue in keysvalues)
                {
                    var storekeys = keysBatch.StringSetAsync(keyvalue.Key, keyvalue.Value, timetolive, When.NotExists, CommandFlags.FireAndForget);
                    keys.Add(keyvalue.Key);
                }
                keysBatch.Execute();

            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }

            return keys;
        }


    }
}
