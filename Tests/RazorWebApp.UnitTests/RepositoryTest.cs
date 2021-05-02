using Moq;
using Xunit;
using FluentAssertions;
using MockQueryable.Moq;
using System.Linq;
using System.Threading.Tasks;
using RazorWebApp.UnitTests.Database;
using RazorWebApp.LogicContext.Repository;
using RazorWebApp.Infrastructure.Database;

namespace RazorWebApp.UnitTests
{
    public class RepositoryTest
    {
        private readonly IRepository FRepository;

        public RepositoryTest()
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
            FRepository = new Repository(LMockDbContext.Object);
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
