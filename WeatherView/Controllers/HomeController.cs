using System;
using System.Configuration;
using System.Drawing;
using System.Web.Mvc;
using LaunchDarkly.Client;
using WeatherView.Data;
using WeatherView.FeatureFlags;
using WeatherView.Models;

namespace WeatherView.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IWeatherService _weatherService;
        private readonly IFeatureFlagProvider _ffProvider;

        public HomeController(IUserRepository userRepo,
            IWeatherService weatherService, IFeatureFlagProvider ffProvider)
        {
            _userRepo = userRepo;
            _weatherService = weatherService;
            _ffProvider = ffProvider;
        }

        // GET: Home
        public ActionResult Index(int userId = 1)
        {
            var profile = _userRepo.GetUserProfile(userId);
            var conditions = _weatherService.GetWeatherForCityAndCountry(profile.CityName, profile.CountryCode);

            var bgColour = _ffProvider.Toggle("weather-background", userId, profile.Name);

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