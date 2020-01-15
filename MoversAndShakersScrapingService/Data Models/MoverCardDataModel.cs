﻿using Newtonsoft.Json;
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

        [JsonProperty("MoversAndShakersData")]
        public List<CardInfo> ListOfCards { get; set; }
    }
}
