smartconf.sources.appsettings
=============================

Implements a custom [smartconf](https://github.com/nemec/smartconf)
ConfigurationSource that integrates the traditional AppSettings object
into the ConfigurationManager pipeline.

We're on [NuGet](https://nuget.org/packages/smartconf.sources.appsettings/)!

* By default, the AppSettings key is the name of the property, camel cased
  (the first letter is forced to lowercase, all other characters stay the same).
* If the key name in the AppSettings is different than the property name,
  tag the property with a ConfigurationPropertyAttribute and give it the
  exact (case sensitive) name to use when retrieving settings.

Usage
=====

App.config:

    <configuration>
      <appSettings>
        <add key="age" value="43"/>
        <add key="name" value="Frank"/>
        <add key="Occupation" value="Unemployed"/>
      </appSettings>
    </configuration>

Config object:

    public class Config
    {
        public string Name { get; set; }

        public int Age { get; set; }

        [ConfigurationProperty("Occupation")]
        public string Occupation { get; set; }
    }

Test code:

    var appSettingsSource = new AppSettingsConfigurationSource<Config>();
    var configManager = new ConfigurationManager<Config>(appSettingsSource);

    var config = configManager.Out;


    Console.WriteLine(config.Name);
    //> Frank

    Console.WriteLine(config.Age);
    //> 43

    Console.WriteLine(config.Occupation);
    //> Unemployed
