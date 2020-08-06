using System.Threading.Tasks;

namespace SecureWebApp.UnitTests.Mocks
{

    public sealed class DnsLookup : Extensions.DnsLookup.IDnsLookup
    {

        public async Task<bool> IsDomainExist(string AEmailAddress)
        {

            try
            {

                // Provide test implementation

                return true;

            }
            catch
            {
                return false;
            }

        }

    }

}
