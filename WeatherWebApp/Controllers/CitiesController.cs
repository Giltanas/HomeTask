using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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


        public async Task<ActionResult> ShowUserCities()
        {
            ViewData["FavoriteCities"] = (await AppUserManager.FindByIdAsync(User.Identity.GetUserId())).Cities; 
            return View("FavoriteCities");
        }

        public async Task<ActionResult> AddCity(string name)
        {
            var user = await AppUserManager.FindByIdAsync(User.Identity.GetUserId());
            Request.GetOwinContext().Get<WeatherContext>().Entry(user).State = EntityState.Detached;
            var city = await WeatherManager.GetCityByNameAsync(name);
            using (var db = new WeatherContext())
            {

                db.Cities.Attach(city);
                db.Users.Attach(user);

                user.Cities.Add(city);
                city.Users.Add(user);

                await db.SaveChangesAsync();
            }
            ViewData["FavoriteCities"] = user.Cities;
            return View("FavoriteCities");
        }

        // GET: Cities/Delete/5
        public async Task<ActionResult> Delete(string cityName)
        {
            var user = await AppUserManager.FindByIdAsync(User.Identity.GetUserId());
            Request.GetOwinContext().Get<WeatherContext>().Entry(user).State = EntityState.Detached;
            var city = await WeatherManager.GetCityByNameAsync(cityName);

            using (var db = new WeatherContext())
            {
                
                user.Cities.Remove(city);
                city.Users.Remove(user);
                await db.SaveChangesAsync();
            }
            ViewData["FavoriteCities"] = AppUserManager.FindById(User.Identity.GetUserId()).Cities;
            return View("FavoriteCities");
        }

        public async Task<ActionResult> WatchLog()
        {
            var user = await AppUserManager.FindByIdAsync(User.Identity.GetUserId());
            return View();
        }
    }
}
