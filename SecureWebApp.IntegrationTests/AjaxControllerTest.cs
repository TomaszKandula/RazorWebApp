using Xunit;
using FluentAssertions;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SecureWebApp.Models.Json;
using SecureWebApp.IntegrationTests.CustomRest;
using SecureWebApp.IntegrationTests.Configuration;

namespace SecureWebApp.IntegrationTests
{

    public class AjaxControllerTest : IClassFixture<TestFixture<SecureWebApp.Startup>>
    {

        public class Startup 
        {        
        }

        private readonly HttpClient FHttpClient;

        public AjaxControllerTest(TestFixture<SecureWebApp.Startup> ACustomFixture)
        {
            FHttpClient = ACustomFixture.FClient;
        }

        [Theory]
        [InlineData("tokan@dfds.com")]
        public async Task CheckEmailAsync(string AEmailAddress) 
        {

            // Arrange
            var LEndpoint = $"/api/v1/ajax/validation/{AEmailAddress}";
            var LAuthString = "";

            // Act
            var LResult = new RestResponse();

            using (var LRestClient = new RestClient())
            {
                LRestClient.FHttpClient = FHttpClient;
                LResult = await LRestClient.Execute("get", LAuthString, LEndpoint, string.Empty);
            }

            var LDeserialized = JsonConvert.DeserializeObject<EmailValidation>(LResult.ResponseContent);

            // Assert
            LResult.StatusCode.Should().Be(200);
            LDeserialized.IsEmailValid.Should().BeFalse();

        }

        [Fact]
        public async Task ReturnCountryAsync() 
        {

            // Arrange
            var LEndpoint = $"/api/v1/ajax/countries/";
            var LAuthString = "";

            // Act
            var LResult = new RestResponse();

            using (var LRestClient = new RestClient())
            {
                LRestClient.FHttpClient = FHttpClient;
                LResult = await LRestClient.Execute("get", LAuthString, LEndpoint, string.Empty);
            }

            var LDeserialized = JsonConvert.DeserializeObject<ReturnCountryList>(LResult.ResponseContent);

            // Assert
            LResult.StatusCode.Should().Be(200);
            LDeserialized.Countries.Should().HaveCount(249);

        }

        [Theory]
        [InlineData(1)]
        public async Task ReturnCityAsync(int AId) 
        {

            // Arrange
            var LEndpoint = $"/api/v1/ajax/cities/{AId}";
            var LAuthString = "";

            // Act
            var LResult = new RestResponse();

            using (var LRestClient = new RestClient())
            {
                LRestClient.FHttpClient = FHttpClient;
                LResult = await LRestClient.Execute("get", LAuthString, LEndpoint, string.Empty);
            }

            var LDeserialized = JsonConvert.DeserializeObject<ReturnCityList>(LResult.ResponseContent);

            // Assert
            LResult.StatusCode.Should().Be(200);
            LDeserialized.Cities.Should().HaveCount(11);

        }

        [Fact]
        public async Task CreateAccountAsync()
        {

            // Arrange
            var LEndpoint = $"/api/v1/ajax/users/signup/";
            var LAuthString = "";

            var LPayLoad = new UserCreate()
            {
                FirstName    = "John",
                LastName     = "Deer",
                NickName     = "Deer",
                EmailAddress = "johnny.d@gmail.com",
                Password     = "johnny123456789",
                CityId       = 187,
                CountryId    = 47
            };

            var LSerializedPayLoad = JsonConvert.SerializeObject(LPayLoad);

            // Act
            var LResult = new RestResponse();

            using (var LRestClient = new RestClient())
            {
                LRestClient.FHttpClient = FHttpClient;
                LResult = await LRestClient.Execute("post", LAuthString, LEndpoint, LSerializedPayLoad);
            }

            var LDeserialized = JsonConvert.DeserializeObject<UserCreated>(LResult.ResponseContent);

            // Assert
            LResult.StatusCode.Should().Be(200);
            LDeserialized.IsUserCreated.Should().BeTrue();

        }

        [Fact]
        public async Task LogToAccountAsync()
        {

            // Arrange
            var LEndpoint = $"/api/v1/ajax/users/signin/";
            var LAuthString = "";

            var LPayLoad = new UserLogin()
            {
                EmailAddr = "tokan@dfds.com",
                Password = "Timex#099#"
            };

            var LSerializedPayLoad = JsonConvert.SerializeObject(LPayLoad);

            // Act
            var LResult = new RestResponse();

            using (var LRestClient = new RestClient())
            {
                LRestClient.FHttpClient = FHttpClient;
                LResult = await LRestClient.Execute("post", LAuthString, LEndpoint, LSerializedPayLoad);
            }

            var LDeserialized = JsonConvert.DeserializeObject<UserLogged>(LResult.ResponseContent);

            // Assert
            LResult.StatusCode.Should().Be(200);
            LDeserialized.IsLogged.Should().BeTrue();

        }

    }

}
