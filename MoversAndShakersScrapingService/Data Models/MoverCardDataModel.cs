using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MoversAndShakersScrapingService.Data_Models
{
    public class MoverCardDataModel
    {
        public class CardInfo
        {
            public string PriceChange { get; set; }
            public string Name { get; set; }
            public string TotalPrice { get; set; }
            public string ChangePercentage { get; set; }
        }

        [JsonProperty("DailyIncreaseData")]
        public List<CardInfo> DailyIncreaseList { get; set; }
        [JsonProperty("DailyDecreaseData")]
        public List<CardInfo> DailyDecreaseList { get; set; }
        //[JsonProperty("WeeklyIncreaseData")]
        //public List<CardInfo> WeeklyIncreaseList { get; set; }
        //[JsonProperty("WeeklyDecreaseData")]
        //public List<CardInfo> WeeklyDecreaseList { get; set; }
        [JsonProperty("FormatName")]
        public string Format { get; set; }
        public DateTime PageLastUpdated { get; set; }

    }
}