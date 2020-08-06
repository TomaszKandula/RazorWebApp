using Xunit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SecureWebApp.Controllers;
using SecureWebApp.Models.Database;
using SecureWebApp.Extensions.AppLogger;
using SecureWebApp.Extensions.DnsLookup;

namespace SecureWebApp.UnitTests
{

    public class Startup 
    {   
    }

    public class DbFixture
    {

        public DbFixture()
        {
            var LServices = new ServiceCollection();
            LServices.AddDbContext<MainDbContext>(Options => Options
                .UseSqlServer("Server=.\\MSSQLSERVER19;Initial Catalog=TestDatabase;User ID=Tomek;Password=;Integrated Security=True;Connection Timeout=30;"), ServiceLifetime
                .Transient);
            FServiceProvider = LServices.BuildServiceProvider();
        }

        public ServiceProvider FServiceProvider { get; private set; }

    }

    public class AjaxControllerTest : IClassFixture<DbFixture>
    {

        private readonly AjaxController  FAjaxController;
        private readonly ServiceProvider FServiceProvider;

        public AjaxControllerTest(DbFixture ADbFixture)
        {

            FServiceProvider = ADbFixture.FServiceProvider;
            MainDbContext FMainDbContext = FServiceProvider.GetService<MainDbContext>();

            IAppLogger FAppLogger = new Mocks.AppLogger();
            IDnsLookup FDnsLookup = new Mocks.DnsLookup();

            FAjaxController = new AjaxController(
                FMainDbContext,
                FAppLogger,
                FDnsLookup
            );

        }

        [Theory]
        [InlineData("tokan@dfds.com")]
        [InlineData("tomasz.kandula@gmail.com")]
        public void IsEmailAddressCorrect_Test(string AEmailAddress)
        {
            var LResult = FAjaxController.IsEmailAddressCorrect(AEmailAddress);
            Assert.True(LResult, $"Email address '{AEmailAddress}' is malformed.");
        }

        [Theory]
        [InlineData("tokan@wp.pl")]
        [InlineData("tokan@dfds.com")]
        [InlineData("tokan@onet.pl")]
        public async Task IsEmailAddressExist_Test(string AEmailAddress)
        {
            var LResult = await FAjaxController.IsEmailAddressExist(AEmailAddress);
            Assert.True(LResult, $"Email address domain '{AEmailAddress}' does not exist.");
        }

    }

}
