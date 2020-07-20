using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureWebApp.Helpers;
using SecureWebApp.Extensions.AppLogger;

namespace SecureWebApp.Pages
{

    public class LoginModel : PageModel
    {

        private readonly IAppLogger FAppLogger;

        public LoginModel(IAppLogger AAppLogger)
        {
            FAppLogger = AAppLogger;
        }

        public IActionResult OnGet()
        {

            try 
            {

                var LoggedUser = HttpContext.Session.GetString(Constants.Sessions.KeyNames.LoggedUser);

                if (!string.IsNullOrEmpty(LoggedUser)) 
                {
                    return RedirectToPage("./Index");
                }

                return Page();

            }
            catch (Exception E)
            {
                var ErrorDesc = string.IsNullOrEmpty(E.InnerException?.Message) ? E.Message : $"{E.Message} ({ E.InnerException.Message}).";
                FAppLogger.LogFatality($"[LoginModel.OnGet]: an error has been thrown: {ErrorDesc}.");
                throw;
            }

        }

    }

}
