using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.IO;
using static BlazorServer.Server.Log;

namespace BlazorServer.Server
{
    public class Program
    {
        // 設定 NLog 檔名
        static void SetNlogFileName()
        {
            LogManager.Configuration.Variables["MY_DATE"] = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        }

        static IConfigurationRoot _config;// 讀取 hosting.json 裡指定的 Port 的設定物件

        public static void Main(string[] args)
        {
            // 讀取 hosting.json 裡的 Port 設定
            _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hosting.json", optional: true)
            .Build();

            // NLog: setup the logger first to catch all errors
            SetupLogger();
            SetNlogFileName();
            try
            {
                Print("init main");

                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                Print("Stopped program because of exception \n" + ex.ToString());
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
                //.UseUrls("http://*:5000") // 指定預設 Port 5000
                .UseConfiguration(_config) // 讀取 hosting.json 裡面設定的 Port 60000 與 60001
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
