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
        /// <param name="PayLoad"></param>
        /// <param name="PasswordSalt"></param>
        /// <returns></returns>
        Task<int> SignUp(UserCreate PayLoad, int PasswordSalt);

        /// <summary>
        /// Perform sign-in action and log it to the history table.
        /// </summary>
        /// <param name="EmailAddr"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        Task<Tuple<Guid, bool>> SignIn(string EmailAddr, string Password);

    }

}
