using System.Web.Mvc;
using WeatherView.Data;
using WeatherView.Models;

namespace WeatherView.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserRepository _userRepo;
        private readonly WeatherService _weatherService;

        public HomeController()
        {
            _userRepo = new UserRepository();
            _weatherService = new WeatherService();
        }

        // GET: Home
        public ActionResult Index(int userId = 1)
        {
            var profile = _userRepo.GetUserProfile(userId);
            var conditions = _weatherService.GetWeatherForCityAndCountry(profile.CityName, profile.CountryCode);
            var viewModel = new PersonalWeatherViewModel
            {
                Name = profile.Name,
                CityName = profile.CityName,
                CountryCode = profile.CountryCode,
                Temperature = conditions.Temperature,
                WeatherDescription = conditions.Description,
                Icon = conditions.IconId
            };

            return View(viewModel);
        }

    }
}