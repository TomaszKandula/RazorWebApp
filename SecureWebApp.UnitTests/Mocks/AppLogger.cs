using System.Diagnostics;

namespace SecureWebApp.UnitTests.Mocks
{

    public sealed class AppLogger : Extensions.AppLogger.IAppLogger
    {

        public void LogDebug(string AMessage)
        {
            Trace.TraceInformation("[Debug]: " + AMessage);
        }

        public void LogInfo(string AMessage)
        {
            Trace.TraceInformation("[Information]: " + AMessage);
        }

        public void LogWarn(string AMessage)
        {
            Trace.TraceInformation("[Warning]: " + AMessage);
        }

        public void LogError(string AMessage)
        {
            Trace.TraceInformation("[Error]: " + AMessage);
        }

        public void LogFatality(string AMessage)
        {
            Trace.TraceInformation("[Fatal]: " + AMessage);
        }

    }

}
