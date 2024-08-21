using MoversAndShakersScrapingService.Data_Models;
using MoversAndShakersScrapingService.Enums;
using MoversAndShakersScrapingService.File_Management;
using MoversAndShakersScrapingService.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using CommandLine;
using Microsoft.AspNetCore.SignalR.Client;
using MoversAndShakersScrapingService.Options;
using MoversAndShakersScrapingService.Scrapers;
using Newtonsoft.Json;
using System.Reflection;
using System.IO.Compression;

namespace MoversAndShakersScrapingService
{
    internal class Program
    {
        private Timer _aTimer = new Timer();
        private TimeSpan _defaultInterval;
        private List<string> _completedFormats = new List<string>();
        private static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();
        private async Task MainAsync(string[] args)
        {
            var chromeDriverLocation = await DownloadChromeAndGetChromeDriverLocation();
            var parserResult = Parser.Default.ParseArguments<ScrapingOptions>(args);
            _defaultInterval = new TimeSpan(parserResult.Value.IntervalDays, parserResult.Value.IntervalHours, parserResult.Value.IntervalMinutes, parserResult.Value.IntervalSeconds);
            ConfigHelper.SetConfigValue("ChromeDriverLocation", chromeDriverLocation);
            ConfigHelper.SetConfigValue("IsHeadless", parserResult.Value.IsHeadless ? "true" : "false");
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
            foreach (var formatName in (MTGFormatsEnum[])Enum.GetValues(typeof(MTGFormatsEnum)))
            {
                var newScrapedData = new MoversAndShakersScraper().GetSrapedMoversShakersData(formatName);
                var oldScrapedData = MoversShakersJsonController.ReadMoversShakersJsonByName($"{formatName}.json");
                DetermineNewData(newScrapedData, oldScrapedData, formatName);
            }
            Console.Clear();
            stopWatch.Stop();
            Console.WriteLine($"\n \n Job Complete at {DateTime.Now.ToString("dd MMM hh:mm:ss")} \n Elapsed Time: {stopWatch.Elapsed}");
            Console.WriteLine($"\n Next scrape will begin at {DateTime.Now.AddMinutes(45).ToString("dd MMM hh:mm:ss")}");
            if (_completedFormats.Count > 0)
            {
                MoversShakersJsonController.UpdateScrapeTime();
                Console.WriteLine($"\n Formats Updated: ");
                var output = _completedFormats.Aggregate("", (current, item) => current + $"{item}, ");
                output = output.Trim();
                output = output.TrimEnd(',');
                Console.WriteLine($"\n {output}");
                _completedFormats = new List<string>();

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
                        MoversShakersJsonController.WriteMoverShakersJsonByFileName(newScrapedData, $"{format.ToString()}.json");
                        _completedFormats.Add($"{newScrapedData.Format}");
                        break;
                    }
                }
            }
            else if (newScrapedData.DailyIncreaseList.Count > 0 && oldScrapedData.DailyIncreaseList.Count == 0)
            {
                for (int i = 0; i < newScrapedData.DailyIncreaseList.Count; i++)
                {
                    Console.WriteLine(AddDateTimeConsoleWrite.AddDateTime($"{nameof(oldScrapedData)}.{nameof(oldScrapedData.DailyIncreaseList)} was empty. Created {format.ToString()}.json"));
                    MoversShakersJsonController.WriteMoverShakersJsonByFileName(newScrapedData, $"{format.ToString()}.json");
                    _completedFormats.Add($"{newScrapedData.Format}");
                    break;
                }
            }
        }

        private void ResetTimer()
        {
            _aTimer.Dispose();
            _aTimer = new Timer {Interval = _defaultInterval.Milliseconds};
            _aTimer.Elapsed += OnTimedEvent;
            _aTimer.Start();
            Console.WriteLine("\n");
            Console.WriteLine(" Timer Reset.");
        }

        private async Task<string> DownloadChromeAndGetChromeDriverLocation()
        {
            var platformName = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux64" : "win64";
            using var httpClient = new HttpClient();
            var documentRequest = await httpClient.GetAsync(new Uri("https://googlechromelabs.github.io/chrome-for-testing/last-known-good-versions-with-downloads.json"));
            var lastKnownDocument = await documentRequest.Content.ReadAsStringAsync();
            var deserializedDocumentResponse = JsonConvert.DeserializeObject<ChromeDownloadModel>(lastKnownDocument);
            var chromeDownloadLink = deserializedDocumentResponse.Channels.Stable.Downloads.Chrome.FirstOrDefault(x => x.Platform == platformName).Url;
            var chromeDriverDownloadLink = deserializedDocumentResponse.Channels.Stable.Downloads.Chromedriver.FirstOrDefault(x => x.Platform == platformName).Url;
            var expectedChromeDownloadDirectory = Path.Join(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ChromeDriver");
            var expectedDownloadDirectory = Path.Join(expectedChromeDownloadDirectory, $"chrome-{platformName}");
            var unzippedChromeDriverLocation = Path.Join(expectedChromeDownloadDirectory, $"chromedriver-{platformName}\\chromedriver.exe");
            var finalUnzippedChromeDriverLocation = Path.Join(expectedChromeDownloadDirectory, $"chrome-{platformName}\\chromedriver.exe");

            if (!Directory.Exists(expectedChromeDownloadDirectory))
            {
                Directory.CreateDirectory(expectedChromeDownloadDirectory);
            }

            var manifestFileLocation = Path.Combine(expectedDownloadDirectory, $"{deserializedDocumentResponse.Channels.Stable.Version}.manifest");
            if (!File.Exists(manifestFileLocation))
            {
                var chromeFilePath = Path.Join(expectedChromeDownloadDirectory, "chrome.zip");
                var chromeDownload = await httpClient.GetAsync(chromeDownloadLink);
                var chromeData = await chromeDownload.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(chromeFilePath, chromeData);

                var chromeDriverFilePath = Path.Join(expectedChromeDownloadDirectory, "chromedriver.zip");
                var chromeDriverDownload = await httpClient.GetAsync(chromeDriverDownloadLink);
                var chromeDriverData = await chromeDriverDownload.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(chromeDriverFilePath, chromeDriverData);

                ZipFile.ExtractToDirectory(chromeFilePath, expectedChromeDownloadDirectory, true);
                ZipFile.ExtractToDirectory(chromeDriverFilePath, expectedChromeDownloadDirectory, true);

                File.Move(unzippedChromeDriverLocation, finalUnzippedChromeDriverLocation);
            }

            return expectedDownloadDirectory;
        }

        private async Task SendPricingData(string jsonRequest)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/communicationHub")
                .Build();

            await connection.StartAsync();
            await connection.InvokeAsync("SendMessage", jsonRequest);
        }
    }
}
