namespace WeatherView.Models
{
    public class PersonalWeatherViewModel
    {
        public string Name { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public float Temperature { get; set; }
        public string WeatherDescription { get; set; }
        public string Icon { get; set; }
    }
}