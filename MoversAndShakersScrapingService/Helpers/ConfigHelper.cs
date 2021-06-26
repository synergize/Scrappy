using ConfigurationManager = MoversAndShakersScrapingService.Managers.ConfigurationManager;

namespace MoversAndShakersScrapingService.Helpers
{
    public class ConfigHelper
    {
        private static ConfigurationManager ConfigManager => new("564523c0-fcd3-49e1-bba3-70c854584dda");

        public static string GetConfigValue(string key)
        {
            return ConfigManager.GetConfigKey(key);
        }

        public static void SetConfigValue(string key, string value)
        {
            ConfigManager.SetConfigKey(key, value);
        }
    }
}
