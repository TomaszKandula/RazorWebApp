using System.Net;

namespace SecureWebApp.IntegrationTests.CustomRest
{

    public class RestResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseContent { get; set; }
    }

}
