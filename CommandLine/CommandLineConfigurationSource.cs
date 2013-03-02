using System;
using System.Collections.Generic;
using CommandLine;

// ReSharper disable CheckNamespace
namespace SmartConf.Sources
// ReSharper restore CheckNamespace
{
    public class CommandLineConfigurationSource<T> : IConfigurationSource<T> where T : class, new()
    {
        public CommandLineConfigurationSource(string[] args)
            : this(args, Parser.Default)
        {
        } 

        public CommandLineConfigurationSource(string[] args, Parser parser)
        {
            Config = new T();
            if (!parser.ParseArguments(args, Config))
            {
                throw new ArgumentException("Could not parse arguments.");
            }
        }

        public T Config { get; private set; }

        public void Invalidate()
        {
        }

        public void PartialSave(T obj, IEnumerable<string> propertyNames)
        {
            throw new InvalidOperationException("Cannot save command line arguments.");
        }

        public bool PrimarySource { get; set; }

        public void Save(T obj)
        {
            PartialSave(obj, null);
        }


        public bool ReadOnly
        {
            get { return true; }
        }

        public bool Required { get; set; }
    }
}
