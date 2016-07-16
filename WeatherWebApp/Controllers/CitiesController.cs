using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ninject;
using WeatherWebApp.Context;
using WeatherWebApp.Managers;
using WeatherWebApp.Models;

namespace WeatherWebApp.Controllers
{
    public class CitiesController : Controller
    {
        private readonly WeatherContext _db = new WeatherContext();

        [Inject]
        public WeatherManager WeatherManager { get; set; }

        // GET: Cities
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowUserCities(int userId)
        {
            ViewData["UserId"] = userId;
            ViewData["FavoriteCities"] = WeatherManager.GetUsersFavoriteCities(userId);
            return View("FavoriteCities");
        }

        public ActionResult AddCity(string cityName, int userId)
        {
            WeatherManager.AddCityToUser(cityName, userId);
            ViewData["FavoriteCities"] = WeatherManager.GetUsersFavoriteCities(userId);
            return View("FavoriteCities");
        }

        // GET: Cities/Delete/5
        public ActionResult Delete(int cityId, int userId)
        {
            var cityToRemove = _db.UserCities.First(uc => uc.CityId == cityId && uc.UserId == userId);
            _db.UserCities.Remove(cityToRemove);
            _db.SaveChanges();
            ViewData["FavoriteCities"] = WeatherManager.GetUsersFavoriteCities(userId);
            return View("FavoriteCities");
        }

        public ActionResult WatchLog(int userId)
        {
            ViewData["UserId"] = userId;
            return View(WeatherManager.GetUserLog(userId));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
