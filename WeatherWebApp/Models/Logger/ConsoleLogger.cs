using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherWebApp.Models.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel logLevel, string text)
        {
            Console.WriteLine($"{logLevel} : {text}");
        }
    }
}