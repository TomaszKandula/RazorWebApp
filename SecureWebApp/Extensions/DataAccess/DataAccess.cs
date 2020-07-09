using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SecureWebApp.Helpers;
using SecureWebApp.Models.Database;
using SecureWebApp.Extensions.AppLogger;
using Microsoft.Extensions.Primitives;
using Microsoft.EntityFrameworkCore;

namespace SecureWebApp.Extensions.DataAccess
{

    public class DataAccess : IDataAccess
    {

        private readonly MainDbContext FMainDbContext;
        private readonly IAppLogger    FLogger;

        public DataAccess(
            MainDbContext AMainDbContext,
            IAppLogger    ALogger
        )
        {
            FMainDbContext = AMainDbContext;
            FLogger        = ALogger;
        }

        public async Task<Types.AuthorizeResponse> AuthorizeRequestAsync(HttpContext AHttpContext, string AHeaderName)
        {

            var BaseUrl = AHttpContext.Request.Scheme + "://" + AHttpContext.Request.Host.Value.ToString();
            try
            {

                StringValues LApiKey;
                AHttpContext.Request?.Headers.TryGetValue(AHeaderName, out LApiKey);

                if (LApiKey.Count == 0)
                {

                    FLogger.LogError("[AuthorizeRequestAsync]: No authorization header has been found.");
                    return new Types.AuthorizeResponse
                    {
                        SourceDomain = BaseUrl,
                        StatusCode   = HttpStatusCode.Unauthorized,
                        LastMessage  = string.Format("Unauthorized ({0}).", BaseUrl)
                    };

                }

                var LPublicApiKey = LApiKey.ToString();

                var AuthDomains = (await FMainDbContext.AuthDomains
                    .AsNoTracking()
                    .Where(R => R.Domain == BaseUrl && R.ApiKey == LPublicApiKey)
                    .Select(R => new { R.Id, R.Maturity })
                    .ToListAsync()).FirstOrDefault();

                if (AuthDomains == null)
                {

                    return new Types.AuthorizeResponse
                    {
                        SourceDomain = BaseUrl,
                        StatusCode   = HttpStatusCode.Unauthorized,
                        LastMessage  = string.Format("Unauthorized ({0}).", BaseUrl)
                    };

                }
                else 
                {

                    if (AuthDomains.Maturity != null && DateTime.Now > AuthDomains.Maturity) 
                    { 
                    
                        return new Types.AuthorizeResponse
                        {
                            SourceDomain = BaseUrl,
                            StatusCode   = HttpStatusCode.Unauthorized,
                            LastMessage  = string.Format("Unauthorized ({0}). The API key has expired.", BaseUrl)
                        };

                    }

                    return new Types.AuthorizeResponse
                    {
                        SourceDomain = BaseUrl,
                        StatusCode   = HttpStatusCode.OK,
                        LastMessage  = string.Format("Authorized ({0}).", BaseUrl) 
                    };
                }

            }
            catch (Exception E)
            {

                FLogger.LogError(string.Format("[AuthorizeRequestAsync]: {0} ({1})", E.Message, E.InnerException.Message));
                return new Types.AuthorizeResponse
                {
                    SourceDomain = BaseUrl,
                    StatusCode   = HttpStatusCode.InternalServerError,
                    LastMessage  = string.Format("Internal Server Error: {0} ({1}).", E.Message, E.InnerException.Message)
                };

            }

        }

    }

}
