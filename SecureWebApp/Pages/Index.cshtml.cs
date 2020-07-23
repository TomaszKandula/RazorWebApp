using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureWebApp.Helpers;
using SecureWebApp.Extensions.AppLogger;

namespace SecureWebApp.Pages
{

    public class IndexModel : PageModel
    {

        private readonly IAppLogger FAppLogger;

        public IndexModel(IAppLogger AAppLogger)
        {
            FAppLogger = AAppLogger;
        }

        public IActionResult OnGet()
        {

            try 
            {
                var LSessionId = HttpContext.Session.GetString(Constants.Sessions.KeyNames.SessionId);
                var LExpiresAt = HttpContext.Session.GetString(Constants.Sessions.KeyNames.ExpiresAt);

                if (!string.IsNullOrEmpty(LSessionId)) 
                {
                    
                    var LCookieOptions = new CookieOptions()
                    {
                        Path        = "/",
                        Expires     = DateTime.Parse(LExpiresAt),
                        IsEssential = false,
                        SameSite    = SameSiteMode.Strict,
                        HttpOnly    = true,
                        Secure      = false,//Set to true before deployment on HTTPS
                    };

                    /* 
                     * We add short-lived HttpOnly cookie containing Session Id to the response header. 
                     * This token would be later necessary to request data from API for logged user. 
                     * Such API request must contain Anit-Forgery token along with Session Id in the request header. 
                     * If session expires, then any requests to the API will fail and user will have to login again. 
                     * Please note that Session Id is randomly generated key with short lifespan; and it is restricted 
                     * to this domain only. Thus, if CORS is enabled, make sure it allows only certain domain, 
                     * on which front-end runs. 
                     */

                    HttpContext.Response.Cookies.Append("SessionId", LSessionId, LCookieOptions);

                }

                return Page();

            }
            catch (Exception E) 
            {
                var ErrorDesc = string.IsNullOrEmpty(E.InnerException?.Message) ? E.Message : $"{E.Message} ({ E.InnerException.Message}).";
                FAppLogger.LogFatality($"[IndexModel.OnGet]: an error has been thrown: {ErrorDesc}.");
                throw;
            }

        }

    }
}
