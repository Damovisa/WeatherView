using System.Drawing;
using WeatherView.Data;

namespace WeatherView.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public Color FavouriteColor { get; set; }
    }
}