using RazorWebApp.Logic.Emails;
using RazorWebApp.Logic.Accounts;
using RazorWebApp.Logic.Repository;
using RazorWebApp.Infrastructure.Database;

namespace RazorWebApp.Logic
{
    public class LogicContext : ILogicContext
    {
        private readonly MainDbContext FMainDbContext;

        private IAccounts FAccounts;
        private IEmails FEmails;
        private IRepository FRepository;

        public LogicContext(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        public IAccounts Accounts 
        {
            get { return FAccounts ?? (FAccounts = new Accounts.Accounts(FMainDbContext)); }
        }

        public IEmails Emails
        {
            get { return FEmails ?? (FEmails = new Emails.Emails(FMainDbContext)); }
        }

        public IRepository Repository
        {
            get { return FRepository ?? (FRepository = new Repository.Repository(FMainDbContext)); }
        }
    }
}
