using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Logger;
using RazorWebApp.Exceptions;

namespace RazorWebApp.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        private string RequestId { get; set; }
        
        private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        
        private readonly IAppLogger FAppLogger;

        public ErrorModel(IAppLogger AAppLogger)
            => FAppLogger = AAppLogger;

        public IActionResult OnGet()
        {
            try 
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
                if (ShowRequestId)
                {
                    ViewData["RequestId"] = RequestId;
                    FAppLogger.LogError($"An error occurred while processing your request: {RequestId}");
                }
                else 
                {
                    ViewData["RequestId"] = "n/a.";
                }

                return Page();
            }
            catch (Exception LException)
            {
                FAppLogger.LogFatality(ControllerException.Handle(LException).ErrorDesc);
                throw;
            }
        }
    }
}
