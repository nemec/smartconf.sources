using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

// ReSharper disable CheckNamespace
namespace SmartConf.Sources.Environment
// ReSharper restore CheckNamespace
{
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

        public T Config
        {
            get { return _config ?? (_config = CreateConfig()); }
        }

        public T CreateConfig()
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

        public void Invalidate()
        {
            _config = null;
        }

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

        public bool PrimarySource { get; set; }

        public bool ReadOnly
        {
            get { return false; }
        }

        public bool Required { get; set; }

        public void Save(T obj)
        {
            PartialSave(obj, null);
        }
    }
}
