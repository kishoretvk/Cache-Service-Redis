using CSVProcessor;
using NRediSearch;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NRediSearch.Schema;

namespace CacheManager
{
    public class SearchRepository : ISearchRepository
    {
        private IDatabase cacheDb;
        public SearchRepository()
        {
            cacheDb = ConnectionFactory.RedisCache;

        }

        public  List<DocumentMapper> ConvertDataForDocument(List<AddressDTO> addressdata)
        {
            var documentList = new List<DocumentMapper>();
            Parallel.ForEach(addressdata, singledoc=>
           {
                var documentMapperData = new DocumentMapper();
               documentMapperData.RedisKey = singledoc.Key,
               documentMapperData.DocumentData = singledoc.AddressData.
               Where(pair =>
                ((pair.Value != null) || (!string.IsNullOrEmpty(pair.Value))))
               .ToDictionary(kvp => kvp.Key,
               kvp => (RedisValue) kvp.Value );
               documentList.Add(documentMapperData);

           });
            return documentList;
        }
        public List<AddressHash> ConvertDataForHash(List<AddressDTO>  addressdata )
        {
            var addressList = new List<AddressHash>();
            Parallel.ForEach(addressdata , address =>
            {
                var addressKeyHash = new AddressHash();
                addressKeyHash.RedisKey = address.Key;
                var dictionarydata = address.AddressData.Where(pair =>
                ((pair.Value != null) || (!string.IsNullOrEmpty(pair.Value))))
                .Select( pair => new HashEntry(pair.Key, pair.Value)).ToArray();
                addressKeyHash.addressHashData = dictionarydata;
                addressList.Add(addressKeyHash);
            });
            return addressList;
        }
        public Schema BuildSchemaForSearch(SchemaHelperModel schemaModel)
        {
            var currentSchema = new Schema();
            foreach (var item in schemaModel.SearchFields)
            {
                currentSchema.AddField(item);
            }
            foreach (var item in schemaModel.TextFields)
            {
                currentSchema.AddSortableTextField(item.FIeldName, item.Weignt);
            }
            foreach (var item in schemaModel.NumericFieldName)
            {
                currentSchema.AddNumericField(item);
            }
            foreach (var item in schemaModel.TagFilterName)
            {
                currentSchema.AddTagField(item);
            }
            foreach (var item in schemaModel.GeoFilterName)
            {
                currentSchema.AddGeoField(item);
            }

            return currentSchema;
        }


        public Boolean SearchSchemaBuilder(IDatabase cacheDb, Schema schema, string indexName)
        {
            var cacheClient = new Client(indexName, cacheDb);

            var result = cacheClient.CreateIndex(schema, Client.IndexOptions.Default);
            return result;
        }
        public Boolean PushHashBatchToRedis(List<AddressHash> documentData)
        {
            var batch = cacheDb.CreateBatch();
            var list = new List<Task>();
            foreach (var item in documentData)
            {

                var addressMap = batch.HashSetAsync(item.RedisKey, item.addressHashData, CommandFlags.FireAndForget);
                list.Add(addressMap);
            }
            batch.Execute();
            var data = Task.WhenAll(list.Where(x=>x!=null).ToArray());

            return true;
        }

        public Boolean AddDataToIndex(IDatabase cacheDb, string IndexName, List<DocumentMapper> documentData)
        {
            try
            {
                var documentList = new List<Task<bool>>();
                var redisClient = new Client(IndexName, cacheDb);
                foreach (var item in documentData)
                {
                    var documentTask = redisClient.AddDocumentAsync(item.RedisKey, item.DocumentData, item.Score, item.NoSave, item.ReplaceFlag, null);
                    documentList.Add(documentTask);
                }

                var result = Task.WhenAll(documentList.Where(x => x != null).ToArray());

                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return false;
               // throw ex;
            }
            
           
        }
    }
}
