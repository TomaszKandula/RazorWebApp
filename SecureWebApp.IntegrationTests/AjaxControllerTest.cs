using Xunit;
using FluentAssertions;
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

        private readonly ServiceProvider FServiceProvider;
        private readonly MainDbContext   FMainDbContext;
        private readonly IAppLogger      FAppLogger;
        private readonly IDnsLookup      FDnsLookup;

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
           
            LResult.Any().Should().BeTrue();


        }

        [Fact]
        public async Task GetCities_Test() 
        {

            var LResult = await FMainDbContext.Cities
                .AsNoTracking()
                .Select(R => R)
                .ToListAsync();

            LResult.Any().Should().BeTrue();

        }

        [Fact]
        public async Task GetCountries_Test() 
        {

            var LResult = await FMainDbContext.Countries
                .AsNoTracking()
                .Select(R => R)
                .ToListAsync();

            LResult.Any().Should().BeTrue();

        }

        [Fact]
        public async Task GetSigninHistory_Test() 
        {

            var LResult = await FMainDbContext.SigninHistory
                .AsNoTracking()
                .Select(R => R)
                .ToListAsync();

            LResult.Any().Should().BeTrue();

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

            // Assert
            Users.Id.Should().BeGreaterThan(1);
        
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

            // Assert
            SigninHistory.Id.Should().BeGreaterThan(1);

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
            LResult.Should().BeTrue();

        }

    }

}
