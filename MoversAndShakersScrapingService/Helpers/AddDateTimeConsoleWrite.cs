using System;

namespace MoversAndShakersScrapingService.Helpers
{
   public static class AddDateTimeConsoleWrite
    {
        public static string AddDateTime(string message)
        {
            return $"[[{DateTime.Now.ToString("hh:mm:ss")}]]: {message}";
        }
    }
}
