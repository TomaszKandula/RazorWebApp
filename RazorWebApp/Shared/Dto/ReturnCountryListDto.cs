using System.Collections.Generic;
using System.Text.Json.Serialization;
using RazorWebApp.Models;

namespace RazorWebApp.Shared.Dto
{
    public class ReturnCountryListDto
    {
        [JsonPropertyName("Countries")]
        public List<CountryList> Countries { get; set; }
    }
}
