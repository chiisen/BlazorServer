using NLog;
using System.Diagnostics;
using System.IO;

namespace BlazorServer.Server
{
    public class Log
    {
        public static string _logFileName = "";
        static Logger _logger = null;

        public static Logger SetupTheLogger
        {
            set { _logger = value; }
        }

        public static void Print(string msg)
        {
            // 新增 印出 程式碼的行號
            StackFrame SF_ = new StackFrame(1, true);
            string fileName_ = SF_.GetFileName();
            string methodName_ = SF_.GetMethod().ToString();
            int lineNumber_ = SF_.GetFileLineNumber();

            string msg_ = string.Format("{0}({1}:{2})\n{3}", methodName_, Path.GetFileName(fileName_), lineNumber_, msg);

            _logger.Warn(msg_);
        }
    }
}
