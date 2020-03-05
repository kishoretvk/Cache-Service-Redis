using System;
using System.Collections.Generic;
using System.Text;
using static NRediSearch.Schema;

namespace CacheManager
{
   public class SchemaHelperModel
    {
        public List<Field> SearchFields { get; set; }
        public List<TextFilterFields> TextFields { get; set; }
        public List <string> NumericFieldName { get; set; }
        public List<string> TagFilterName { get; set; }
        public List<string> GeoFilterName { get; set; }

    }
    public class TextFilterFields
    {
        public string FIeldName { get; set; }
        public double Weignt { get; set; } = 1;

    }
   
}
