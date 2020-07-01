using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
                return Page();
            }
            catch (Exception E) 
            {
                FAppLogger.LogFatality("[IndexModel.OnGet]: an error has been thrown: " + E.Message + " (" + E.StackTrace + ").");
                throw;
            }

        }

    }
}
