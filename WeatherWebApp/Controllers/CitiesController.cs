using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ninject;
using WeatherWebApp.Context;
using WeatherWebApp.Managers;
using WeatherWebApp.Models;

namespace WeatherWebApp.Controllers
{
    public class CitiesController : Controller
    {
        
        private AppUserManager _userManager;

        [Inject]
        public WeatherManager WeatherManager { get; set; }
        public AppUserManager AppUserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Cities
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ShowUserCities()
        {
            ViewData["FavoriteCities"] = AppUserManager.FindById(User.Identity.GetUserId()).Cities; 
            return View("FavoriteCities");
        }

        public ActionResult AddCity(string name)
        {
            var user = AppUserManager.FindById(User.Identity.GetUserId());
            Request.GetOwinContext().Get<WeatherContext>().Entry(user).State = EntityState.Detached;
            var city = WeatherManager.GetCityByName(name);
            using (var db = new WeatherContext())
            {

                db.Cities.Attach(city);
                db.Users.Attach(user);

                user.Cities.Add(city);
                city.Users.Add(user);

                db.SaveChanges();
            }
            ViewData["FavoriteCities"] = user.Cities;
            return View("FavoriteCities");
        }

        // GET: Cities/Delete/5
        public ActionResult Delete(string cityName)
        {
            var user = AppUserManager.FindById(User.Identity.GetUserId());
            Request.GetOwinContext().Get<WeatherContext>().Entry(user).State = EntityState.Detached;
            var city = WeatherManager.GetCityByName(cityName);

            using (var db = new WeatherContext())
            {
                
                user.Cities.Remove(city);
                city.Users.Remove(user);
                db.SaveChanges();
            }
            ViewData["FavoriteCities"] = AppUserManager.FindById(User.Identity.GetUserId()).Cities;
            return View("FavoriteCities");
        }

        public ActionResult WatchLog()
        {
            var user = AppUserManager.FindById(User.Identity.GetUserId());
            return View();
        }
    }
}
