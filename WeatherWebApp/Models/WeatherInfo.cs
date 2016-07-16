using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Web;

namespace WeatherWebApp.Models
{
    public class WeatherInfo
    { 
        public class Coord
        {
            public double Lon { get; set; }
            public double Lat { get; set; }
        }

        public class City
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Coord Coord { get; set; }
            public string Country { get; set; }
            public int Population { get; set; }
        }

        public class Temp
        {
            public double Day { get; set; }
            public double Min { get; set; }
            public double Max { get; set; }
            public double Night { get; set; }
            public double Eve { get; set; }
            public double Morn { get; set; }
        }

        public class Weather
        {
            public int Id { get; set; }
            public string Main { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }

            public string IconLink()
            {
                return "http://openweathermap.org/img/w/" + Icon + ".png";
            }
        }

        public class List
        {
            public int Dt { get; set; }
            public Temp Temp { get; set; }
            public double Pressure { get; set; }
            public int Humidity { get; set; }
            public List<Weather> Weather { get; set; }
            public double Speed { get; set; }
            public int Deg { get; set; }
            public int Clouds { get; set; }
            public double? Rain { get; set; }

            public DateTime Date()
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return dtDateTime.AddSeconds(Dt).ToLocalTime();
            }

            public int PressureBar()
            {
                return (int)Pressure*3/4;
            }
        }

        public class RootObject
        {
            public City City { get; set; }
            public string Cod { get; set; }
            public double Message { get; set; }
            public int Cnt { get; set; }
            public List<List> List { get; set; }

            [Required(ErrorMessage = "Please,enter city name")]
            public string CityName { get; set; }
        }
    }
}