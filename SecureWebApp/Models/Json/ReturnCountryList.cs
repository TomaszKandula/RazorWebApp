using System.Collections.Generic;
using System.Text.Json.Serialization;
using SecureWebApp.Models.Views;

namespace SecureWebApp.Models.Json
{

    public class ReturnCountryList
    {

        [JsonPropertyName("Countries")]
        public List<CountryList> Countries { get; set; }

        [JsonPropertyName("Error")]
        public ErrorHandler Error { get; set; }

        public ReturnCountryList()
        {
            Error = new ErrorHandler();
        }

    }

}
