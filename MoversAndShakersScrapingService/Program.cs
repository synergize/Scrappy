using MoversAndShakersScrapingService.Data_Models;
using MoversAndShakersScrapingService.Element_Maps;
using MoversAndShakersScrapingService.Enums;
using MoversAndShakersScrapingService.File_Management;
using MoversAndShakersScrapingService.Helpers;
using MoversAndShakersScrapingService.Scrapers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace MoversAndShakersScrapingService
{
    class Program
    {
        private Timer aTimer = new Timer();
        public Program()
        {
            aTimer.Interval = 2700000;
        }
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        private async Task MainAsync()
        {
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Start();

            ScrapeMoversShakersJob();
            await Task.Delay(-1);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ScrapeMoversShakersJob();
        }

        private void ScrapeMoversShakersJob()
        {
            Console.WriteLine("Starting Job..");
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
            Console.WriteLine("Job Complete..");
        }

        /// <summary>
        /// Takes in two objects and will run them through an IEqualityComparer to determine if they're equal. If they're not, we create a new JSON document.
        /// </summary>
        /// <param name="newDailyIncrease"></param>
        /// <param name="oldDailyIncrease"></param>
        /// <param name="movertype"></param>
        /// <param name="format"></param>
        private void DetermineNewData(MoverCardDataModel newDailyIncrease, MoverCardDataModel oldDailyIncrease, MoversShakersTableEnum movertype, MTGFormatsEnum format)
        {
            MoverCardDataEqualityComparer Compare = new MoverCardDataEqualityComparer();

            for (var i = 0; i < newDailyIncrease.ListOfCards.Count; i++)
            {
                if (!Compare.Equals(newDailyIncrease.ListOfCards[i], oldDailyIncrease.ListOfCards[i]))
                {
                    Console.WriteLine($"{nameof(newDailyIncrease.ListOfCards)} and {nameof(oldDailyIncrease.ListOfCards)} Differ. Writing to disk...");
                    MoversShakersJSONController.WriteMoverShakersJsonByFileName(newDailyIncrease, $"{movertype.ToString()}_{format.ToString()}.json");
                    break;
                }
            }
                
            
        }
    }
}
