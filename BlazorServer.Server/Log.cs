using BlazorServer.Server.Controllers;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System.Diagnostics;
using System.IO;

namespace BlazorServer.Server
{
    public class Log
    {
        public enum LOGGER
        {
            Trace,
            Debug,
            Info,
            Warn,
            Error,
            Fatal,
        }

        private static Logger _logger = null;
        public static void SetupLogger()
        {
            if (_logger == null)
            {
                _logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            }
        }

        /*
        private static ILogger<SameController> _ilogger;
        public static ILogger<SameController> SetupTheIlogger
        {
            set { _ilogger = value; }
        }
        */

        public static void Print(string msg, LOGGER l = LOGGER.Warn)
        {
            // 新增 印出 程式碼的行號
            StackFrame SF_ = new StackFrame(1, true);
            string fileName_ = SF_.GetFileName();
            string methodName_ = SF_.GetMethod().ToString();
            int lineNumber_ = SF_.GetFileLineNumber();

            string msg_ = string.Format("{0}({1}:{2})\n{3}", methodName_, Path.GetFileName(fileName_), lineNumber_, msg);

            SwitchLoggerLevel(msg_, l);
        }

        private static void SwitchLoggerLevel(string msg, LOGGER l)
        {
            switch (l)
            {
                case LOGGER.Trace:
                    {
                        _logger.Trace(msg);
                        break;
                    }
                case LOGGER.Debug:
                    {
                        _logger.Debug(msg);
                        break;
                    }
                case LOGGER.Info:
                    {
                        _logger.Info(msg);
                        break;
                    }
                case LOGGER.Warn:
                    {
                        _logger.Warn(msg);
                        break;
                    }
                case LOGGER.Error:
                    {
                        _logger.Error(msg);
                        break;
                    }
                case LOGGER.Fatal:
                    {
                        _logger.Fatal(msg);
                        break;
                    }
                default:
                    {
                        break;
                    }
            };
        }
    }
}
