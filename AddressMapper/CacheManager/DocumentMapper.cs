using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager
{
  public class DocumentMapper
    {
        public string RedisKey { get; set; }
        public Dictionary<string, RedisValue> DocumentData { get; set; }
        public double Score { get; set; } = 1;
        public Boolean NoSave { get; set; } = false;
        public Boolean ReplaceFlag { get; set; } = true;
    }
    public class AddressHash
    {
        public string RedisKey { get; set; }
        public HashEntry[] addressHashData { get; set; }
        public DateTime ExpirationTime { get; set; }

    }

}
