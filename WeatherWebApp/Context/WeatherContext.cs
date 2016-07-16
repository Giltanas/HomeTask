using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WeatherWebApp.Models;

namespace WeatherWebApp.Context
{
    public class WeatherContext : DbContext
    {
        public WeatherContext() : base()
        {
            Database.SetInitializer<WeatherContext>(new WeatherContextInitializer());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserCity> UserCities { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCity>().HasKey(e => new { e.UserId, e.CityId });
        }

        public DbSet<UserWeatherLog> Logs { get; set; }
        public System.Data.Entity.DbSet<WeatherWebApp.Models.City> Cities { get; set; }
    }
}