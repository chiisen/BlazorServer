using BlazorServer.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static BlazorServer.Server.Log;

namespace BlazorServer.Server.Controllers
{
    /// <summary>
    /// 簡單的 Controller 範例
    /// </summary>
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
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
