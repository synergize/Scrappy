using MoversAndShakersScrapingService.Data_Models;
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
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        private async Task MainAsync()
        {
            ScrapeMoversShakersJob();            
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
            Console.WriteLine($" Starting Job at {DateTime.Now.ToString("dd MMM hh:mm:ss")}..");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (MTGFormatsEnum formatName in (MTGFormatsEnum[])Enum.GetValues(typeof(MTGFormatsEnum)))
            {
                var newScrapedData = new ScrapeMoversShakers().GetSrapedMoversShakersData(formatName).GetAwaiter().GetResult();
                var oldScrapedData = MoversShakersJSONController.ReadMoversShakersJsonByName($"{formatName.ToString()}.json");
                DetermineNewData(newScrapedData, oldScrapedData, formatName);
            }
            Console.Clear();
            stopWatch.Stop();
            Console.WriteLine($"\n \n Job Complete at {DateTime.Now.ToString("dd MMM hh:mm:ss")} \n Elapsed Time: {stopWatch.Elapsed}");
            Console.WriteLine($"\n Next scrape will begin at {DateTime.Now.AddMinutes(45).ToString("dd MMM hh:mm:ss")}");
            if (completedFormats.Count > 0)
            {
                MoversShakersJSONController.UpdateScrapeTime();
                Console.WriteLine($"\n Formats Updated: ");
                var output = "";
                foreach (var item in completedFormats)
                {
                    output += $"{item}, ";
                }
                output = output.Trim();
                output = output.TrimEnd(',');
                Console.WriteLine($"\n {output}");
                completedFormats = new List<string>();
            }
            else
            {
                Console.WriteLine("\n No Formats Updated. :(");
            }
            ResetTimer();
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
            newScrapedData.Format = format.ToString();

            if (newScrapedData.DailyIncreaseList.Count != 0 && oldScrapedData.DailyIncreaseList.Count != 0)
            {
                for (var i = 0; i < newScrapedData.DailyIncreaseList.Count; i++)
                {
                    if (!new MoverCardDataEqualityComparer().Equals(newScrapedData.DailyIncreaseList[i], oldScrapedData.DailyIncreaseList[i]))
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
                    Console.WriteLine(AddDateTimeConsoleWrite.AddDateTime($"{nameof(oldScrapedData)}.{nameof(oldScrapedData.DailyIncreaseList)} was empty. Created {format.ToString()}.json"));
                    MoversShakersJSONController.WriteMoverShakersJsonByFileName(newScrapedData, $"{format.ToString()}.json");
                    completedFormats.Add($"{newScrapedData.Format}");
                    break;
                }
            }
        }

        private void ResetTimer()
        {
            aTimer.Dispose();
            aTimer = new Timer();
            aTimer.Interval = 2700000;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Start();
            Console.WriteLine("\n");
            Console.WriteLine(" Timer Reset.");
        }
    }
}
