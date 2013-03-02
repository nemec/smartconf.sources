smartconf.sources.commandline
=============================

Implements a custom [smartconf](https://github.com/nemec/smartconf) ConfigurationSource that integrates
command line arguments parsed by the [commandline](https://github.com/gsscoder/commandline)
library into the configuration pipeline.

If this source is specified as the last source in the list and a previous source 
has its PrimarySource value set to `true`, the values parsed will take priority over
any other configuration source, but will not be factored in when determining which
properties have changed since loading.

Usage
=====

settings.xml:

    <Config>
      <Name>Timothy</Name>
      <Age>33</Age>
    </Config>

Config object:

    public class Config
    {
        [Option("c", null)]
        public string ConfigFile { get; set; }

        [Option("n", null)]
        public string Name { get; set; }

        public int Age { get; set; }
    }

Test code:

    // These are the command line arguments to be parsed by the CommandLineParser library
    var args = new string[] { "-c", "settings.xml", "-n", "Richard" };

    var commandLineSource = new CommandLineConfigurationSource<Config>(args);
    // The XML source is processed before the commandLineSource, but the XML
    // source also depends on the output of the commandline parser...
    // This function will process and extract the config filename before
    // processing the XML source.
    Func<string> getConfigFileFromCommandLine =
        () => commandLineSource.Config.ConfigFile;

    // Since the XML source is marked as the primary source,
    // any of its values that are different than the base, but
    // not overwritten in the Out object will also be marked as
    // "changed" so that saving will keep any values unique
    // to the source as well as those that changed.
    var configManager = new ConfigurationManager<Config>(
        new XmlFileConfigurationSource<Config>(getConfigFileFromCommandLine)
        {
            PrimarySource = true
        }, commandLineSource);

    Console.WriteLine("Loading config file from {0}", getConfigFileFromCommandLine());
    //> Loading config file from settings.xml

    Console.WriteLine(configManager.Out);
    //> Name: Richard, Age: 33

    // Note that because the XML source is marked as primary, the
    // Name property value (Timothy) from that source will be
    // serialized rather than the value (Richard) from the 
    // command line source that overwrote it.
    Console.WriteLine("Properties changed:");
    foreach (var prop in configManager.GetPropertyChangesByName())
    {
        Console.WriteLine("  {0}: {1}", prop.Key, prop.Value);
    }

    //> Properties changed:
    //>   Name: Timothy
    //>   Age: 33
