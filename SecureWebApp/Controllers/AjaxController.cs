using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SecureWebApp.Helpers;
using SecureWebApp.Models.Json;
using SecureWebApp.Models.Views;
using SecureWebApp.Models.Database;
using SecureWebApp.Extensions.BCrypt;
using SecureWebApp.Extensions.AppLogger;
using SecureWebApp.Extensions.DnsLookup;

namespace SecureWebApp.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "ResponseCache")]
    public class AjaxController : Controller
    {

        private readonly MainDbContext FMainDbContext;
        private readonly IAppLogger    FAppLogger;
        private readonly IDnsLookup    FDnsLookup;

        public AjaxController(
            MainDbContext AMainDbContext,
            IAppLogger    AAppLogger,
            IDnsLookup    ADnsLookup
        )
        {
            FMainDbContext = AMainDbContext;
            FAppLogger     = AAppLogger;
            FDnsLookup     = ADnsLookup;
        }

        /// <summary>
        /// Parse given email address using MailAddress class provided in NET Core.
        /// This is alternative approach to classic RegEx.
        /// </summary>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        public bool IsEmailAddressCorrect(string AEmailAddress)
        {
            try
            {
                var LEmailAddress = new MailAddress(AEmailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Check if given email address aready exists.
        /// </summary>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        public async Task<bool> IsEmailAddressExist(string AEmailAddress) 
        {

            var LEmailList = await FMainDbContext.Users
                .AsNoTracking()
                .Where(R => R.EmailAddr == AEmailAddress)
                .Select(R => R.EmailAddr)
                .ToListAsync();

            return LEmailList.Any();

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

                if (!IsEmailAddressCorrect(EmailAddress)) 
                {
                    LResponse.Error.ErrorCode = Constants.Errors.EmailAddressMalformed.ErrorCode;
                    LResponse.Error.ErrorDesc = Constants.Errors.EmailAddressMalformed.ErrorDesc;
                    FAppLogger.LogWarn($"GET api/v1/ajax/validation/{EmailAddress}. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(200, LResponse);
                }
               
                if (await IsEmailAddressExist(EmailAddress)) 
                {
                    LResponse.Error.ErrorCode = Constants.Errors.EmailAlreadyExists.ErrorCode;
                    LResponse.Error.ErrorDesc = Constants.Errors.EmailAlreadyExists.ErrorDesc;
                    FAppLogger.LogWarn($"GET api/v1/ajax/validation/{EmailAddress}. {LResponse.Error.ErrorDesc}.");
                    return StatusCode(200, LResponse);
                }

                if (!await FDnsLookup.IsDomainExist(EmailAddress)) 
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
        /// Return list of all available countries.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CountryList>> ReturnCountryList() 
        {

            var LCountries = await FMainDbContext.Countries
                .AsNoTracking()
                .Select(R => new CountryList() 
                { 
                    Id = R.Id,
                    Name = R.CountryName
                })
                .ToListAsync();

            return LCountries;
        
        }

        /// <summary>
        /// Endpoint returning list of countries in JSON format.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        // GET api/v1/ajax/countries/
        [ValidateAntiForgeryToken]
        [HttpGet("countries")]
        public async Task<IActionResult> ReturnCountryAsync(int Id)
        {

            var LResponse = new ReturnCountryList();
            try
            {
                LResponse.Countries = await ReturnCountryList();
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
        /// Return list of cities for given Country Id.
        /// </summary>
        /// <param name="AId"></param>
        /// <returns></returns>
        public async Task<List<CityList>> ReturnCityList(int AId) 
        {

            var LCities = await FMainDbContext.Cities
                .AsNoTracking()
                .Where(R => R.CountryId == AId)
                .Select(R => new CityList()
                {
                    Id = R.Id,
                    Name = R.CityName
                })
                .ToListAsync();

            return LCities;

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
                LResponse.Cities = await ReturnCityList(Id);
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
        /// Perform sign-up action for given PayLoad and Password Salt (reccommended value is > 10).
        /// </summary>
        /// <param name="PayLoad"></param>
        /// <param name="PasswordSalt"></param>
        /// <returns></returns>
        public async Task<int> SignUp(UserCreate PayLoad, int PasswordSalt) 
        { 
        
            var NewUser = new Users()
            { 
                FirstName   = PayLoad.FirstName,
                LastName    = PayLoad.LastName,
                NickName    = PayLoad.NickName,
                EmailAddr   = PayLoad.EmailAddress,
                Password    = BCrypt.HashPassword(PayLoad.Password, BCrypt.GenerateSalt(PasswordSalt)),
                PhoneNum    = null,
                CreatedAt   = DateTime.Now,
                IsActivated = false,
                CountryId   = PayLoad.CountryId,
                CityId      = PayLoad.CityId
            };

            FMainDbContext.Users.Add(NewUser);
            await FMainDbContext.SaveChangesAsync();

            return NewUser.Id;

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

                if (await IsEmailAddressExist(PayLoad.EmailAddress))
                {
                    LResponse.Error.ErrorCode = Constants.Errors.EmailAlreadyExists.ErrorCode;
                    LResponse.Error.ErrorDesc = Constants.Errors.EmailAlreadyExists.ErrorDesc;
                    FAppLogger.LogWarn($"POST api/v1/ajax/users/signup/. {LResponse.Error.ErrorDesc}");
                    return StatusCode(200, LResponse);
                }

                LResponse.UserId = await SignUp(PayLoad, 12);
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
        /// Perform sign-in action and log it to the history table.
        /// </summary>
        /// <param name="EmailAddr"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public async Task<Tuple<Guid, bool>> SignIn(string EmailAddr, string Password) 
        {

            var Users = (await FMainDbContext.Users.Where(r => r.EmailAddr == EmailAddr).ToListAsync()).Single();
            if (!Users.IsActivated) 
            {
                return Tuple.Create(Guid.Empty, false);
            }

            var CheckPassword = BCrypt.CheckPassword(Password, Users.Password);
            if (!CheckPassword) 
            {
                return Tuple.Create(Guid.Empty, true);
            }

            var SessionId = Guid.NewGuid();
            var LogHistory = new SigninHistory()
            {
                UserId   = Users.Id,
                LoggedAt = DateTime.Now,
            };

            FMainDbContext.SigninHistory.Add(LogHistory);
            await FMainDbContext.SaveChangesAsync();

            return Tuple.Create(SessionId, true);

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

                var SignInResult = await SignIn(PayLoad.EmailAddr, PayLoad.Password);

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
