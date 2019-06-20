using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Sobiens.Connectors.Common.Extensions
{
    public static class SerializationManager
    {
        static readonly object _settingsFileLocker = new object();

        /// <summary>
        /// Reads the settings.
        /// </summary>
        /// <param name="settingFilePath">The setting file path.</param>
        /// <returns></returns>
        public static T ReadSettings<T>(string settingFilePath)
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
                        XmlSerializer xmlSerialiser = new XmlSerializer(typeof(T));
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

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public static void SaveConfiguration<T>(object configuration, string configurationFilePath)
        {
            lock (_settingsFileLocker)
            {
                Debug.WriteLine("Entering " + System.Reflection.MethodBase.GetCurrentMethod().Name);

                if (File.Exists(configurationFilePath) == true)
                {
                    File.Copy(configurationFilePath, configurationFilePath + ".bak", true);
                }

                using (FileStream fs = new FileStream(configurationFilePath, FileMode.Create))
                {
                    try
                    {
                        XmlSerializer xmlSerialiser = new XmlSerializer(typeof(T));
                        xmlSerialiser.Serialize(fs, configuration);
                    }
                    catch (Exception ex)
                    {
                        File.Copy(configurationFilePath + ".bak", configurationFilePath, true);
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
    }
}
