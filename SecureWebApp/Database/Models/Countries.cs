using System.Collections.Generic;

namespace SecureWebApp.Database.Models
{
    public class Countries
    {

        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }

        public virtual ICollection<Cities> Cities { get; set; } = new HashSet<Cities>();
        public virtual ICollection<Users> Users { get; set; } = new HashSet<Users>();
    }
}
