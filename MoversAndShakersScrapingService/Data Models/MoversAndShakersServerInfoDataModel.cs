using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoversAndShakersScrapingService.Data_Models
{
    public class MoversAndShakersServerInfoDataModel
    {
        [JsonProperty("ConfiguredDiscordGuilds", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ListOfRegisteredDiscordGuilds { get; set; }
        public DateTime LastSuccessfulScrape { get; set; }
    }
}
