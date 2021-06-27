using MoversAndShakersScrapingService.Data_Models;
using MoversAndShakersScrapingService.Element_Maps;
using MoversAndShakersScrapingService.Enums;
using MoversAndShakersScrapingService.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MoversAndShakersScrapingService.Scrapers
{
    public class MoversAndShakersScraper : WebDriverBase
    {
        public MoverCardDataModel GetSrapedMoversShakersData(MTGFormatsEnum format)
        {
            try
            {
                Console.WriteLine(AddDateTimeConsoleWrite.AddDateTime($"[Scraping {format.ToString()}]: Waiting 5 seconds before we begin..."));
                Thread.Sleep(5000);

                var scrapedData = new MoverCardDataModel();

                Driver.Navigate().GoToUrl($"https://www.mtggoldfish.com/movers/paper/{format}");

                var pageUpdatedTime = Driver.FindElement(MoversShakersMappings.PageLastUpdatedXpath).GetAttribute("title").Replace("UTC", "");
                DateTime.TryParse(pageUpdatedTime, out DateTime parsedPageUpdatedTime);
                scrapedData.PageLastUpdated = parsedPageUpdatedTime;
                scrapedData.Format = format.ToString();
                scrapedData.DailyIncreaseList = GetMoversAndShakersData(format, true, true, true);
                scrapedData.DailyDecreaseList = GetMoversAndShakersData(format, false, true, true);
                scrapedData.WeeklyIncreaseList = GetMoversAndShakersData(format, true, true, false);
                scrapedData.WeeklyDecreaseList = GetMoversAndShakersData(format, false, true, false);

                Console.WriteLine(AddDateTimeConsoleWrite.AddDateTime($"### Successfully scraped {format}. Quitting Driver.. ###"));
                Driver.Quit();
                return scrapedData;
            }
            catch (Exception e)
            {
                Driver.Quit();
                Console.WriteLine(e);
                throw;
            }
        }

        private List<MoverCardDataModel.CardInfo> GetMoversAndShakersData(MTGFormatsEnum format, bool isWinner, bool isPaper, bool isDaily)
        {
            var platform = isPaper ? "paper" : "online";
            var winnerOrLoser = isWinner ? "winners" : "losers";
            var daily = isDaily ? "dod" : "wow";
            Driver.Navigate().GoToUrl($"https://www.mtggoldfish.com/movers-details/{platform}/{format}/{winnerOrLoser}/{daily}");
            var rows = Driver.FindElement(MoversShakersMappings.TblMovers).FindElements(By.XPath("//tr"));
            var cardData = new List<MoverCardDataModel.CardInfo>();

            foreach (var row in rows)
            {
                var rowData = row.FindElements(By.XPath("./*"));

                if (rowData.Count != 5)
                {
                    if (rowData.Count == 1)
                    {
                        Console.WriteLine($"Skipping single cell row. Text: {rowData[0].Text}");
                    }
                    continue;
                }

                cardData.Add(new MoverCardDataModel.CardInfo
                {
                    PriceChange = rowData[0].Text,
                    Name = rowData[2].Text,
                    TotalPrice = rowData[3].Text,
                    ChangePercentage = rowData[4].Text
                });
            }
            return cardData;
        }
    }
}
