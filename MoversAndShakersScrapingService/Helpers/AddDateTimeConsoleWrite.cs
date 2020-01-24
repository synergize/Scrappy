using System;

namespace MoversAndShakersScrapingService.Helpers
{
   public static class AddDateTimeConsoleWrite
    {
        public static string AddDateTime(string message)
        {
            return $"[[{DateTime.Now.ToString("HH:mm:ss")}]]: {message}";
        }
    }
}
