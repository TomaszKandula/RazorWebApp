using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureWebApp.Helpers;
using SecureWebApp.Models.Json;
using SecureWebApp.Models.Views;
using SecureWebApp.Models.Database;
using SecureWebApp.Extensions.AppLogger;
using SecureWebApp.Extensions.DnsLookup;

namespace SecureWebApp.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AjaxController : Controller
    {

        private readonly IAppLogger    FAppLogger;
        private readonly MainDbContext FMainDbContext;
        private readonly IDnsLookup    FDnsLookup;

        public AjaxController(
            IAppLogger    AAppLogger,
            MainDbContext AMainDbContext,
            IDnsLookup    ADnsLookup
        )
        {
            FAppLogger     = AAppLogger;
            FMainDbContext = AMainDbContext;
            FDnsLookup     = ADnsLookup;
        }

        /// <summary>
        /// Parse given email address.
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
        /// Validate supplied email address. It checks format and email domain, and if it is already registered in database.
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <returns></returns>
        // GET api/v1/ajax/validation/{emailaddress}/
        [HttpGet("validation/{emailaddress}")]
        public async Task<IActionResult> CheckEmailAsync(string EmailAddress)
        {

            var EResponse = new EmailValidation();
            try 
            {

                if (!IsEmailAddressCorrect(EmailAddress)) 
                {
                    EResponse.Error.ErrorCode = Constants.Errors.EmailAddressMalformed.ErrorCode;
                    EResponse.Error.ErrorDesc = Constants.Errors.EmailAddressMalformed.ErrorDesc;
                    FAppLogger.LogWarn(string.Format("GET api/v1/ajax/validation/{0}. {1}.", EmailAddress, EResponse.Error.ErrorDesc));
                    return StatusCode(200, EResponse);
                }

                var EmailList = await FMainDbContext.Users
                    .AsNoTracking()
                    .Where(R => R.EmailAddr == EmailAddress)
                    .Select(R => R.EmailAddr)
                    .ToListAsync();
                
                if (EmailList.Any()) 
                {
                    EResponse.Error.ErrorCode = Constants.Errors.EmailAlreadyExists.ErrorCode;
                    EResponse.Error.ErrorDesc = Constants.Errors.EmailAlreadyExists.ErrorDesc;
                    FAppLogger.LogWarn(string.Format("GET api/v1/ajax/validation/{0}. {1}.", EmailAddress, EResponse.Error.ErrorDesc));
                    return StatusCode(200, EResponse);
                }

                if (!await FDnsLookup.IsDomainExist(EmailAddress)) 
                {
                    EResponse.Error.ErrorCode = Constants.Errors.EmailDomainNotExist.ErrorCode;
                    EResponse.Error.ErrorDesc = Constants.Errors.EmailDomainNotExist.ErrorDesc;
                    FAppLogger.LogWarn(string.Format("GET api/v1/ajax/validation/{0}. {1}.", EmailAddress, EResponse.Error.ErrorDesc));
                    return StatusCode(200, EResponse);
                }

                EResponse.IsEmailValid = true;
                return StatusCode(200, EResponse);

            } 
            catch (Exception E)
            {
                EResponse.Error.ErrorCode = E.HResult.ToString();
                EResponse.Error.ErrorDesc = E.Message;
                FAppLogger.LogFatality(string.Format("GET api/v1/ajax/validation/{0} | Error has been raised while processing request. Message: {1}.", EmailAddress, E.Message));
                return StatusCode(500, EResponse);
            }

        }

        /// <summary>
        /// Return list of cities for given Country Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        // GET api/v1/ajax/cities/{id}/
        [HttpGet("cities/{id}")]
        public async Task<IActionResult> ReturnCityAsync(int Id) 
        {

            var EResponse = new ReturnCityList();
            try 
            {

                var Cities = await FMainDbContext.Cities
                    .AsNoTracking()
                    .Where(R => R.CountryId == Id)
                    .Select(R => new CityList() 
                    { 
                        Id   = R.Id, 
                        Name = R.CityName 
                    })
                    .ToListAsync();

                EResponse.Cities = Cities;
                return StatusCode(200, EResponse);

            } 
            catch (Exception E)
            {
                EResponse.Error.ErrorCode = E.HResult.ToString();
                EResponse.Error.ErrorDesc = E.Message;
                FAppLogger.LogFatality(string.Format("GET api/v1/cities/{0} | Error has been raised while processing request. Message: {1}.", Id, E.Message));
                return StatusCode(500, EResponse);
            }

        }

    }

}
