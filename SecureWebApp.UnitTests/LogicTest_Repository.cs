using Moq;
using Xunit;
using FluentAssertions;
using MockQueryable.Moq;
using System.Linq;
using System.Threading.Tasks;
using SecureWebApp.Database;
using SecureWebApp.Logic.Repository;
using SecureWebApp.UnitTests.Database;

namespace SecureWebApp.UnitTests
{
    public class LogicTest_Repository
    {
        private readonly Mock<MainDbContext> FMockDbContext;
        private readonly IRepository FRepository;

        public LogicTest_Repository()
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
            FRepository = new Repository(FMockDbContext.Object);
        }

        [Theory]
        [InlineData(1)]
        public async Task Should_ReturnCities(int ACountryId)
        {
            var LResult = await FRepository.ReturnCityList(ACountryId);
            LResult.Should().HaveCount(1);
        }

        [Fact]
        public async Task Should_ReturnCountries()
        {
            var LResult = await FRepository.ReturnCountryList();
            LResult.Any().Should().BeTrue();
        }
    }
}
