
namespace SmartConf.Sources.AppSettings
{
    public interface IConfigurationOptions
    {
        /// <summary>
        /// Convert a property name (typically PascalCase) to the
        /// naming convention used in your App.config. Common conversions
        /// are camelCase (the default), dotted.case or snake_case.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        string ConvertPropertyNameToAppSettingsName(string propertyName);
    }
}
