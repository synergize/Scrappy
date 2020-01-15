using MoversAndShakersScrapingService.Element_Maps;
using MoversAndShakersScrapingService.Enums;
using MoversAndShakersScrapingService.File_Management;
using System;
using System.Threading.Tasks;
using System.Timers;
using static MoversAndShakersScrapingService.Scrapers.MoversAndShakersScraper;

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
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Start();

            ScrapeMoversShakersJob();
            await Task.Delay(-1);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("Starting Job..");
            //foreach (MTGFormatsEnum formatName in (MTGFormatsEnum[])Enum.GetValues(typeof(MTGFormatsEnum)))
            //{
            //    ScrapeMoversShakers Format = new ScrapeMoversShakers();
            //    var newDailyIncrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.DailyIncrease, formatName, MoversShakersMappings.DailyIncreaseXpath);
            //    var oldDailyIncrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.DailyIncrease.ToString()}_{formatName.ToString()}.json");

            //    var newDailyDecrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.DailyDecrease, formatName, MoversShakersMappings.DailyDecreaseXpath);
            //    var oldDailyDecrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.DailyDecrease.ToString()}_{formatName.ToString()}.json");

            //    var newWeeklyIncrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.WeeklyIncrease, formatName, MoversShakersMappings.WeeklyIncreaseXpath);
            //    var oldWeeklyIncrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.WeeklyIncrease.ToString()}_{formatName.ToString()}.json");

            //    var newWeeklyDecrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.WeeklyDecrease, formatName, MoversShakersMappings.WeeklyDecreaseXpath);
            //    var oldWeeklyDecrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.WeeklyDecrease.ToString()}_{formatName.ToString()}.json");
            //}
            //Console.WriteLine("Job Complete..");
        }

        private void ScrapeMoversShakersJob()
        {
            Console.WriteLine("Starting Job..");
            foreach (MTGFormatsEnum formatName in (MTGFormatsEnum[])Enum.GetValues(typeof(MTGFormatsEnum)))
            {
                ScrapeMoversShakers Format = new ScrapeMoversShakers();
                var newDailyIncrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.DailyIncrease, formatName, MoversShakersMappings.DailyIncreaseXpath);
                var oldDailyIncrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.DailyIncrease.ToString()}_{formatName.ToString()}.json");

                var newDailyDecrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.DailyDecrease, formatName, MoversShakersMappings.DailyDecreaseXpath);
                var oldDailyDecrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.DailyDecrease.ToString()}_{formatName.ToString()}.json");

                var newWeeklyIncrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.WeeklyIncrease, formatName, MoversShakersMappings.WeeklyIncreaseXpath);
                var oldWeeklyIncrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.WeeklyIncrease.ToString()}_{formatName.ToString()}.json");

                var newWeeklyDecrease = Format.GetListMoversShakesTable(MoversShakersTableEnum.WeeklyDecrease, formatName, MoversShakersMappings.WeeklyDecreaseXpath);
                var oldWeeklyDecrease = MoversShakersJSONController.ReadMoversShakersJsonByName($"{MoversShakersTableEnum.WeeklyDecrease.ToString()}_{formatName.ToString()}.json");
            }
            Console.WriteLine("Job Complete..");
        }
    }
}
