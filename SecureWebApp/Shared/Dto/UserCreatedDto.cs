using System.Text.Json.Serialization;

namespace SecureWebApp.Shared.Dto
{
    public class UserCreatedDto
    {
        [JsonPropertyName("IsUserCreated")]
        public bool IsUserCreated { get; set; }

        [JsonPropertyName("UserId")]
        public int UserId { get; set; }

        [JsonPropertyName("Error")]
        public ErrorHandlerDto Error { get; set; } = new ErrorHandlerDto();
    }
}
