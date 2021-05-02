using RazorWebApp.LogicContext.Emails;
using RazorWebApp.LogicContext.Accounts;
using RazorWebApp.LogicContext.Repository;
using RazorWebApp.Infrastructure.Database;

namespace RazorWebApp.LogicContext
{
    public class LogicContext : ILogicContext
    {
        private readonly MainDbContext FMainDbContext;

        private IAccounts FAccounts;
        
        private IEmails FEmails;

        private IRepository FRepository;

        public LogicContext(MainDbContext AMainDbContext) => FMainDbContext = AMainDbContext;

        public IAccounts Accounts => FAccounts ?? (FAccounts = new Accounts.Accounts(FMainDbContext));

        public IEmails Emails => FEmails ?? (FEmails = new Emails.Emails(FMainDbContext));

        public IRepository Repository => FRepository ?? (FRepository = new Repository.Repository(FMainDbContext));
    }
}
