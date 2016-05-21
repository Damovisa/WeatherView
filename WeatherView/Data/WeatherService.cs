using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using WeatherView.Models;

namespace WeatherView.Data
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherCacheProvider _weatherCacheProvider;
        private readonly string _apiKey;

        public WeatherService(IWeatherCacheProvider weatherCacheProvider)
        {
            _weatherCacheProvider = weatherCacheProvider;
            _apiKey = ConfigurationManager.AppSettings["WeatherApiKey"];
        }

        public WeatherConditions GetWeatherForCityAndCountry(string cityName, string countryCode)
        {
            var cityCountry = $"{cityName},{countryCode}";
            var weather = _weatherCacheProvider.WeatherCacheDictionary.ContainsKey(cityCountry) ? _weatherCacheProvider.WeatherCacheDictionary[cityCountry] : null;

            if (weather == null)
            {
                try
                {
                    var url = $"http://api.openweathermap.org/data/2.5/weather?q={cityCountry}&units=metric&APPID={_apiKey}";

                    var client = new HttpClient();
                    var response = client.GetAsync(url).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    weather = ParseWeather(result);
                    AddToDictionary(cityCountry, weather);
                }
                catch (Exception)
                {
                    weather = WeatherConditions.Random(cityName, countryCode);
                }
                return weather;
            }
            return weather;
        }

        public WeatherConditions ParseWeather(string json)
        {
            var conditions = JsonConvert.DeserializeObject<WeatherConditionsJson>(json);
            return WeatherConditions.FromJsonObject(conditions);
        }

        private void AddToDictionary(string cityCountry, WeatherConditions weather)
        {
           _weatherCacheProvider.AddToCacheDictionary(cityCountry, weather);
        }
    }


    public class WeatherConditionsJson
    {
        public Weather[] weather { get; set; }
        public Main main { get; set; }
        public Sys sys { get; set; }
        public string name { get; set; }
    }

    public class Sys
    {
        public string country { get; set; }
    }

    public class Main
    {
        public float temp { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
    }

    public class Weather
    {
        public string main { get; set; }
        public string icon { get; set; }
    }


    
}