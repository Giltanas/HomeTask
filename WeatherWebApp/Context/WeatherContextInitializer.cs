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
            City city = new City() {Name = "Kiev"};
            City city1 = new City() { Name = "Lvov" };
            City city2 = new City() { Name = "Kharkov" };
            context.Cities.Add(city);
            context.Cities.Add(city1);
            context.Cities.Add(city2);
            context.SaveChanges();
        }
    }
}