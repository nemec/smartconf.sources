using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

namespace SmartConf.Sources.AppSettings
{
    public class AppSettingsConfigurationSource<T> : IConfigurationSource<T> where T : class, new()
    {
        public T Config { get; private set; }

        public AppSettingsConfigurationSource()
            : this(new BasicConfigurationOptions())
        {
        }

        public AppSettingsConfigurationSource(IConfigurationOptions options)
        {
            Config = new T();
            var props = typeof (T).GetProperties();
            foreach (var prop in props)
            {
                string name;
                var attrs = prop.GetCustomAttributes(typeof (ConfigurationPropertyAttribute), true);
                if (attrs.Length == 1)
                {
                    name = ((ConfigurationPropertyAttribute) attrs[0]).Name;
                }
                else
                {
                    name = options.ConvertPropertyNameToAppSettingsName(prop.Name);
                }

                var converted = TypeDescriptor
                    .GetConverter(prop.PropertyType)
                    .ConvertFromString(
                        ConfigurationManager.AppSettings[name]);
                prop.SetValue(Config, converted, null);
            }
        }

        /// <inheritdoc />
        /// <exception cref="NotImplementedException"></exception>
        public void Invalidate()
        {
            throw new NotImplementedException(
                "Modifying settings of AppConfig source is not yet implemented.");
        }

        /// <inheritdoc />
        /// <exception cref="NotImplementedException"></exception>
        public void PartialSave(T obj, IEnumerable<string> propertyNames)
        {
            throw new NotImplementedException(
                "Saving values in AppConfig source is not yet implemented.");
        }

        public bool PrimarySource { get; set; }

        public bool ReadOnly { get { return true; } }

        public bool Required { get; set; }

        /// <inheritdoc />
        /// <exception cref="NotImplementedException"></exception>
        public void Save(T obj)
        {
            throw new NotImplementedException(
                "Saving values in AppConfig source is not yet implemented.");
        }
    }
}
