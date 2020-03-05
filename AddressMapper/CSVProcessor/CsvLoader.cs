using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVProcessor
{
    public class CsvLoader : ICsvLoader
    {

        public List<AddressDTO> ProcessDataToDictionary(CityAddressCollecton data)
        {
            var addressBook = new List<AddressDTO>();
           Parallel.ForEach(data.AddressBook, addressDataItem =>
            {
                var addressdata = new AddressDTO();
                var JsonData = JsonConvert.SerializeObject(addressDataItem);
                var addressDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData);
                addressdata.AddressData = addressDictionary;
                addressdata.Key = addressDataItem.HASH;
                addressBook.Add(addressdata);
            });
            return addressBook;


        }

        public CityAddressCollecton ProcessDataCsv(string filepath)
        {
            var book = new CityAddressCollecton();

            book.City = filepath;
            using (TextReader fileReader = File.OpenText(filepath))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = false;
                csv.Read();
                book.AddressBook = csv.GetRecords<AddressModel>().ToList();
            }



            return book;
        }
    }
}
