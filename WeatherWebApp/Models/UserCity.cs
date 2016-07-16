using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherWebApp.Models
{
    public class UserCity
    {

        public int UserId { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public virtual User User { get; set; }
        public string CityName { get; set; }
    }
}