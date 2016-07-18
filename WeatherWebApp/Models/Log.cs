using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherWebApp.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
    }
}