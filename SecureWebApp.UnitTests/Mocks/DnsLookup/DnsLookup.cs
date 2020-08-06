using System.Threading.Tasks;

namespace SecureWebApp.UnitTests.Mocks.DnsLookup
{

    public sealed class DnsLookup : Extensions.DnsLookup.DnsLookup, IDnsLookup
    {

        public override async Task<bool> IsDomainExist(string AEmailAddress)
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
