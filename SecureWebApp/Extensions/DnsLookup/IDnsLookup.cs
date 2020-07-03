using System.Threading.Tasks;

namespace SecureWebApp.Extensions.DnsLookup
{

    /// <summary>
    /// Exposes method(s) that allows to verify domain.
    /// </summary>
    /// <remarks>
    /// Instantiate the interface only once per application, so we always use the same reference (singleton).
    /// </remarks>
    public interface IDnsLookup
    {

        /// <summary>
        /// Check if given address email have valid domain.
        /// </summary>
        /// <seealso cref="https://dnsclient.michaco.net"/>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        Task<bool> IsDomainExist(string AEmailAddress);

    }

}
