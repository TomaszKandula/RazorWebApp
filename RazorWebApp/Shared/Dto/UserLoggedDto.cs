using System.Text.Json.Serialization;

namespace RazorWebApp.Shared.Dto
{
    public class UserLoggedDto
    {
        [JsonPropertyName("IsLogged")]
        public bool IsLogged { get; set; }
    }
}
