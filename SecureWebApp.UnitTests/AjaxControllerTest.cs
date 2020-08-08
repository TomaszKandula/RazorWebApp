using Moq;
using Xunit;
using MockQueryable.Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using SecureWebApp.Controllers;
using SecureWebApp.Models.Json;
using SecureWebApp.UnitTests.Mocks;
using SecureWebApp.Extensions.AppLogger;
using SecureWebApp.Extensions.DnsLookup;
using System.Threading;

namespace SecureWebApp.UnitTests
{

    public class Startup 
    {   
    }

    public class AjaxControllerTest
    {

        private readonly AjaxController FAjaxController;
        private readonly Mock<MainDbContext> FMockDbContext;

        public AjaxControllerTest()
        {

            // Create instances to mocked all dependencies           
            FMockDbContext = new Mock<MainDbContext>();
            IAppLogger FAppLogger = new Mocks.AppLogger();
            IDnsLookup FDnsLookup = new Mocks.DnsLookup();

            // Upload pre-fixed dummy data
            var CountriesDbSet = DummyData.ReturnDummyCountries().AsQueryable().BuildMockDbSet();
            var CitiesDbSet    = DummyData.ReturnDummyCities().AsQueryable().BuildMockDbSet();
            var UsersDbSet     = DummyData.ReturnDummyUsers().AsQueryable().BuildMockDbSet();
            var SigninHistory  = DummyData.ReturnSigninHistory().AsQueryable().BuildMockDbSet();

            // Populate database tables with dummy data
            FMockDbContext.Setup(R => R.Countries).Returns(CountriesDbSet.Object);
            FMockDbContext.Setup(R => R.Cities).Returns(CitiesDbSet.Object);
            FMockDbContext.Setup(R => R.Users).Returns(UsersDbSet.Object);
            FMockDbContext.Setup(R => R.SigninHistory).Returns(SigninHistory.Object);

            // Create test instance with mocked depenencies
            FAjaxController = new AjaxController(
                FMockDbContext.Object,
                FAppLogger,
                FDnsLookup
            );

        }

        [Theory]
        [InlineData("bob.dylan@gmail.com")]
        public void IsEmailAddressCorrect_Test(string AEmailAddress)
        {
            var LResult = FAjaxController.IsEmailAddressCorrect(AEmailAddress);
            Assert.True(LResult, $"Email address '{AEmailAddress}' is malformed.");
        }

        [Theory]
        [InlineData("bob.dylan@gmail.com")]
        public async Task IsEmailAddressExist_Test(string AEmailAddress)
        {
            var LResult = await FAjaxController.IsEmailAddressExist(AEmailAddress);
            Assert.True(LResult, $"Email address domain '{AEmailAddress}' does not exist.");
        }

        [Theory]
        [InlineData(1)]
        public async Task ReturnCityList_Test(int ACityId)
        {
            var LResult = await FAjaxController.ReturnCityList(ACityId);
            Assert.True(LResult.Any(), $"List is empty for CountryId = {ACityId}.");
        }

        [Fact]
        public async Task ReturnCountryList_Test()
        {
            var LResult = await FAjaxController.ReturnCountryList();
            Assert.True(LResult.Any(), "List is empty!");
        }

        [Theory]
        [InlineData("f.mercury@gmail.com", "ThisIsMyPassword$2020")]
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
        [InlineData(2, 3, "ester.exposito@gmail.com")]
        public async Task SignUp_Test(int ACountryId, int ACityId, string AEmailAddress)
        {

            // Arrange
            var PayLoad = new UserCreate()
            {
                FirstName    = "Ester",
                LastName     = "Expósito",
                NickName     = "Ester",
                EmailAddress = AEmailAddress,
                Password     = "ThisIsMyPassword$2020",
                CountryId    = ACityId,
                CityId       = ACountryId
            };

            // Act
            var LResult = await FAjaxController.SignUp(PayLoad, 12);

            // Verify
            FMockDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        }

    }

}
