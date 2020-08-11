using Moq;
using Xunit;
using FluentAssertions;
using MockQueryable.Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SecureWebApp.Models.Json;
using SecureWebApp.UnitTests.Mocks;
using SecureWebApp.Logic.Emails;
using SecureWebApp.Logic.Accounts;
using SecureWebApp.Logic.Repository;

namespace SecureWebApp.UnitTests
{

    public class Startup 
    {   
    }

    public class AjaxControllerTest
    {

        private readonly Mock<MainDbContext> FMockDbContext;
        private readonly IAccounts   FAccounts;
        private readonly IEmails     FEmails;
        private readonly IRepository FRepository;

        public AjaxControllerTest()
        {

            // Create instances to mocked all dependencies           
            FMockDbContext = new Mock<MainDbContext>();

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
            FAccounts   = new Accounts(FMockDbContext.Object);
            FEmails     = new Emails(FMockDbContext.Object);
            FRepository = new Repository(FMockDbContext.Object);

        }

        [Theory]
        [InlineData("bob.dylan@gmail.com")]
        public void IsEmailAddressCorrect_Test(string AEmailAddress)
        {

            var LResult = FEmails.IsEmailAddressCorrect(AEmailAddress);

            LResult.Should().BeTrue();

        }

        [Theory]
        [InlineData("bob.dylan@gmail.com")]
        public async Task IsEmailAddressExist_Test(string AEmailAddress)
        {

            var LResult = await FEmails.IsEmailAddressExist(AEmailAddress);

            LResult.Should().BeTrue();

        }

        [Theory]
        [InlineData(1)]
        public async Task ReturnCityList_Test(int ACityId)
        {

            var LResult = await FRepository.ReturnCityList(ACityId);

            LResult.Any().Should().BeTrue();

        }

        [Fact]
        public async Task ReturnCountryList_Test()
        {

            var LResult = await FRepository.ReturnCountryList();

            LResult.Any().Should().BeTrue();

        }

        [Theory]
        [InlineData("f.mercury@gmail.com", "ThisIsMyPassword$2020")]
        public async Task SignIn_Test(string AEmailAddr, string APassword)
        {

            var LResult = await FAccounts.SignIn(AEmailAddr, APassword);

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
            await FAccounts.SignUp(PayLoad, 12);

            // Verify action
            FMockDbContext.Verify(R => R.SaveChangesAsync(CancellationToken.None), Times.Once);

        }

    }

}
