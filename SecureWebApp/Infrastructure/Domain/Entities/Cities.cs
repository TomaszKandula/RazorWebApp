using System.Collections.Generic;

namespace SecureWebApp.Infrastructure.Domain.Entities
{
    public class Cities : Entity<int>
    {
        public int CountryId { get; set; }


        public string CityName { get; set; }

        public virtual Countries Country { get; set; }

        public virtual ICollection<Users> Users { get; set; } = new HashSet<Users>();
    }
}
