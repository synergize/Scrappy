﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Configuration;

namespace MoversAndShakersScrapingService.Helpers
{
    public abstract class WebDriverBase
    {
        public IWebDriver Driver;

        protected WebDriverBase()
        {
            if (Driver == null)
            {
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
                Driver = new ChromeDriver(ConfigHelper.GetConfigValue("ChromeDriverLocation"), options);
            }
        }
    }
}
