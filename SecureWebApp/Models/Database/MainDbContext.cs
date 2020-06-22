using Microsoft.EntityFrameworkCore;
using SecureWebApp.Extensions.ConnectionService;

namespace SecureWebApp.Models.Database
{

    public class MainDbContext : DbContext
    {
        
        private readonly IConnectionService FConnectionService;

        public MainDbContext(DbContextOptions<MainDbContext> options, IConnectionService AConnectionService) : base(options)
        {
            FConnectionService = AConnectionService;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder AOptionsBuilder)
        {

            var ConnectionString = FConnectionService.GetMainDatabase();

            /// <seealso cref="https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency"/>
            AOptionsBuilder.UseSqlServer(ConnectionString, AddOptions =>
                    AddOptions.EnableRetryOnFailure());

        }



    }

}
