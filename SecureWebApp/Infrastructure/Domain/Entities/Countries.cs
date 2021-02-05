using System.Collections.Generic;

namespace SecureWebApp.Infrastructure.Domain.Entities
{
    public class Countries : Entity<int>
    {
        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public virtual ICollection<Cities> Cities { get; set; } = new HashSet<Cities>();

        public virtual ICollection<Users> Users { get; set; } = new HashSet<Users>();
    }
}
