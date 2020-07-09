using System;

namespace SecureWebApp.Models.Database
{

    public class AuthDomains
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string ApiKey { get; set; }
        public DateTime? Maturity { get; set; }
    }

}
