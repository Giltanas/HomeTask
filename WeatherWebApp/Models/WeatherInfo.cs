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
using WeatherWebApp.Models.Dt;

namespace WeatherWebApp.Models
{
    public class WeatherInfo
    { 
        public class Coord
        {
            public double Lon { get; set; }
            public double Lat { get; set; }

            public Coord FromDto(CoordDto dto)
            {
                Lon = dto.lon;
                Lat = dto.lat;
                return this;
            }
        }

        public class City
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Coord Coord { get; set; }
            public string Country { get; set; }


            public City FromDto(CityDto dto)
            {
                Id = dto.id;
                Name = dto.name;
                Coord = new Coord().FromDto(dto.coord);
                Country = dto.country;
                return this;
            }
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

            public Temperature FromDto(TemperatureDto dto)
            {
                Day = dto.day;
                Min = dto.min;
                Max = dto.max;
                Night = dto.night;
                Eve = dto.eve;
                Morn = dto.morn;
                return this;
            }
        }

        public class Weather
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }

            public string IconLink()
            {
                return $"http://openweathermap.org/img/w/{Icon}.png";
            }

            public Weather FromDto(WeatherDto dto)
            {
                Id = dto.id;
                Description = dto.description;
                Icon = dto.icon;
                return this;
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

            public List<Weather> Weather { get; set; } = new List<Weather>();

            public double WindSpeed { get; set; }
            public int WindDeg { get; set; }
            public int Clouds { get; set; }
            
            public DateTime Date()
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return dtDateTime.AddSeconds(Dt).ToLocalTime();
            }

            public int PressureBar()
            {
                return (int)Pressure*3/4;
            }

            public OneDayWeather FromDto(OneDayWeatherDto dto)
            {
                Temperature = new Temperature().FromDto(dto.temp);
                Pressure = dto.pressure;
                Humidity = dto.humidity;
                WindSpeed = dto.speed;
                Clouds = dto.clouds;
                Dt = dto.dt;
                foreach (var weatherDto in dto.weather)
                {
                    Weather.Add(new Weather().FromDto(weatherDto));
                }
                return this;
            }
        }
        [JsonObject(Title = "RootObject")]
        public class WeatherContainer
        {
            public City City { get; set; }
            //public string Cod { get; set; }
            //public double Message { get; set; }
            //public int Cnt { get; set; }

            public List<OneDayWeather> AllDaysWeatherList { get; set; } = new List<OneDayWeather>();

            [Required(ErrorMessage = "Please,enter city name")]
            public string CityName { get; set; }

            public WeatherContainer FromDto(WeatherContainerDto dto)
            {
                City = new City().FromDto(dto.city);
                foreach (var oneDayWeatherDto in dto.list)
                {
                    AllDaysWeatherList.Add(new OneDayWeather().FromDto(oneDayWeatherDto));
                }
                return this;
            }
        }
    }
}