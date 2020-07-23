using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureWebApp.Helpers;
using SecureWebApp.Extensions.AppLogger;

namespace SecureWebApp.Pages
{

    public class LogoutModel : PageModel
    {

        private readonly IAppLogger FAppLogger;

        public LogoutModel(IAppLogger AAppLogger)
        {
            FAppLogger = AAppLogger;
        }

        public IActionResult OnGet()
        {

            try 
            {

                var LSessionId = HttpContext.Session.GetString(Constants.Sessions.KeyNames.SessionId);

                if (!string.IsNullOrEmpty(LSessionId))
                {
                    HttpContext.Session.SetString(Constants.Sessions.KeyNames.SessionId, string.Empty);
                    HttpContext.Session.SetString(Constants.Sessions.KeyNames.EmailAddr, string.Empty);
                    HttpContext.Session.SetString(Constants.Sessions.KeyNames.ExpiresAt, string.Empty);
                }
                else 
                {
                    return RedirectToPage("./Index");
                }

                return Page();

            }
            catch (Exception E)
            {
                var ErrorDesc = string.IsNullOrEmpty(E.InnerException?.Message) ? E.Message : $"{E.Message} ({ E.InnerException.Message}).";
                FAppLogger.LogFatality($"[LogoutModel.OnGet]: an error has been thrown: {ErrorDesc}.");
                throw;
            }

        }

    }

}
