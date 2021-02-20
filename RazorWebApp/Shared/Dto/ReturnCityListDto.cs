using System.Collections.Generic;
using System.Text.Json.Serialization;
using RazorWebApp.Models;

namespace RazorWebApp.Shared.Dto
{
    public class ReturnCityListDto
    {
        [JsonPropertyName("Cities")]
        public List<CityList> Cities { get; set; }
    }
}
