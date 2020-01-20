using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoversAndShakersScrapingService.BasePage
{
    public partial class MainPage : AssertedNavigatablePage
    {
        public MainPage(IWebDriver driver) : base(driver)
        {

        }
        public override string Url => @"searchEngineUrl";
        public void Search(string textToType)
        {

        }
    }
}
