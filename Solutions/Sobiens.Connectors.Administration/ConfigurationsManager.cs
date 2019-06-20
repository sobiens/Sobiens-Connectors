using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Sobiens.Connectors.Entities.Settings;
using System.Collections;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Services.Settings
{
    public class ConfigurationsManager
    {
        static readonly object _configurationsFileLocker = new object();

        private string GetConfigurationsFolder()
        {
            string folderPath = GetApplicationFolder();
            folderPath += "\\Configurations";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        private string GetAppConfigurationFolder(AppConfiguration configuration)
        {
            string configurationFolder = this.GetConfigurationsFolder() + "\\" + configuration.Name;
            if (Directory.Exists(configurationFolder) == false)
            {
                Directory.CreateDirectory(configurationFolder);
            }
            return configurationFolder;
        }

        private string GetAppConfigurationFilePath(AppConfiguration configuration)
        {
            return GetAppConfigurationFolder(configuration) + "\\AppAdministrativeConfiguration.xml";
        }

        private string GetExplorerStateFilePath(AppConfiguration configuration)
        {
            return GetAppConfigurationFolder(configuration) + "\\AppAdministrativeExplorerState.xml";
        }

        private string GetApplicationFolder()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folderPath += "\\Sobiens";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            folderPath += "\\Sobiens.Connectors.Administrations";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public void DeleteConfiguration(AppConfiguration configuration)
        {
            string configurationFolder = GetAppConfigurationFolder(configuration);
            if (Directory.Exists(configurationFolder) == true)
            {
                Directory.Delete(configurationFolder,true);
            }
        }

        public void SaveAppConfiguration(AppConfiguration configuration)
        {
            string configurationFilePath = GetAppConfigurationFilePath(configuration);
            SaveConfiguration(configuration, configurationFilePath);
        }

        public void SaveExplorerState(AppConfiguration configuration, ConnectorExplorerState explorerState)
        {
            string configurationFilePath = GetExplorerStateFilePath(configuration);
            SaveConfiguration(explorerState, configurationFilePath);
        }

        public ConnectorExplorerState GetExplorerState(AppConfiguration configuration)
        {
            string explorerStateFilePath = GetExplorerStateFilePath(configuration);
            return (ConnectorExplorerState)readConfigurations(explorerStateFilePath, typeof(ConnectorExplorerState));
        }

        private void SaveConfiguration(object configurations, string configurationFilePath)
        {
            lock (_configurationsFileLocker)
            {
                Debug.WriteLine("Entering " + System.Reflection.MethodBase.GetCurrentMethod().Name);

                using (FileStream fs = new FileStream(configurationFilePath, FileMode.Create))
                {
                    try
                    {
                        XmlSerializer xmlSerialiser = new XmlSerializer(configurations.GetType());
                        xmlSerialiser.Serialize(fs, configurations);
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
        public List<AppConfiguration> GetConfigurations()
        {
            List<AppConfiguration> appConfigurations = new List<AppConfiguration>();
            string configurationFolder = GetConfigurationsFolder();
            foreach (string folderName in Directory.GetDirectories(configurationFolder))
            {
                string configurationFilePath = folderName + "\\AppAdministrativeConfiguration.xml";
                if (File.Exists(configurationFilePath) == false)
                    continue;

                AppConfiguration configuration = (AppConfiguration)readConfigurations(configurationFilePath, typeof(AppConfiguration));
                appConfigurations.Add(configuration);
            }
            return appConfigurations;
        }

        /// <summary>
        /// Reads the configurations.
        /// </summary>
        /// <param name="configurationFilePath">The configuration file path.</param>
        /// <returns></returns>
        private object readConfigurations(string configurationFilePath, Type type)
        {
            Debug.WriteLine("Entering " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            object configurations = null;
            lock (_configurationsFileLocker)
            {
                if (!File.Exists(configurationFilePath))
                    return null;
                using (FileStream fs = new FileStream(configurationFilePath, FileMode.Open))
                {
                    try
                    {
                        System.Xml.Serialization.XmlSerializer xmlSerialiser = new System.Xml.Serialization.XmlSerializer(type);
                        configurations = xmlSerialiser.Deserialize(fs);
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
                return configurations;
            }
        }
    }
}
