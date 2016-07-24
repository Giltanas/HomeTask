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

        [HttpPost]
        public async Task<ActionResult> AddCity(City modelCity)
        {
            var user = await AppUserManager.FindByIdAsync(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                
                Request.GetOwinContext().Get<WeatherContext>().Entry(user).State = EntityState.Detached;
                using (var db = new WeatherContext())
                {
                    var city = await WeatherManager.GetCityByNameOrAddNewCityAsync(db, modelCity.Name);
                    if (!await WeatherManager.AddUserCityAsync(user, city, db))
                    {
                        ModelState.AddModelError("","You have already added this city ");
                    }
                }
            }
            
            ViewData["FavoriteCities"] = user.Cities;
            return View("FavoriteCities",modelCity);
        }

        // GET: Cities/Delete/5
        public async Task<ActionResult> Delete(string cityName)
        {
            var user = await AppUserManager.FindByIdAsync(User.Identity.GetUserId());
            Request.GetOwinContext().Get<WeatherContext>().Entry(user).State = EntityState.Detached;

            using (var db = new WeatherContext())
            {
                var city = await WeatherManager.GetCityByNameOrAddNewCityAsync(db,cityName);
                await WeatherManager.RemoveUserCityAsync(user, city, db);
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
