namespace Rantt.Domain.Configuration.Implementations
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Implementation of IFolderMappingService to be run on desktop.
    /// </summary>
    public class DesktopFolderMappingService : IFolderMappingService
    {
        /// <summary>
        /// Gets configuration folder path.
        /// </summary>
        /// <returns>The path.</returns>
        public string GetConfigurationFolderPath()
        {
            #if (!SILVERLIGHT)
            if (ConfigurationManager.AppSettings.AllKeys.Contains("ConfigurationFolder"))
            {
                string folder = ConfigurationManager.AppSettings["ConfigurationFolder"];
                if (Directory.Exists(folder))
                {
                    return folder.Last().ToString(CultureInfo.InvariantCulture) == @"\" ? folder : folder + @"\";
                }
            }
            #endif
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Rantt\";
        }

        public string GetStateFolderPath()
        {
            #if (!SILVERLIGHT)
            if (ConfigurationManager.AppSettings.AllKeys.Contains("StateFolder"))
            {
                string folder = ConfigurationManager.AppSettings["StateFolder"];
                if (Directory.Exists(folder))
                {
                    return folder.Last().ToString(CultureInfo.InvariantCulture) == @"\" ? folder : folder + @"\";
                }
            }
            #endif
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Rantt\_Layout_\";
        }
    }
}
