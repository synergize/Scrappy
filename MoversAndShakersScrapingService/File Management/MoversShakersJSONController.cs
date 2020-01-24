using MoversAndShakersScrapingService.Data_Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using VTFileSystemManagement;

namespace MoversAndShakersScrapingService.File_Management
{
    class MoversShakersJSONController
    {
        private static string fileLocation = ConfigurationManager.AppSettings.Get("MoversShakersScrapedDataDirectory");

        public static MoverCardDataModel ReadMoversShakersJsonByName(string fileName)
        {
            FileSystemManager fileSystem = new FileSystemManager();

            if (fileSystem.IsFileExists(fileName, fileLocation))
            {                
                return JsonConvert.DeserializeObject<MoverCardDataModel>(fileSystem.ReadJsonFileFromSpecificLocation(fileName, fileLocation));
            }
            else
            {
                return null;
            }
        }

        public static void WriteMoverShakersJsonByFileName(object obj, string fileName)
        {
            FileSystemManager fileSystem = new FileSystemManager();
            fileSystem.SaveJsonFileToSpecificLocation(obj, fileLocation, fileName);
        }

        public static void UpdateScrapeTime()
        {
            var currentTime = DateTime.Now;
            FileSystemManager fileSystem = new FileSystemManager();

            var obj = new MoversAndShakersServerInfoDataModel
            {
                LastSuccessfulScrape = currentTime
            };

            fileSystem.SaveJsonFileToSpecificLocation(obj, fileLocation, $"SuccessfulScrapedTime.json");
        }
    }
}

