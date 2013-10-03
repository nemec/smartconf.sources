using System;
using System.Collections.Generic;
using clipr;

namespace SmartConf.Sources.clipr
{
    public class CliprConfigurationSource<T> : IConfigurationSource<T> where T : class, new()
    {
        public T Config { get; private set; }

        public CliprConfigurationSource(string[] args)
        {
            Config = new T();
            var parser = new CliParser<T>(Config);
            parser.Parse(args);
        } 

        /// <summary>
        /// Does nothing because once created the command line arguments
        /// never change.
        /// </summary>
        public void Invalidate()
        {
        }

        public bool PrimarySource { get; set; }

        public bool ReadOnly { get { return true; } }

        public bool Required { get; set; }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> because
        /// this configuration source cannot be saved.
        /// </summary>
        /// <exception cref="InvalidOperationException">Cannot save a read-only source.</exception>
        /// <param name="obj"></param>
        /// <param name="propertyNames"></param>
        public void PartialSave(T obj, IEnumerable<string> propertyNames)
        {
            throw new InvalidOperationException("Cannot save command line arguments.");
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> because
        /// this configuration source cannot be saved.
        /// </summary>
        /// <exception cref="InvalidOperationException">Cannot save a read-only source.</exception>
        /// <param name="obj"></param>
        public void Save(T obj)
        {
            throw new InvalidOperationException("Cannot save command line arguments.");
        }
    }
}
