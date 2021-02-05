using System.Collections.Generic;

namespace SecureWebApp.Database.Models
{
    public class Cities
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        public string CityName { get; set; }

        public virtual Countries Country { get; set; }
        public virtual ICollection<Users> Users { get; set; } = new HashSet<Users>();
    }
}
