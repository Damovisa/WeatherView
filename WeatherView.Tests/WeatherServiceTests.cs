using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Mvc;
using FluentAssertions;
using LaunchDarkly.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WeatherView.Controllers;
using WeatherView.Data;
using WeatherView.FeatureFlags;
using WeatherView.Models;
using User = WeatherView.Models.User;

namespace WeatherView.Tests
{
    [TestClass]
    public class WeatherServiceTests
    {
        [TestMethod]
        public void WeatherServiceShouldReturnValidWeatherFromJson()
        {
            var jsonString = "{\"coord\":{\"lon\":153.03,\"lat\":-27.47},\"weather\":[{\"id\":521,\"main\":\"Rain\",\"description\":\"shower rain\",\"icon\":\"09n\"}],\"base\":\"cmc stations\",\"main\":{\"temp\":20.36,\"pressure\":1024,\"humidity\":68,\"temp_min\":18.2,\"temp_max\":21.67},\"wind\":{\"speed\":2.6,\"deg\":140},\"clouds\":{\"all\":20},\"dt\":1463827650,\"sys\":{\"type\":1,\"id\":8164,\"message\":0.0033,\"country\":\"AU\",\"sunrise\":1463775884,\"sunset\":1463814244},\"id\":2174003,\"name\":\"Brisbane\",\"cod\":200}";
            var expectedWeather = new WeatherConditions
            {
                CountryCode = "AU",
                CityName = "Brisbane",
                Description = "Rain",
                Temperature = 20.36f,
                IconId = "09n",
                MaxTemperature = 21.67f,
                MinTemperature = 18.2f
            };
            var cacheProvider = Substitute.For<IWeatherCacheProvider>();
            cacheProvider.WeatherCacheDictionary.Returns(new Dictionary<string, WeatherConditions>());
            var weatherService = new WeatherService(cacheProvider);


            var parsed = weatherService.ParseWeather(jsonString);


            parsed.ShouldBeEquivalentTo(expectedWeather);
        }

        [TestMethod]
        public void ControllerShouldReturnCorrectWeatherInViewModel()
        {
            var userId = 1;
            var userRepo = Substitute.For<IUserRepository>();
            var weatherService = Substitute.For<IWeatherService>();
            var ffProvider = Substitute.For<IFeatureFlagProvider>();

            var expectedUser = new User
            {
                Id = userId,
                CityName = "MyCity",
                CountryCode = "AU"
            };
            var expectedWeather = new WeatherConditions
            {
                CountryCode = "AU",
                CityName = "MyCity",
                Temperature = 20.36f
            };
            var expectedViewModel = new PersonalWeatherViewModel
            {
                CityName = expectedUser.CityName,
                CountryCode = expectedWeather.CountryCode,
                Temperature = Math.Round(expectedWeather.Temperature, 1)
            };

            ffProvider.Toggle(Arg.Any<string>(), userId, Arg.Any<string>())
                .Returns(false);
            userRepo.GetUserProfile(1)
                .Returns(expectedUser);
            weatherService.GetWeatherForCityAndCountry(expectedUser.CityName, expectedUser.CountryCode)
                .Returns(expectedWeather);

            var controller = new HomeController(userRepo, weatherService, ffProvider);

            var result = controller.Index(userId) as ViewResult;
            var viewModel = result.Model as PersonalWeatherViewModel;


            viewModel.Temperature.ShouldBeEquivalentTo(expectedViewModel.Temperature);
        }
    }

}
