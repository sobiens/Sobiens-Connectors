using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sobiens.Connectors.WPF.Controls;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.WPF.Controls.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Common;
using Microsoft.Win32;
using System.IO;
using Sobiens.Connectors.Entities.Documents;
using Sobiens.Connectors.Services.SharePoint;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.WPF.Controls.Selectors
{
    /// <summary>
    /// Interaction logic for DocumentTemplateForm.xaml
    /// </summary>
    public partial class DocumentTemplateSelection : BaseUserControl, IDocumentTemplateSelection
    {
        private List<DocumentTemplate> DocumentTemplates { get; set; }
        public bool IsDataLoaded { get; set; }
        public SiteSettings siteSettings {get;set;}
        public DocumentTemplateSelection()
        {
            InitializeComponent();
        }

        public void Initialize(SiteSettings siteSettings, List<DocumentTemplate> documentTemplates)
        {
            this.IsDataLoaded = false;
            this.siteSettings = siteSettings;
            this.DocumentTemplates = documentTemplates;
        }

        public void RefreshControls()
        {
            if (this.IsDataLoaded == false)
            {
                TemplatesGrid.ItemsSource = this.DocumentTemplates;
                TemplatesGrid.UpdateLayout();
                this.IsDataLoaded = true;
            }
        }

        public bool HasAnythingToDisplay
        {
            get
            {
                return this.DocumentTemplates.Count > 0 ? true : false;
            }
        }


        private void TemplatesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DocumentTemplate documentTemplate = (DocumentTemplate)TemplatesGrid.SelectedItem;

            List<DocumentTemplateMapping> mappings = ConfigurationManager.GetInstance().GetDocumentTemplateMappings(documentTemplate.ID);

            DocumentTemplateMapping selectedDocumentTemplateMapping = null;
            Folder selectedTargetFolder = null;
            SiteSetting siteSetting = null;
            ContentType contentType = null;
            IServiceManager serviceManager = null;

            if (mappings.Count == 0)
            {
                MessageBox.Show(Languages.Translate("This template does not have any mapping for location"));
                return;
            }
            else if (mappings.Count == 1)
            {
                selectedDocumentTemplateMapping = mappings[0];
                siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(selectedDocumentTemplateMapping.Folder.SiteSettingID);

                serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
                selectedTargetFolder = serviceManager.GetFolder(siteSetting, selectedDocumentTemplateMapping.Folder);
                contentType = serviceManager.GetContentType(siteSetting, selectedTargetFolder, selectedDocumentTemplateMapping.ContentTypeID, false);
            }
            else
            {
                DocumentTemplateLocationSelectionForm documentTemplateLocationSelectionForm = new DocumentTemplateLocationSelectionForm();
                documentTemplateLocationSelectionForm.Initialize(mappings);
                if (documentTemplateLocationSelectionForm.ShowDialog(null, Languages.Translate("Select a location")) == true)
                {
                    selectedDocumentTemplateMapping = documentTemplateLocationSelectionForm.SelectedDocumentTemplateMapping;
                    selectedTargetFolder = documentTemplateLocationSelectionForm.SelectedFolder;
                    siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(selectedDocumentTemplateMapping.Folder.SiteSettingID);

                    serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
                    contentType = serviceManager.GetContentType(siteSetting, selectedTargetFolder, selectedDocumentTemplateMapping.ContentTypeID, false);
                }
                else
                {
                    return;
                }
            }

            List<ContentType> contentTypes = new List<ContentType>();
            contentTypes.Add(contentType);

            FolderSettings folderSettings = ConfigurationManager.GetInstance().GetFolderSettings(ApplicationContext.Current.GetApplicationType()).GetRelatedFolderSettings(selectedTargetFolder.GetUrl());
            FolderSetting defaultFolderSetting = ConfigurationManager.GetInstance().GetFolderSettings(ApplicationContext.Current.GetApplicationType()).GetDefaultFolderSetting();

            EditItemPropertiesControl editItemPropertiesControl = new EditItemPropertiesControl(selectedTargetFolder.GetWebUrl(), null, contentTypes, folderSettings, defaultFolderSetting, siteSetting, selectedTargetFolder.GetUrl(), null,true);
            bool? dialogResult = editItemPropertiesControl.ShowDialog(null, Languages.Translate("Meta Data"));

            if (dialogResult == true)
            {
                LoadingWindow lw = new LoadingWindow();
                lw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                lw.Show();

                ContentType selectedContentType;
                Dictionary<object, object> values = editItemPropertiesControl.GetValues(out selectedContentType);
                string templateFilePath = documentTemplate.TemplatePath;
                FileInfo fi = new FileInfo(templateFilePath);
                string fileName = string.Empty;
                foreach (object key in values.Keys)
                {
                    Field f = key as Field;
                    if (f.Name.Equals("Title", StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        fileName = values[key] + fi.Extension;
                        break;
                    }
                }
                //string fileName = string.Format("w_{0}{1}", Guid.NewGuid().ToString().Replace("-", ""), fi.Extension);
                string actualFilePath = ConfigurationManager.GetInstance().GetTempFolder() + "\\" + fileName;

                ApplicationContext.Current.CreateNewFile(templateFilePath, actualFilePath);

                bool isListItemAndAttachment = ConfigurationManager.GetInstance().GetListItemAndAttachmentOption();

                UploadItem uploadItem = new UploadItem();
                uploadItem.FilePath = actualFilePath;
                uploadItem.Folder = selectedTargetFolder;
                uploadItem.ContentType = contentType;
                uploadItem.FieldInformations = values;
                Sobiens.Connectors.Common.ApplicationContext.Current.UploadFile(siteSetting, uploadItem, null, false, isListItemAndAttachment, Upload_Success, Upload_Failed);

                string documentURL = selectedTargetFolder.GetUrl() + "/" + fileName;

                object document = ApplicationContext.Current.OpenFile(siteSetting, documentURL);
                ApplicationContext.Current.RefreshControlsFromConfiguration();
                lw.Close();
                ApplicationContext.Current.ActivateDocument(document);
            }
        }

        public void Upload_Success(object sender, UploadEventArgs e)
        {
        }

        public void Upload_Failed(object sender, EventArgs e)
        {
        }

    }
}
