using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SecureWebApp.Extensions.AppLogger;
using SecureWebApp.Models.Database;

namespace SecureWebApp.Pages
{

    public class IndexModel : PageModel
    {

        private readonly IAppLogger    FAppLogger;
        private readonly MainDbContext FMainDbContext;

        public IndexModel(
            IAppLogger    AAppLogger,
            MainDbContext AMainDbContext
        )
        {
            FAppLogger     = AAppLogger;
            FMainDbContext = AMainDbContext;
        }

        public IActionResult OnGet()
        {



            return Page();

        }

    }
}
