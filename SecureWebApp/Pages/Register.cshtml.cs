using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SecureWebApp.Extensions.AppLogger;
using SecureWebApp.Models.Database;

namespace SecureWebApp.Pages
{

    public class RegisterModel : PageModel
    {

        private readonly IAppLogger    FAppLogger;
        private readonly MainDbContext FMainDbContext;

        [BindProperty]
        public List<string> CountryList { get; set; }

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
                CountryList = await FMainDbContext.Countries.Select(R => R.CountryName).ToListAsync();
                return Page();
            }
            catch (Exception E) 
            {
                FAppLogger.LogFatality("[RegisterModel.OnGet]: an error has been thrown: " + E.Message + " (" + E.StackTrace + ").");
                throw;
            }

        }

    }

}
