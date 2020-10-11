using System.Collections.Generic;
using System.Text.Json.Serialization;
using SecureWebApp.Controllers.ViewModels;

namespace SecureWebApp.Controllers.Models
{

    public class ReturnCityList
    {

        [JsonPropertyName("Cities")]
        public List<CityList> Cities { get; set; }

        [JsonPropertyName("Error")]
        public ErrorHandler Error { get; set; }

        public ReturnCityList()
        {
            Error = new ErrorHandler();
        }

    }

}
