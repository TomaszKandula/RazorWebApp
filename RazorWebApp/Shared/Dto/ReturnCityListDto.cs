using System.Collections.Generic;
using System.Text.Json.Serialization;
using RazorWebApp.ViewModel;

namespace RazorWebApp.Shared.Dto
{
    public class ReturnCityListDto
    {
        [JsonPropertyName("Cities")]
        public List<CityList> Cities { get; set; }
    }
}
