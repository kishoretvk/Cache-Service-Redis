using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CacheManager;
using CSVProcessor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace AddressMapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private IConfiguration _config;

        private ISearchRepository _cacheRepo;

        private ICsvLoader _csvLoader;
        private IDatabase _cacheDb;

        public ValuesController(IConfiguration config, ISearchRepository cacheRepo,
            ICsvLoader csvLoader
            )
        {
            _config = config;
            _csvLoader = csvLoader;
            _cacheRepo = cacheRepo;
            _cacheDb = ConnectionFactory.RedisCache;

        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                var folderpath = _config.GetSection("AddressPath").Value;
                var filePaths = Directory.GetFiles(folderpath, "*_*.csv");
                foreach (var item in filePaths)
                {
                    //extract file name 
                    var filename = Convert.ToString(filePaths.FirstOrDefault().Split('\\')[3].Split('.')[0]);
                    //add key to hash match 
                    var dataFromCsv = _csvLoader.ProcessDataCsv(item);
                    var dataForCache = _csvLoader.ProcessDataToDictionary(dataFromCsv);
                    var datafromHash = _cacheRepo.ConvertDataForHash(dataForCache);
                    //push to cache store db
                    var result = _cacheRepo.PushHashBatchToRedis(datafromHash);
                    //data pushed to redis as hash
                    var dataForDocument = _cacheRepo.ConvertDataForDocument(dataForCache);
                    //push to search index
                    var resultforIndex = _cacheRepo.AddDataToIndex(cacheDb, "addressIndex", dataForDocument);
                    //once check 

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
           
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
