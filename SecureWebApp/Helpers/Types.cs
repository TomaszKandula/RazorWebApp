using System.Net;

namespace SecureWebApp.Helpers
{

    /// <summary>
    /// Customized return types for non-void methods.
    /// </summary>
    public class Types
    {

        public class AuthorizeResponse
        {
            public string SourceDomain { get; set; }
            public HttpStatusCode StatusCode { get; set; }
            public string LastMessage { get; set; }
        }

    }

}
