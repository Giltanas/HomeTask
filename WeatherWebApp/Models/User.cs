using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherWebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserCity> UserCities { get; set; }

    }
}