using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Configuration;

namespace MoversAndShakersScrapingService.Helpers
{
    public class GetSeleniumDriver
    {       
        public IWebDriver CreateDriver(IWebDriver webDriver)
        {
            webDriver?.Quit();

            var options = new ChromeOptions();
            List<string> arguments;

            if (Convert.ToBoolean(ConfigHelper.GetConfigValue("IsHeadless")))
            {
                arguments = new List<string>
                {
                    "--headless",
                    "--silent-launch",
                    "no-sandbox",
                    "--no-startup-window"
                };
            }
            else
            {
                arguments = new List<string>
                {
                    "no-sandbox",
                };
            }

            options.AddArguments(arguments);
            webDriver = new ChromeDriver(ConfigHelper.GetConfigValue("ChromeDriverLocation"), options);
            return webDriver;
        }
    }
}
