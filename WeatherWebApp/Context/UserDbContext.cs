using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using WeatherWebApp.Models;

namespace WeatherWebApp.Context
{
    public class UserDbContext : IdentityDbContext<User>
    {

    }
}