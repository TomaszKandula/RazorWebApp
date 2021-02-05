using Moq;
using Xunit;
using FluentAssertions;
using MockQueryable.Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SecureWebApp.Database;
using SecureWebApp.Logic.Accounts;
using SecureWebApp.Controllers.Models;
using SecureWebApp.UnitTests.Database;

namespace SecureWebApp.UnitTests
{
    public class LogicTest_Accounts
    {
        private readonly Mock<MainDbContext> FMockDbContext;
        private readonly IAccounts   FAccounts;

        public LogicTest_Accounts()
        {
            // Create instances to mocked all dependencies           
            FMockDbContext = new Mock<MainDbContext>();

            // Upload pre-fixed dummy data
            var LCountriesDbSet = DummyLoad.ReturnDummyCountries().AsQueryable().BuildMockDbSet();
            var LCitiesDbSet    = DummyLoad.ReturnDummyCities().AsQueryable().BuildMockDbSet();
            var LUsersDbSet     = DummyLoad.ReturnDummyUsers().AsQueryable().BuildMockDbSet();
            var LSigninHistory  = DummyLoad.ReturnSigninHistory().AsQueryable().BuildMockDbSet();

            // Populate database tables with dummy data
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.Countries).Returns(LCountriesDbSet.Object);
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.Cities).Returns(LCitiesDbSet.Object);
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.Users).Returns(LUsersDbSet.Object);
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.SigninHistory).Returns(LSigninHistory.Object);

            // Create test instance with mocked dependencies
            FAccounts   = new Accounts(FMockDbContext.Object);
        }

        [Theory]
        [InlineData("f.mercury@gmail.com", "ThisIsMyPassword$2020")]
        public async Task Should_SignIn(string AEmailAddr, string APassword)
        {
            var LResult = await FAccounts.SignIn(AEmailAddr, APassword);
            var LIsGuidEmpty = LResult.Item1 == Guid.Empty;

            LIsGuidEmpty.Should().BeFalse();
            LResult.Item2.Should().BeTrue();
        }

        [Theory]
        [InlineData(2, 3, "ester.exposito@gmail.com")]
        public async Task Should_SignUp(int ACountryId, int ACityId, string AEmailAddress)
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
