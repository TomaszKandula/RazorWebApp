using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace RazorWebApp.IntegrationTests.Extractor
{
    public static class AntiForgeryTokenExtractor
    {
        public const string ANTI_FORGERY_FIELD_NAME = "AntiForgeryTokenField";

        public const string ANTI_FORGERY_COOKIE_NAME = "AntiForgeryTokenCookie";

        public static async Task<(string FieldValue, string CookieValue)> ExtractAntiForgeryValues(HttpResponseMessage AResponse)
        {
            var LCookie = ExtractAntiForgeryCookieValueFrom(AResponse);
            var LToken  = ExtractAntiForgeryToken(await AResponse.Content.ReadAsStringAsync());
            return (LToken, LCookie);
        }

        private static string ExtractAntiForgeryCookieValueFrom(HttpResponseMessage AResponse)
        {
            var LAntiForgeryCookie = AResponse.Headers.GetValues("Set-Cookie").FirstOrDefault(AHeaders => AHeaders.Contains(ANTI_FORGERY_COOKIE_NAME));
            if (LAntiForgeryCookie is null)
                throw new ArgumentException($@"Cookie '{ANTI_FORGERY_COOKIE_NAME}' not found in HTTP response", nameof(AResponse));

            return SetCookieHeaderValue.Parse(LAntiForgeryCookie).Value.ToString();
        }

        private static string ExtractAntiForgeryToken(string AHtmlBody)
        {
            const string DATA_TAG = "data-xsrf=\"";
            const int DATA_LEN = 155;

            var LDataValue = AHtmlBody.Substring(AHtmlBody.IndexOf(DATA_TAG, StringComparison.Ordinal) + DATA_TAG.Length, DATA_LEN);
            if (!string.IsNullOrWhiteSpace(LDataValue) && !string.IsNullOrEmpty(LDataValue)) 
                return LDataValue;

            throw new ArgumentException($@"Anti forgery token '{ANTI_FORGERY_FIELD_NAME}' not found in HTML", nameof(AHtmlBody));
        }
    }
}
