using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Ninject;
using WeatherWebApp.Context;
using WeatherWebApp.Managers;
using WeatherWebApp.Models;
using WeatherWebApp.Models.Logger;

namespace WeatherWebApp.Controllers
{
    public class HomeController : Controller
    {
        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public WeatherManager WeatherManager { get; set; }

    //    public  User User = new User() {Id = 1};

        [HttpGet]
        public ActionResult Index()
        {
            Logger.Log(LogLevel.Debug, "Getting start page");
            var rootObject = WeatherManager.GetCountWeathersByCity("Kiev", 1);
            ViewData["ListFavoriteCities"] = WeatherManager.GetUsersFavoriteCities(1);
            return View("Index", rootObject);
        }

        public ActionResult ShowSomeDayWeather(int count, string city)
        {
            Logger.Log(LogLevel.Debug, $"Getting page with weather in {city} for {count} days");
            var rootObject = WeatherManager.GetCountWeathersByCity(city, count) ??
                             WeatherManager.GetCountWeathersByCity("Kiev", 1);
            ViewData["ListFavoriteCities"] = WeatherManager.GetUsersFavoriteCities(1);
            WeatherManager.AddUserWeatherLog(1, city);
            return View("Index", rootObject);
        }

        public ActionResult SearchCityWeather(WeatherInfo.RootObject root)
        {
            if (ModelState.IsValid)
            {
                Logger.Log(LogLevel.Debug, $"Getting page with weather in one of custom cities for days");
                var rootObject = WeatherManager.GetCountWeathersByCity(root.CityName, 1) ??
                            WeatherManager.GetCountWeathersByCity("Kiev", 1);
                WeatherManager.AddUserWeatherLog(1, root.CityName);
                ViewData["ListFavoriteCities"] = WeatherManager.GetUsersFavoriteCities(1);
                return View("Index", rootObject);
            }
            Logger.Log(LogLevel.Debug, $"Getting page with weather in city with wrong name");
            ViewData["ListFavoriteCities"] = WeatherManager.GetUsersFavoriteCities(1);
            return View("Index", WeatherManager.GetCountWeathersByCity("Antananarivo", 1));
        }
    }
}