using System;

namespace MoversAndShakersScrapingService.Data_Models
{
    public class ChromeDownloadModel
    {
        public DateTime Timestamp { get; set; }
        public Channels Channels { get; set; }
    }

    public class Channels
    {
        public Stable Stable { get; set; }
        public Beta Beta { get; set; }
        public Dev Dev { get; set; }
        public Canary Canary { get; set; }
    }

    public class Stable
    {
        public string Channel { get; set; }
        public string Version { get; set; }
        public long Revision { get; set; }
        public Downloads Downloads { get; set; }
    }

    public class Downloads
    {
        public ChromeItem[] Chrome { get; set; }
        public ChromedriverItem[] Chromedriver { get; set; }
        public ChromeHeadlessShellItem[] ChromeHeadlessShell { get; set; }
    }

    public class ChromeItem
    {
        public string Platform { get; set; }
        public string Url { get; set; }
    }

    public class ChromedriverItem
    {
        public string Platform { get; set; }
        public string Url { get; set; }
    }

    public class ChromeHeadlessShellItem
    {

    public string Platform { get; set; }
    public string Url { get; set; }
    }

    public class Beta
    {
        public string Channel { get; set; }
        public string Version { get; set; }
        public long Revision { get; set; }
        public Downloads Downloads { get; set; }
    }

    public class Dev
    {
        public string Channel { get; set; }
        public string Version { get; set; }
        public long Revision { get; set; }
        public Downloads Downloads { get; set; }
    }

    public class Canary
    {
        public string Channel { get; set; }
        public string Version { get; set; }
        public long Revision { get; set; }
        public Downloads Downloads { get; set; }
    }
}
