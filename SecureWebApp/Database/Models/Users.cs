using System;
using System.Collections.Generic;

namespace SecureWebApp.Database.Models
{
    public class Users
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        public int CityId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public string EmailAddr { get; set; }

        public string PhoneNum { get; set; }

        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActivated { get; set; }

        public virtual Cities City { get; set; }

        public virtual Countries Country { get; set; }

        public virtual ICollection<SigninHistory> SigninHistory { get; set; } = new HashSet<SigninHistory>();
    }
}
