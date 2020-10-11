using Moq;
using Xunit;
using FluentAssertions;
using MockQueryable.Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SecureWebApp.Logic.Emails;
using SecureWebApp.Logic.Accounts;
using SecureWebApp.UnitTests.Mocks;
using SecureWebApp.Logic.Repository;
using SecureWebApp.Controllers.Models;

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
            var LCountriesDbSet = DummyData.ReturnDummyCountries().AsQueryable().BuildMockDbSet();
            var LCitiesDbSet    = DummyData.ReturnDummyCities().AsQueryable().BuildMockDbSet();
            var LUsersDbSet     = DummyData.ReturnDummyUsers().AsQueryable().BuildMockDbSet();
            var LSigninHistory  = DummyData.ReturnSigninHistory().AsQueryable().BuildMockDbSet();

            // Populate database tables with dummy data
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.Countries).Returns(LCountriesDbSet.Object);
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.Cities).Returns(LCitiesDbSet.Object);
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.Users).Returns(LUsersDbSet.Object);
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.SigninHistory).Returns(LSigninHistory.Object);

            // Create test instance with mocked dependencies
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
        [InlineData("tokan@dfds.com")]
        public async Task IsEmailDomainExist(string AEmailAddress)
        {
            var LResult = await FEmails.IsEmailDomainExist(AEmailAddress);
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
            var LIsGuidEmpty = LResult.Item1 == Guid.Empty;

            LIsGuidEmpty.Should().BeFalse();
            LResult.Item2.Should().BeTrue();

        }

        [Theory]
        [InlineData(2, 3, "ester.exposito@gmail.com")]
        public async Task SignUp_Test(int ACountryId, int ACityId, string AEmailAddress)
        {

            // Arrange
            var LPayLoad = new UserCreate
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
            await FAccounts.SignUp(LPayLoad, 12);

            // Verify action
            FMockDbContext.Verify(AMainDbContext => AMainDbContext.SaveChangesAsync(CancellationToken.None), Times.Once);

        }

    }

}
