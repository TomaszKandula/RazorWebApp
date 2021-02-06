using System.Text.Json.Serialization;

namespace RazorWebApp.Shared.Dto
{
    public class ReturnSessionStateDto
    {
        [JsonPropertyName("IsSessionAlive")]
        public bool IsSessionAlive { get; set; }
    }
}
