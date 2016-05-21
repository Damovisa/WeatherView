using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using WeatherView.Models;

namespace WeatherView.Data
{
    public class WeatherService
    {
        private readonly string _apiKey;
        private readonly Dictionary<string, WeatherConditions> _cacheDictionary;
        private readonly string _cacheFile;

        public WeatherService()
        {
            _apiKey = ConfigurationManager.AppSettings["WeatherApiKey"];
            _cacheFile = ConfigurationManager.AppSettings["WeatherCacheFile"];
            if (System.IO.File.Exists(_cacheFile))
            {
                _cacheDictionary = System.IO.File.ReadAllLines(ConfigurationManager.AppSettings["WeatherCacheFile"])
                    .ToDictionary(w => w.Split('|')[0], w => JsonConvert.DeserializeObject<WeatherConditions>(w.Split('|')[1]));
            }
            else
            {
                _cacheDictionary = new Dictionary<string, WeatherConditions>();
            }
        }

        public WeatherConditions GetWeatherForCityAndCountry(string cityName, string countryCode)
        {
            var cityCountry = $"{cityName},{countryCode}";
            var weather = _cacheDictionary.ContainsKey(cityCountry) ? _cacheDictionary[cityCountry] : null;

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

        public static WeatherConditions ParseWeather(string json)
        {
            var conditions = JsonConvert.DeserializeObject<WeatherConditionsJson>(json);
            return WeatherConditions.FromJsonObject(conditions);
        }

        private void AddToDictionary(string cityCountry, WeatherConditions weather)
        {
            _cacheDictionary.Add(cityCountry, weather);
            System.IO.File.WriteAllLines(_cacheFile,
                _cacheDictionary
                .Select(p => $"{p.Key}|{JsonConvert.SerializeObject(p.Value)}"));
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