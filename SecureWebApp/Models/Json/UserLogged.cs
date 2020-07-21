using System;
using System.Text.Json.Serialization;

namespace SecureWebApp.Models.Json
{

    public class UserLogged
    {

        [JsonPropertyName("SessionId")]
        public Guid SessionId { get; set; }

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
