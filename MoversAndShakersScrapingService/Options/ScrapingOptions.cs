using CommandLine;

namespace MoversAndShakersScrapingService.Options
{
    public class ScrapingOptions
    {
        [Option("intervalDays", SetName = "ParameterSet", HelpText = "Interval in which scraping will happen in days")]
        public int IntervalDays { get; set; } = 1;

        [Option("intervalHours", SetName = "ParameterSet", HelpText = "Interval in which scraping will happen in hours")]
        public int IntervalHours { get; set; }

        [Option("intervalMinutes", SetName = "ParameterSet", HelpText = "Interval in which scraping will happen in minutes")]
        public int IntervalMinutes { get; set; } 

        [Option("intervalSeconds", SetName = "ParameterSet", HelpText = "Interval in which scraping will happen in seconds")]
        public int IntervalSeconds { get; set; }

        [Option("WebDriverLocation", SetName = "WebDriverSet", Required = true, HelpText = "Location Of Web Driver")]
        public string WebDriverLocation { get; set; }

        [Option("IsHeadless", SetName = "HeadlessSet", HelpText = "Determines if web scraped using headless mode")]
        public bool IsHeadless { get; set; }
    }
}
