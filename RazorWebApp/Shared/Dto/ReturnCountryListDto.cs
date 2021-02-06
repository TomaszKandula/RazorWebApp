using System.Collections.Generic;
using System.Text.Json.Serialization;
using RazorWebApp.ViewModel;

namespace RazorWebApp.Shared.Dto
{
    public class ReturnCountryListDto
    {
        [JsonPropertyName("Countries")]
        public List<CountryList> Countries { get; set; }
    }
}
