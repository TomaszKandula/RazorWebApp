using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace SecureWebApp.IntegrationTests.Extractor
{

    public static class AntiForgeryTokenExtractor
    {

        public static string AntiForgeryFieldName { get; } = "AntiForgeryTokenField";
        public static string AntiForgeryCookieName { get; } = "AntiForgeryTokenCookie";

        public static async Task<(string FieldValue, string CookieValue)> ExtractAntiForgeryValues(HttpResponseMessage AResponse)
        {
            var LCookie = ExtractAntiForgeryCookieValueFrom(AResponse);
            var LToken  = ExtractAntiForgeryToken(await AResponse.Content.ReadAsStringAsync());
            return (LToken, LCookie);
        }

        private static string ExtractAntiForgeryCookieValueFrom(HttpResponseMessage AResponse)
        {

            string LAntiForgeryCookie = AResponse.Headers.GetValues("Set-Cookie").FirstOrDefault(AHeaders => AHeaders.Contains(AntiForgeryCookieName));

            if (LAntiForgeryCookie is null)
            {
                throw new ArgumentException($"Cookie '{AntiForgeryCookieName}' not found in HTTP response", nameof(AResponse));
            }

            return SetCookieHeaderValue.Parse(LAntiForgeryCookie).Value.ToString();

        }

        private static string ExtractAntiForgeryToken(string AHtmlBody)
        {

            var LDataTag = "data-xsrf=\"";
            var LDataLen = 155;

            var LDataValue = AHtmlBody.Substring(AHtmlBody.IndexOf(LDataTag) + LDataTag.Length, LDataLen);

            if (!string.IsNullOrWhiteSpace(LDataValue) && !string.IsNullOrEmpty(LDataValue)) 
            {
                return LDataValue;
            }

            throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' not found in HTML", nameof(AHtmlBody));

        }

    }

}
