using MoversAndShakersScrapingService.Data_Models;
using MoversAndShakersScrapingService.Enums;
using MoversAndShakersScrapingService.File_Management;
using MoversAndShakersScrapingService.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace MoversAndShakersScrapingService.Scrapers
{
    class MoversAndShakersScraper
    {
        public class ScrapeMoversShakers
        {
            private GetSeleniumDriver BuildDriver;
            private IWebDriver driver;

            public ScrapeMoversShakers()
            {
                try
                {
                    BuildDriver = new GetSeleniumDriver();                  
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            public MoverCardDataModel GetListMoversShakesTable(MoversShakersTableEnum movertype, MTGFormatsEnum format, string elementXPath)
            {
                try
                {                       
                    var NewCard = new MoverCardDataModel.CardInfo();
                    var DailyList = new MoverCardDataModel();
                    DailyList.ListOfCards = new List<MoverCardDataModel.CardInfo>();
                    
                    driver = BuildDriver.CreateDriver($"https://www.mtggoldfish.com/movers/paper/{format.ToString()}");
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

                    MoversShakersJSONController.WriteMoverShakersJsonByFileName(DailyList, $"{movertype.ToString()}_{format.ToString()}.json");
                    Console.WriteLine($"## Successfully created {movertype.ToString()}_{format.ToString()}.json ##");
                    driver.Close();
                    Console.WriteLine("Closing Driver...");
                    return DailyList;

                }
                catch (Exception E)
                {
                    Console.WriteLine(E);
                    driver.Close();
                    throw new Exception("Undefined exception occured. Selenium driver closed.");
                }
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
                    var DailyName = driver.FindElement(By.XPath($"/html/body/main/div[6]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
                    NameArry[i] = DailyName.Text;
                }
                return NameArry;

            }

            private string[] GetDailyDecreaseNames()
            {
                string[] NameArry = new string[10];

                for (int i = 0; i < NameArry.Length; i++)
                {
                    var Name = driver.FindElement(By.XPath($"/html/body/main/div[6]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
                    NameArry[i] = Name.Text;
                }
                return NameArry;
            }

            private string[] GetWeeklyIncreaseNames()
            {
                string[] NameArry = new string[10];

                for (int i = 0; i < NameArry.Length; i++)
                {
                    var Name = driver.FindElement(By.XPath($"/html/body/main/div[6]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
                    NameArry[i] = Name.Text;
                }
                return NameArry;
            }

            private string[] GetWeeklyDecreaseNames()
            {
                string[] NameArry = new string[10];

                for (int i = 0; i < NameArry.Length; i++)
                {
                    var Name = driver.FindElement(By.XPath($"/html/body/main/div[6]/div[1]/div/div/div[1]/table/tbody/tr[{i + 1}]/td[4]/a"));
                    NameArry[i] = Name.Text;
                }
                return NameArry;
            }
        }
    }
}
