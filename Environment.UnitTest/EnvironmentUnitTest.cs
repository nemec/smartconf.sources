using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartConf.Sources.Environment;

namespace Environment.UnitTest
{
    [TestClass]
    public class EnvironmentUnitTest
    {
        public class ConfigWithoutName
        {
            [EnvironmentVariable]
            public string Option { get; set; }
        }

        public class ConfigWithName
        {
            [EnvironmentVariable("SOMETHING")]
            public string Option { get; set; }
        }

        public class ConfigWithNonString
        {
            [EnvironmentVariable]
            public int Age { get; set; }
        }

        private const string OptionValue = "opt";
        private const string SomethingValue = "borg";
        private const int AgeValue = 23;

        [TestInitialize]
        public void Init()
        {
            System.Environment.SetEnvironmentVariable("OPTION", OptionValue,
                EnvironmentVariableTarget.Process);
            System.Environment.SetEnvironmentVariable("SOMETHING", SomethingValue,
                EnvironmentVariableTarget.Process);
            System.Environment.SetEnvironmentVariable("AGE", AgeValue.ToString(),
                EnvironmentVariableTarget.Process);
        }

        [TestCleanup]
        public void Cleanup()
        {
            System.Environment.SetEnvironmentVariable("OPTION", String.Empty,
                EnvironmentVariableTarget.Process);
            System.Environment.SetEnvironmentVariable("SOMETHING", String.Empty,
                EnvironmentVariableTarget.Process);
            System.Environment.SetEnvironmentVariable("AGE", String.Empty,
                EnvironmentVariableTarget.Process);
        }

        [TestMethod]
        public void GetConfig_WithNoSpecifiedOptionName_UsesPropertyName()
        {
            var configManager = new EnvironmentConfigurationSource<ConfigWithoutName>
                {
                    GlobalTarget = EnvironmentVariableTarget.Process
                };

            var actual = configManager.Config.Option;

            Assert.AreEqual(OptionValue, actual);
        }

        [TestMethod]
        public void GetConfig_WithCustomOptionName_UsesCustomOptionName()
        {
            var configManager = new EnvironmentConfigurationSource<ConfigWithName>
            {
                GlobalTarget = EnvironmentVariableTarget.Process
            };

            var actual = configManager.Config.Option;

            Assert.AreEqual(SomethingValue, actual);
        }

        [TestMethod]
        public void GetConfig_WithNonStringValue_ConvertsEnvironmentVar()
        {
            var configManager = new EnvironmentConfigurationSource<ConfigWithNonString>
            {
                GlobalTarget = EnvironmentVariableTarget.Process
            };

            var actual = configManager.Config.Age;

            Assert.AreEqual(AgeValue, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetConfig_WithNonStringValueAndEnvVarNotConvertible_ThrowsNotSupportedException()
        {
            System.Environment.SetEnvironmentVariable("AGE", "NOT_INTEGER",
                EnvironmentVariableTarget.Process);

            var configManager = new EnvironmentConfigurationSource<ConfigWithNonString>
            {
                GlobalTarget = EnvironmentVariableTarget.Process
            };

// ReSharper disable UnusedVariable
            var config = configManager.Config;
// ReSharper restore UnusedVariable
        }

        [TestMethod]
        public void GetConfig_WithNonStringValueAndEnvVarNotConvertibleAndConversionExceptionsIgnored_LeavesValueAsDefault()
        {
            System.Environment.SetEnvironmentVariable("AGE", "NOT_INTEGER",
                EnvironmentVariableTarget.Process);

            var configManager = new EnvironmentConfigurationSource<ConfigWithNonString>
            {
                GlobalTarget = EnvironmentVariableTarget.Process,
                IgnoreConversionErrors = true
            };

            var config = configManager.Config;

            Assert.AreEqual(0, config.Age);
        }

        [TestMethod]
        public void Save_WithNoSpecifiedOptionName_SavesValueToEnvironment()
        {
            const string expected = "NEW OPTION";

            var configManager = new EnvironmentConfigurationSource<ConfigWithoutName>
            {
                GlobalTarget = EnvironmentVariableTarget.Process
            };

            var config = configManager.Config;
            config.Option = expected;

            configManager.Save(config);

            var actual = System.Environment.GetEnvironmentVariable(
                "OPTION", EnvironmentVariableTarget.Process);

            Assert.AreEqual(expected, actual);
        }
    }
}
