using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureWebApp.Shared;
using SecureWebApp.Logger;
using SecureWebApp.ViewModel;
using SecureWebApp.Infrastructure.Database;
using SecureWebApp.Exceptions;

namespace SecureWebApp.Pages
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
                var LLoggedUser = HttpContext.Session.GetString(Constants.Sessions.KeyNames.EmailAddr);

                if (!string.IsNullOrEmpty(LLoggedUser))
                {
                    return RedirectToPage("./Index");
                }

                CountryList = await FMainDbContext.Countries.Select(ACountries => new CountryList() 
                { 
                    Id   = ACountries.Id, 
                    Name = ACountries.CountryName 
                })
                .AsNoTracking()
                .ToListAsync();

                var LHtmlList = "";
                foreach (var LCountry in CountryList)
                {
                    LHtmlList += $"<option value = '{LCountry.Id}'>{LCountry.Name}</option>";
                }

                ViewData["CountryList"] = LHtmlList;
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
