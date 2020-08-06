namespace SecureWebApp.UnitTests.Mocks.AppLogger
{

    public interface IAppLogger
    {
        void LogDebug(string AMessage);
        void LogError(string AMessage);
        void LogInfo(string AMessage);
        void LogWarn(string AMessage);
        void LogFatality(string AMessage);
    }

}
