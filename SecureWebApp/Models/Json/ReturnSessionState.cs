using System.Text.Json.Serialization;

namespace SecureWebApp.Models.Json
{

    public class ReturnSessionState
    {

        [JsonPropertyName("IsSessionAlive")]
        public bool IsSessionAlive { get; set; }

        [JsonPropertyName("Error")]
        public ErrorHandler Error { get; set; }

        public ReturnSessionState()
        {
            Error = new ErrorHandler();
        }

    }

}
