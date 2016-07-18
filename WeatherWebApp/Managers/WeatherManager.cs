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
using WeatherWebApp.Context;
using WeatherWebApp.Models;
using WeatherWebApp.Models.Logger;
using WeatherWebApp.ViewModels;

namespace WeatherWebApp.Managers
{
    public class WeatherManager
    {
        private static readonly ILogger Logger = DependencyResolver.Current.GetService<ILogger>();

        public WeatherInfo.RootObject GetCountWeathersByCity(string city, int count)
        {

            for (int i = 0; i < 5; i++)
            {
                string url = "http://api.openweathermap.org/data/2.5/forecast/daily?q=" + city + "&units=metric&cnt=" +
                             count + "&APPID=da93bc68b89c625fc87562aa5ed53377";
                var webClient = new WebClient();
                try
                {
                    var result = webClient.DownloadString(url);
                webClient.Dispose();
                
                    var rootObject = JsonConvert.DeserializeObject<WeatherInfo.RootObject>(result);
                    Logger.Log(LogLevel.Info, $"Successfully got weather in {city}  for {count} days");
                    return rootObject;
                }
                catch (Exception e)
                {
                    Logger.Log(LogLevel.Error, e.Message);
                    if (i != 4)
                    {
                        Logger.Log(LogLevel.Warning,
                            $"Weather wanst got in {city}  for {count} days, retry in 5 seconds");
                        System.Threading.Thread.Sleep(2000);
                    }        
                }
            }
            Logger.Log(LogLevel.Error, $"Connectrion problems");
            return null;
        }

        public void AddDefaultCities(User user)
        {
            using (var context = new WeatherContext())
            {
                user.Cities = new List<City>
                {
                    context.Cities.First()
                };
                context.SaveChanges();
            }
        }

        public City GetCityByName(string cityName)
        {
            try
            {
                using (var context = new WeatherContext())
                {
                    return context.Cities.First(c => c.Name.Equals(cityName));
                }
            }
            catch (Exception)
            {
                City city = new City() {Name = cityName};
                using (var context = new WeatherContext())
                {
                    context.Cities.Add(city);
                    context.SaveChanges();
                }
                return city;
            }   
        }
    }
}