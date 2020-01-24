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
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Start();
            Console.WriteLine($"Scraping Service has started at {DateTime.Now.ToString("dd MMM HH:mm:ss")}");

            await Task.Delay(-1);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ScrapeMoversShakersJob();
        }

        private void ScrapeMoversShakersJob()
        {
            Console.WriteLine($"Starting Job at {DateTime.Now.ToString("dd MMM HH:mm:ss")}..");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (MTGFormatsEnum formatName in (MTGFormatsEnum[])Enum.GetValues(typeof(MTGFormatsEnum)))
            {
                ScrapeMoversShakers Format = new ScrapeMoversShakers();
                var newDailyIncrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.DailyIncrease, formatName, MoversShakersMappings.DailyIncreaseXpath);
                var oldDailyIncrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.DailyIncrease.ToString()}_{formatName.ToString()}.json");
                DetermineNewData(newDailyIncrease, oldDailyIncrease, MoversShakersTableEnum.DailyIncrease, formatName);

                var newDailyDecrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.DailyDecrease, formatName, MoversShakersMappings.DailyDecreaseXpath);
                var oldDailyDecrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.DailyDecrease.ToString()}_{formatName.ToString()}.json");
                DetermineNewData(newDailyDecrease, oldDailyDecrease, MoversShakersTableEnum.DailyDecrease, formatName);

                var newWeeklyIncrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.WeeklyIncrease, formatName, MoversShakersMappings.WeeklyIncreaseXpath);
                var oldWeeklyIncrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.WeeklyIncrease.ToString()}_{formatName.ToString()}.json");
                DetermineNewData(newWeeklyIncrease, oldWeeklyIncrease, MoversShakersTableEnum.WeeklyIncrease, formatName);

                var newWeeklyDecrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.WeeklyDecrease, formatName, MoversShakersMappings.WeeklyDecreaseXpath);
                var oldWeeklyDecrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.WeeklyDecrease.ToString()}_{formatName.ToString()}.json");
                DetermineNewData(newWeeklyDecrease, oldWeeklyDecrease, MoversShakersTableEnum.WeeklyDecrease, formatName);
            }

            MoversShakersJSONController.UpdateScrapeTime();
            stopWatch.Stop();
            Console.Clear();
            Console.WriteLine($"\n \n Job Complete at {DateTime.Now.ToString("dd MMM HH:mm:ss")} \n Elapsed Time: {stopWatch.Elapsed}");
            if (completedFormats.Count > 0)
            {
                Console.WriteLine("Formats Updated:");
                foreach (var item in completedFormats)
                {
                    Console.WriteLine(item);
                }
            }
        }

        /// <summary>
        /// Takes in two objects and will run them through an IEqualityComparer to determine if they're equal. If they're not, we create a new JSON document.
        /// </summary>
        /// <param name="newDailyIncrease"></param>
        /// <param name="oldDailyIncrease"></param>
        /// <param name="movertype"></param>
        /// <param name="format"></param>
        private void DetermineNewData(MoverCardDataModel newScrapedData, MoverCardDataModel oldScrapedData, MoversShakersTableEnum movertype, MTGFormatsEnum format)
        {
            MoverCardDataEqualityComparer Compare = new MoverCardDataEqualityComparer();
            newScrapedData.Format = format.ToString();

            if (oldScrapedData == null)
            {
                MoversShakersJSONController.WriteMoverShakersJsonByFileName(newScrapedData, $"{movertype.ToString()}_{format.ToString()}.json");
            }
            if (newScrapedData.ListOfCards.Count != 0 && oldScrapedData.ListOfCards.Count != 0)
            {
                for (var i = 0; i < newScrapedData.ListOfCards.Count; i++)
                {
                    if (!Compare.Equals(newScrapedData.ListOfCards[i], oldScrapedData.ListOfCards[i]))
                    {                        
                        Console.WriteLine($"{nameof(newScrapedData.ListOfCards)} and {nameof(oldScrapedData.ListOfCards)} Differ. Writing to disk...");
                        MoversShakersJSONController.WriteMoverShakersJsonByFileName(newScrapedData, $"{movertype.ToString()}_{format.ToString()}.json");
                        completedFormats.Add(newScrapedData.Format);
                        break;
                    }
                }
            }
            else if (newScrapedData.ListOfCards.Count > 0 && oldScrapedData.ListOfCards.Count == 0)
            {
                for (var i = 0; i < newScrapedData.ListOfCards.Count; i++)
                {
                    MoversShakersJSONController.WriteMoverShakersJsonByFileName(newScrapedData, $"{movertype.ToString()}_{format.ToString()}.json");
                    break;
                }
            }
        }
    }
}
