using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        private readonly ILogger _logger;
        private readonly WeatherManager _weatherManager;

        public HomeController(ILogger logger, WeatherManager weatherManager)
        {
            _logger = logger;
            _weatherManager = weatherManager;
            
        }

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
        public async Task<ActionResult> Index()
        {
            _logger.Log(LogLevel.Debug, "Getting start page");
            var weatherContainer = await _weatherManager.GetCountWeathersByCityAsync("Kiev", 1);

            if (Request.IsAuthenticated)
            {
                ViewData["ListFavoriteCities"] = (await AppUserManager.FindByIdAsync(User.Identity.GetUserId())).Cities;
            }
            else
            {
                ViewData["ListFavoriteCities"] = new List<City>() { new City() { Name = "Kiev" }, new City() { Name = "Lvov" }, new City() { Name = "Kharkov" } };
            }

            return View("Index", weatherContainer);
        }
        //[HttpGet]
        public async Task<ActionResult> ShowSomeDayWeather(int count, string city)
        {
            _logger.Log(LogLevel.Debug, $"Getting page with weather in {city} for {count} days");

            var weatherContainer = await _weatherManager.GetCountWeathersByCityAsync(city, count) ??
                             await _weatherManager.GetCountWeathersByCityAsync("Kiev", 1);


            if (Request.IsAuthenticated)
            {
                var user = await AppUserManager.FindByIdAsync(User.Identity.GetUserId());
                await user.AddLog(city, Request.GetOwinContext().Get<WeatherContext>());
                ViewData["ListFavoriteCities"] = (await AppUserManager.FindByIdAsync(User.Identity.GetUserId())).Cities;
            }
            else
            {
                ViewData["ListFavoriteCities"] = new List<City>() { new City() { Name = "Kiev" }, new City() { Name = "Lvov" }, new City() { Name = "Kharkov" } };
            }
            return View("Index", weatherContainer);
        }
        //[HttpGet]
        public async Task<ActionResult> SearchCityWeather(string cityName)
        {
            var user = await AppUserManager.FindByIdAsync(User.Identity.GetUserId());
            bool isAutentificated = Request.IsAuthenticated;
            var weatherContainer = await _weatherManager.GetCountWeathersByCityAsync(cityName, 1) ??
                            await _weatherManager.GetCountWeathersByCityAsync("Kiev", 1);
            if (Request.IsAuthenticated)
            {
                _logger.Log(LogLevel.Debug, $"Getting page with weather in one of custom cities for days");

                await _weatherManager.WriteLogAsync(user, isAutentificated,
                    Request.GetOwinContext().Get<WeatherContext>(), cityName);
                user = await AppUserManager.FindByIdAsync(User.Identity.GetUserId());
                ViewData["ListFavoriteCities"] = user.Cities;
                return View("Index", weatherContainer);
            }
            _logger.Log(LogLevel.Debug, $"Getting page with weather in city with wrong name");
            ViewData["ListFavoriteCities"] = new List<City>() { new City() { Name = "Kiev" }, new City() { Name = "Lvov" }, new City() { Name = "Kharkov" } };
            return View("Index", weatherContainer);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var signInManager = Request.GetOwinContext().Get<UserSignInManager>();
                var result = signInManager.PasswordSignIn(login.Email, login.Password, shouldLockout: false,
                    isPersistent: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToAction("Index", "Home");
                    case SignInStatus.LockedOut:
                        return RedirectToAction("Index", "Home");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("Index", "Home");
                    default:
                        ModelState.AddModelError("", "Wrong email or password");
                        return View(login);
                }
            }
            return View(login);
        }
        [HttpGet]
        public ActionResult Registrate()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registrate(UserRegistrationViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new User() { UserName = vm.Email, Email = vm.Email };
                await _weatherManager.AddDefaultCitiesAsync(user);
                var result =  await AppUserManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(vm);
        }

        public ActionResult LogOut()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}