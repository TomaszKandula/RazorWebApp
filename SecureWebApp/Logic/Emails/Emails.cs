using DnsClient;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DnsClient;
using SecureWebApp.Infrastructure.Database;

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
                // ReSharper disable once UnusedVariable
                var LEmailAddress = new MailAddress(AEmailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Check if given email address already exists.
        /// </summary>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        public async Task<bool> IsEmailAddressExist(string AEmailAddress)
        {
            var LEmailList = await FMainDbContext.Users
                .AsNoTracking()
                .Where(AUsers => AUsers.EmailAddr == AEmailAddress)
                .Select(AUsers => AUsers.EmailAddr)
                .ToListAsync();

            return LEmailList.Any();
        }

        /// <summary>
        /// Check if given address email have valid domain.
        /// </summary>
        /// <seealso href="https://dnsclient.michaco.net"/>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        public async Task<bool> IsEmailDomainExist(string AEmailAddress) 
        {
            try 
            {
                var LLookupClient = new LookupClient();

                var LGetEmailDomain = AEmailAddress.Split("@");
                var LEmailDomain = LGetEmailDomain[1];

                var LCheckRecordA = await LLookupClient.QueryAsync(LEmailDomain, QueryType.A).ConfigureAwait(false); 
                var LCheckRecordAaaa = await LLookupClient.QueryAsync(LEmailDomain, QueryType.AAAA).ConfigureAwait(false); 
                var LCheckRecordMx = await LLookupClient.QueryAsync(LEmailDomain, QueryType.MX).ConfigureAwait(false);

                var LRecordA = LCheckRecordA.Answers.Where(ARecord => ARecord.RecordType == DnsClient.Protocol.ResourceRecordType.A);
                var LRecordAaaa = LCheckRecordAaaa.Answers.Where(ARecord => ARecord.RecordType == DnsClient.Protocol.ResourceRecordType.AAAA);
                var LRecordMx = LCheckRecordMx.Answers.Where(ARecord => ARecord.RecordType == DnsClient.Protocol.ResourceRecordType.MX);

                var LIsRecordA = LRecordA.Any();
                var LIsRecordAaaa = LRecordAaaa.Any();
                var LIsRecordMx = LRecordMx.Any();

                return LIsRecordA || LIsRecordAaaa || LIsRecordMx;               
            }
            catch (DnsResponseException)
            {
                return false;
            }
        }
    }
}
