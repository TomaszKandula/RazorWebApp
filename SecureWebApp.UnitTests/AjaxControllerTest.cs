using Xunit;
using SecureWebApp.Controllers;
using SecureWebApp.Extensions.AppLogger;
using SecureWebApp.Extensions.DnsLookup;

namespace SecureWebApp.UnitTests
{

    public class Startup 
    {   
    }

    public class AjaxControllerTest
    {

        private readonly AjaxController FAjaxController;

        public AjaxControllerTest()
        {

            IAppLogger FAppLogger = new Mocks.AppLogger();
            IDnsLookup FDnsLookup = new Mocks.DnsLookup();

            FAjaxController = new AjaxController(
                null,
                FAppLogger,
                FDnsLookup
            );

        }

        [Theory]
        [InlineData("tokan@dfds.com")]
        [InlineData("tomasz.kandula@gmail.com")]
        public void IsEmailAddressCorrect_Test(string AEmailAddress)
        {
            var LResult = FAjaxController.IsEmailAddressCorrect(AEmailAddress);
            Assert.True(LResult, $"Email address '{AEmailAddress}' is malformed.");
        }

    }

}
