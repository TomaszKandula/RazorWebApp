using System.Threading.Tasks;

namespace SecureWebApp.UnitTests.Mocks.DnsLookup
{

    public interface IDnsLookup
    {
        Task<bool> IsDomainExist(string AEmailAddress);
    }

}
