
using System;

namespace SmartConf.Sources.AppSettings
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigurationPropertyAttribute : Attribute
    {
        public string Name { get; private set; }

        public ConfigurationPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
