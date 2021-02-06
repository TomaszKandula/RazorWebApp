using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SecureWebApp.Logic;
using SecureWebApp.Logger;
using SecureWebApp.Shared;
using SecureWebApp.Shared.Dto;
using SecureWebApp.Shared.Resources;

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
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        // GET api/v1/ajax/validation/{aemailaddress}/
        [ValidateAntiForgeryToken]
        [HttpGet("validation/{aemailaddress}")]
        public async Task<IActionResult> CheckEmailAsync([FromRoute] string AEmailAddress)
        {
            var LResponse = new EmailValidationDto();
            try 
            {
                if (!FLogicContext.Emails.IsEmailAddressCorrect(AEmailAddress)) 
                {
                    LResponse.Error.ErrorCode = nameof(ErrorCodes.EMAIL_ADDRESS_MALFORMED);
                    LResponse.Error.ErrorDesc = ErrorCodes.EMAIL_ADDRESS_MALFORMED;
                    FAppLogger.LogWarn($"GET api/v1/ajax/validation/{AEmailAddress}. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(400, LResponse);
                }
               
                if (await FLogicContext.Emails.IsEmailAddressExist(AEmailAddress)) 
                {
                    LResponse.Error.ErrorCode = nameof(ErrorCodes.EMAIL_ALREADY_EXISTS);
                    LResponse.Error.ErrorDesc = ErrorCodes.EMAIL_ALREADY_EXISTS;
                    FAppLogger.LogWarn($"GET api/v1/ajax/validation/{AEmailAddress}. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(400, LResponse);
                }

                if (!await FLogicContext.Emails.IsEmailDomainExist(AEmailAddress)) 
                {
                    LResponse.Error.ErrorCode = nameof(ErrorCodes.EMAIL_DOMAIN_NOT_EXISTS);
                    LResponse.Error.ErrorDesc = ErrorCodes.EMAIL_DOMAIN_NOT_EXISTS;
                    FAppLogger.LogWarn($"GET api/v1/ajax/validation/{AEmailAddress}. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(400, LResponse);
                }

                LResponse.IsEmailValid = true;
                return StatusCode(200, LResponse);
            } 
            catch (Exception LException)
            {
                LResponse.Error.ErrorCode = LException.HResult.ToString();
                LResponse.Error.ErrorDesc = string.IsNullOrEmpty(LException.InnerException?.Message) 
                    ? LException.Message 
                    : $"{LException.Message} ({ LException.InnerException.Message}).";
                FAppLogger.LogFatality($"GET api/v1/ajax/validation/{AEmailAddress} | Error has been raised while processing request. Message: {LResponse.Error.ErrorDesc}.");
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
            var LResponse = new ReturnCountryListDto();
            try
            {
                LResponse.Countries = await FLogicContext.Repository.ReturnCountryList();
                return StatusCode(200, LResponse);
            }
            catch (Exception LException)
            {
                LResponse.Error.ErrorCode = LException.HResult.ToString();
                LResponse.Error.ErrorDesc = string.IsNullOrEmpty(LException.InnerException?.Message) 
                    ? LException.Message 
                    : $"{LException.Message} ({ LException.InnerException.Message}).";
                FAppLogger.LogFatality($"GET api/v1/ajax/countries/ | Error has been raised while processing request. Message: {LResponse.Error.ErrorDesc}.");
                return StatusCode(500, LResponse);
            }
        }

        /// <summary>
        /// Endpoint returning list of cities for given Country Id in JSON format.
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns></returns>
        // GET api/v1/ajax/cities/?countryid={id}
        [ValidateAntiForgeryToken]
        [HttpGet("cities")]
        // ReSharper disable once InconsistentNaming for query string
        public async Task<IActionResult> ReturnCityAsync([FromQuery] int CountryId) 
        {
            var LResponse = new ReturnCityListDto();
            try 
            {
                LResponse.Cities = await FLogicContext.Repository.ReturnCityList(CountryId);
                return StatusCode(200, LResponse);
            } 
            catch (Exception LException)
            {
                LResponse.Error.ErrorCode = LException.HResult.ToString();
                LResponse.Error.ErrorDesc = string.IsNullOrEmpty(LException.InnerException?.Message) 
                    ? LException.Message 
                    : $"{LException.Message} ({ LException.InnerException.Message}).";
                FAppLogger.LogFatality($"GET api/v1/ajax/cities/{CountryId} | Error has been raised while processing request. Message: {LResponse.Error.ErrorDesc}.");
                return StatusCode(500, LResponse);
            }
        }

        /// <summary>
        /// Endpoint adding new user to the database.
        /// </summary>
        /// <param name="APayLoad"></param>
        /// <returns></returns>
        // POST api/v1/ajax/users/signup/
        [ValidateAntiForgeryToken]
        [HttpPost("users/signup")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] UserCreateDto APayLoad) 
        {
            var LResponse = new UserCreatedDto();
            try
            {
                if (!ModelState.IsValid) 
                {
                    LResponse.Error.ErrorCode = nameof(ErrorCodes.INVALID_PAYLOAD);
                    LResponse.Error.ErrorDesc = ErrorCodes.INVALID_PAYLOAD;
                    FAppLogger.LogError($"POST api/v1/ajax/users/signup/. {LResponse.Error.ErrorDesc}.");
                    LResponse.IsUserCreated = false;
                    return StatusCode(400, LResponse);
                }

                if (await FLogicContext.Emails.IsEmailAddressExist(APayLoad.EmailAddress))
                {
                    LResponse.Error.ErrorCode = nameof(ErrorCodes.EMAIL_ALREADY_EXISTS);
                    LResponse.Error.ErrorDesc = ErrorCodes.EMAIL_ALREADY_EXISTS;
                    FAppLogger.LogWarn($"POST api/v1/ajax/users/signup/. {LResponse.Error.ErrorDesc}");
                    return StatusCode(400, LResponse);
                }

                LResponse.UserId = await FLogicContext.Accounts.SignUp(APayLoad, 12);
                LResponse.IsUserCreated = true;
                
                FAppLogger.LogInfo($"POST api/v1/ajax/users/signup/ | New user '{APayLoad.EmailAddress}' has been successfully registered.");
                return StatusCode(200, LResponse);
            }
            catch (Exception LException)
            {
                LResponse.Error.ErrorCode = LException.HResult.ToString();
                LResponse.Error.ErrorDesc = string.IsNullOrEmpty(LException.InnerException?.Message) 
                    ? LException.Message 
                    : $"{LException.Message} ({ LException.InnerException.Message}).";
                FAppLogger.LogFatality($"POST api/v1/ajax/users/signup/ | Error has been raised while processing request. Message: {LResponse.Error.ErrorDesc}.");
                return StatusCode(500, LResponse);
            }
        }

        /// <summary>
        /// Endpoint allowing singin to the website.
        /// </summary>
        /// <param name="APayLoad"></param>
        /// <returns></returns>
        // POST api/v1/ajax/users/signin/
        [ValidateAntiForgeryToken]
        [HttpPost("users/signin")]
        public async Task<IActionResult> LogToAccountAsync([FromBody] UserLoginDto APayLoad) 
        {
            var LResponse = new UserLoggedDto();
            try
            {
                if (!ModelState.IsValid)
                {
                    LResponse.Error.ErrorCode = nameof(ErrorCodes.INVALID_PAYLOAD);
                    LResponse.Error.ErrorDesc = ErrorCodes.INVALID_PAYLOAD;
                    FAppLogger.LogError($"POST api/v1/ajax/users/signin/. {LResponse.Error.ErrorDesc}.");
                    LResponse.IsLogged = false;
                    return StatusCode(400, LResponse);
                }

                var (LSessionId, LIsSignedIn) = await FLogicContext.Accounts.SignIn(APayLoad.EmailAddr, APayLoad.Password);

                if (LSessionId == Guid.Empty && LIsSignedIn) 
                {
                    LResponse.Error.ErrorCode = nameof(ErrorCodes.INVALID_CREDENTIALS);
                    LResponse.Error.ErrorDesc = ErrorCodes.INVALID_CREDENTIALS;
                    FAppLogger.LogError($"POST api/v1/ajax/users/signin/. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(400, LResponse);
                }

                if (!LIsSignedIn) 
                {
                    LResponse.Error.ErrorCode = nameof(ErrorCodes.ACCOUNT_NOT_ACTIVATED);
                    LResponse.Error.ErrorDesc = ErrorCodes.ACCOUNT_NOT_ACTIVATED;
                    FAppLogger.LogError($"POST api/v1/ajax/users/signin/. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(400, LResponse);
                }

                HttpContext.Session.SetString(Constants.Sessions.KeyNames.SessionId, LSessionId.ToString());
                HttpContext.Session.SetString(Constants.Sessions.KeyNames.EmailAddr, APayLoad.EmailAddr);
                HttpContext.Session.SetString(Constants.Sessions.KeyNames.ExpiresAt, DateTime.Now.AddMinutes(Constants.Sessions.IdleTimeout).ToString(CultureInfo.InvariantCulture));

                LResponse.IsLogged  = true;
                return StatusCode(200, LResponse);
            }
            catch (Exception LException)
            {
                LResponse.Error.ErrorCode = LException.HResult.ToString();
                LResponse.Error.ErrorDesc = string.IsNullOrEmpty(LException.InnerException?.Message) 
                    ? LException.Message 
                    : $"{LException.Message} ({ LException.InnerException.Message}).";
                FAppLogger.LogFatality($"POST api/v1/ajax/users/signin/ | Error has been raised while processing request. Message: {LResponse.Error.ErrorDesc}.");
                return StatusCode(500, LResponse);
            }
        }      
    }
}
