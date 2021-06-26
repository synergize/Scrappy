using OpenQA.Selenium;

namespace MoversAndShakersScrapingService.Element_Maps
{
    public class MoversShakersMappings
    {
        public static By DailyIncreaseXpath { get; } = By.XPath("//div[contains(@class, 'movers-daily')]//span[contains(@class, 'increase')]");

        public static By DailyDecreaseXpath { get; } = By.XPath("//div[contains(@class, 'movers-daily')]//span[contains(@class, 'decrease')]");

        public static By WeeklyIncreaseXpath { get; } = By.XPath("//div[contains(@class, 'movers-weekly')]//span[contains(@class, 'increase')]");

        public static By WeeklyDecreaseXpath { get; } = By.XPath("//div[contains(@class, 'movers-weekly')]//span[contains(@class, 'decrease')]");

        public static By PageLastUpdatedXpath { get; } = By.XPath("//p[contains(@class, 'movers-last-updated')]/abbr");
    }
}
