using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        [BindProperty]
        public List<CountryList> CountryList { get; set; }

        public RegisterModel(
            IAppLogger    AAppLogger,
            MainDbContext AMainDbContext
        )
        {
            FAppLogger     = AAppLogger;
            FMainDbContext = AMainDbContext;
        }

        public async Task<IActionResult> OnGet()
        {

            try
            {

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
