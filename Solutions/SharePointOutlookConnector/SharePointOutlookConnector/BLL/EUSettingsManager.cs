#define SerializeSettingsAsXml
#define SerializeTreeAsXml
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Threading;
using System.Diagnostics;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public class EUSettingsManager
    {
        // JOEL JEFFERY 20110713
        static readonly object _treeViewFileLocker = new object();
        static readonly object _settingsFileLocker = new object();

        private static EUSettingsManager _Instance = null;
        private EUSettings _Settings = null;
        public EUSettings Settings
        {
            get
            {
                if (_Settings == null)
                    _Settings = LoadSettings();
                return _Settings;
            }
            set
            {
                _Settings = value;
            }
        }
        public static EUSettingsManager GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new EUSettingsManager();
            }
            return _Instance;
        }
        private EUSettingsManager()
        {
        }
        public string GetMappedSharePointFieldName(string rootFolderPath, string outlookFieldName)
        {
            EUListSetting listSetting = GetListSetting(rootFolderPath);
            if (listSetting == null)
                return String.Empty;
            EUEmailMapping emailMapping = listSetting.EmailMappings.SingleOrDefault(em => em.OutlookFieldName == outlookFieldName && em.SharePointFieldName != null && em.SharePointFieldName != String.Empty);
            if (emailMapping != null)
                return emailMapping.SharePointFieldName;
            return String.Empty;
        }
        public EUSiteSetting GetSiteSetting(string rootWebURL)
        {
            return Settings.SiteSettings.SingleOrDefault(ss => ss.Url == rootWebURL);
        }
        public EUListSetting GetListSetting(string rootFolderPath)
        {
            EUListSetting listSetting = Settings.ListSettings.SingleOrDefault(ss => ss.RootFolderPath == rootFolderPath);
            if (listSetting == null)
                listSetting = Settings.DefaultListSetting;
            return listSetting;
        }
        public string GetCommonApplicationFolder()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            folderPath += "\\Sobiens";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            folderPath += "\\SharePointOutlookConnector";
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
            folderPath += "\\SharePointOutlookConnector";
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

        public void CleanLogFiles()
        {
            string logFolder = GetLogFolder();
            string[] fileNames = Directory.GetFiles(logFolder);
            foreach (string fileName in fileNames)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.CreationTime.AddDays(1) < DateTime.Now)
                {
                    File.Delete(fileName);
                }
            }
        }

        public void CleanTempFolders()
        {
            string tempFolder = GetTempFolder();
            string[] directoryNames = Directory.GetDirectories(tempFolder);
            foreach (string directoryName in directoryNames)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
                if (directoryInfo.CreationTime.AddDays(1) < DateTime.Now)
                {
                    Directory.Delete(directoryName, true);
                }
            }
        }

        public string CreateATempFolder()
        {
            CleanTempFolders();
            string sourceFolder = GetTempFolder() + "\\" + Guid.NewGuid().ToString();
            Directory.CreateDirectory(sourceFolder);
            return sourceFolder;
        }

        public string CreateALogFile()
        {
            CleanLogFiles();
            return GetLogFolder() + "\\" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
        }

        public string GetSetttingsFolder()
        {
            string folderPath = GetApplicationFolder();
            folderPath += "\\Settings";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }
        public string GetSettingFilePath()
        {
            return GetSetttingsFolder() + "\\settings.xml";
        }
        public string GetSPTreeviewFilePath()
        {
            return GetSetttingsFolder() + "\\sharepointTreeview.xml";
        }
        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSettings()
        {
            lock (_settingsFileLocker)
            {
                Debug.WriteLine("Entering " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Settings.IsMultipleConfigurationSettingVersion = true;
                string settingFilePath = GetSettingFilePath();

                using (FileStream fs = new FileStream(settingFilePath, FileMode.Create)) // JOEL JEFFERY 20110713 changed to Create
                {
                    try
                    {
#if SerializeSettingsAsXml
                        XmlSerializer xmlSerialiser = new XmlSerializer(typeof(EUSettings));
                        xmlSerialiser.Serialize(fs, Settings);
#else
                        BinaryFormatter bf1 = new BinaryFormatter();
                        bf1.Serialize(fs, Settings);
#endif
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

        private void FillNodes(List<EUTreeNode> euTreeNodes, TreeNodeCollection treeNodes)
        {
            foreach (TreeNode node in treeNodes)
            {
                EUTreeNode newNode = new EUTreeNode();
                newNode.Text = node.Text;
                newNode.Tag = node.Tag;
                newNode.IsExpanded = node.IsExpanded;
                euTreeNodes.Add(newNode);
                FillNodes(newNode.Nodes, node.Nodes);
            }
        }

        /// <summary>
        /// Saves the state of the share point tree view.
        /// </summary>
        /// <param name="treeview">The treeview.</param>
        public void SaveSharePointTreeViewState(TreeView treeview)
        {
            lock (_treeViewFileLocker)
            {
                Debug.WriteLine("Entering " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                List<EUTreeNode> nodes = new List<EUTreeNode>();
                FillNodes(nodes, treeview.Nodes);
                string spTreeviewFilePath = GetSPTreeviewFilePath();

                using (FileStream fs = new FileStream(spTreeviewFilePath, FileMode.Create)) // JOEL JEFFERY 20110713 changed to Create
                {
                    try
                    {
#if SerializeTreeAsXml
                        XmlSerializer xmlSerialiser = new XmlSerializer(typeof(List<EUTreeNode>));
                        xmlSerialiser.Serialize(fs, nodes);
#else
                        BinaryFormatter bf1 = new BinaryFormatter();
                        bf1.Serialize(fs, nodes);
#endif
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

        public void ClearSharePointTreeViewState()
        {
            string spTreeviewFilePath = GetSPTreeviewFilePath();
            File.Delete(spTreeviewFilePath);
        }

        /// <summary>
        /// Loads the SP treeview.
        /// </summary>
        /// <returns></returns>
        public List<EUTreeNode> LoadSPTreeview()
        {
            List<EUTreeNode> nodes = null;
            lock (_treeViewFileLocker)
            {
                Debug.WriteLine("Entering " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                string spTreeviewFilePath = GetSPTreeviewFilePath();
                if (File.Exists(spTreeviewFilePath) == false)
                    return null;

                using (FileStream fs = new FileStream(spTreeviewFilePath, FileMode.Open))
                {
                    try
                    {
#if SerializeTreeAsXml
                        XmlSerializer xmlSerialiser = new XmlSerializer(typeof(List<EUTreeNode>));
                        nodes = (List<EUTreeNode>)xmlSerialiser.Deserialize(fs);
#else
                        BinaryFormatter bf1 = new BinaryFormatter();
                        fs.Position = 0;

                        nodes = (List<EUTreeNode>)bf1.Deserialize(fs);
#endif

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
                return nodes;
            }
        }


        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        public EUSettings LoadSettings()
        {
            string settingFilePath = GetSettingFilePath();
            if (File.Exists(settingFilePath) == false)
                return null;

            // JOEL JEFFERY 20110713
            EUSettings settings = readSettings(settingFilePath);

            if (settings.IsMultipleConfigurationSettingVersion == false)
            {
                settings.IsMultipleConfigurationSettingVersion = true;
                if (settings.Url != null || settings.Url != String.Empty)
                {
                    EUSiteSetting siteSetting = new EUSiteSetting();
                    siteSetting.Url = settings.Url;
                    siteSetting.UseDefaultCredential = settings.UseDefaultCredential;
                    siteSetting.User = settings.User;
                    siteSetting.Password = settings.Password;
                    settings.SiteSettings = new List<EUSiteSetting>();
                    settings.SiteSettings.Add(siteSetting);

                    EUListSetting listSetting = new EUListSetting();
                    if (settings.EmailTitleSPFieldName != null && settings.EmailTitleSPFieldName != String.Empty)
                    {
                        EUEmailMapping emailMapping = new EUEmailMapping();
                        emailMapping.OutlookFieldName = "Subject";
                        emailMapping.OutlookFieldName = settings.EmailTitleSPFieldName;
                        listSetting.EmailMappings.Add(emailMapping);
                    }
                    if (settings.EmailFromSPFieldName != null && settings.EmailFromSPFieldName != String.Empty)
                    {
                        EUEmailMapping emailMapping = new EUEmailMapping();
                        emailMapping.OutlookFieldName = "From";
                        emailMapping.OutlookFieldName = settings.EmailFromSPFieldName;
                        listSetting.EmailMappings.Add(emailMapping);
                    }
                    if (settings.EmailToSPFieldName != null && settings.EmailToSPFieldName != String.Empty)
                    {
                        EUEmailMapping emailMapping = new EUEmailMapping();
                        emailMapping.OutlookFieldName = "To";
                        emailMapping.OutlookFieldName = settings.EmailToSPFieldName;
                        listSetting.EmailMappings.Add(emailMapping);
                    }
                    if (settings.EmailCCSPFieldName != null && settings.EmailCCSPFieldName != String.Empty)
                    {
                        EUEmailMapping emailMapping = new EUEmailMapping();
                        emailMapping.OutlookFieldName = "CC";
                        emailMapping.OutlookFieldName = settings.EmailCCSPFieldName;
                        listSetting.EmailMappings.Add(emailMapping);
                    }
                    if (settings.EmailBCCSPFieldName != null && settings.EmailBCCSPFieldName != String.Empty)
                    {
                        EUEmailMapping emailMapping = new EUEmailMapping();
                        emailMapping.OutlookFieldName = "BCC";
                        emailMapping.OutlookFieldName = settings.EmailBCCSPFieldName;
                        listSetting.EmailMappings.Add(emailMapping);
                    }
                    if (settings.EmailBodySPFieldName != null && settings.EmailBodySPFieldName != String.Empty)
                    {
                        EUEmailMapping emailMapping = new EUEmailMapping();
                        emailMapping.OutlookFieldName = "Body";
                        emailMapping.OutlookFieldName = settings.EmailBodySPFieldName;
                        listSetting.EmailMappings.Add(emailMapping);
                    }
                    if (settings.EmailSentSPFieldName != null && settings.EmailSentSPFieldName != String.Empty)
                    {
                        EUEmailMapping emailMapping = new EUEmailMapping();
                        emailMapping.OutlookFieldName = "Sent";
                        emailMapping.OutlookFieldName = settings.EmailSentSPFieldName;
                        listSetting.EmailMappings.Add(emailMapping);
                    }
                    if (settings.EmailReceivedSPFieldName != null && settings.EmailReceivedSPFieldName != String.Empty)
                    {
                        EUEmailMapping emailMapping = new EUEmailMapping();
                        emailMapping.OutlookFieldName = "Received";
                        emailMapping.OutlookFieldName = settings.EmailReceivedSPFieldName;
                        listSetting.EmailMappings.Add(emailMapping);
                    }
                    settings.DefaultListSetting = listSetting;
                    settings.ListSettings = new List<EUListSetting>();
                }
            }

            return settings;
        }

        /// <summary>
        /// Reads the settings.
        /// </summary>
        /// <param name="settingFilePath">The setting file path.</param>
        /// <returns></returns>
        private static EUSettings readSettings(string settingFilePath)
        {
            Debug.WriteLine("Entering " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            EUSettings settings = null;
            lock (_settingsFileLocker)
            {
                if (!File.Exists(settingFilePath))
                    return null;
                using (FileStream fs = new FileStream(settingFilePath, FileMode.Open))
                {
                    try
                    {

#if SerializeSettingsAsXml
                        XmlSerializer xmlSerialiser = new XmlSerializer(typeof(EUSettings));
                        settings = (EUSettings)xmlSerialiser.Deserialize(fs);
#else

                        BinaryFormatter bf1 = new BinaryFormatter();
                        fs.Position = 0;

                        EUSettings settings = (EUSettings)bf1.Deserialize(fs);
#endif
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
