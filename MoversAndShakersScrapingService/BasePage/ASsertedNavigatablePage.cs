using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoversAndShakersScrapingService.BasePage
{
    public abstract class AssertedNavigatablePage : NavigatablePage
    {
        protected AssertedNavigatablePage(IWebDriver driver) : base(driver)
        {
            // Resolve the IAssert through some of the popular IoC containers.
            ////Assert = ServiceContainer.Provider.Resolve<IAssert>();
        }
        //protected IAssert Assert { get; }
    }
}
