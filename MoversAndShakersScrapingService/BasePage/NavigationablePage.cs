using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoversAndShakersScrapingService.BasePage
{
    public abstract class NavigatablePage : Page
    {
        protected NavigatablePage(IWebDriver driver) : base(driver)
        {

        }
        public abstract string Url { get; }
        public void GoTo() => WrappedDriver.Navigate().GoToUrl(Url);
        public void Refresh() => WrappedDriver.Navigate().Refresh();
    }
}
