using SecureWebApp.Logic.Accounts;
using SecureWebApp.Logic.Emails;
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

        public IAccounts _Accounts 
        {

            get
            {

                if (FAccounts == null) 
                {
                    FAccounts = new Accounts.Accounts(FMainDbContext);
                }

                return FAccounts;

            }

        }

        public IEmails _Emails
        {

            get
            {

                if (FEmails == null)
                {
                    FEmails = new Emails.Emails(FMainDbContext);
                }

                return FEmails;

            }

        }

        public IRepository _Repository
        {

            get
            {

                if (FRepository == null)
                {
                    FRepository = new Repository.Repository(FMainDbContext);
                }

                return FRepository;

            }

        }

    }

}
