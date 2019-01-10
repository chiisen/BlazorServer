using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;

namespace BlazorServer.Server
{
    public class Program
    {
        static string _logFileName = "";
        public static Logger _logger = null;

        // 設定 NLog 檔名
        static void SetNlogFileName()
        {
            _logFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            LogManager.Configuration.Variables["MY_DATE"] = _logFileName;
        }

        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            _logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            SetNlogFileName();
            try
            {
                _logger.Warn("init main");
                _logger.Warn("Log File Name:" + _logFileName);

                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                _logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                        .AddCommandLine(args)
                        .Build())
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog()  // NLog: setup NLog for Dependency injection
                .Build();
    }
}
