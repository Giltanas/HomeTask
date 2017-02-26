using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        [JsonObject(Title = "Temp")]
        public class Temperature
        {
            public double Day { get; set; }
            public double Min { get; set; }
            public double Max { get; set; }
            public double Night { get; set; }
            public double Eve { get; set; }
            public double Morn { get; set; }
        }

        public class OneDayIntervalWeather
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
        [JsonObject(Title = "List")]
        public class OneDayWeather
        {
            private Temperature _temperature;
            public int Dt { get; set; }

            public Temperature Temperature
            {
                get { return _temperature ?? new Temperature(); }
                set { _temperature = value; }
            }

            public double Pressure { get; set; }
            public int Humidity { get; set; }
            public List<OneDayIntervalWeather> Weather { get; set; }
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
        [JsonObject(Title = "RootObject")]
        public class WeatherContainer
        {
            public City City { get; set; }
            public string Cod { get; set; }
            public double Message { get; set; }
            public int Cnt { get; set; }

            public List<OneDayWeather> AllDaysWeatherList { get; set; }

            [Required(ErrorMessage = "Please,enter city name")]
            public string CityName { get; set; }
        }
    }
}