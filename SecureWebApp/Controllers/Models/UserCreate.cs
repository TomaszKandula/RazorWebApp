using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SecureWebApp.Controllers.Models
{

    public class UserCreate
    {

        [JsonPropertyName("FirstName")]
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        [JsonPropertyName("NickName")]
        [Required]
        [StringLength(255)]
        public string NickName { get; set; }

        [JsonPropertyName("EmailAddress")]
        [Required]
        [StringLength(255)]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("Password")]
        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [JsonPropertyName("CountryId")]
        [Required]
        [Range(1, 1000000)]
        public int CountryId { get; set; }

        [JsonPropertyName("CityId")]
        [Required]
        [Range(1, 1000000)]
        public int CityId { get; set; }

    }

}
