using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Ninject.Activation;
using WeatherWebApp.Context;
using WeatherWebApp.Managers;

namespace WeatherWebApp.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<City> Cities { get; set; }
        public virtual List<Log> Logs { get; set; }

        public async Task<User> AddLog(string cityName, WeatherContext context)
        {
            if (Logs == null)
            {
                Logs = new List<Log>();
            }
            var log = new Log() { CityName = cityName, Date = DateTime.Now };
            
            context.Entry(this).State = EntityState.Detached;
            
            using (var db = new WeatherContext())
            {
                db.Logs.Attach(log);
                db.Users.Attach(this);            

                db.Logs.Add(log);
                Logs.Add(log);

                log.User = this;
                await db.SaveChangesAsync();
            }
            return this;
        }
        }
}