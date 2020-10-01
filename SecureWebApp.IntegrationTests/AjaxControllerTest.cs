using Xunit;
using Newtonsoft.Json;
using FluentAssertions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using SecureWebApp.Models.Json;
using SecureWebApp.IntegrationTests.Extractor;
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
            var LRegisterPageResponse = await FHttpClient.GetAsync("/register");
            var LAntiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(LRegisterPageResponse);

            // Act
            var LNewRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/ajax/validation/{AEmailAddress}");

            LNewRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, LAntiForgeryValues.CookieValue).ToString());
            LNewRequest.Headers.TryAddWithoutValidation(AntiForgeryTokenExtractor.AntiForgeryFieldName, LAntiForgeryValues.FieldValue);

            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            var LDeserialized = JsonConvert.DeserializeObject<EmailValidation>(LContent);

            // Assert
            LResponse.StatusCode.Should().Be(200);
            LDeserialized.IsEmailValid.Should().BeFalse();

        }

        [Fact]
        public async Task ReturnCountryAsync() 
        {

            // Arrange
            var LRegisterPageResponse = await FHttpClient.GetAsync("/register");
            var LAntiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(LRegisterPageResponse);

            // Act
            var LNewRequest = new HttpRequestMessage(HttpMethod.Get, "/api/v1/ajax/countries");

            LNewRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, LAntiForgeryValues.CookieValue).ToString());
            LNewRequest.Headers.TryAddWithoutValidation(AntiForgeryTokenExtractor.AntiForgeryFieldName, LAntiForgeryValues.FieldValue);

            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            var LDeserialized = JsonConvert.DeserializeObject<ReturnCountryList>(LContent);

            // Assert
            LResponse.StatusCode.Should().Be(200);
            LDeserialized.Countries.Should().HaveCount(249);

        }

        [Theory]
        [InlineData(1)]
        public async Task ReturnCityAsync(int AId) 
        {

            // Arrange
            var LRegisterPageResponse = await FHttpClient.GetAsync("/register");
            var LAntiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(LRegisterPageResponse);

            // Act
            var LNewRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/ajax/cities/{AId}");

            LNewRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, LAntiForgeryValues.CookieValue).ToString());
            LNewRequest.Headers.TryAddWithoutValidation(AntiForgeryTokenExtractor.AntiForgeryFieldName, LAntiForgeryValues.FieldValue);

            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            var LDeserialized = JsonConvert.DeserializeObject<ReturnCityList>(LContent);

            // Assert
            LResponse.StatusCode.Should().Be(200);
            LDeserialized.Cities.Should().HaveCount(11);

        }

        [Fact]
        public async Task CreateAccountAsync()
        {

            // Arrange
            var LRegisterPageResponse = await FHttpClient.GetAsync("/register");
            var LAntiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(LRegisterPageResponse);

            // Act
            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/ajax/users/signup/");

            LNewRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, LAntiForgeryValues.CookieValue).ToString());
            LNewRequest.Headers.TryAddWithoutValidation(AntiForgeryTokenExtractor.AntiForgeryFieldName, LAntiForgeryValues.FieldValue);

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

            LNewRequest.Content = new StringContent(JsonConvert.SerializeObject(LPayLoad), System.Text.Encoding.Default, "application/json");

            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            var LDeserialized = JsonConvert.DeserializeObject<UserCreated>(LContent);

            // Assert
            LResponse.StatusCode.Should().Be(200);
            LDeserialized.IsUserCreated.Should().BeFalse();

        }

        [Fact]
        public async Task LogToAccountAsync()
        {

            // Arrange
            var LRegisterPageResponse = await FHttpClient.GetAsync("/login");
            var LAntiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(LRegisterPageResponse);

            // Act
            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/ajax/users/signin/");

            LNewRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, LAntiForgeryValues.CookieValue).ToString());
            LNewRequest.Headers.TryAddWithoutValidation(AntiForgeryTokenExtractor.AntiForgeryFieldName, LAntiForgeryValues.FieldValue);

            var LPayLoad = new UserLogin()
            {
                EmailAddr = "tokan@dfds.com",
                Password  = "Timex#099#"
            };

            LNewRequest.Content = new StringContent(JsonConvert.SerializeObject(LPayLoad), System.Text.Encoding.Default, "application/json");

            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            var LDeserialized = JsonConvert.DeserializeObject<UserLogged>(LContent);

            // Assert
            LResponse.StatusCode.Should().Be(200);
            LDeserialized.IsLogged.Should().BeTrue();

        }

    }

}
