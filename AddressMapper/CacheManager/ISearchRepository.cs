using CSVProcessor;
using NRediSearch;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace CacheManager
{
    public interface ISearchRepository
    {
        List<DocumentMapper> ConvertDataForDocument(List<AddressDTO> addressdata);
        List<AddressHash> ConvertDataForHash(List<AddressDTO> addressdata);
        Schema BuildSchemaForSearch(SchemaHelperModel schemaModel);
        bool SearchSchemaBuilder(IDatabase cacheDb, Schema schema, string indexName);
        Boolean PushHashBatchToRedis(List<AddressHash> documentData);
        Boolean AddDataToIndex(IDatabase cacheDb, string IndexName, List<DocumentMapper> documentData);
    }
}