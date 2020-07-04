using System.Text.Json.Serialization;

namespace SecureWebApp.Models.Json
{

    public class UserCreated
    {

        [JsonPropertyName("IsUserCreated")]
        public bool IsUserCreated { get; set; }

        [JsonPropertyName("UserId")]
        public int UserId { get; set; }

        [JsonPropertyName("Error")]
        public ErrorHandler Error { get; set; }

        public UserCreated()
        {
            Error = new ErrorHandler();
        }

    }

}
