#if General
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Threading;
using System.Diagnostics;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities;
using System.Windows.Controls;
using Sobiens.Connectors.Entities.Documents;
using Sobiens.Connectors.Common.Interfaces;
using System.Reflection;
using System.Xml.Linq;
using Sobiens.Connectors.Common.Threading;
using Sobiens.Connectors.Common.Extensions;

namespace Sobiens.Connectors.Common
{
    public class ConfigurationManager
    {

        private static ConfigurationManager _Instance = null;

        private Dictionary<ApplicationTypes, List<ApplicationItemProperty>> ApplicationItemProperties = new Dictionary<ApplicationTypes,List<ApplicationItemProperty>>();

        public List<ApplicationItemProperty> GetApplicationItemProperties(ApplicationTypes applicationType)
        {
            if (ApplicationItemProperties.ContainsKey(applicationType) == false)
            {
                ApplicationItemProperties.Add(applicationType, GetApplicationItemPropertiesFromAssembly(applicationType));
            }

            return ApplicationItemProperties[applicationType];
        }

        private List<ApplicationItemProperty> GetApplicationItemPropertiesFromAssembly(ApplicationTypes applicationType)
        {
            List<ApplicationItemProperty> applicationItemProperties = new List<ApplicationItemProperty>();

            Assembly _assembly;
            Stream _imageStream;
            StreamReader _textStreamReader;

            _assembly = Assembly.GetExecutingAssembly();
            _imageStream = _assembly.GetManifestResourceStream("Sobiens.Connectors.Common.Resources." + applicationType.ToString() + "Settings.xml");
            _textStreamReader = new StreamReader(_imageStream);
            string xml = _textStreamReader.ReadToEnd();

            XElement solutionXml = XElement.Parse(xml);
            XNamespace m_Namespace = solutionXml.GetDefaultNamespace();

            var properties =
                from element in solutionXml.Elements(m_Namespace + "ItemProperties").Elements(m_Namespace + "Property")
                //where element.Attributes(m_Namespace + "Hidden").First().Value == "False"
                select element;

            foreach (XElement property in properties)
            {
                string name = string.Empty;
                string displayName = string.Empty;
                Type type = null;
                bool hidden = true;

                foreach (XAttribute attribute in property.Attributes())
                {
                    switch (attribute.Name.LocalName)
                    {
                        case "Name":
                            name = attribute.Value;
                            break;
                        case "DisplayName":
                            displayName = attribute.Value;
                            break;
                        case "Type":
                            switch (attribute.Value)
                            {
                                case "olNumber":
                                    type = typeof(decimal);
                                    break;
                                case "olText":
                                    type = typeof(string);
                                    break;
                                case "olDateTime":
                                    type = typeof(DateTime);
                                    break;
                                case "olYesNo":
                                    type = typeof(bool);
                                    break;
                                case "olOutlookInternal":
                                default:
                                    break;
                                //Not Supported
                            }
                            break;
                        case "Hidden":
                            if (bool.TryParse(attribute.Value, out hidden) == false)
                            {
                                hidden = true;
                            }
                            break;
                        default:
                            continue;
                    }
                }
                if (hidden == false && type != null && string.IsNullOrEmpty(name) == false && string.IsNullOrEmpty(displayName) == false)
                {
                    ApplicationItemProperty itemProperty = new ApplicationItemProperty(name, displayName, type);
                    applicationItemProperties.Add(itemProperty);
                }
            }
            return applicationItemProperties;
        }


        private AppConfiguration _Configuration = null;
        public AppConfiguration Configuration
        {
            get
            {
                if (_Configuration == null)
                    _Configuration = LoadConfiguration();
                return _Configuration;
            }
            set
            {
                _Configuration = value;
            }
        }

        private AppConfiguration _AdministrativeConfiguration = null;
        public AppConfiguration AdministrativeConfiguration
        {
            get
            {
                if (_AdministrativeConfiguration == null)
                    _AdministrativeConfiguration = LoadAdministrativeConfiguration();
                return _AdministrativeConfiguration;
            }
            set
            {
                _AdministrativeConfiguration = value;
            }
        }

        private ExternalAdministrationConfiguration _ExternalAdministrationConfiguration = null;
        public ExternalAdministrationConfiguration ExternalAdministrationConfiguration
        {
            get
            {
                if (_ExternalAdministrationConfiguration == null)
                    _ExternalAdministrationConfiguration = GetExternalAdministrativeConfiguration();
                return _ExternalAdministrationConfiguration;
            }
            set
            {
                _ExternalAdministrationConfiguration = value;
            }
        }

