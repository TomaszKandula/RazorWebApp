using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureWebApp.Helpers;
using SecureWebApp.Models.Views;
using SecureWebApp.Models.Database;
using SecureWebApp.Extensions.AppLogger;

namespace SecureWebApp.Pages
{

    public class RegisterModel : PageModel
    {

        private readonly IAppLogger    FAppLogger;
        private readonly MainDbContext FMainDbContext;
        private readonly IAntiforgery  FAntiforgery;

        [BindProperty]
        public List<CountryList> CountryList { get; set; }

        public RegisterModel(IAppLogger AAppLogger, MainDbContext AMainDbContext, IAntiforgery AAntiforgery)
        {
            FAppLogger     = AAppLogger;
            FMainDbContext = AMainDbContext;
            FAntiforgery   = AAntiforgery;
        }

        public async Task<IActionResult> OnGet()
        {

            try
            {

                ViewData["XCSRF"] = FAntiforgery.GetAndStoreTokens(HttpContext).RequestToken;
                var LoggedUser = HttpContext.Session.GetString(Constants.Sessions.KeyNames.EmailAddr);

                if (!string.IsNullOrEmpty(LoggedUser))
                {
                    return RedirectToPage("./Index");
                }

                CountryList = await FMainDbContext.Countries.Select(R => new CountryList() 
                { 
                    Id   = R.Id, 
                    Name = R.CountryName 
                })
                .AsNoTracking()
                .ToListAsync();

                var HtmlList = "";
                foreach (var Country in CountryList)
                {
                    HtmlList += $"<option value = '{Country.Id}'>{Country.Name}</option>";
                }

                ViewData["CountryList"] = HtmlList;

                return Page();
            
            }
            catch (Exception E) 
            {
                var ErrorDesc = string.IsNullOrEmpty(E.InnerException?.Message) ? E.Message : $"{E.Message} ({ E.InnerException.Message}).";
                FAppLogger.LogFatality($"[RegisterModel.OnGet]: an error has been thrown: {ErrorDesc}.");
                throw;
            }

        }

    }

}
