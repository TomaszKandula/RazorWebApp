using System.Linq;
using System.Threading.Tasks;
using DnsClient;

namespace SecureWebApp.Extensions.DnsLookup
{

    public class DnsLookup : IDnsLookup
    {

        /// <summary>
        /// Check if given address email have valid domain.
        /// </summary>
        /// <seealso cref="https://dnsclient.michaco.net"/>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        public async Task<bool> IsDomainExist(string AEmailAddress) 
        {

            try 
            {

                var LLookupClient = new LookupClient();

                var GetEmailDomain = AEmailAddress.Split("@");
                var EmailDomain    = GetEmailDomain[1];

                var CheckRecordA    = await LLookupClient.QueryAsync(EmailDomain, QueryType.A).ConfigureAwait(false); 
                var CheckRecordAAAA = await LLookupClient.QueryAsync(EmailDomain, QueryType.AAAA).ConfigureAwait(false); 
                var CheckRecordMX   = await LLookupClient.QueryAsync(EmailDomain, QueryType.MX).ConfigureAwait(false);

                var RecordA    = CheckRecordA.Answers.Where(Record => Record.RecordType == DnsClient.Protocol.ResourceRecordType.A);
                var RecordAAAA = CheckRecordA.Answers.Where(Record => Record.RecordType == DnsClient.Protocol.ResourceRecordType.AAAA);
                var RecordMX   = CheckRecordA.Answers.Where(Record => Record.RecordType == DnsClient.Protocol.ResourceRecordType.MX);

                var IsRecordA    = RecordA.Any();
                var IsRecordAAAA = RecordAAAA.Any();
                var IsRecordMX   = RecordMX.Any();

                if (IsRecordA || IsRecordAAAA || IsRecordMX)
                {
                    return true;
                }
                else 
                {
                    return false;
                }

            }
            catch (DnsResponseException)
            {
                return false;
            }

        }

    }

}