        public static ConfigurationManager GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new ConfigurationManager();
            }
            return _Instance;
        }
        private ConfigurationManager()
        {
        }

        public SiteSettings GetSiteSettings()
        {
            SiteSettings siteSettings = new SiteSettings();
            siteSettings.AddRange(Configuration.SiteSettings);
            siteSettings.AddRange(AdministrativeConfiguration.SiteSettings);
            return siteSettings;
        }

        public bool DontAskSaveAttachmentLocation
        {
            get
            {
                if (this.AdministrativeConfiguration.Exist == true)
                {
                    return this.AdministrativeConfiguration.OutlookConfigurations.DontAskSaveAttachmentLocation;
                }
                else if (this.Configuration.Exist == true)
                {
                    return this.Configuration.OutlookConfigurations.DontAskSaveAttachmentLocation;
                }
                else
                {
                    return true;
                }
            }
        }

            
        public Folder GetDefaultAttachmentSaveFolder()
        {
            if (this.AdministrativeConfiguration.OutlookConfigurations.DefaultAttachmentSaveFolder != null && string.IsNullOrEmpty(this.AdministrativeConfiguration.OutlookConfigurations.DefaultAttachmentSaveFolder.FolderUrl) == false)
            {
                SiteSetting siteSetting = this.GetSiteSetting(this.AdministrativeConfiguration.OutlookConfigurations.DefaultAttachmentSaveFolder.SiteSettingID);
                return ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetFolder(siteSetting, this.AdministrativeConfiguration.OutlookConfigurations.DefaultAttachmentSaveFolder);
            }
            else if (this.Configuration.OutlookConfigurations.DefaultAttachmentSaveFolder != null && string.IsNullOrEmpty(this.Configuration.OutlookConfigurations.DefaultAttachmentSaveFolder.FolderUrl) == false)
            {
                SiteSetting siteSetting = this.GetSiteSetting(this.Configuration.OutlookConfigurations.DefaultAttachmentSaveFolder.SiteSettingID);
                return ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetFolder(siteSetting, this.Configuration.OutlookConfigurations.DefaultAttachmentSaveFolder);
            }
            else
            {
                return null;
            }
        }

        public LogModes GetLogMode()
        {
            //if (this.AdministrativeConfiguration != null && this.AdministrativeConfiguration.Exist == true)
            //{
            //    return (this.AdministrativeConfiguration.DetailedLogMode == true ? LogModes.Detailed : LogModes.Normal);
            //}
            //else
                if (this.Configuration != null)
            {
                return (this.Configuration.DetailedLogMode == true ? LogModes.Detailed : LogModes.Normal);
            }
            else
            {
                return LogModes.Normal;
            }
        }

        public SiteSetting GetSiteSetting(Guid id)
        {
            return this.GetSiteSettings().SingleOrDefault(ss => ss.ID == id);
        }

        public SiteSetting GetSiteSetting(string rootWebURL)
        {
            return this.GetSiteSettings().SingleOrDefault(ss => ss.Url == rootWebURL);
        }

        public SiteSetting GetProbableSiteSetting(string webUrl)
        {
            SiteSettings siteSettings = this.GetSiteSettings();
            var query = from siteSetting in siteSettings
                        where webUrl.StartsWith(siteSetting.Url, StringComparison.InvariantCultureIgnoreCase) == true
                        select siteSetting;
            List<SiteSetting> matchedSiteSettings = query.ToList();
            SiteSetting bestMatchSiteSetting = null;
            foreach (SiteSetting siteSetting in matchedSiteSettings)
            {
                if (bestMatchSiteSetting != null)
                {
                    if (siteSetting.Url.Length > bestMatchSiteSetting.Url.Length)
                    {
                        bestMatchSiteSetting = siteSetting;
                    }
                }
                else
                {
                    bestMatchSiteSetting = siteSetting;
                }
            }

            return bestMatchSiteSetting;
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

        public void CleanLogFiles()
        {
            string logFolder = GetLogFolder();
            string[] fileNames = Directory.GetFiles(logFolder);
            foreach (string fileName in fileNames)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.CreationTime.AddDays(1) < DateTime.Now)
                {
                    // File might be in use
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch { }
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

        public string GetDocumentTemplatesFolder()
        {
            string folderPath = GetConfigurationsFolder();
            folderPath += "\\DocumentTemplates";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public string GetStateFilePath()
        {
            return GetConfigurationsFolder() + "\\AppState.xml";
        }

        private string GetConfigurationFilePath()
        {
            return GetConfigurationsFolder() + "\\AppConfiguration.xml";
        }

        public string GetAdministrativeConfigurationFilePath()
        {
            return GetConfigurationsFolder() + "\\AppAdministrativeConfiguration.xml";
        }

        private string GetExternalAdministrativeConfigurationFilePath()
        {
            return GetConfigurationsFolder() + "\\AppExternalAdministrativeConfiguration.xml";
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveAppConfiguration()
        {
            string configurationFilePath = this.GetConfigurationFilePath();
            DocumentTemplateManager.GetInstance().CopyTemplateDocumentsIntoTemplatesFolder(this.Configuration);
            SerializationManager.SaveConfiguration<AppConfiguration>(this.Configuration, configurationFilePath);
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveExternalAdministrationConfiguration()
        {
            string configurationFilePath = this.GetExternalAdministrativeConfigurationFilePath();
            SerializationManager.SaveConfiguration<ExternalAdministrationConfiguration>(this.ExternalAdministrationConfiguration, configurationFilePath);
        }



        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        public AppConfiguration LoadConfiguration()
        {
            string settingFilePath = GetConfigurationFilePath();
            if (File.Exists(settingFilePath) == false)
                return new AppConfiguration() { Exist = false };

            AppConfiguration configuration = SerializationManager.ReadSettings<AppConfiguration>(settingFilePath);
            configuration.Exist = true;
            DocumentTemplateManager.GetInstance().UpdateTemplateImages(configuration.DocumentTemplates);
            return configuration;
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        public AppConfiguration LoadAdministrativeConfiguration()
        {
            string settingFilePath = GetAdministrativeConfigurationFilePath();
            if (File.Exists(settingFilePath) == false)
                return new AppConfiguration() { Exist = false };

            AppConfiguration configuration = SerializationManager.ReadSettings<AppConfiguration>(settingFilePath);
            configuration.Exist = true;
            DocumentTemplateManager.GetInstance().UpdateTemplateImages(configuration.DocumentTemplates);

            foreach (ExplorerLocation explorerLocation in configuration.ExplorerConfiguration.ExplorerLocations)
            {
                if (explorerLocation.Folder == null && explorerLocation.BasicFolderDefinition != null)
                {
                    SiteSetting siteSetting = configuration.SiteSettings[explorerLocation.BasicFolderDefinition.SiteSettingID];
                    IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
                    explorerLocation.Folder = serviceManager.GetFolderByBasicFolderDefinition(siteSetting, explorerLocation.BasicFolderDefinition, true);
                }
            }

            return configuration;
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        public List<ExplorerLocation> GetExplorerLocations(ApplicationTypes applicationType)
        {
            List<ExplorerLocation> explorerLocations = new List<ExplorerLocation>();

            explorerLocations.AddRange(ConfigurationManager.GetInstance().AdministrativeConfiguration.ExplorerConfiguration.ExplorerLocations[applicationType]);
            explorerLocations.AddRange(ConfigurationManager.GetInstance().Configuration.ExplorerConfiguration.ExplorerLocations[applicationType]);

            return explorerLocations;
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        public List<WorkflowConfiguration> GetWorkflowConfigurations()
        {
            List<WorkflowConfiguration> workflowConfigurations = new List<WorkflowConfiguration>();

            workflowConfigurations.Add(this.AdministrativeConfiguration.WorkflowConfiguration);
            workflowConfigurations.Add(this.Configuration.WorkflowConfiguration);

            return workflowConfigurations;
        }


        public List<Folder> GetFoldersByExplorerLocations(List<ExplorerLocation> explorerLocations, bool returnAll)
        {
            List<Folder> folders = new List<Folder>();

            foreach (ExplorerLocation explorerLocation in explorerLocations)
            {
                if (returnAll == true)
                {
                    folders.Add(explorerLocation.Folder);
                }
                else
                {
                    folders.Add(explorerLocation.Folder.GetSelectedFolders());
                }
            }

            return folders;
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        private ExternalAdministrationConfiguration GetExternalAdministrativeConfiguration()
        {
            string settingFilePath = GetExternalAdministrativeConfigurationFilePath();
            if (File.Exists(settingFilePath) == false)
                return null;

            return SerializationManager.ReadSettings<ExternalAdministrationConfiguration>(settingFilePath);
        }


        public bool GetListItemAndAttachmentOption()
        {
            return ApplicationContext.Current.GetApplicationType() == ApplicationTypes.Outlook
                ? ConfigurationManager.GetInstance().Configuration.OutlookConfigurations.SaveAsWord
                : false;
        }

        public List<DocumentTemplateMapping> GetDocumentTemplateMappings(Guid templateID)
        {
            List<DocumentTemplateMapping> documentTemplateMapping = new List<DocumentTemplateMapping>();
            documentTemplateMapping.AddRange(this.AdministrativeConfiguration.DocumentTemplateMappings.GetDocumentTemplateMappings(templateID));
            documentTemplateMapping.AddRange(this.Configuration.DocumentTemplateMappings.GetDocumentTemplateMappings(templateID));
            return documentTemplateMapping;
        }

        public List<DocumentTemplateMapping> GetDocumentTemplateMappings()
        {
            List<DocumentTemplateMapping> documentTemplateMapping = new List<DocumentTemplateMapping>();
            documentTemplateMapping.AddRange(this.AdministrativeConfiguration.DocumentTemplateMappings);
            documentTemplateMapping.AddRange(this.Configuration.DocumentTemplateMappings);
            return documentTemplateMapping;
        }

        public void DownloadAdministrationXml(Action callback)
        {
            ExternalAdministrationConfiguration config = GetExternalAdministrativeConfiguration();
            if (config == null)
                return;

            WorkItem workItem = new WorkItem(Languages.Translate("Retrieving administrative configuration"));
            workItem.CallbackFunction = new WorkRequestDelegate(LoadAdministrative_Callback);
            workItem.CallbackData = new object[]{config, callback};
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        void LoadAdministrative_Callback(object item, DateTime dateTime)
        {
            object[] args = (object[])item;
            ExternalAdministrationConfiguration config = (ExternalAdministrationConfiguration)args[0];
            Action callback = (Action)args[1];
            ServiceManagerFactory.GetServiceManager(config.SiteSetting.SiteSettingType).DownloadAdministrativeConfiguration(config.SiteSetting, config.Url, GetAdministrativeConfigurationFilePath());
            LoadAdministrativeConfiguration();
            callback();
        }

        public List<DocumentTemplate> GetDocumentTemplates(ApplicationTypes applicationType)
        {
            List<DocumentTemplate> documentTemplates = new List<DocumentTemplate>();
            documentTemplates.AddRange(this.AdministrativeConfiguration.DocumentTemplates.GetDocumentTemplates(applicationType));
            documentTemplates.AddRange(this.Configuration.DocumentTemplates.GetDocumentTemplates(applicationType));
            return documentTemplates;
        }

        public DocumentTemplates GetDocumentTemplates()
        {
            DocumentTemplates documentTemplates = new DocumentTemplates();
            documentTemplates.AddRange(this.AdministrativeConfiguration.DocumentTemplates);
            documentTemplates.AddRange(this.Configuration.DocumentTemplates);
            return documentTemplates;
        }

        public FolderSettings GetFolderSettings(ApplicationTypes applicationType)
        {
            FolderSettings folderSettings = new FolderSettings();

            FolderSettings folderSettings1 = this.Configuration.FolderSettings.GetFolderSettings(applicationType);
            if(folderSettings1 != null)
            {
                folderSettings.AddRange(folderSettings1);
            }

            FolderSettings folderSettings2 = this.AdministrativeConfiguration.FolderSettings.GetFolderSettings(applicationType);
            if (folderSettings2 != null)
            {
                folderSettings.AddRange(folderSettings2);
            }

            return folderSettings;
        }

        public void SetFolderSetting(ApplicationTypes applicationType, FolderSetting folderSetting)
        {
            FolderSettings folderSettings = this.Configuration.FolderSettings.GetFolderSettings(applicationType);
            if (folderSettings[folderSetting.Folder.GetUrl()] != null)
            {
                folderSettings[folderSetting.Folder.GetUrl()] = folderSetting;
            }
            else
            {
                this.Configuration.FolderSettings.Add(folderSetting);
            }

        }

        public string GetMappedServicePropertyName(ApplicationTypes applicationType, string url, string applicationPropertyName, string contentTypeID)
        {
            FolderSetting folderSetting = GetFolderSettings(applicationType)[url];
            if (folderSetting == null)
                return String.Empty;
            ItemPropertyMapping itemPropertyMapping = folderSetting.ItemPropertyMappings.SingleOrDefault(em => em.ContentTypeID.Equals(contentTypeID, StringComparison.InvariantCultureIgnoreCase) && em.ApplicationPropertyName.Equals(applicationPropertyName, StringComparison.InvariantCultureIgnoreCase) && string.IsNullOrEmpty(em.ServicePropertyName) == false);
            if (itemPropertyMapping != null)
                return itemPropertyMapping.ServicePropertyName;
            return String.Empty;
        }


        public string GetSyncTaskFolder(SyncTask syncTask)
        {
            string folderPath = GetSyncTasksFolder();
            folderPath += "\\SyncData";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            folderPath += "\\" + syncTask.ProcessID.ToString().Replace("{", "").Replace("}", "").Replace("-", "");
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public string GetSyncTaskFolder(Guid processId)
        {
            string folderPath = GetSyncTasksFolder();
            folderPath += "\\SyncData";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            folderPath += "\\" + processId.ToString().Replace("{", "").Replace("}", "").Replace("-", "");
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }


        public string GetSyncTasksFilePath()
        {
            string settingFilePath = System.Configuration.ConfigurationManager.AppSettings["SyncTasksLocation"];
            if (string.IsNullOrEmpty(settingFilePath) == false
                && System.IO.File.Exists(settingFilePath) == true)
            {
                return settingFilePath;
            }

            return GetSyncTasksFolder() + "\\SyncTasks.xml";
        }

        public string GetSyncTaskStatusFilePath(SyncTask syncTask)
        {
            string settingFilePath = System.Configuration.ConfigurationManager.AppSettings["SyncTasksLocation"];
            if (string.IsNullOrEmpty(settingFilePath) == false
                && System.IO.File.Exists(settingFilePath) == true)
            {
                FileInfo fi = new FileInfo(settingFilePath);
                return fi.Directory.FullName + "\\SyncTaskStatus.xml";
            }

            return ConfigurationManager.GetInstance().GetSyncTaskFolder(syncTask) + "\\SyncTaskStatus.xml";
        }


        public string GetSyncTasksFolder()
        {
            string settingFilePath = System.Configuration.ConfigurationManager.AppSettings["SyncTasksLocation"];
            if (string.IsNullOrEmpty(settingFilePath) == false
                && System.IO.File.Exists(settingFilePath) == true)
            {
                FileInfo fi = new FileInfo(settingFilePath);
                return fi.Directory.FullName;
            }


            string folderPath = GetApplicationFolder();
            folderPath += "\\SyncTasks";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public string GetSyncTaskHistoriesFolder()
        {
            string folderPath = GetSyncTasksFolder();
            folderPath += "\\Histories";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public string GetCurrentSyncTaskHistoryFilePath()
        {
            return GetSyncTaskHistoryFilePath(DateTime.Now);
        }

        public string GetSyncTaskHistoryFilePath(DateTime date)
        {
            string folderPath = GetSyncTaskHistoriesFolder();
            return folderPath + "\\" + date.ToString("yyyyMMdd") + ".txt";
        }

        public string GetProjectsFolder()
        {
            string folderPath = GetApplicationFolder();
            folderPath += "\\Projects";
            if (System.IO.Directory.Exists(folderPath) == false)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public List<QueryProjectObject> GetProjects()
        {
            List<QueryProjectObject> queryProjectObjects = new List<QueryProjectObject>();
            string folderPath = GetProjectsFolder();
            string[] projectFolders = Directory.GetDirectories(folderPath);
            foreach (string projectFolder in projectFolders)
            {
                string projectFilePath = projectFolder + "\\QueryPanelProject.xml";
                QueryProjectObject queryProjectObject = SerializationManager.ReadSettings<QueryProjectObject>(projectFilePath);
                queryProjectObjects.Add(queryProjectObject);
            }
            return queryProjectObjects;
        }

        public void SaveProject(QueryProjectObject queryProjectObject)
        {
            string folderPath = ConfigurationManager.GetInstance().GetProjectsFolder() + "\\" + queryProjectObject.FolderName;
            if (Directory.Exists(folderPath) == false)
                Directory.CreateDirectory(folderPath);

            SerializationManager.SaveConfiguration<QueryProjectObject>(queryProjectObject, folderPath + "\\QueryPanelProject.xml");

        }

    }
}
#endif
