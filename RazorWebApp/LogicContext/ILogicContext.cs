using RazorWebApp.LogicContext.Emails;
using RazorWebApp.LogicContext.Accounts;
using RazorWebApp.LogicContext.Repository;

namespace RazorWebApp.LogicContext
{
    public interface ILogicContext
    {
        IAccounts Accounts { get; }

        IEmails Emails { get; }

        IRepository Repository { get; }
    }
}
