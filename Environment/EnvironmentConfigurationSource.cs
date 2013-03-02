using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

// ReSharper disable CheckNamespace
namespace SmartConf.Sources.Environment
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Configuration source for pulling in Environment Variables.
    /// </summary>
    /// <typeparam name="T">Configuration object type.</typeparam>
    public class EnvironmentConfigurationSource<T> : IConfigurationSource<T> where T : class, new()
    {
        /// <summary>
        /// Use this <see cref="EnvironmentVariableTarget"/> for any
        /// <see cref="EnvironmentVariableAttribute"/> that does not
        /// specify a Target.
        /// </summary>
        public EnvironmentVariableTarget GlobalTarget { get; set; }

        /// <summary>
        /// If string conversion to one of the config properties fails,
        /// do not throw an exception (property will remain the default
        /// value).
        /// </summary>
        public bool IgnoreConversionErrors { get; set; }

        private T _config;

        /// <summary>
        /// Parsed configuration object.
        /// </summary>
        public T Config
        {
            get { return _config ?? (_config = CreateConfig()); }
        }

        private T CreateConfig()
        {
            var obj = new T();

            var props = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<EnvironmentVariableAttribute>() != null);

            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<EnvironmentVariableAttribute>();

                var envVarName = attr.VariableName;
                if (attr.VariableName == null)
                {
                    envVarName = prop.Name.ToUpperInvariant();
                }

                var stringVal = System.Environment.GetEnvironmentVariable(
                    envVarName, attr.Target.GetValueOrDefault(GlobalTarget));

                var converter = TypeDescriptor.GetConverter(prop.PropertyType);
                object value;

                try
                {
                    value = converter.ConvertFromInvariantString(stringVal);
                }
                catch (Exception)
                {
                    // Not pretty, but this is the only exception we can catch. 
                    // Try to do as little as possible in the try block.
                    if (!IgnoreConversionErrors)
                    {
                        throw;
                    }

                    continue;  // Don't attempt to set value.
                }

                prop.SetValue(obj, value);
            }
            return obj;
        }

        /// <summary>
        /// Mark this configuration source as invalidated. The next time
        /// the configuration object is requested, re-check all environment
        /// variables and parse their values.
        /// </summary>
        public void Invalidate()
        {
            _config = null;
        }

        /// <summary>
        /// For each property in <paramref name="propertyNames"/> with an
        /// <see cref="EnvironmentVariableAttribute"/>, set the environment
        /// variable for that property with the property's value.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyNames"></param>
        public void PartialSave(T obj, IEnumerable<string> propertyNames)
        {
            var props = typeof (T).GetProperties()
                .Where(p => p.GetCustomAttribute<EnvironmentVariableAttribute>() != null);
            if (propertyNames != null)
            {
                props = props.Where(p => !propertyNames.Contains(p.Name));
            }

            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<EnvironmentVariableAttribute>();

                var envVarName = attr.VariableName;
                if (attr.VariableName == null)
                {
                    envVarName = prop.Name.ToUpperInvariant();
                }

                System.Environment.SetEnvironmentVariable(
                    envVarName, 
                    prop.GetValue(obj).ToString(),
                    attr.Target.GetValueOrDefault(GlobalTarget));
            }
        }

        /// <summary>
        /// Mark this configuration source as the one that gets saved by default.
        /// </summary>
        public bool PrimarySource { get; set; }

        /// <summary>
        /// This configuration source is not read only.
        /// </summary>
        public bool ReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Mark this configuration source as required.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Save all properties with an
        /// <see cref="EnvironmentVariableAttribute"/> and set the environment
        /// variable for that property with the property's value.
        /// </summary>
        /// <param name="obj"></param>
        public void Save(T obj)
        {
            PartialSave(obj, null);
        }
    }
}
