using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using WeatherView.Models;

namespace WeatherView.Data
{
    public interface IWeatherCacheProvider
    {
        Dictionary<string, WeatherConditions> WeatherCacheDictionary { get; }
        void AddToCacheDictionary(string key, WeatherConditions conditions);
    }

    public class WeatherCacheProvider : IWeatherCacheProvider
    {
        private string _cacheFile;
        private Dictionary<string, WeatherConditions> _cacheDictionary;

        public WeatherCacheProvider()
        {
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

        public Dictionary<string, WeatherConditions> WeatherCacheDictionary
        {
            get { return _cacheDictionary; }
        }

        public void AddToCacheDictionary(string key, WeatherConditions conditions)
        {
            _cacheDictionary.Add(key, conditions);
            System.IO.File.WriteAllLines(_cacheFile,
               _cacheDictionary
               .Select(p => $"{p.Key}|{JsonConvert.SerializeObject(p.Value)}"));
        }
    }
}