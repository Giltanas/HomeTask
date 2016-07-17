using System;

namespace WeatherWebApp.ViewModels
{
    public class ViewModelUserWeatherLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }
        public DateTime Date { get; set; }
        public string CityName { get; set; }
    }
}