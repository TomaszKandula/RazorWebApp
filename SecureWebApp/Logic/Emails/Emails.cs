using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DnsClient;
using SecureWebApp.Models.Database;

namespace SecureWebApp.Logic.Emails
{

    public class Emails : IEmails
    {

        private readonly MainDbContext FMainDbContext;

        public Emails(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        /// <summary>
        /// Parse given email address using MailAddress class provided in NET Core.
        /// This is alternative approach to classic RegEx.
        /// </summary>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        public bool IsEmailAddressCorrect(string AEmailAddress)
        {
            try
            {
                var LEmailAddress = new MailAddress(AEmailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Check if given email address aready exists.
        /// </summary>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        public async Task<bool> IsEmailAddressExist(string AEmailAddress)
        {

            var LEmailList = await FMainDbContext.Users
                .AsNoTracking()
                .Where(R => R.EmailAddr == AEmailAddress)
                .Select(R => R.EmailAddr)
                .ToListAsync();

            return LEmailList.Any();

        }

        /// <summary>
        /// Check if given address email have valid domain.
        /// </summary>
        /// <seealso cref="https://dnsclient.michaco.net"/>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        public async Task<bool> IsEmailDomainExist(string AEmailAddress) 
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
