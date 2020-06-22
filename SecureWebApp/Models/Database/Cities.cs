using System.Collections.Generic;

namespace SecureWebApp.Models.Database
{

    public partial class Cities
    {

        public Cities()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string CityName { get; set; }

        public virtual Countries Country { get; set; }
        public virtual ICollection<Users> Users { get; set; }

    }

}
