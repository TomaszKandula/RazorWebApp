using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SecureWebApp.Logic;
using SecureWebApp.Helpers;
using SecureWebApp.Models.Json;
using SecureWebApp.Extensions.AppLogger;

namespace SecureWebApp.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "ResponseCache")]
    public class AjaxController : Controller
    {

        private readonly ILogicContext FLogicContext;
        private readonly IAppLogger FAppLogger;

        public AjaxController(ILogicContext ALogicContext, IAppLogger AAppLogger) 
        {
            FLogicContext = ALogicContext;
            FAppLogger = AAppLogger;
        }

        /// <summary>
        /// Endpoint validating supplied email address. It checks format and email domain, and if it is already registered in database.
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <returns></returns>
        // GET api/v1/ajax/validation/{emailaddress}/
        [ValidateAntiForgeryToken]
        [HttpGet("validation/{emailaddress}")]
        public async Task<IActionResult> CheckEmailAsync(string EmailAddress)
        {

            var LResponse = new EmailValidation();
            try 
            {

                if (!FLogicContext.Emails.IsEmailAddressCorrect(EmailAddress)) 
                {
                    LResponse.Error.ErrorCode = Constants.Errors.EmailAddressMalformed.ErrorCode;
                    LResponse.Error.ErrorDesc = Constants.Errors.EmailAddressMalformed.ErrorDesc;
                    FAppLogger.LogWarn($"GET api/v1/ajax/validation/{EmailAddress}. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(200, LResponse);
                }
               
                if (await FLogicContext.Emails.IsEmailAddressExist(EmailAddress)) 
                {
                    LResponse.Error.ErrorCode = Constants.Errors.EmailAlreadyExists.ErrorCode;
                    LResponse.Error.ErrorDesc = Constants.Errors.EmailAlreadyExists.ErrorDesc;
                    FAppLogger.LogWarn($"GET api/v1/ajax/validation/{EmailAddress}. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(200, LResponse);
                }

                if (!await FLogicContext.Emails.IsEmailDomainExist(EmailAddress)) 
                {
                    LResponse.Error.ErrorCode = Constants.Errors.EmailDomainNotExist.ErrorCode;
                    LResponse.Error.ErrorDesc = Constants.Errors.EmailDomainNotExist.ErrorDesc;
                    FAppLogger.LogWarn($"GET api/v1/ajax/validation/{EmailAddress}. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(200, LResponse);
                }

                LResponse.IsEmailValid = true;
                return StatusCode(200, LResponse);

            } 
            catch (Exception E)
            {
                LResponse.Error.ErrorCode = E.HResult.ToString();
                LResponse.Error.ErrorDesc = string.IsNullOrEmpty(E.InnerException?.Message) ? E.Message : $"{E.Message} ({ E.InnerException.Message}).";
                FAppLogger.LogFatality($"GET api/v1/ajax/validation/{EmailAddress} | Error has been raised while processing request. Message: {LResponse.Error.ErrorDesc}.");
                return StatusCode(500, LResponse);
            }

        }

        /// <summary>
        /// Endpoint returning list of countries in JSON format.
        /// </summary>
        /// <returns></returns>
        // GET api/v1/ajax/countries/
        [ValidateAntiForgeryToken]
        [HttpGet("countries")]
        public async Task<IActionResult> ReturnCountryAsync()
        {

            var LResponse = new ReturnCountryList();
            try
            {
                LResponse.Countries = await FLogicContext.Repository.ReturnCountryList();
                return StatusCode(200, LResponse);
            }
            catch (Exception E)
            {
                LResponse.Error.ErrorCode = E.HResult.ToString();
                LResponse.Error.ErrorDesc = string.IsNullOrEmpty(E.InnerException?.Message) ? E.Message : $"{E.Message} ({ E.InnerException.Message}).";
                FAppLogger.LogFatality($"GET api/v1/ajax/countries/ | Error has been raised while processing request. Message: {LResponse.Error.ErrorDesc}.");
                return StatusCode(500, LResponse);
            }

        }

        /// <summary>
        /// Endpoint returning list of cities for given Country Id in JSON format.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        // GET api/v1/ajax/cities/{id}/
        [ValidateAntiForgeryToken]
        [HttpGet("cities/{id}")]
        public async Task<IActionResult> ReturnCityAsync(int Id) 
        {

            var LResponse = new ReturnCityList();
            try 
            {
                LResponse.Cities = await FLogicContext.Repository.ReturnCityList(Id);
                return StatusCode(200, LResponse);
            } 
            catch (Exception E)
            {
                LResponse.Error.ErrorCode = E.HResult.ToString();
                LResponse.Error.ErrorDesc = string.IsNullOrEmpty(E.InnerException?.Message) ? E.Message : $"{E.Message} ({ E.InnerException.Message}).";
                FAppLogger.LogFatality($"GET api/v1/ajax/cities/{Id} | Error has been raised while processing request. Message: {LResponse.Error.ErrorDesc}.");
                return StatusCode(500, LResponse);
            }

        }

        /// <summary>
        /// Endpoint adding new user to the database.
        /// </summary>
        /// <param name="PayLoad"></param>
        /// <returns></returns>
        // POST api/v1/ajax/users/signup/
        [ValidateAntiForgeryToken]
        [HttpPost("users/signup")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] UserCreate PayLoad) 
        {

            var LResponse = new UserCreated();
            try
            {

                if (!ModelState.IsValid) 
                {
                    LResponse.Error.ErrorCode = Constants.Errors.InvalidPayLoad.ErrorCode;
                    LResponse.Error.ErrorDesc = Constants.Errors.InvalidPayLoad.ErrorDesc;
                    FAppLogger.LogError($"POST api/v1/ajax/users/signup/. {LResponse.Error.ErrorDesc}.");
                    LResponse.IsUserCreated = false;
                    return StatusCode(200, LResponse);
                }

                if (await FLogicContext.Emails.IsEmailAddressExist(PayLoad.EmailAddress))
                {
                    LResponse.Error.ErrorCode = Constants.Errors.EmailAlreadyExists.ErrorCode;
                    LResponse.Error.ErrorDesc = Constants.Errors.EmailAlreadyExists.ErrorDesc;
                    FAppLogger.LogWarn($"POST api/v1/ajax/users/signup/. {LResponse.Error.ErrorDesc}");
                    return StatusCode(200, LResponse);
                }

                LResponse.UserId = await FLogicContext.Accounts.SignUp(PayLoad, 12);
                LResponse.IsUserCreated = true;
                
                FAppLogger.LogInfo($"POST api/v1/ajax/users/signup/ | New user '{PayLoad.EmailAddress}' has been successfully registered.");
                return StatusCode(200, LResponse);

            }
            catch (Exception E)
            {
                LResponse.Error.ErrorCode = E.HResult.ToString();
                LResponse.Error.ErrorDesc = string.IsNullOrEmpty(E.InnerException?.Message) ? E.Message : $"{E.Message} ({ E.InnerException.Message}).";
                FAppLogger.LogFatality($"POST api/v1/ajax/users/signup/ | Error has been raised while processing request. Message: {LResponse.Error.ErrorDesc}.");
                return StatusCode(500, LResponse);
            }

        }

        /// <summary>
        /// Endpoint allowing singin to the website.
        /// </summary>
        /// <param name="PayLoad"></param>
        /// <returns></returns>
        // POST api/v1/ajax/users/signin/
        [ValidateAntiForgeryToken]
        [HttpPost("users/signin")]
        public async Task<IActionResult> LogToAccountAsync([FromBody] UserLogin PayLoad) 
        {

            var LResponse = new UserLogged();
            try
            {

                if (!ModelState.IsValid)
                {
                    LResponse.Error.ErrorCode = Constants.Errors.InvalidPayLoad.ErrorCode;
                    LResponse.Error.ErrorDesc = Constants.Errors.InvalidPayLoad.ErrorDesc;
                    FAppLogger.LogError($"POST api/v1/ajax/users/signin/. {LResponse.Error.ErrorDesc}.");
                    LResponse.IsLogged = false;
                    return StatusCode(200, LResponse);
                }

                var SignInResult = await FLogicContext.Accounts.SignIn(PayLoad.EmailAddr, PayLoad.Password);

                if (SignInResult.Item1 == Guid.Empty && SignInResult.Item2 == true) 
                {
                    LResponse.Error.ErrorCode = Constants.Errors.InvalidCredentials.ErrorCode;
                    LResponse.Error.ErrorDesc = Constants.Errors.InvalidCredentials.ErrorDesc;
                    FAppLogger.LogError($"POST api/v1/ajax/users/signin/. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(200, LResponse);
                }

                if (SignInResult.Item2 == false) 
                {
                    LResponse.Error.ErrorCode = Constants.Errors.AccountNotActivated.ErrorCode;
                    LResponse.Error.ErrorDesc = Constants.Errors.AccountNotActivated.ErrorDesc;
                    FAppLogger.LogError($"POST api/v1/ajax/users/signin/. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(200, LResponse);
                }

                HttpContext.Session.SetString(Constants.Sessions.KeyNames.SessionId, SignInResult.Item1.ToString());
                HttpContext.Session.SetString(Constants.Sessions.KeyNames.EmailAddr, PayLoad.EmailAddr);
                HttpContext.Session.SetString(Constants.Sessions.KeyNames.ExpiresAt, DateTime.Now.AddMinutes(Constants.Sessions.IdleTimeout).ToString());

                LResponse.IsLogged  = true;
                return StatusCode(200, LResponse);

            }
            catch (Exception E)
            {
                LResponse.Error.ErrorCode = E.HResult.ToString();
                LResponse.Error.ErrorDesc = string.IsNullOrEmpty(E.InnerException?.Message) ? E.Message : $"{E.Message} ({ E.InnerException.Message}).";
                FAppLogger.LogFatality($"POST api/v1/ajax/users/signin/ | Error has been raised while processing request. Message: {LResponse.Error.ErrorDesc}.");
                return StatusCode(500, LResponse);
            }

        }
        
    }

}
