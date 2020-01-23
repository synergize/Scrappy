using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace MoversAndShakersScrapingService.Helpers
{
    public static class GetSeleniumDriver
    {       
        public static IWebDriver CreateDriver()
        {
            var options = new ChromeOptions();
            options.AddArguments(new List<string>()
            {
                "--headless",
                "--silent-launch",
                "no-sandbox",
                "--no-startup-window"
            });
            IWebDriver driver = new ChromeDriver(@"\\DESKTOP-JF26JGH\ChromeDriver", options);

            return driver;
        }
    }
}
