using System;
using System.Linq;
using WeatherView.Data;

namespace WeatherView.Models
{
    public class WeatherConditions
    {
        public float Temperature { get; set; }
        public float MinTemperature { get; set; }
        public float MaxTemperature { get; set; }
        public string Description { get; set; }
        public string IconId { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }

        public static WeatherConditions FromJsonObject(WeatherConditionsJson json)
        {
            return new WeatherConditions
            {
                CityName = json.name,
                CountryCode = json.sys.country,
                Description = json.weather.FirstOrDefault()?.main,
                IconId = json.weather.FirstOrDefault()?.icon,
                Temperature = (float)Math.Round(json.main.temp, 1),
                MinTemperature = json.main.temp_min,
                MaxTemperature = json.main.temp_max
            };
        }

        public static WeatherConditions Random(string cityName, string countryCode)
        {
            var rnd = new Random();
            var temp = rnd.Next(-20, 40);
            var desc = rnd.Next(0, 3);
            return new WeatherConditions
            {
                CityName = cityName,
                CountryCode = countryCode,
                Description = new[] { "Clear", "Raining", "Thunderstorms", "Cloudy" }[desc],
                IconId = new[] {"01d","10d","11d","03d"}[desc],
                Temperature = temp,
                MinTemperature = rnd.Next(temp, 40),
                MaxTemperature = rnd.Next(-20, temp)
            };
        }
    }
}