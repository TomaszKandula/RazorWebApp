using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Shared;
using RazorWebApp.Logger;
using RazorWebApp.Exceptions;

namespace RazorWebApp.Pages
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
            catch (Exception AException)
            {
                FAppLogger.LogFatality(ControllerException.Handle(AException).ErrorDesc);
                throw;
            }
        }
    }
}
