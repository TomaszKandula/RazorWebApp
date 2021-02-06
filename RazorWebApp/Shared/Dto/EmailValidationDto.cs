using System.Text.Json.Serialization;

namespace RazorWebApp.Shared.Dto
{   
    public class EmailValidationDto
    {
        [JsonPropertyName("IsEmailValid")]
        public bool IsEmailValid { get; set; }
    }
}
