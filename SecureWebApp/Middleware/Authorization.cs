using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SecureWebApp.Extensions.DataAccess;

namespace SecureWebApp.Middleware
{

    public sealed class Authorization
    {

        private readonly RequestDelegate LRequestNextDelegate;

        public Authorization(RequestDelegate ARequestNextDelegate)
        {
            LRequestNextDelegate = ARequestNextDelegate;
        }

        /// <summary>
        /// Checks the request path on each endpoint call and authorize request.
        /// Some paths are excluded from this process.
        /// </summary>
        /// <param name="AHttpContext"></param>
        /// <param name="ADataAccess"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext AHttpContext, IDataAccess ADataAccess)
        {

            if (IsAuthorizationRequired(AHttpContext.Request.Path.ToString().ToLower()))
            {

                var Authorization = await ADataAccess.AuthorizeRequestAsync(AHttpContext, "X-ApiKey");

                if (Authorization.StatusCode != HttpStatusCode.OK)
                {
                    AHttpContext.Response.StatusCode = (int)Authorization.StatusCode;
                    await AHttpContext.Response.WriteAsync(Authorization.LastMessage);
                }
                else
                {
                    await LRequestNextDelegate(AHttpContext);
                }

            }
            else
            {
                await LRequestNextDelegate(AHttpContext);
            }

        }

        /// <summary>
        /// List of paths that does not require authorization.
        /// </summary>
        /// <param name="RequestPath"></param>
        /// <returns></returns>
        private static bool IsAuthorizationRequired(string RequestPath)
        {

            var Exceptions = new List<string>
            {
                "/index",
                "/login",
                "/register",
                "/error",
                "/dist",
                "/images",
                "/modals",
                "/scripts",
                "/styles"
            };

            var Checks = 0;

            // wwwroot
            if (RequestPath == "/")
            {
                ++Checks;
            }
            else 
            {

                // web pages and static content
                foreach (var Exception in Exceptions)
                {
                    Checks = RequestPath.Contains(Exception) ? ++Checks : Checks;
                }

            }

            return Checks == 0;

        }

    }

}
