using System;
using System.Threading.Tasks;
using SecureWebApp.Models.Json;

namespace SecureWebApp.Logic.Accounts
{

    public interface IAccounts
    {

        /// <summary>
        /// Perform sign-up action for given PayLoad and Password Salt (reccommended value is > 10).
        /// </summary>
        /// <param name="APayLoad"></param>
        /// <param name="APasswordSalt"></param>
        /// <returns></returns>
        Task<int> SignUp(UserCreate APayLoad, int APasswordSalt);

        /// <summary>
        /// Perform sign-in action and log it to the history table.
        /// </summary>
        /// <param name="AEmailAddr"></param>
        /// <param name="APassword"></param>
        /// <returns></returns>
        Task<Tuple<Guid, bool>> SignIn(string AEmailAddr, string APassword);

    }

}
