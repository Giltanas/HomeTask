using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using WeatherWebApp.Models;
using Image = System.Web.UI.WebControls.Image;

namespace WeatherWebApp.Managers
{
    public class WeatherManager
    {
        //public WeatherInfo.List GetCurentDayWeatherByCity(string city)
        //{
        //    string url = "http://api.openweathermap.org/data/2.5/forecast/daily?q=" + city + "&units=metric&cnt=1&APPID=da93bc68b89c625fc87562aa5ed53377";
        //    var webClient = new WebClient();
        //    var result = webClient.DownloadString(url);
        //    var a = JsonConvert.DeserializeObject<WeatherInfo.RootObject>(result);
        //    return a.List[0];
        //}

        public static WeatherInfo.RootObject GetCountWeathersByCity(string city, int count)
        {
            string url = "http://api.openweathermap.org/data/2.5/forecast/daily?q=" + city + "&units=metric&cnt=" +
                         count + "&APPID=da93bc68b89c625fc87562aa5ed53377";
            var webClient = new WebClient();
            var result = webClient.DownloadString(url);
            return JsonConvert.DeserializeObject<WeatherInfo.RootObject>(result);
        }
    }
}