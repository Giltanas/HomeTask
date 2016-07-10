using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace WeatherWebApp.Models.Logger
{
    public class DebugConsoleLogger : ILogger
    {
        public void Log(LogLevel logLevel, string text)
        {
            Debug.WriteLine($"{logLevel} : {text}");
        }
    }
}