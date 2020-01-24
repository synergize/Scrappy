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
            if (webDriver != null)
            {
                webDriver.Quit();
            }

            var options = new ChromeOptions();
            options.AddArguments(new List<string>()
            {
                "--headless",
                "--silent-launch",
                "no-sandbox",
                "--no-startup-window"
            });

            webDriver = new ChromeDriver(ConfigurationManager.AppSettings.Get("ChromeDriverLocation"), options);
            return webDriver;
        }
    }
}
