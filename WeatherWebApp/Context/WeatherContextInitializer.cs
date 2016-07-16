using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WeatherWebApp.Managers;
using WeatherWebApp.Models;

namespace WeatherWebApp.Context
{
    public class WeatherContextInitializer : DropCreateDatabaseIfModelChanges<WeatherContext>
    {
        protected override void Seed(WeatherContext context)
        {
          User user = new User() {Name = "Vasja"};
            WeatherManager weatherManager = new WeatherManager();
            weatherManager.AddDefaultCities(user);   
        }
    }
}