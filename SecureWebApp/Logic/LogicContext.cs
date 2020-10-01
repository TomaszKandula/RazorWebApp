using SecureWebApp.Logic.Emails;
using SecureWebApp.Logic.Accounts;
using SecureWebApp.Logic.Repository;
using SecureWebApp.Models.Database;

namespace SecureWebApp.Logic
{

    public class LogicContext : ILogicContext
    {

        private readonly MainDbContext FMainDbContext;

        private IAccounts   FAccounts;
        private IEmails     FEmails;
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
