using Moq;
using Xunit;
using FluentAssertions;
using MockQueryable.Moq;
using System.Linq;
using System.Threading.Tasks;
using SecureWebApp.Logic.Emails;
using SecureWebApp.UnitTests.Database;
using SecureWebApp.Infrastructure.Database;

namespace SecureWebApp.UnitTests
{

    public class LogicTest_Emails
    {

        private readonly Mock<MainDbContext> FMockDbContext;
        private readonly IEmails FEmails;

        public LogicTest_Emails() 
        {

            // Create instances to mocked all dependencies           
            FMockDbContext = new Mock<MainDbContext>();

            // Upload pre-fixed dummy data
            var LCountriesDbSet = DummyLoad.ReturnDummyCountries().AsQueryable().BuildMockDbSet();
            var LCitiesDbSet = DummyLoad.ReturnDummyCities().AsQueryable().BuildMockDbSet();
            var LUsersDbSet = DummyLoad.ReturnDummyUsers().AsQueryable().BuildMockDbSet();
            var LSigninHistory = DummyLoad.ReturnSigninHistory().AsQueryable().BuildMockDbSet();

            // Populate database tables with dummy data
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.Countries).Returns(LCountriesDbSet.Object);
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.Cities).Returns(LCitiesDbSet.Object);
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.Users).Returns(LUsersDbSet.Object);
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.SigninHistory).Returns(LSigninHistory.Object);

            // Create test instance with mocked dependencies
            FEmails = new Emails(FMockDbContext.Object);

        }

        [Theory]
        [InlineData("bob.dylan@gmail.com")]
        public void Should_ValidateEmailAddress(string AEmailAddress)
        {
            var LResult = FEmails.IsEmailAddressCorrect(AEmailAddress);
            LResult.Should().BeTrue();
        }

        [Theory]
        [InlineData("bob.dylan@gmail.com")]
        public async Task Should_ChcekEmailAddress(string AEmailAddress)
        {
            var LResult = await FEmails.IsEmailAddressExist(AEmailAddress);
            LResult.Should().BeTrue();
        }

        [Theory]
        [InlineData("tokan@dfds.com")]
        public async Task Should_CheckEmailDomain(string AEmailAddress)
        {
            var LResult = await FEmails.IsEmailDomainExist(AEmailAddress);
            LResult.Should().BeTrue();
        }

    }

}
