using System.Collections.Generic;

namespace CSVProcessor
{
    public interface ICsvLoader
    {
        CityAddressCollecton ProcessDataCsv(string filepath);
        List<AddressDTO> ProcessDataToDictionary(CityAddressCollecton data);

    }
}