using System.Text.Json.Serialization;

namespace SecureWebApp.Controllers.Models
{
    
    public class EmailValidation
    {

        [JsonPropertyName("IsEmailValid")]
        public bool IsEmailValid { get; set; }

        [JsonPropertyName("Error")]
        public ErrorHandler Error { get; set; }

        public EmailValidation()
        {
            Error = new ErrorHandler();
        }

    }

}
