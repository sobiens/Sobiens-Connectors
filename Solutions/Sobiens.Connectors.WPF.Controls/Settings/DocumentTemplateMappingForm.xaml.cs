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
using Sobiens.Connectors.Entities.Documents;
using Sobiens.Connectors.Common.Threading;
using System.Windows.Threading;
using System.Threading;

namespace Sobiens.Connectors.WPF.Controls.Settings
{
    /// <summary>
    /// Interaction logic for DocumentTemplateForm.xaml
    /// </summary>
    public partial class DocumentTemplateMappingForm : HostControl
    {
        private Guid SelectedSiteSettingID { get; set; }
        private Guid SelectedTemplateID { get; set; }
        private string SelectedContentTypeID { get; set; }
        private Folder SelectedFolder { get; set; }
        private string SelectedFolderPath { get; set; }
        private string SelectedFolderType { get; set; }
        private SiteSettings SiteSettings { get; set; }
        private List<DocumentTemplate> DocumentTemplates { get; set; }

        public DocumentTemplateMappingForm()
        {
            InitializeComponent();
        }

        public void Initialize(List<DocumentTemplate> documentTemplates, SiteSettings siteSettings)
        {
            this.SiteSettings = siteSettings;
            this.DocumentTemplates = documentTemplates;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            LoadDocumentTemplates();
            LoadSiteSettings();

            DocumentTemplateMapping documentTemplateMapping = this.Tag as DocumentTemplateMapping;
            if (documentTemplateMapping.Folder != null)
            {
                this.SelectedSiteSettingID = documentTemplateMapping.Folder.SiteSettingID;
                this.SelectedFolderPath = documentTemplateMapping.Folder.FolderUrl;
                this.SelectedFolderType = documentTemplateMapping.Folder.FolderType;
            }
            this.SelectedTemplateID = documentTemplateMapping.DocumentTemplateID;
            this.SelectedContentTypeID = documentTemplateMapping.ContentTypeID;
            this.AllowToSelectSubFoldersCheckBox.IsChecked = documentTemplateMapping.AllowToSelectSubFolders;

            if (SelectedTemplateID != null && SelectedTemplateID != Guid.Empty)
            {
                TemplatesComboBox.SelectedValue = SelectedTemplateID;
            }
            else
            {
                if (TemplatesComboBox.Items.Count > 0)
                    TemplatesComboBox.SelectedIndex = 0;
            }

            if (SelectedSiteSettingID != null && SelectedSiteSettingID != Guid.Empty)
            {
                SiteSettingComboBox.SelectedValue = SelectedSiteSettingID;
            }
            else
            {
                if (SiteSettingComboBox.Items.Count > 0)
                    SiteSettingComboBox.SelectedIndex = 0;
            }

            /*
            if (this.SelectedFolder != null)
            {
                this.SaveLocationTextBox.Text = this.SelectedFolder.Title;
                this.LoadContentTypes(this.SelectedFolder);
            }
             * */
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            this.ParentWindow.OKButtonSelected += new EventHandler(ParentWindow_OKButtonSelected);
        }

        protected void ParentWindow_OKButtonSelected(object sender, EventArgs e)
        {
            DocumentTemplate documentTemplate = TemplatesComboBox.SelectedItem as DocumentTemplate;
            DocumentTemplateMapping documentTemplateMapping = (DocumentTemplateMapping)this.Tag;
            documentTemplateMapping.Folder = this.SelectedFolder.GetBasicFolderDefinition();
            documentTemplateMapping.ContentTypeID = ContentTypeComboBox.SelectedValue.ToString();
            documentTemplateMapping.ContentTypeName = ContentTypeComboBox.Text;
            documentTemplateMapping.DocumentTemplateName = TemplatesComboBox.Text;
            documentTemplateMapping.DocumentTemplateID = (Guid)TemplatesComboBox.SelectedValue;
            documentTemplateMapping.AllowToSelectSubFolders = AllowToSelectSubFoldersCheckBox.IsChecked.Value;
        }

        private void SelectLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if (SiteSettingComboBox.SelectedItem == null)
            {
                MessageBox.Show(Languages.Translate("Please select a site setting."));
                return;
            }

            string siteSettingUrl = SiteSettingComboBox.SelectedItem.ToString();
            Guid siteSettingId = (Guid)SiteSettingComboBox.SelectedValue;
            List<Folder> folders = new List<Folder>();
            SPWeb web = new SPWeb(siteSettingUrl, siteSettingUrl, siteSettingId, siteSettingUrl, siteSettingUrl, siteSettingUrl);
            web.PublicFolder = true;
            folders.Add(web);
            SiteContentSelections siteContentSelections = new SiteContentSelections();
            siteContentSelections.InitializeForm(this.SiteSettings, folders, true, null);
            if (siteContentSelections.ShowDialog(this.ParentWindow, Languages.Translate("Save Location Selections")) == true)
            {
                this.SelectedFolder = siteContentSelections.SelectedFolder;
                this.SaveLocationTextBox.Text = this.SelectedFolder.Title;
                this.LoadContentTypes(this.SelectedFolder);
            }
        }

        private void LoadDocumentTemplates()
        {
            TemplatesComboBox.ItemsSource = this.DocumentTemplates;
        }

        private void LoadSiteSettings()
        {
            SiteSettingComboBox.ItemsSource = this.SiteSettings;
        }

        void callback(object args, DateTime dateTime)
        {
            object[] arguments = args as object[];
            SiteSetting siteSetting = arguments[0] as SiteSetting;
            Folder folder = arguments[1] as Folder;
            ComboBox ContentTypeComboBox = arguments[2] as ComboBox;

            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            List<ContentType> contentTypes = serviceManager.GetContentTypes(siteSetting, this.SelectedFolder, false);

            ContentTypeComboBox.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                ContentTypeComboBox.Items.Clear();
                foreach (ContentType contentType in contentTypes)
                {
                    ContentTypeComboBox.Items.Add(new { Name = contentType.Name, ID = contentType.ID });
                }

                if (ContentTypeComboBox.Items.Count > 0)
                {
                    if (string.IsNullOrEmpty(this.SelectedContentTypeID) == true)
                    {
                        ContentTypeComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        ContentTypeComboBox.SelectedValue = this.SelectedContentTypeID;
                    }
                }
                this.HideLoadingStatus("Ready");
            }));
        }

        private void LoadContentTypes(Folder selectedFolder)
        {
            this.ShowLoadingStatus(Languages.Translate("Loading content types..."));

            SiteSetting siteSetting = this.SiteSettings[this.SelectedFolder.SiteSettingID];
            object[] args = new object[] { siteSetting, this.SelectedFolder, ContentTypeComboBox };

            WorkItem workItem = new WorkItem(Languages.Translate("Populating content types"));
            workItem.CallbackFunction = new WorkRequestDelegate(callback);
            workItem.CallbackData = args;
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        private void ContentTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
