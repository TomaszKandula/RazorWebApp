using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Logger;

namespace RazorWebApp.Pages
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
                var LErrorDesc = string.IsNullOrEmpty(LException.InnerException?.Message) 
                    ? LException.Message 
                    : $"{LException.Message} ({ LException.InnerException.Message}).";
                FAppLogger.LogFatality($"[ErrorModel.OnGet]: an error has been thrown: {LErrorDesc}.");
                throw;
            }
        }
    }
}
