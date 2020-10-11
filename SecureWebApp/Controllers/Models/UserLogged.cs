using System.Text.Json.Serialization;

namespace SecureWebApp.Controllers.Models
{

    public class UserLogged
    {

        [JsonPropertyName("IsLogged")]
        public bool IsLogged { get; set; }

        [JsonPropertyName("Error")]
        public ErrorHandler Error { get; set; }

        public UserLogged()
        {
            Error = new ErrorHandler();
        }

    }

}
