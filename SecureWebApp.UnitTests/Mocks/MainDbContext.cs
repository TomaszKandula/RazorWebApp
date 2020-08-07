using Microsoft.EntityFrameworkCore;
using SecureWebApp.Models.Database;

namespace SecureWebApp.UnitTests.Mocks
{

    public class MainDbContext : DbContext
    {
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<SigninHistory> SigninHistory { get; set; }
    }

}
