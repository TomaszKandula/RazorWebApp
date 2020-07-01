using System.Text.Json.Serialization;

namespace SecureWebApp.Models.Json
{
    
    public class EmailValidation
    {

        [JsonPropertyName("IsUnique")]
        public bool IsUnique { get; set; }

        [JsonPropertyName("Error")]
        public ErrorHandler Error { get; set; }

        public EmailValidation()
        {
            Error = new ErrorHandler();
        }

    }

}
