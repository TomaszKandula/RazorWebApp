using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecureWebApp.Extensions.AppLogger;
using SecureWebApp.Models.Views;
using SecureWebApp.Models.Database;

namespace SecureWebApp.Pages
{

    public class RegisterModel : PageModel
    {

        private readonly IAppLogger FAppLogger;
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
                FAppLogger.LogFatality(string.Format("[RegisterModel.OnGet]: an error has been thrown: {0} ({1}).", E.Message, E.StackTrace));
                throw;
            }

        }

    }

}
