using Moq;
using Xunit;
using FluentAssertions;
using MockQueryable.Moq;
using System.Linq;
using System.Threading.Tasks;
using RazorWebApp.UnitTests.Database;
using RazorWebApp.LogicContext.Emails;
using RazorWebApp.Infrastructure.Database;

namespace RazorWebApp.UnitTests
{
    public class EmailsTest
    {
        private readonly IEmails FEmails;

        public EmailsTest() 
        {
            // Create instances to mocked all dependencies           
            var LMockDbContext = new Mock<MainDbContext>();

            // Upload pre-fixed dummy data
            var LCountriesDbSet = DummyLoad.ReturnDummyCountries().AsQueryable().BuildMockDbSet();
            var LCitiesDbSet = DummyLoad.ReturnDummyCities().AsQueryable().BuildMockDbSet();
            var LUsersDbSet = DummyLoad.ReturnDummyUsers().AsQueryable().BuildMockDbSet();
            var LSigninHistory = DummyLoad.ReturnSigninHistory().AsQueryable().BuildMockDbSet();

            // Populate database tables with dummy data
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.Countries).Returns(LCountriesDbSet.Object);
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.Cities).Returns(LCitiesDbSet.Object);
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.Users).Returns(LUsersDbSet.Object);
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.SigninHistory).Returns(LSigninHistory.Object);

            // Create test instance with mocked dependencies
            FEmails = new Emails(LMockDbContext.Object);
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
        public async Task Should_CheckEmailAddress(string AEmailAddress)
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
