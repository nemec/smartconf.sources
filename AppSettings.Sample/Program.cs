using System;
using SmartConf;
using SmartConf.Sources.AppSettings;

namespace AppSettings.Sample
{
    public class Program
    {
        public class Config
        {
            public string Name { get; set; }

            public int Age { get; set; }

            [ConfigurationProperty("Occupation")]
            public string Occupation { get; set; }
        }

        public static void Main(string[] args)
        {
            var manager = new ConfigurationManager<Config>(
                new AppSettingsConfigurationSource<Config>());

            var config = manager.Out;

            // Frank
            Console.WriteLine(config.Name);
            
            // 43
            Console.WriteLine(config.Age);

            // Unemployed
            Console.WriteLine(config.Occupation);
        }
    }
}
