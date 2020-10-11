using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureWebApp.Helpers;
using SecureWebApp.AppLogger;

namespace SecureWebApp.Pages
{

    public class LoginModel : PageModel
    {

        private readonly IAppLogger FAppLogger;
        private readonly IAntiforgery FAntiforgery;

        public LoginModel(IAppLogger AAppLogger, IAntiforgery AAntiforgery)
        {
            FAppLogger = AAppLogger;
            FAntiforgery = AAntiforgery;
        }

        public IActionResult OnGet()
        {

            try 
            {

                ViewData["XCSRF"] = FAntiforgery.GetAndStoreTokens(HttpContext).RequestToken;
                var LLoggedUser = HttpContext.Session.GetString(Constants.Sessions.KeyNames.EmailAddr);

                if (!string.IsNullOrEmpty(LLoggedUser)) 
                {
                    return RedirectToPage("./Index");
                }

                return Page();

            }
            catch (Exception LException)
            {
                var LErrorDesc = string.IsNullOrEmpty(LException.InnerException?.Message) 
                    ? LException.Message 
                    : $"{LException.Message} ({ LException.InnerException.Message}).";
                FAppLogger.LogFatality($"[LoginModel.OnGet]: an error has been thrown: {LErrorDesc}.");
                throw;
            }

        }

    }

}
