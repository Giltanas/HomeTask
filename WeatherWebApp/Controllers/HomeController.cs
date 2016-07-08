using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WeatherWebApp.Managers;
using WeatherWebApp.Models;

namespace WeatherWebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(WeatherManager.GetCountWeathersByCity("Kiev", 1));
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult ShowSomeDayWeather(int count, string city)
        {
            return View("Index", WeatherManager.GetCountWeathersByCity(city, count));
        }


        public ActionResult SearchCityWeather(WeatherInfo.RootObject root)
        {
            if (ModelState.IsValid)
            {
                return View("Index", WeatherManager.GetCountWeathersByCity(root.CityName, 1));
            }
            return View("Index", WeatherManager.GetCountWeathersByCity("Antananarivo", 1));
        }
    }
}