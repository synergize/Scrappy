
using OpenQA.Selenium;

namespace MoversAndShakersScrapingService.Element_Maps
{
    public class MoversShakersMappings
    {
        private readonly static By DailyIncrease = By.XPath("//div[contains(@class, 'movers-daily')]//span[contains(@class, 'increase')]");
        private readonly static By DailyDecrease = By.XPath("//div[contains(@class, 'movers-daily')]//span[contains(@class, 'decrease')]");
        private readonly static By WeeklyIncrease = By.XPath("//div[contains(@class, 'movers-weekly')]//span[contains(@class, 'increase')]");
        private readonly static By WeeklyDecrease = By.XPath("//div[contains(@class, 'movers-weekly')]//span[contains(@class, 'decrease')]");
        private readonly static By PageLastUpdated = By.XPath("//p[contains(@class, 'movers-last-updated')]/abbr");

        public static By DailyIncreaseXpath
        {
            get { return DailyIncrease; }
        }
        public static By DailyDecreaseXpath
        {
            get { return DailyDecrease; }
        }
        public static By WeeklyIncreaseXpath
        {
            get { return WeeklyIncrease; }
        }
        public static By WeeklyDecreaseXpath
        {
            get { return WeeklyDecrease; }
        }
        public static By PageLastUpdatedXpath
        {
            get { return PageLastUpdated; }
        }
    }
}
