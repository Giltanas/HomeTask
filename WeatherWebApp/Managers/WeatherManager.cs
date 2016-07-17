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

        public List<City> GetUsersFavoriteCities(int id)
        {

            using (var context = new WeatherContext())
            {
                var list = context.UserCities.Where(item => item.UserId == id);
                return  (from city in context.Cities from l in list where l.CityId == city.Id select city).ToList();
            }
        }

        public void AddCityToUser(string cityName, int userId)
        {
            using (var context = new WeatherContext())
            {
                if (!context.Cities.Any(c => c.Name == cityName))
                {
                    context.Cities.Add(new City() {Name = cityName});
                    context.SaveChanges();
                }
             

                var cityId = context.Cities.First(c => c.Name.Equals(cityName)).Id;
                if (!context.UserCities.Any(uc => uc.CityId == cityId && uc.UserId == userId))
                {
                    context.Users.First(u=> u.Id == userId).UserCities.Add(new UserCity() {UserId = userId,CityId = cityId});
                    context.SaveChanges();
                }

            }
        }

        public void AddUserWeatherLog(int userId, int cityId)
        {
            using (var context = new WeatherContext())
            {
                context.Logs.Add(new ViewModelUserWeatherLog()
                { UserId = userId,
                    CityId = cityId,
                    Date = DateTime.Now,
                    CityName = context.Cities.First(c=> c.Id== cityId).Name
                });
                context.SaveChanges();
            }
        }

        public void AddUserWeatherLog(int userId, string cityName)
        {
            int cityId;
            using (var context = new WeatherContext())
            {
                cityId = context.Cities.First(c=> c.Name.Equals(cityName)).Id;
            }
            AddUserWeatherLog(userId,cityId);
        }

        public void AddDefaultCities(User user)
        {
            var kiev = new City() {Name = "Kiev"};
            var lvov = new City() { Name = "Lvov" };
            var kharkov = new City() { Name = "Kharkov" };
            user.UserCities = new List<UserCity>();
            user.UserCities.Add(new UserCity() {User = user, City = kiev, CityName = kiev.Name});
            user.UserCities.Add(new UserCity() { User = user, City = lvov, CityName = lvov.Name });
            user.UserCities.Add(new UserCity() { User= user, City = kharkov, CityName = kharkov.Name });
            using (var context = new WeatherContext())
            {
                context.Cities.Add(kiev);
                context.Cities.Add(lvov);
                context.Cities.Add(kharkov);
                context.Users.Add(user);
                context.SaveChanges();
            }

        }

        public List<ViewModelUserWeatherLog> GetUserLog(int userId)
        {
            using (var context = new WeatherContext())
            {
                return context.Logs.Where(l => l.UserId == userId).ToList();
            }
        }
    }
}