using System.Text.Json.Serialization;

namespace SecureWebApp.Shared.Dto
{
    public class ReturnSessionStateDto
    {
        [JsonPropertyName("IsSessionAlive")]
        public bool IsSessionAlive { get; set; }
    }
}
