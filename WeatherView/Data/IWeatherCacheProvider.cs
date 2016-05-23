using System.Collections.Generic;
using WeatherView.Models;

namespace WeatherView.Data
{
    public interface IWeatherCacheProvider
    {
        Dictionary<string, WeatherConditions> WeatherCacheDictionary { get; }
        void AddToCacheDictionary(string key, WeatherConditions conditions);
    }

    public class WeatherNoCacheProvider : IWeatherCacheProvider
    {
        private Dictionary<string, WeatherConditions> _zeroCache;

        public WeatherNoCacheProvider()
        {
            _zeroCache = new Dictionary<string, WeatherConditions>();
        }
        public Dictionary<string, WeatherConditions> WeatherCacheDictionary
        {
            get { return _zeroCache; }
        }

        public void AddToCacheDictionary(string key, WeatherConditions conditions)
        {
            // do nothing
        }
    }
}