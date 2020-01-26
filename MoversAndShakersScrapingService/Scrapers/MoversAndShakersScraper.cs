using MoversAndShakersScrapingService.Data_Models;
using MoversAndShakersScrapingService.Enums;
using MoversAndShakersScrapingService.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MoversAndShakersScrapingService.Scrapers
{
    class MoversAndShakersScraper
    {
        public class ScrapeMoversShakers
        {
            private IWebDriver driver;
            public MoverCardDataModel GetListMoversShakesTable(MoversShakersTableEnum movertype, MTGFormatsEnum format, string elementXPath)
            {
                Console.WriteLine(AddDateTimeConsoleWrite.AddDateTime("Waiting 5 seconds before we begin..."));
                Thread.Sleep(5000);
                var NewCard = new MoverCardDataModel.CardInfo();
                var DailyList = new MoverCardDataModel();
                DailyList.ListOfCards = new List<MoverCardDataModel.CardInfo>();
                driver = new GetSeleniumDriver().CreateDriver(driver);
                driver.Navigate().GoToUrl($"https://www.mtggoldfish.com/movers/paper/{format.ToString()}");
                var DailyChangeIncrease = driver.FindElements(By.XPath(elementXPath));
                int elementCounter = 0;
                int nameCounter = 0;
                string[] CardNames = DetermineCardNames(movertype);

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
                            DailyList.ListOfCards.Add(NewCard);
                            NewCard.Name = CardNames[nameCounter];
                            nameCounter++;
                            NewCard = new MoverCardDataModel.CardInfo();
                            break;
                    }

                }
                Console.WriteLine($"## Successfully acquired {movertype.ToString()}_{format.ToString()} ##");
                driver.Quit();
                return DailyList;
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
                    case MoversShakersTableEnum.WeeklyIncrease:
                        return GetWeeklyIncreaseNames();
                    case MoversShakersTableEnum.WeeklyDecrease:
                        return GetWeeklyDecreaseNames();
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

            private string[] GetWeeklyIncreaseNames()
            {
                string[] NameArry = new string[10];

                for (int i = 0; i < NameArry.Length; i++)
                {
                    try
                    {
                        var Name = driver.FindElement(By.XPath($"/html/body/main/div[7]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
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

            private string[] GetWeeklyDecreaseNames()
            {
                string[] NameArry = new string[10];

                for (int i = 0; i < NameArry.Length; i++)
                {
                    try
                    {
                        var Name = driver.FindElement(By.XPath($"/html/body/main/div[7]/div[1]/div/div/div[2]/table/tbody/tr[{i + 1}]/td[4]/a"));
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
        }
    }
}
