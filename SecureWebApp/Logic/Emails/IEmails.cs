using System.Threading.Tasks;

namespace SecureWebApp.Logic
{

    public interface IEmails
    {

        /// <summary>
        /// Parse given email address using MailAddress class provided in NET Core.
        /// This is alternative approach to classic RegEx.
        /// </summary>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        bool IsEmailAddressCorrect(string AEmailAddress);

        /// <summary>
        /// Check if given email address aready exists.
        /// </summary>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        Task<bool> IsEmailAddressExist(string AEmailAddress);

        /// <summary>
        /// Check if given address email have valid domain.
        /// </summary>
        /// <seealso cref="https://dnsclient.michaco.net"/>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        Task<bool> IsEmailDomainExist(string AEmailAddress);

    }

}
