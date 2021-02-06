using RazorWebApp.Logic.Emails;
using RazorWebApp.Logic.Accounts;
using RazorWebApp.Logic.Repository;

namespace RazorWebApp.Logic
{
    public interface ILogicContext
    {
        IAccounts Accounts { get; }

        IEmails Emails { get; }

        IRepository Repository { get; }
    }
}
