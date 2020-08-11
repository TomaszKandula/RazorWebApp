using SecureWebApp.Logic.Emails;
using SecureWebApp.Logic.Accounts;
using SecureWebApp.Logic.Repository;

namespace SecureWebApp.Logic
{

    public interface ILogicContext
    {
        IAccounts Accounts { get; }
        IEmails Emails { get; }
        IRepository Repository { get; }
    }

}
