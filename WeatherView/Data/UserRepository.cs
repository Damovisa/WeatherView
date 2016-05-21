using System.Collections.Generic;
using System.Drawing;
using WeatherView.Models;

namespace WeatherView.Data
{
    internal class UserRepository
    {
        private List<User> _users =
        new List<User>
        {
            new User { Id=1, CityName = "brisbane", CountryCode = "au", Name = "Damian", FavouriteColor = Color.Navy},
            new User { Id=2, CityName = "melbourne", CountryCode = "au", Name = "Frank", FavouriteColor = Color.PaleGoldenrod},
            new User { Id=3, CityName = "oslo", CountryCode = "no", Name = "Gerard", FavouriteColor = Color.Aquamarine},
            new User { Id=4, CityName = "bergen", CountryCode = "no", Name = "Simon", FavouriteColor = Color.Teal},
            new User { Id=5, CityName = "london", CountryCode = "uk", Name = "Veronica", FavouriteColor = Color.Chocolate},
            new User { Id=6, CityName = "cambridge", CountryCode = "uk", Name = "Ina", FavouriteColor = Color.DeepPink},
            new User { Id=7, CityName = "chicago", CountryCode = "us", Name = "Sofie", FavouriteColor = Color.DarkMagenta},
            new User { Id=8, CityName = "seattle", CountryCode = "us", Name = "Henrik", FavouriteColor = Color.DarkGoldenrod},
            new User { Id=9, CityName = "dallas", CountryCode = "us", Name = "Paul", FavouriteColor = Color.LawnGreen},
            new User { Id=10, CityName = "sydney", CountryCode = "us", Name = "Lisa", FavouriteColor = Color.Plum},
            new User { Id=11, CityName = "auckland", CountryCode = "nz", Name = "Bruce", FavouriteColor = Color.BurlyWood},
            new User { Id=12, CityName = "barcelona", CountryCode = "es", Name = "Karoline", FavouriteColor = Color.Gainsboro}
        };

        private readonly WeatherService _weatherService = new WeatherService();

        public User GetUserProfile(int id)
        {
            var user = _users[id-1];
            return user;
        }
    }
}