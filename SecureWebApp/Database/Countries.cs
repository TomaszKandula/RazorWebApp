using System.Collections.Generic;

namespace SecureWebApp.Database
{
    public partial class Countries
    {
        public Countries()
        {
            Cities = new HashSet<Cities>();
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }

        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
