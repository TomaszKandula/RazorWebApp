using System.Text.Json.Serialization;

namespace RazorWebApp.Shared.Dto
{
    public class UserCreatedDto
    {
        [JsonPropertyName("IsUserCreated")]
        public bool IsUserCreated { get; set; }

        [JsonPropertyName("UserId")]
        public int UserId { get; set; }
    }
}
