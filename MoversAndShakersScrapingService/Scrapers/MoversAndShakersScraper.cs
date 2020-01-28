using MoversAndShakersScrapingService.Data_Models;
using MoversAndShakersScrapingService.Element_Maps;
using MoversAndShakersScrapingService.Enums;
using MoversAndShakersScrapingService.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoversAndShakersScrapingService.Scrapers
{
    class MoversAndShakersScraper
    {
        public class ScrapeMoversShakers
        {
            private IWebDriver driver;
            public async Task<MoverCardDataModel> GetSrapedMoversShakersData(MTGFormatsEnum format)
            {
                Console.WriteLine(AddDateTimeConsoleWrite.AddDateTime($"[Scraping {format.ToString()}]: Waiting 5 seconds before we begin..."));
                Thread.Sleep(5000);

                var scrapedData = new MoverCardDataModel();

                driver = new GetSeleniumDriver().CreateDriver(driver);
                driver.Navigate().GoToUrl($"https://www.mtggoldfish.com/movers/paper/{format.ToString()}");

                scrapedData.Format = format.ToString();

                try
                {
                    scrapedData.DailyIncreaseList = ScrapeMoversShakersData(MoversShakersTableEnum.DailyIncrease);
                    scrapedData.DailyDecreaseList = ScrapeMoversShakersData(MoversShakersTableEnum.DailyDecrease);

                    //scrapedData.WeeklyIncreaseList = ScrapeMoversShakersData(MoversShakersTableEnum.WeeklyIncrease);
                    //scrapedData.WeeklyDecreaseList = ScrapeMoversShakersData(MoversShakersTableEnum.WeeklyDecrease);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine($"## Successfully scraped {format.ToString()} ##");
                driver.Quit();
                return scrapedData;

            }

            private List<MoverCardDataModel.CardInfo> ScrapeMoversShakersData(MoversShakersTableEnum table)
            {
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> DailyChangeIncrease = null;
                var cardInformation = new List<MoverCardDataModel.CardInfo>();
                var NewCard = new MoverCardDataModel.CardInfo();
                int elementCounter = 0;
                string[] CardNames = new string[10];
                int nameCounter = 0;

                switch (table)
                {
                    case MoversShakersTableEnum.DailyIncrease:
                        DailyChangeIncrease = driver.FindElements(By.XPath(MoversShakersMappings.DailyIncreaseXpath));
                        CardNames = DetermineCardNames(table);
                        break;
                    case MoversShakersTableEnum.DailyDecrease:
                        DailyChangeIncrease = driver.FindElements(By.XPath(MoversShakersMappings.DailyDecreaseXpath));
                        CardNames = DetermineCardNames(table);
                        break;
                    //case MoversShakersTableEnum.WeeklyIncrease:
                    //    DailyChangeIncrease = driver.FindElements(By.XPath(MoversShakersMappings.WeeklyIncreaseXpath));
                    //    CardNames = DetermineCardNames(table);
                    //    break;
                    //case MoversShakersTableEnum.WeeklyDecrease:
                    //    DailyChangeIncrease = driver.FindElements(By.XPath(MoversShakersMappings.WeeklyDecreaseXpath));
                    //    CardNames = DetermineCardNames(table);
                    //    break;
                    default:
                        return new List<MoverCardDataModel.CardInfo>();
                }

                foreach (var item in DailyChangeIncrease)
                {
                    switch (elementCounter)
                    {
                        default:

                            break;
                        case 0:
                            NewCard.PriceChange = item.Text;
                            elementCounter++;
                            break;
                        case 1:
                            NewCard.TotalPrice = item.Text;
                            elementCounter++;
                            break;
                        case 2:
                            NewCard.ChangePercentage = item.Text;
                            elementCounter = 0;
                            cardInformation.Add(NewCard);
                            NewCard.Name = CardNames[nameCounter];
                            nameCounter++;
                            NewCard = new MoverCardDataModel.CardInfo();
                            break;
                    }
                }

                return cardInformation;
            }

            private string[] DetermineCardNames(MoversShakersTableEnum moverType)
            {
                switch (moverType)
                {
                    default:
                        return null;
                    case MoversShakersTableEnum.DailyIncrease:
                        return GetDailyIncreaseNames();
                    case MoversShakersTableEnum.DailyDecrease:
                        return GetDailyDecreaseNames();
                    //case MoversShakersTableEnum.WeeklyIncrease:
                    //    return GetWeeklyIncreaseNames();
                    //case MoversShakersTableEnum.WeeklyDecrease:
                    //    return GetWeeklyDecreaseNames();
                }
            }

            private string[] GetDailyIncreaseNames()
            {
                string[] NameArry = new string[10];

                for (int i = 0; i < NameArry.Length; i++)
                {
                    try
                    {
                        var DailyName = driver.FindElement(By.XPath($"/html/body/main/div[6]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
                        NameArry[i] = DailyName.Text;
                    }
                    catch (Exception)
                    {
                        driver.Quit();
                        return new string[10];
                    }
                }
                return NameArry;
            }

            private string[] GetDailyDecreaseNames()
            {
                string[] NameArry = new string[10];

                for (int i = 0; i < NameArry.Length; i++)
                {
                    try
                    {
                        var Name = driver.FindElement(By.XPath($"/html/body/main/div[6]/div[1]/div/div/div[2]/table/tbody/tr[{i + 1}]/td[4]/a"));
                        NameArry[i] = Name.Text;
                    }
                    catch (Exception)
                    {
                        driver.Quit();
                        return new string[10];
                    }
                }
                return NameArry;
            }

            //private string[] GetWeeklyIncreaseNames()
            //{
            //    string[] NameArry = new string[10];

            //    for (int i = 0; i < NameArry.Length; i++)
            //    {
            //        try
            //        {
            //            var Name = driver.FindElement(By.XPath($"/html/body/main/div[7]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
            //            NameArry[i] = Name.Text;
            //        }
            //        catch (Exception)
            //        {
            //            driver.Quit();
            //            return new string[10];
            //        }

            //    }
            //    return NameArry;
            //}

            //private string[] GetWeeklyDecreaseNames()
            //{
            //    string[] NameArry = new string[10];

            //    for (int i = 0; i < NameArry.Length; i++)
            //    {
            //        try
            //        {
            //            var Name = driver.FindElement(By.XPath($"/html/body/main/div[7]/div[1]/div/div/div[2]/table/tbody/tr[{i + 1}]/td[4]/a"));
            //            NameArry[i] = Name.Text;
            //        }
            //        catch (Exception)
            //        {
            //            driver.Quit();
            //            return new string[10];
            //        }
            //    }
            //    return NameArry;
            //}
        }
    }
}
