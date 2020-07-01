using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecureWebApp.Models.Json;
using SecureWebApp.Models.Database;
using SecureWebApp.Extensions.AppLogger;

namespace SecureWebApp.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AjaxController : Controller
    {

        private readonly IAppLogger    FAppLogger;
        private readonly MainDbContext FMainDbContext;

        public AjaxController(
            IAppLogger    AAppLogger,
            MainDbContext AMainDbContext
        )
        {
            FAppLogger     = AAppLogger;
            FMainDbContext = AMainDbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailaddress"></param>
        /// <returns></returns>
        // GET api/v1/ajax/validation/{emailaddress}/
        [HttpGet("validation/{emailaddress}")]
        public async Task<IActionResult> CheckEmailAsync(string emailaddress)
        {

            var EResponse = new EmailValidation();
            try 
            {

                EResponse.IsUnique = true;
                return StatusCode(200, EResponse);

            } 
            catch (Exception E)
            {
                EResponse.Error.ErrorCode = E.HResult.ToString();
                EResponse.Error.ErrorDesc = E.Message;
                FAppLogger.LogFatality("GET api/v1/ajax/validation/" + emailaddress + "/ | Error has been raised while processing request. Message: " + E.Message);
                return StatusCode(500, EResponse);
            }

        }

    }

}
