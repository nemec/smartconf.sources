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
        {
            Config = new T();
            var props = typeof (T).GetProperties();
            foreach (var prop in props)
            {
                var name = ToCamelCase(prop.Name);
                var attrs = prop.GetCustomAttributes(typeof (ConfigurationPropertyAttribute), true);
                if (attrs.Length == 1)
                {
                    name = ((ConfigurationPropertyAttribute) attrs[0]).Name;
                }

                var converted = TypeDescriptor
                    .GetConverter(prop.PropertyType)
                    .ConvertFromString(
                        ConfigurationManager.AppSettings[name]);
                prop.SetValue(Config, converted, null);
            }
        }

        private static string ToCamelCase(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return s;
            }
            var fst = Char.ToLowerInvariant(s[0]);
            return fst + s.Substring(1, s.Length - 1);
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
