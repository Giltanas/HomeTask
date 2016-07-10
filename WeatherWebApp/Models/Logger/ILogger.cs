using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWebApp.Models.Logger
{
    public interface ILogger
    {
        void Log(LogLevel logLevel, string text);
    }
}
