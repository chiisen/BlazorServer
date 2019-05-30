using BlazorServer.Shared;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using static BlazorServer.Server.Log;

namespace BlazorServer.Server.Controllers
{
    /// <summary>
    /// http://localhost:port/swagger 可以看到 swagger UI
    /// </summary>
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        /// <summary>
        /// http://localhost:port/profiler/results 可以看到 MiniProfiler
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string url1 = string.Empty;
            string url2 = string.Empty;
            using (MiniProfiler.Current.Step("Get方法"))
            {
                using (MiniProfiler.Current.Step("準備數據"))
                {
                    using (MiniProfiler.Current.CustomTiming("SQL", "SELECT * FROM Config"))
                    {
                        // 模擬一個SQL查詢
                        Thread.Sleep(500);

                        url1 = "https://www.baidu.com";
                        url2 = "https://www.sina.com.cn/";
                    }
                }


                using (MiniProfiler.Current.Step("使用從數據庫中查詢的數據，進行Http請求"))
                {
                    using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url1))
                    {
                        var client = new WebClient();
                        var reply = client.DownloadString(url1);
                    }

                    using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url2))
                    {
                        var client = new WebClient();
                        var reply = client.DownloadString(url2);
                    }
                }
            }
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 氣溫感受狀況
        /// </summary>
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        /// <summary>
        /// 取得氣溫資料列表
        /// </summary>
        /// <returns>回傳氣溫資料陣列給用戶</returns>
        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            Print("收到 Client 呼叫 api/SampleData/WeatherForecasts");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
    }
}
