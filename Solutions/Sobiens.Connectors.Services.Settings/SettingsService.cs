using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        static readonly object _treeViewFileLocker = new object();
        static readonly object _settingsFileLocker = new object();

        private string test = "";
        public string GetConfigurationsFolder()
        {
            string folderPath = GetApplicationFolder();
            folderPath += "\\Configurations";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }
        public string GetAppConfigurationsFilePath()
        {
            return GetConfigurationsFolder() + "\\AppConfiguration.xml";
        }
        public string GetAppAdministrativeConfigurationsFilePath()
        {
            return GetConfigurationsFolder() + "\\AppAdministrativeConfiguration.xml";
        }
        public string GetCommonApplicationFolder()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            folderPath += "\\Sobiens";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            folderPath += "\\Sobiens.Connectors";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public string GetApplicationFolder()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folderPath += "\\Sobiens";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            folderPath += "\\Sobiens.Connectors";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public string GetTempFolder()
        {
            string folderPath = GetApplicationFolder();
            folderPath += "\\Temp";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public string GetLogFolder()
        {
            string folderPath = GetApplicationFolder();
            folderPath += "\\Log";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <returns></returns>
        public void SaveConfiguration(AppConfiguration configuration)
        {
            lock (_settingsFileLocker)
            {
                Debug.WriteLine("Entering " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                string settingFilePath = GetAppConfigurationsFilePath();

                using (FileStream fs = new FileStream(settingFilePath, FileMode.Create))
                {
                    try
                    {
                        XmlSerializer xmlSerialiser = new XmlSerializer(typeof(AppConfiguration));
                        xmlSerialiser.Serialize(fs, configuration);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        fs.Close();
                        Debug.WriteLine("Leaving " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        public AppConfiguration LoadConfiguration()
        {
            string configurationFilePath = GetAppConfigurationsFilePath();
            if (File.Exists(configurationFilePath) == false)
                return null;

            AppConfiguration configuration = readSettings<AppConfiguration>(configurationFilePath);

            return configuration;
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        public AppConfiguration LoadAdministrativeConfiguration()
        {
            string configurationFilePath = GetAppAdministrativeConfigurationsFilePath();
            if (File.Exists(configurationFilePath) == false)
                return null;

            AppConfiguration configuration = readSettings<AppConfiguration>(configurationFilePath);

            return configuration;
        }

        /// <summary>
        /// Reads the settings.
        /// </summary>
        /// <param name="settingFilePath">The setting file path.</param>
        /// <returns></returns>
        private T readSettings<T>(string settingFilePath)
        {
            Debug.WriteLine("Entering " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            T settings;
            lock (_settingsFileLocker)
            {
                if (!File.Exists(settingFilePath))
                    return default(T);
                using (FileStream fs = new FileStream(settingFilePath, FileMode.Open))
                {
                    try
                    {
                        XmlSerializer xmlSerialiser = new XmlSerializer(typeof(ExplorerConfiguration));
                        settings = (T)xmlSerialiser.Deserialize(fs);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        fs.Close();
                        Debug.WriteLine("Leaving " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                }
                return settings;
            }
        }
    }
}
