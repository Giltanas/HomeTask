using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WeatherWebApp.Models.Logger
{
    public class TextLogger : ILogger
    {
        public void Log(LogLevel logLevel, string text)
        {
            using (var sw = new StreamWriter("log.txt", true))
            {
                sw.WriteLine($"{logLevel} : {text}");
            }
        }
    }
}