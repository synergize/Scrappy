using MoversAndShakersScrapingService.Data_Models;
using MoversAndShakersScrapingService.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using MoversAndShakersScrapingService.Managers;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using System.IO;
using System.Reflection;

namespace MoversAndShakersScrapingService.File_Management
{
    internal class MoversShakersJsonController
    {
        private static readonly string FileLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public static MoverCardDataModel ReadMoversShakersJsonByName(string fileName)
        {
            FileSystemManager fileSystem = new FileSystemManager();

            if (fileSystem.IsFileExists(fileName, FileLocation))
            {                
                return JsonConvert.DeserializeObject<MoverCardDataModel>(fileSystem.ReadJsonFileFromSpecificLocation(fileName, FileLocation));
            }
            else
            {
                return new MoverCardDataModel
                {
                    DailyIncreaseList = new List<MoverCardDataModel.CardInfo>(),
                    DailyDecreaseList = new List<MoverCardDataModel.CardInfo>(),
                    PageLastUpdated = DateTime.MinValue
                };
            }
        }

        public static void WriteMoverShakersJsonByFileName(object obj, string fileName)
        {
            var fileSystem = new FileSystemManager();
            fileSystem.SaveJsonFileToSpecificLocation(obj, FileLocation, fileName);
        }

        public static void UpdateScrapeTime()
        {
            var currentTime = DateTime.Now;
            var fileSystem = new FileSystemManager();

            var obj = new MoversAndShakersServerInfoDataModel
            {
                LastSuccessfulScrape = currentTime
            };

            fileSystem.SaveJsonFileToSpecificLocation(obj, FileLocation, $"SuccessfulScrapedTime.json");
        }
    }
}

