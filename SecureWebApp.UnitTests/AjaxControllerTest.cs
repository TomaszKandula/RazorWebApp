using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SecureWebApp.Controllers;
using SecureWebApp.Models.Json;
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
                .UseSqlServer(""), ServiceLifetime
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

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(240)]
        public async Task ReturnCityList_Test(int AId)
        {
            var LResult = await FAjaxController.ReturnCityList(AId);
            Assert.True(LResult.Any(), $"List is empty for CountryId = {AId}.");
        }

        [Theory]
        [InlineData("john.kowalski@wp.pl", "ThisIsMyPassword$2020")]
        [InlineData("tokan@dfds.com", "Timex")]
        public async Task SignIn_Test(string AEmailAddr, string APassword)
        {

            var LResult = await FAjaxController.SignIn(AEmailAddr, APassword);

            var IsGuidEmpty = false;

            if (LResult.Item1 == Guid.Empty) 
            {
                IsGuidEmpty = true;
            }

            Assert.False(IsGuidEmpty, $"Cannot login with '{AEmailAddr}' and '{APassword}'. Session cannot be created.");          
            Assert.True(LResult.Item2, $"Cannot login with '{AEmailAddr}' and '{APassword}'. Account is inactive.");

        }

        [Theory]
        [InlineData(178, 80880, "john.kowalski@wp.pl")]
        [InlineData(178, 80880, "john.kowalski@onet.pl")]
        public async Task SignUp_Test(int ACountryId, int ACityId, string AEmailAddress) 
        {

            // Arrange
            var PayLoad = new UserCreate() 
            { 
                FirstName    = "John",
                LastName     = "Kowalski",
                NickName     = "Koval",
                EmailAddress = AEmailAddress,
                Password     = "ThisIsMyPassword$2020",
                CountryId    = ACityId,
                CityId       = ACountryId
            };

            // Act
            var LResult = await FAjaxController.SignUp(PayLoad, 12);

            // Assert
            Assert.InRange(LResult, 1, 255);

        }

    }

}
