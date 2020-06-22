namespace SecureWebApp.Models.Database
{

    public partial class Users
    {

        public int Id { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string EmailAddr { get; set; }
        public string PhoneNum { get; set; }

        public virtual Cities City { get; set; }
        public virtual Countries Country { get; set; }

    }

}
