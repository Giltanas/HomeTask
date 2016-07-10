using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Ninject;
using WeatherWebApp.Managers;
using WeatherWebApp.Models;
using WeatherWebApp.Models.Logger;

namespace WeatherWebApp.Controllers
{
    public class HomeController : Controller
    {
        [Inject]
        public ILogger Logger { get; set; }

        [HttpGet]
        public ActionResult Index()
        {
            Logger.Log(LogLevel.Debug, "Getting start page");
            return View(WeatherManager.GetCountWeathersByCity("Kiev", 1));
        }

        public ActionResult ShowSomeDayWeather(int count, string city)
        {
            Logger.Log(LogLevel.Debug, $"Getting page with weather in {city} for {count} days");
            return View("Index", WeatherManager.GetCountWeathersByCity(city, count));
        }

        public ActionResult SearchCityWeather(WeatherInfo.RootObject root)
        {
            if (ModelState.IsValid)
            {
                Logger.Log(LogLevel.Debug, $"Getting page with weather in one of custom cities for days");
                return View("Index", WeatherManager.GetCountWeathersByCity(root.CityName, 1));
            }
            Logger.Log(LogLevel.Debug, $"Getting page with weather in city with wrong name");
            return View("Index", WeatherManager.GetCountWeathersByCity("Antananarivo", 1));
        }
    }
}