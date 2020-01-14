using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MoversAndShakersScrapingService.Helpers
{
    class GetSeleniumDriver
    {
        public static string _DriverDirectory = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "Driver");
        public IWebDriver CreateDriver(string url)
        {
            var option = new ChromeOptions();
            option.AddArgument("--headless");
            IWebDriver driver = new ChromeDriver(@"\\DESKTOP-JF26JGH\ChromeDriver", option);
            //driver.Manage().Window.Maximize();
            try
            {
                driver.Navigate().GoToUrl(url);
                return driver;
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg);
                return driver;
            }
        }

        private static string BuildFilePathDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return Path.Combine(directory);
        }
    }
}
