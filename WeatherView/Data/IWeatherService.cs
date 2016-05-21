using WeatherView.Models;

namespace WeatherView.Data
{
    public interface IWeatherService
    {
        WeatherConditions GetWeatherForCityAndCountry(string cityName, string countryCode);
        WeatherConditions ParseWeather(string json);
    }
}