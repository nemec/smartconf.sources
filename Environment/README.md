smartconf.sources.environment
=============================

Implements a custom [smartconf](https://github.com/nemec/smartconf) 
ConfigurationSource that adds environment variables (eg. System.Environment)
into the configuration pipeline.

We're on [NuGet](https://nuget.org/packages/smartconf.sources.environment/)!


Usage
=====

Options class:

    public class Options
    {
        [EnvironmentVariable("INPUT_FILE")]
        [Option('r', "read", HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }
    }

Test Code:

    // Make sure the env var is set.
    Environment.SetEnvironmentVariable("INPUT_FILE", "default.txt");

    var configurationManager = new SmartConf.ConfigurationManager<Options>(
        new EnvironmentConfigurationSource<Options>
            {
                GlobalTarget = EnvironmentVariableTarget.Process,
                IgnoreConversionErrors = true
            }
        );

    // Should print "default.txt"
    Console.WriteLine(configurationManager.Out.InputFile);
