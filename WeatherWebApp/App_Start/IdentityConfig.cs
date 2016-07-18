using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using WeatherWebApp.Context;
using WeatherWebApp.Managers;
using WeatherWebApp.Models;

[assembly: OwinStartup(typeof(WeatherWebApp.IdentityConfig))]

namespace WeatherWebApp
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new WeatherContext());
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            //app.CreatePerOwinContext<RoleManager<AppRole>>((options, context) =>
            //    new RoleManager<AppRole>(
            //        new RoleStore<AppRole>(context.Get<WeatherContext>())));
            app.CreatePerOwinContext<UserSignInManager>(UserSignInManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
            });
        }
    }
}
