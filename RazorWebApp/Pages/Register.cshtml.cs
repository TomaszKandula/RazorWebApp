using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Shared;
using RazorWebApp.Logger;
using RazorWebApp.Models;
using RazorWebApp.Exceptions;
using RazorWebApp.Infrastructure.Database;

namespace RazorWebApp.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IAppLogger FAppLogger;
        
        private readonly MainDbContext FMainDbContext;

        private readonly IAntiforgery FAntiforgery;

        [BindProperty]
        public List<CountryList> CountryList { get; set; }

        public RegisterModel(IAppLogger AAppLogger, MainDbContext AMainDbContext, IAntiforgery AAntiforgery)
        {
            FAppLogger = AAppLogger;
            FMainDbContext = AMainDbContext;
            FAntiforgery = AAntiforgery;
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                ViewData["XCSRF"] = FAntiforgery.GetAndStoreTokens(HttpContext).RequestToken;
                var LLoggedUser = HttpContext.Session.GetString(Constants.Sessions.KeyNames.EMAIL_ADDRESS);

                if (!string.IsNullOrEmpty(LLoggedUser))
                    return RedirectToPage("./Index");

                CountryList = await FMainDbContext.Countries
                    .AsNoTracking()
                    .Select(ACountries => new CountryList
                    { 
                        Id   = ACountries.Id, 
                        Name = ACountries.CountryName 
                    })
                    .ToListAsync();

                var LHtmlList = CountryList
                    .Aggregate("", (ACurrent, ACountry) 
                        => ACurrent + $"<option value = '{ACountry.Id}'>{ACountry.Name}</option>");

                ViewData["CountryList"] = LHtmlList;
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
