using System.Collections.Generic;
using System.Text.Json.Serialization;
using SecureWebApp.ViewModel;

namespace SecureWebApp.Controllers.Models
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
