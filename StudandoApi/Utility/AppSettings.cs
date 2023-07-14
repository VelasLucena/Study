using SudyApi.Properties.Enuns;

namespace SudyApi.Utility
{
    public class AppSettings
    {
        public static IConfiguration AppSetting { get; }

        static AppSettings()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }

        public static string GetKey(ConfigKeys key)
        {
            string response = AppSetting["Settings:" + (ConfigKeys)key];

            return response;
        }
    }
}
