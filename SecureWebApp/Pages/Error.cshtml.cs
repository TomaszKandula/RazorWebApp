using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureWebApp.Extensions.AppLogger;

namespace SecureWebApp.Pages
{

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {

        private string RequestId { get; set; }
        private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        private readonly IAppLogger FAppLogger;

        public ErrorModel(IAppLogger AAppLogger)
        {
            FAppLogger = AAppLogger;
        }

        public IActionResult OnGet()
        {

            try 
            {

                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

                if (ShowRequestId)
                {
                    ViewData["RequestId"] = RequestId;
                    FAppLogger.LogError(string.Format("An error occurred while processing your request: {0}", RequestId));
                }
                else 
                {
                    ViewData["RequestId"] = "n/a.";
                }

                return Page();

            }
            catch (Exception E)
            {
                FAppLogger.LogFatality(string.Format("[ErrorModel.OnGet]: an error has been thrown: {0} ({1}).", E.Message, E.StackTrace));
                throw;
            }

        }

    }

}
