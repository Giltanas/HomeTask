using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WeatherWebApp.Models.Dt
{
    [JsonObject(Title = "RootObject")]
    public class WeatherContainerDto
    {
        public CityDto city { get; set; }
        public string cod { get; set; }
        public double message { get; set; }
        public int cnt { get; set; }
        public List<OneDayWeatherDto> list { get; set; }

    }
    [JsonObject(Title = "Coord")]
    public class CoordDto
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    [JsonObject(Title = "City")]
    public class CityDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public CoordDto coord { get; set; }
        public string country { get; set; }
        public int population { get; set; }
    }
    [JsonObject(Title = "Temp")]
    public class TemperatureDto
    {
        public double day { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }
    [JsonObject(Title = "Weather")]
    public class WeatherDto
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
    [JsonObject(Title = "List")]
    public class OneDayWeatherDto
    {
        public int dt { get; set; }
        public TemperatureDto temp { get; set; }
        public double pressure { get; set; }
        public int humidity { get; set; }
        public List<WeatherDto> weather { get; set; }
        public double speed { get; set; }
        public int deg { get; set; }
        public int clouds { get; set; }
    }
}