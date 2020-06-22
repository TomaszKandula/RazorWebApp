using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SecureWebApp.Pages.Service
{

    [AllowAnonymous]
    public class AjaxModel : PageModel
    {

        public AjaxModel()
        {
            //
        }

    }
}
