using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SecureWebApp.Helpers;

namespace SecureWebApp.Extensions.DataAccess
{

    /// <summary>
    /// DataAccess interface exposes methods for API request validation.
    /// </summary>
    public interface IDataAccess
    {

        /// <summary>
        /// Gets access token from authorization header and validate it against underlying database storge.
        /// </summary>
        /// <returns></returns>
        Task<Types.AuthorizeResponse> AuthorizeRequestAsync(HttpContext AHttpContext, string AHeaderName);

    }

}
