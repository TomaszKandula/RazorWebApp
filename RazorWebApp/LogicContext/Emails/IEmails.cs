﻿using System.Threading;
using System.Threading.Tasks;

namespace RazorWebApp.LogicContext.Emails
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
        /// Check if given email address ready exists.
        /// </summary>
        /// <param name="AEmailAddress"></param>
        /// <param name="ACancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsEmailAddressExist(string AEmailAddress, CancellationToken ACancellationToken = default);

        /// <summary>
        /// Check if given address email have valid domain.
        /// </summary>
        /// <seealso href="https://dnsclient.michaco.net"/>
        /// <param name="AEmailAddress"></param>
        /// <param name="ACancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsEmailDomainExist(string AEmailAddress, CancellationToken ACancellationToken = default);
    }
}
