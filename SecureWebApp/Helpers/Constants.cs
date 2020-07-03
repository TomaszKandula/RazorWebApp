namespace SecureWebApp.Helpers
{

    /// <summary>
    /// This class is responsible only for providing constants to all classes/methods etc. accross the application.
    /// It can be a partial class if necessary; and if so, then put the module in the root folder and additional 
    /// partials in other project folders.
    public class Constants
    {

        public static class Errors 
        {

            internal class EmailAlreadyExists
            {
                public const string ErrorCode = "email_already_exists";
                public const string ErrorDesc = "This email address already exists.";
            }

            internal class EmailAddressMalformed
            {
                public const string ErrorCode = "email_is_malformed";
                public const string ErrorDesc = "This email address is malformed.";
            }

            internal class EmailDomainNotExist
            {
                public const string ErrorCode = "is_email_domain_exist";
                public const string ErrorDesc = "The email domain is not exist.";
            }

        }

    }

}
