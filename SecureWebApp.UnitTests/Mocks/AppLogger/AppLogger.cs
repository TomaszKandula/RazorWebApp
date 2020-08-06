using System.Diagnostics;

namespace SecureWebApp.UnitTests.Mocks.AppLogger
{

    public sealed class AppLogger : Extensions.AppLogger.AppLogger, IAppLogger
    {

        public override void LogDebug(string AMessage)
        {
            Trace.TraceInformation("[Debug]: " + AMessage);
        }

        public override void LogInfo(string AMessage)
        {
            Trace.TraceInformation("[Information]: " + AMessage);
        }

        public override void LogWarn(string AMessage)
        {
            Trace.TraceInformation("[Warning]: " + AMessage);
        }

        public override void LogError(string AMessage)
        {
            Trace.TraceInformation("[Error]: " + AMessage);
        }

        public override void LogFatality(string AMessage)
        {
            Trace.TraceInformation("[Fatal]: " + AMessage);
        }

    }

}
