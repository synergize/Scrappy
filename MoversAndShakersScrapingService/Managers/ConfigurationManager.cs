using Microsoft.Extensions.Configuration;

namespace MoversAndShakersScrapingService.Managers
{
    public class ConfigurationManager
    {
        private static IConfiguration _configuration;
        private static string ConfigName { get; set; }

        public IConfiguration Configuration
        {
            get
            {
                return _configuration ??= new ConfigurationBuilder()
                    .AddUserSecrets(ConfigName)
                    .Build();
            }
        }

        public string GetConfigKey(string key)
        {
            return Configuration[key];
        }

        public void SetConfigKey(string key, string value)
        {
            Configuration[key] = value;
        }

        public ConfigurationManager(string configName)
        {
            ConfigName = configName;
        }
    }
}
