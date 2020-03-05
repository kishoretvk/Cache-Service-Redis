using System;
using System.Collections.Generic;
using System.Text;

namespace CSVProcessor
{
   public class AddressModel
    {
        public double LAT { get; set; }
        public double LON { get; set; }
        public int NUMBER { get; set; }
        public string STREET { get; set; }
        public int UNIT { get; set; }
        public string CITY { get; set; }
        public string DISTRICT { get; set; }
        public string REGION { get; set; }
        public int POSTCODE { get; set;  }
        public int ID { get; set; }
        public string HASH { get; set; }
    }
    public class CityAddressCollecton
    {
        public string City { get; set; }
        public List<AddressModel> AddressBook {get;set;}

    }
}
