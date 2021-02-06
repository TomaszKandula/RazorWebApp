namespace SecureWebApp.Shared
{
    /// <summary>
    /// This class is responsible only for providing constants to all classes/methods etc. accross the application.
    /// It can be a partial class if necessary; and if so, then put the module in the root folder and additional 
    /// partials in other project folders.
    /// </summary>
    public static class Constants
    {
        public static class Sessions 
        {
            /* Use this value to determine user session timeout */
            public const int IdleTimeout = 15;

            internal static class KeyNames 
            {
                public const string SessionId = "SessionId";
                public const string EmailAddr = "EmailAddr";
                public const string ExpiresAt = "ExpiresAt";
            }
        }       
    }
}
