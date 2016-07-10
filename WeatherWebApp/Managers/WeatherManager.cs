using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Ninject;
using WeatherWebApp.Container;
using WeatherWebApp.Models;
using WeatherWebApp.Models.Logger;

namespace WeatherWebApp.Managers
{
    public class WeatherManager
    {
        private static readonly ILogger _logger = DependencyResolver.Current.GetService<ILogger>();
        //[Inject]
        //public static ILogger Logger { get; set; }

        public static WeatherInfo.RootObject GetCountWeathersByCity(string city, int count)
        {
            string url = "http://api.openweathermap.org/data/2.5/forecast/daily?q=" + city + "&units=metric&cnt=" +
                         count + "&APPID=da93bc68b89c625fc87562aa5ed53377";
            var webClient = new WebClient();
            var result = webClient.DownloadString(url);
            try
            {
                var rootObject = JsonConvert.DeserializeObject<WeatherInfo.RootObject>(result);
                for (int i = 0; i < 5; i++)
                {
                    if (rootObject != null)
                    {
                        _logger.Log(LogLevel.Info, $"Successfully got weather in {city}  for {count} days");
                        return rootObject;
                    }
                    _logger.Log(LogLevel.Warning, $"Weather wanst got in {city}  for {count} days, retry in 5 seconds");
                    System.Threading.Thread.Sleep(2000);
                    rootObject = JsonConvert.DeserializeObject<WeatherInfo.RootObject>(result);
                }

            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, $"City null reference");
                return new WeatherInfo.RootObject();
            }
            
            _logger.Log(LogLevel.Error, $"NULL Root Object");
            return new WeatherInfo.RootObject();
        }
    }
}