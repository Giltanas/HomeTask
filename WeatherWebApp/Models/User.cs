using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject.Activation;
using WeatherWebApp.Context;
using WeatherWebApp.Managers;

namespace WeatherWebApp.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<City> Cities { get; set; }
        public virtual List<Log> Logs { get; set; }

        public void AddLog(string cityName)
        {
            if (Logs == null)
            {
                Logs = new List<Log>();
            }
            var log = new Log() { CityName = cityName, Date = DateTime.Now };
            
         //   Request.GetOwinContext().Get<WeatherContext>().Entry(user).State = EntityState.Detached;

            using (var db = new WeatherContext())
            {
                db.Logs.Attach(log);
                db.Users.Attach(this);            

                db.Logs.Add(log);
                Logs.Add(log);

                log.User = this;
                db.SaveChanges();
            }
        }
        }
}