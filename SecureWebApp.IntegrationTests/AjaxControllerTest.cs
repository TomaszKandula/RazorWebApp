using Xunit;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureWebApp.Models.Database;
using SecureWebApp.Extensions.DnsLookup;
using SecureWebApp.Extensions.AppLogger;

namespace SecureWebApp.IntegrationTests
{

    public class Startup
    {
    }

    public class DbFixture
    {

        public ServiceProvider FServiceProvider { get; private set; }

        public DbFixture()
        {

            var LConfiguration = new ConfigurationBuilder()
                        .AddUserSecrets<DbFixture>()
                        .Build();

            var ConnectionString = LConfiguration.GetValue<string>("DbConnect");

            var LServices = new ServiceCollection();

            LServices.AddSingleton<IDnsLookup, DnsLookup>();
            LServices.AddSingleton<IAppLogger, AppLogger>();
            LServices.AddDbContext<MainDbContext>(Options => Options.UseSqlServer(ConnectionString), ServiceLifetime.Transient);

            FServiceProvider = LServices.BuildServiceProvider();

        }

    }

    public class AjaxControllerTest : IClassFixture<DbFixture>
    {

        private ServiceProvider FServiceProvider;
        private MainDbContext   FMainDbContext;
        private IAppLogger      FAppLogger;
        private IDnsLookup      FDnsLookup;

        public AjaxControllerTest(DbFixture ADbFixture)
        {
            FServiceProvider = ADbFixture.FServiceProvider;
            FMainDbContext   = FServiceProvider.GetService<MainDbContext>();
            FAppLogger       = FServiceProvider.GetService<IAppLogger>();
            FDnsLookup       = FServiceProvider.GetService<IDnsLookup>();
        }

        /* TEST DATA RETRIEVAL */

        [Fact]
        public async Task GetUserList_Test()
        {
            var LResult = await FMainDbContext.Users.Select(R => R).ToListAsync();
            Assert.True(LResult.Any(), "List is empty!");
        }

        [Fact]
        public async Task GetCities_Test() 
        {
            var LResult = await FMainDbContext.Cities.Select(R => R).ToListAsync();
            Assert.True(LResult.Any(), "List is empty!");
        }

        [Fact]
        public async Task GetCountries_Test() 
        {
            var LResult = await FMainDbContext.Countries.Select(R => R).ToListAsync();
            Assert.True(LResult.Any(), "List is empty!");
        }

        [Fact]
        public async Task GetSigninHistory_Test() 
        {
            var LResult = await FMainDbContext.SigninHistory.Select(R => R).ToListAsync();
            Assert.True(LResult.Any(), "List is empty!");
        }

        /* TEST DNS LOOKUP */

        [Theory]
        [InlineData("tomasz.kandula@gmail.com")]
        public async Task CheckEmailDomain_Test(string AEmailAddress) 
        {
            var LResult = await FDnsLookup.IsDomainExist(AEmailAddress);
            Assert.True(LResult, "Email domain does not exist!");       
        }

    }

}
