using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RazorWebApp.Infrastructure.Domain.Entities;

namespace RazorWebApp.Infrastructure.Database
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> AOptions) : base(AOptions)
        {
        }

        public MainDbContext() 
        {        
        }

        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<SigninHistory> SigninHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder AModelBuilder)
        {
            base.OnModelCreating(AModelBuilder);
            ApplyConfiguration(AModelBuilder);
        }

        protected void ApplyConfiguration(ModelBuilder AModelBuilder)
        {
            AModelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
