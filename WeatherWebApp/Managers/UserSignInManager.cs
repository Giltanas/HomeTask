using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using WeatherWebApp.Models;

namespace WeatherWebApp.Managers
{
    public class UserSignInManager : SignInManager<User,string>
    {
        public UserSignInManager(UserManager<User, string> userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager) 
        {
        }
        public static UserSignInManager Create(IdentityFactoryOptions<UserSignInManager> options, IOwinContext context)
        {
            return new UserSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }
    }
}