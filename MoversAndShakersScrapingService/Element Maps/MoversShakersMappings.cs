using OpenQA.Selenium;

namespace MoversAndShakersScrapingService.Element_Maps
{
    public class MoversShakersMappings
    {
        public static By PageLastUpdatedXpath { get; } = By.CssSelector("p.last-updated > abbr");

        public static By TblMovers { get; } = By.CssSelector("table.table-movers");
    }
}
