using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace MoversAndShakersScrapingService.Helpers
{
    public abstract class SeleniumDriver
    {
        public IWebDriver _driver { get; set; }
        public SeleniumDriver()
        {
            _driver = CreateDriver();
        }
        public IWebDriver CreateDriver()
        {
            try
            {
                var option = new ChromeOptions();
                option.AddArguments(new List<string>()
                {
                    "--headless",
                    "--silent-launch",
                    "no-sandbox",
                    "--no-startup-window"
                });
                IWebDriver driver = new ChromeDriver(@"\\DESKTOP-JF26JGH\ChromeDriver", option);
                return driver;
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
                throw;
            }

        }
    }
}
