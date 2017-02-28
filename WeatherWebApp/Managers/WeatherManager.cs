using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;
using Newtonsoft.Json;
using Ninject;
using WeatherWebApp.Container;
using WeatherWebApp.Context;
using WeatherWebApp.Models;
using WeatherWebApp.Models.Dt;
using WeatherWebApp.Models.Logger;
using WeatherWebApp.ViewModels;

namespace WeatherWebApp.Managers
{
    public class WeatherManager
    {
        private readonly ILogger _logger;

        public WeatherManager(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<WeatherInfo.WeatherContainer> GetCountWeathersByCityAsync(string city, int count)
        {

            for (var i = 0; i < 5; i++)
            {
                string url = "http://api.openweathermap.org/data/2.5/forecast/daily?q=" + city + "&units=metric&cnt=" +
                             count + "&APPID=da93bc68b89c625fc87562aa5ed53377";

                try
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetStringAsync(url);
                        var weatherContainerDto = JsonConvert.DeserializeObject<WeatherContainerDto>(result);
                        var weatherContainer = new WeatherInfo.WeatherContainer().FromDto(weatherContainerDto);
                        _logger.Log(LogLevel.Info, $"Successfully got weather in {city}  for {count} days");
                        
                        return weatherContainer;
                    }
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Error, e.Message);
                    if (i != 4)
                    {
                        _logger.Log(LogLevel.Warning,
                            $"Weather wanst got in {city}  for {count} days, retry in 2 seconds");
                        System.Threading.Thread.Sleep(2000);
                    }
                }
            }
            _logger.Log(LogLevel.Error, $"Connectrion problems");
            return null;
        }

        public async Task AddDefaultCitiesAsync(User user)
        {
            using (var context = new WeatherContext())
            {
                user.Cities = new List<City>
                {
                    new City() {Name = "Kiev"},
                new City() { Name = "Lvov" },
                 new City() { Name = "Irpin" }
            };
                await context.SaveChangesAsync();
            }
        }

        public async Task<City> GetCityByNameOrAddNewCityAsync(WeatherContext context,string cityName)
        {
                var city = await context.Cities.FirstOrDefaultAsync(c => c.Name.Equals(cityName));
                if (city == null)
                {
                    city = new City() { Name = cityName };
                    context.Cities.Add(city);
                    await context.SaveChangesAsync();
                }
                return city;           
        }
        public async Task<User> WriteLogAsync(User user, bool isAutentificated, WeatherContext context, string city)
        {
              return  await user.AddLog(city, context);
        }
        public List<City> GetLoggedUserFavoriteCities(User user, bool isAutentificated)
        {
            if (isAutentificated)
            {
                return user.Cities.ToList();
            }
            return new List<City>() { new City() { Name = "Kiev" }, new City() { Name = "Lvov" }, new City() { Name = "Kharkov" } };
        }

        public async Task<bool> AddUserCityAsync(User user, City city, WeatherContext context)
        {
           
            context.Cities.Attach(city);
            context.Users.Attach(user);

            if (user.Cities.Contains(city))
            {
                return false;
            }

            user.Cities.Add(city);
            city.Users.Add(user);

            await context.SaveChangesAsync();
            return true;
        }
        public async Task RemoveUserCityAsync(User user, City city, WeatherContext context)
        {
            context.Cities.Attach(city);
            context.Users.Attach(user);

            user.Cities.Remove(city);
            city.Users.Remove(user);

            await context.SaveChangesAsync();
        }
    }
}