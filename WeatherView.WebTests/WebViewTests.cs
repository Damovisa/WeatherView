using System;
using System.Configuration;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace WeatherView.WebTests
{
    [TestClass]
    public class WebViewTests
    {
        private string _baseUrl;
        private FirefoxDriver _driver;

        [TestInitialize]
        public void Initialize()
        {
            _baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            _driver = new FirefoxDriver();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _driver.Close();
        }

        [TestMethod]
        public void WebViewShowsNameCorrectly()
        {
            var expectedNameTag = "h2";

            _driver.Navigate().GoToUrl(_baseUrl);
            var welcome = _driver.FindElement(By.Id("user-welcome"));

            // should be an h2
            welcome.TagName.Should()
                .Match(t => t.Equals(expectedNameTag, StringComparison.InvariantCultureIgnoreCase));
            // should be in the format "Hi, Name!"
            welcome.Text.Should().MatchRegex("^Hi, .*!$");
        }
    }
}
