using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Ninject;
using WeatherWebApp.Context;
using WeatherWebApp.Managers;
using WeatherWebApp.Models;
using WeatherWebApp.Models.Logger;
using WeatherWebApp.ViewModels;

namespace WeatherWebApp.Controllers
{
    public class HomeController : Controller
    {
        private AppUserManager _userManager;

        [Inject]
        public ILogger Logger { get; set; }

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
        [HttpGet]
        public ActionResult Index()
        {
            Logger.Log(LogLevel.Debug, "Getting start page");
            var rootObject = WeatherManager.GetCountWeathersByCity("Kiev", 1);
            try
            {
                ViewData["ListFavoriteCities"] = AppUserManager.FindById(User.Identity.GetUserId()).Cities;
            }
            catch (Exception)
            {

                ViewData["ListFavoriteCities"] = new List<City>() {new City() {Name = "Kiev"} };
            }
            return View("Index", rootObject);
        }

        public ActionResult ShowSomeDayWeather(int count, string city)
        {
            Logger.Log(LogLevel.Debug, $"Getting page with weather in {city} for {count} days");
            var rootObject = WeatherManager.GetCountWeathersByCity(city, count) ??
                             WeatherManager.GetCountWeathersByCity("Kiev", 1);
            //ViewData["ListFavoriteCities"] = AppUserManager.FindById(User.Identity.GetUserId()).Cities;
            var user = AppUserManager.FindById(User.Identity.GetUserId());
            Request.GetOwinContext().Get<WeatherContext>().Entry(user).State = EntityState.Detached;
            user.AddLog(city);
            Request.GetOwinContext().Get<WeatherContext>().Users.Attach(user);
            ViewData["ListFavoriteCities"] = user.Cities;
            return View("Index", rootObject);
        }

        public ActionResult SearchCityWeather(string cityName)
        {
            var user = AppUserManager.FindById(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                Logger.Log(LogLevel.Debug, $"Getting page with weather in one of custom cities for days");
                var rootObject = WeatherManager.GetCountWeathersByCity(cityName, 1) ??
                            WeatherManager.GetCountWeathersByCity("Kiev", 1);
                Request.GetOwinContext().Get<WeatherContext>().Entry(user).State = EntityState.Detached;
                user.AddLog(cityName);
                ViewData["ListFavoriteCities"] = user.Cities;
                return View("Index", rootObject);
            }
            Logger.Log(LogLevel.Debug, $"Getting page with weather in city with wrong name");
            ViewData["ListFavoriteCities"] = user.Cities;
            return View("Index", WeatherManager.GetCountWeathersByCity("Antananarivo", 1));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel login)
        {

                var signInManager = Request.GetOwinContext().Get<UserSignInManager>();
                var result = signInManager.PasswordSignIn(login.Email, login.Password, shouldLockout: false, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.LockedOut:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("Index", "Home");
                default:
                    ModelState.AddModelError("", "Error");
                    return RedirectToAction("Login", "Home"); ;
            }
        }
        [HttpGet]
        public ActionResult Registrate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrate(UserRegistrationViewModel vm)
        {
            var user = new User() { UserName = vm.Email,Email = vm.Email};
            WeatherManager.AddDefaultCities(user);
            var result = AppUserManager.Create(user,vm.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Registrate", "Home");
        }
    }
}