using Moq;
using Xunit;
using FluentAssertions;
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

            LResult.Should().BeTrue();

        }

        [Theory]
        [InlineData("bob.dylan@gmail.com")]
        public async Task IsEmailAddressExist_Test(string AEmailAddress)
        {

            var LResult = await FAjaxController.IsEmailAddressExist(AEmailAddress);

            LResult.Should().BeTrue();

        }

        [Theory]
        [InlineData(1)]
        public async Task ReturnCityList_Test(int ACityId)
        {

            var LResult = await FAjaxController.ReturnCityList(ACityId);

            LResult.Any().Should().BeTrue();

        }

        [Fact]
        public async Task ReturnCountryList_Test()
        {

            var LResult = await FAjaxController.ReturnCountryList();

            LResult.Any().Should().BeTrue();

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

            IsGuidEmpty.Should().BeFalse();
            LResult.Item2.Should().BeTrue();

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
            await FAjaxController.SignUp(PayLoad, 12);

            // Verify action
            FMockDbContext.Verify(R => R.SaveChangesAsync(CancellationToken.None), Times.Once);

        }

    }

}
