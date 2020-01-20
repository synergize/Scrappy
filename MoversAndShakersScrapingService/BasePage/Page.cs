using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoversAndShakersScrapingService.BasePage
{
    public abstract class Page
    {
        protected Page(IWebDriver driver) => WrappedDriver = driver;
        protected IWebDriver WrappedDriver { get; }
    }
}
