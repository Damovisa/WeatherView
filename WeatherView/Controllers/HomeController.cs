using System;
using System.Configuration;
using System.Drawing;
using System.Web.Mvc;
using LaunchDarkly.Client;
using WeatherView.Data;
using WeatherView.Models;

namespace WeatherView.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IWeatherService _weatherService;

        public HomeController(IUserRepository userRepo, IWeatherService weatherService)
        {
            _userRepo = userRepo;
            _weatherService = weatherService;
        }

        // GET: Home
        public ActionResult Index(int userId = 1)
        {
            var profile = _userRepo.GetUserProfile(userId);
            var conditions = _weatherService.GetWeatherForCityAndCountry(profile.CityName, profile.CountryCode);

            var lcClient = new LdClient(ConfigurationManager.AppSettings["LaunchDarklyApiKey"]);
            var user = new LaunchDarkly.Client.User(userId.ToString());
            user.Name = profile.Name;
            var bgColour = lcClient.Toggle("weather-background", user);

            var viewModel = new PersonalWeatherViewModel
            {
                Name = profile.Name,
                CityName = conditions.CityName,
                CountryCode = conditions.CountryCode,
                Temperature = Math.Round(conditions.Temperature, 1),
                WeatherDescription = conditions.Description,
                Icon = conditions.IconId ,
                BackgroundColor = bgColour ? ColorTranslator.ToHtml(profile.FavouriteColor) : "#eee"
            };

            return View(viewModel);
        }

    }
}