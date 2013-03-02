using System;

// ReSharper disable CheckNamespace
namespace SmartConf.Sources.Environment
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Specify the environment variable that maps to this property.
    /// If no name is provided, it checks the capitalized name
    /// of the property that the attribute is attached to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EnvironmentVariableAttribute : Attribute
    {
        /// <summary>
        /// Environment variable name.
        /// </summary>
        public string VariableName { get; private set; }

        /// <summary>
        /// Optional location where environment variable is stored.
        /// </summary>
        public EnvironmentVariableTarget? Target { get; private set; }

        /// <summary>
        /// Create a new <see cref="EnvironmentVariableAttribute"/> using
        /// the capitalized name of the attached property and the
        /// <see cref="EnvironmentVariableTarget"/> specified in the
        /// configuration source.
        /// </summary>
        public EnvironmentVariableAttribute()
            : this(null)
        {
        }

        /// <summary>
        /// Create a new <see cref="EnvironmentVariableAttribute"/> using
        /// the capitalized name of the attached property and pointing to
        /// the given <see cref="EnvironmentVariableTarget"/>.
        /// </summary>
        /// <param name="target"></param>
        public EnvironmentVariableAttribute(EnvironmentVariableTarget target)
            : this(null, target)
        {
        }

        /// <summary>
        /// Create a new <see cref="EnvironmentVariableAttribute"/> using
        /// the given name and pointing to the
        /// <see cref="EnvironmentVariableTarget"/> specified in the
        /// configuration source.
        /// </summary>
        /// <param name="varname"></param>
        public EnvironmentVariableAttribute(string varname)
        {
            VariableName = varname;
            Target = null;
        }

        /// <summary>
        /// Create a new <see cref="EnvironmentVariableAttribute"/> using
        /// the the given name and pointing to the given 
        /// <see cref="EnvironmentVariableTarget"/>.
        /// </summary>
        /// <param name="varname"></param>
        /// <param name="target"></param>
        public EnvironmentVariableAttribute(string varname, EnvironmentVariableTarget target)
        {
            VariableName = varname;
            Target = target;
        }
    }
}
