using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SecureWebApp.IntegrationTests.CustomRest
{

    public class RestClient : IDisposable
    {

        public HttpClient FHttpClient { get; set; }
        private HttpResponseMessage FResponse;

        public RestClient() 
        {
            FResponse = new HttpResponseMessage();
        }

        public void Dispose() 
        {
            FResponse.Dispose();        
        }

        public async Task<RestResponse> Execute(string AMethod, string AAuthString, string AEndpoint, string AJsonPayLoad) 
        {

            if (!string.IsNullOrEmpty(AAuthString)) 
                FHttpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-CSRF-TOKEN", AAuthString);

            switch (AMethod.ToUpper())
            {

                case "GET":
                    FResponse = await FHttpClient.GetAsync(AEndpoint);
                    break;

                case "DELETE":
                    FResponse = await FHttpClient.DeleteAsync(AEndpoint);
                    break;

                case "POST":
                    FResponse = await FHttpClient.PostAsync(
                        AEndpoint, new StringContent(AJsonPayLoad, System.Text.Encoding.Default, "application/json"));
                    break;

                case "PATCH":
                    FResponse = await FHttpClient.PatchAsync(
                        AEndpoint, new StringContent(AJsonPayLoad, System.Text.Encoding.Default, "application/json"));
                    break;

                case "PUT":
                    FResponse = await FHttpClient.PutAsync(
                        AEndpoint, new StringContent(AJsonPayLoad, System.Text.Encoding.Default, "application/json"));
                    break;

            }

            return new RestResponse
            {
                ResponseContent = await FResponse.Content.ReadAsStringAsync(),
                StatusCode = FResponse.StatusCode
            };

        }

    }

}
