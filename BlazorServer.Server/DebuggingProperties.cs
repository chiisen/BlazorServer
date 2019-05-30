using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BlazorServer.Server
{
    /// <summary>
    /// 包含在運行時判斷編譯器編譯配置中調試信息相關的屬性。
    /// </summary>
    public static class DebuggingProperties
    {
        /// <summary>
        /// 檢查當前正在運行的主程序是否是在 Debug 配置下編譯生成的。
        /// </summary>
        public static bool IsDebug
        {
            get
            {
                if (_isDebugMode == null)
                {
                    var assembly = Assembly.GetEntryAssembly();
                    if (assembly == null)
                    {
                        // 由於調用 GetFrames 的 StackTrace 實例沒有跳過任何幀，所以 GetFrames() 一定不為 null。
                        assembly = new StackTrace().GetFrames().Last().GetMethod().Module.Assembly;
                    }

                    var assemblyConfigurationAttribute = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
                    _isDebugMode = assemblyConfigurationAttribute.Configuration.Equals("Debug");
                }

                return _isDebugMode.Value;
            }
        }

        private static bool? _isDebugMode;

        /// <summary>
        /// 檢查當前正在運行的主程序配置。
        /// </summary>
        public static string Config
        {
            get
            {
                if (_ConfigAttribute == null)
                {
                    var assembly = Assembly.GetEntryAssembly();
                    if (assembly == null)
                    {
                        // 由於調用 GetFrames 的 StackTrace 實例沒有跳過任何幀，所以 GetFrames() 一定不為 null。
                        assembly = new StackTrace().GetFrames().Last().GetMethod().Module.Assembly;
                    }

                    var assemblyConfigurationAttribute = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
                    _ConfigAttribute = assemblyConfigurationAttribute.Configuration;
                }

                return _ConfigAttribute;
            }
        }

        private static string _ConfigAttribute;
    }
}
