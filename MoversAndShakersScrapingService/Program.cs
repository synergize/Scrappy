using MoversAndShakersScrapingService.Data_Models;
using MoversAndShakersScrapingService.Element_Maps;
using MoversAndShakersScrapingService.Enums;
using MoversAndShakersScrapingService.File_Management;
using MoversAndShakersScrapingService.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using static MoversAndShakersScrapingService.Scrapers.MoversAndShakersScraper;

namespace MoversAndShakersScrapingService
{
    class Program
    {
        private Timer aTimer = new Timer();
        private List<string> completedFormats = new List<string>();
        public Program()
        {
            aTimer.Interval = 2700000;
        }
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        private async Task MainAsync()
        {
            ScrapeMoversShakersJob();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Start();
            Console.WriteLine($"Scraping Service has started at {DateTime.Now.ToString("dd MMM HH:mm:ss")}");

            await Task.Delay(-1);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                ScrapeMoversShakersJob();
            }
            catch (Exception z)
            {
                Console.WriteLine(z);
                throw new Exception(z.Message);
            }
        }

        private void ScrapeMoversShakersJob()
        {
            Console.WriteLine($"Starting Job at {DateTime.Now.ToString("dd MMM HH:mm:ss")}..");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (MTGFormatsEnum formatName in (MTGFormatsEnum[])Enum.GetValues(typeof(MTGFormatsEnum)))
            {
                ScrapeMoversShakers Format = new ScrapeMoversShakers();
                var newScrapedData = Format.GetSrapedMoversShakersData(formatName);
                var oldScrapedData = MoversShakersJSONController.ReadMoversShakersJsonByName($"{formatName.ToString()}.json");
                DetermineNewData(newScrapedData, oldScrapedData, formatName);            
            }
            Console.Clear();
            stopWatch.Stop();
            Console.WriteLine($"\n \n Job Complete at {DateTime.Now.ToString("dd MMM HH:mm:ss")} \n Elapsed Time: {stopWatch.Elapsed}");
            if (completedFormats.Count > 0)
            {
                MoversShakersJSONController.UpdateScrapeTime();
                Console.WriteLine("Formats Updated:");
                foreach (var item in completedFormats)
                {
                    Console.WriteLine(item);
                }
                completedFormats = new List<string>();
            }
        }

        /// <summary>
        /// Takes in two objects and will run them through an IEqualityComparer to determine if they're equal. If they're not, we create a new JSON document.
        /// </summary>
        /// <param name="newDailyIncrease"></param>
        /// <param name="oldDailyIncrease"></param>
        /// <param name="movertype"></param>
        /// <param name="format"></param>
        private void DetermineNewData(MoverCardDataModel newScrapedData, MoverCardDataModel oldScrapedData, MTGFormatsEnum format)
        {
            MoverCardDataEqualityComparer Compare = new MoverCardDataEqualityComparer();
            newScrapedData.Format = format.ToString();

            if (oldScrapedData == null)
            {
                MoversShakersJSONController.WriteMoverShakersJsonByFileName(newScrapedData, $"{format.ToString()}.json");
                Console.WriteLine(AddDateTimeConsoleWrite.AddDateTime($"Successfully created {format.ToString()}.json"));
            }
            if (newScrapedData.DailyIncreaseList.Count != 0 && oldScrapedData.DailyIncreaseList.Count != 0)
            {
                for (var i = 0; i < newScrapedData.DailyIncreaseList.Count; i++)
                {
                    if (!Compare.Equals(newScrapedData.DailyIncreaseList[i], oldScrapedData.DailyIncreaseList[i]))
                    {
                        Console.WriteLine($"New: {newScrapedData.Format} and Old: {oldScrapedData.Format} Differ. Writing to disk...");
                        MoversShakersJSONController.WriteMoverShakersJsonByFileName(newScrapedData, $"{format.ToString()}.json");
                        completedFormats.Add($"{newScrapedData.Format}");
                        break;
                    }
                }
            }
            else if (newScrapedData.DailyIncreaseList.Count > 0 && oldScrapedData.DailyIncreaseList.Count == 0)
            {
                for (int i = 0; i < newScrapedData.DailyIncreaseList.Count; i++)
                {
                    MoversShakersJSONController.WriteMoverShakersJsonByFileName(newScrapedData, $"{format.ToString()}.json");
                    break;
                }
            }
        }
    }
}
