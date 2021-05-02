using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace RazorWebApp
{
    public static class Program
    {
        private static IWebHostBuilder CreateWebHostBuilder(string[] AArgs) =>
            WebHost.CreateDefaultBuilder(AArgs).UseStartup<Startup>().UseSerilog();

        public static int Main(string[] AArgs)
        {
            var LOgsPath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(LOgsPath))
                Directory.CreateDirectory(LOgsPath);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.File(
                    LOgsPath + "\\log-.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: null,
                    shared: false)
                .CreateLogger();

            try
            {
                Log.Information("Starting WebHost...");
                CreateWebHostBuilder(AArgs).Build().Run();
                return 0;
            }
            catch (Exception LException)
            {
                Log.Fatal(LException, "WebHost has been terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
