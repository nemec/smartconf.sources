using System;
using CommandLine;

namespace SmartConf.Sources.CommandLine.Sample
{
    public class Program
    {
        public class Config : CommandLineOptionsBase
        {
            [Option("c", null, DefaultValue = "settings1.xml")]
            public string ConfigFile { get; set; }

            [Option("n", null)]
            public string Name { get; set; }

            public int Age { get; set; }

            public override string ToString()
            {
                return String.Format("Name: {0}, Age: {1}", Name, Age);
            }
        }

        public static void PrintPropertiesChanged<T>(ConfigurationManager<T> configManager) where T : class, new()
        {
            Console.WriteLine("Properties changed:");
            foreach (var prop in configManager.GetPropertyChangesByName())
            {
                Console.WriteLine("  {0}: {1}", prop.Key, prop.Value);
            }
        }

        public static void WithDefaultConfigFile()
        {
            var args = new string[] { };
            var commandLineSource = new CommandLineConfigurationSource<Config>(args);
            // Since the XML config file is processed internally before the command
            // line source, the config filename must be generated out-of-band.
            Func<string> getConfigFileFromCommandLine =
                () => commandLineSource.Config.ConfigFile;

            var configManager = new ConfigurationManager<Config>(
                new XmlFileConfigurationSource<Config>(getConfigFileFromCommandLine)
                    {
                        PrimarySource = true
                    }, commandLineSource);

            Console.WriteLine("Loading config file from {0}", getConfigFileFromCommandLine());
            Console.WriteLine(configManager.Out);

            PrintPropertiesChanged(configManager);
        }

        public static void WithCustomConfigFile()
        {
            var args = new[] { "-c", "settings2.xml", "-n", "Richard" };
            var commandLineSource = new CommandLineConfigurationSource<Config>(args);
            // Since the XML config file is processed internally before the command
            // line source, the config filename must be generated out-of-band.
            Func<string> getConfigFileFromCommandLine =
                () => commandLineSource.Config.ConfigFile;

            var configManager = new ConfigurationManager<Config>(
                new XmlFileConfigurationSource<Config>(getConfigFileFromCommandLine)
                {
                    PrimarySource = true  // Make sure command line source does not mark properties as changed
                }, commandLineSource);

            Console.WriteLine("Loading config file from {0}", getConfigFileFromCommandLine());

            Console.WriteLine(configManager.Out);

            Console.WriteLine("Since the XML file is marked as the primary source, " +
                "the command line source is not counted when determining which " +
                "properties have changed.");

            PrintPropertiesChanged(configManager);
        }

        static void Main()
        {
            WithDefaultConfigFile();

            Console.WriteLine();

            WithCustomConfigFile();
        }
    }
}
