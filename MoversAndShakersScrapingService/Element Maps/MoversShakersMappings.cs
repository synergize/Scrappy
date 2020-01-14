
namespace MoversAndShakersScrapingService.Element_Maps
{
    public class MoversShakersMappings
    {
        private readonly static string DailyIncrease = "//div[contains(@class, 'movers-daily')]//span[contains(@class, 'increase')]";
        private readonly static string DailyDecrease = "//div[contains(@class, 'movers-daily')]//span[contains(@class, 'decrease')]";
        private readonly static string WeeklyIncrease = "//div[contains(@class, 'movers-weekly')]//span[contains(@class, 'increase')]";
        private readonly static string WeeklyDecrease = "//div[contains(@class, 'movers-weekly')]//span[contains(@class, 'decrease')]";

        public static string DailyIncreaseXpath
        {
            get { return DailyIncrease; }
        }
        public static string DailyDecreaseXpath
        {
            get { return DailyDecrease; }
        }
        public static string WeeklyIncreaseXpath
        {
            get { return WeeklyIncrease; }
        }
        public static string WeeklyDecreaseXpath
        {
            get { return WeeklyDecrease; }
        }
    }
}
