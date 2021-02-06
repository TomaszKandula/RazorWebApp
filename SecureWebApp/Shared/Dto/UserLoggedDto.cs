using System.Text.Json.Serialization;

namespace SecureWebApp.Shared.Dto
{
    public class UserLoggedDto
    {
        [JsonPropertyName("IsLogged")]
        public bool IsLogged { get; set; }
    }
}
