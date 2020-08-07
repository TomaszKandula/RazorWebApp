using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureWebApp.Models.Database;
using SecureWebApp.Extensions.DnsLookup;
using SecureWebApp.Extensions.AppLogger;

namespace SecureWebApp.IntegrationTests
{

    public class Startup
    {
    }

    public class DbFixture
    {

        public ServiceProvider FServiceProvider { get; private set; }

        public DbFixture()
        {

            var LConfiguration = new ConfigurationBuilder()
                        .AddUserSecrets<DbFixture>()
                        .Build();

            var ConnectionString = LConfiguration.GetValue<string>("DbConnect");

            var LServices = new ServiceCollection();

            LServices.AddSingleton<IDnsLookup, DnsLookup>();
            LServices.AddSingleton<IAppLogger, AppLogger>();
            LServices.AddDbContext<MainDbContext>(Options => Options.UseSqlServer(ConnectionString), ServiceLifetime.Transient);

            FServiceProvider = LServices.BuildServiceProvider();

        }

    }

    public class AjaxControllerTest : IClassFixture<DbFixture>
    {

        private ServiceProvider FServiceProvider;
        private MainDbContext   FMainDbContext;
        private IAppLogger      FAppLogger;
        private IDnsLookup      FDnsLookup;

        public AjaxControllerTest(DbFixture ADbFixture)
        {
            FServiceProvider = ADbFixture.FServiceProvider;
            FMainDbContext   = FServiceProvider.GetService<MainDbContext>();
            FAppLogger       = FServiceProvider.GetService<IAppLogger>();
            FDnsLookup       = FServiceProvider.GetService<IDnsLookup>();
        }

        /* TESTS FOR DATA RETRIEVAL */

        [Fact]
        public async Task GetUserList_Test()
        {
            
            var LResult = await FMainDbContext.Users
                .AsNoTracking()
                .Select(R => R)
                .ToListAsync();            
           
            Assert.True(LResult.Any(), "List is empty!");
        
        }

        [Fact]
        public async Task GetCities_Test() 
        {

            var LResult = await FMainDbContext.Cities
                .AsNoTracking()
                .Select(R => R)
                .ToListAsync();

            Assert.True(LResult.Any(), "List is empty!");

        }

        [Fact]
        public async Task GetCountries_Test() 
        {

            var LResult = await FMainDbContext.Countries
                .AsNoTracking()
                .Select(R => R)
                .ToListAsync();

            Assert.True(LResult.Any(), "List is empty!");

        }

        [Fact]
        public async Task GetSigninHistory_Test() 
        {

            var LResult = await FMainDbContext.SigninHistory
                .AsNoTracking()
                .Select(R => R)
                .ToListAsync();

            Assert.True(LResult.Any(), "List is empty!");

        }

        /* TESTS FOR DATA WRITING */

        [Theory]
        [InlineData(178, 80880)]
        public async Task TryToAddNewUser_Test(int ACountryId, int ACityId) 
        {

            // Arrange
            var Users = new Users() 
            { 
                FirstName   = "Bob",
                LastName    = "Dylan",
                NickName    = "Bob",
                EmailAddr   = "bob.dylan@gmail.com",
                PhoneNum    = null,
                Password    = "TestUnhashedPassword",
                CreatedAt   = DateTime.Now,
                IsActivated = false,
                CityId      = ACityId,
                CountryId   = ACountryId
            };

            // Act
            FMainDbContext.Users.Add(Users);
            await FMainDbContext.SaveChangesAsync();
            var NewUserId = Users.Id;

            // Assert
            Assert.NotEqual(0, NewUserId);
        
        }

        [Theory]
        [InlineData(1)]
        public async Task TryToAddToSigninHistory_Test(int UserId)
        {

            // Arrange
            var SigninHistory = new SigninHistory() 
            { 
                UserId   = UserId,
                LoggedAt = DateTime.Now
            };

            // Act
            FMainDbContext.SigninHistory.Add(SigninHistory);
            await FMainDbContext.SaveChangesAsync();
            var NewId = SigninHistory.Id;

            // Assert
            Assert.NotEqual(0, NewId);

        }

        /* GET FROM DATABASE AND CALL DNS LOOKUP */

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task CheckEmailDomain_Test(int UserId) 
        {

            // Arrange
            var LEmailAddress = (await FMainDbContext.Users
                .AsNoTracking()
                .Where(R => R.Id == UserId)
                .Select(R => R.EmailAddr)
                .ToListAsync())
                .Single();

            // Act
            var LResult = await FDnsLookup.IsDomainExist(LEmailAddress);

            // Assert
            Assert.True(LResult, "Email domain does not exist!");

        }

    }

}
