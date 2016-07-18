using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using WeatherWebApp.Models;
using WeatherWebApp.ViewModels;

namespace WeatherWebApp.Context
{
    public class WeatherContext : IdentityDbContext<User>
    {
        public WeatherContext() : base("WeatherContext")
        {
            Database.SetInitializer<WeatherContext>(new WeatherContextInitializer());
        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<City> Cities { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
              .HasMany(u => u.Cities)
              .WithMany(c => c.Users)
              .Map(us =>
              {
                  us.MapLeftKey("UserId");
                  us.MapRightKey("CityId");
                  us.ToTable("UserCity");
              });
        }


    }
}