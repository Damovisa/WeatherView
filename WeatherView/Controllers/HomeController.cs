using System.Web.Mvc;
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
            var viewModel = new PersonalWeatherViewModel
            {
                Name = profile.Name,
                CityName = conditions.CityName,
                CountryCode = conditions.CountryCode,
                Temperature = conditions.Temperature,
                WeatherDescription = conditions.Description,
                Icon = conditions.IconId
            };

            return View(viewModel);
        }

    }
}