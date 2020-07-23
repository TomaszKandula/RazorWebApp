namespace SecureWebApp.Helpers
{

    /// <summary>
    /// This class is responsible only for providing constants to all classes/methods etc. accross the application.
    /// It can be a partial class if necessary; and if so, then put the module in the root folder and additional 
    /// partials in other project folders.
    public class Constants
    {

        public static class Sessions 
        {

            /* Use this value to determin user session timeout */
            public const int IdleTimeout = 15;

            internal class KeyNames 
            {
                public const string SessionId = "SessionId";
                public const string EmailAddr = "EmailAddr";
                public const string ExpiresAt = "ExpiresAt";
            }

        }
        
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

            internal class InvalidPayLoad 
            {
                public const string ErrorCode = "invalid_payload";
                public const string ErrorDesc = "The received content cannot be validated.";
            }

            internal class InvalidCredentials 
            {
                public const string ErrorCode = "invalid_credentials";
                public const string ErrorDesc = "Supplied email address and/or password is incorrect.";
            }

        }

    }

}
