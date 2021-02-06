using System.Collections.Generic;
using System.Text.Json.Serialization;
using SecureWebApp.ViewModel;

namespace SecureWebApp.Shared.Dto
{
    public class ReturnCountryListDto
    {
        [JsonPropertyName("Countries")]
        public List<CountryList> Countries { get; set; }
    }
}
