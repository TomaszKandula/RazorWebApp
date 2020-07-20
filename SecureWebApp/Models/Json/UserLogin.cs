using System.Text.Json.Serialization;

namespace SecureWebApp.Models.Json
{

    public class UserLogin
    {

        [JsonPropertyName("EmailAddr")]
        public string EmailAddr { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }

    }

}
