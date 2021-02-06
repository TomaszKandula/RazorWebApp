using System.Collections.Generic;
using System.Text.Json.Serialization;
using SecureWebApp.ViewModel;

namespace SecureWebApp.Shared.Dto
{
    public class ReturnCityListDto
    {
        [JsonPropertyName("Cities")]
        public List<CityList> Cities { get; set; }

        [JsonPropertyName("Error")]
        public ErrorHandlerDto Error { get; set; } = new ErrorHandlerDto();
    }
}
