using System.Text.Json.Serialization;

namespace SecureWebApp.Shared.Dto
{   
    public class EmailValidationDto
    {
        [JsonPropertyName("IsEmailValid")]
        public bool IsEmailValid { get; set; }
    }
}
