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
using System.IO;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.WPF.Controls
{
    /// <summary>
    /// Interaction logic for UploadFilesForm.xaml
    /// </summary>
    public partial class UploadFilesForm : HostControl, IConnectorMainView
    {
        private string[] FilesToUpload;

        public object Inspector { get; set; }
        public DateTime InitializedDate { get; set; }
        public IWorkflowExplorer WorkflowExplorer
        {
            get
            {
                return null;
            }
        }

        public IDocumentTemplateSelection DocumentTemplateSelection
        {
            get
            {
                return null;
            }
        }

        public ISearchExplorer SearchExplorer
        {
            get
            {
                return null;
            }
        }

        public IConnectorExplorer ConnectorExplorer
        {
            get
            {
                return UploadLocation;
            }
        }

        public IStatusBar StatusBar
        {
            get
            {
                return StatusBarControl;
            }
        }


        public UploadFilesForm()
        {
            InitializeComponent();
        }

        public void RefreshControls()
        {
            this.ConnectorExplorer.Initialize(ConfigurationManager.GetInstance().GetSiteSettings(), ConfigurationManager.GetInstance().GetExplorerLocations(ApplicationContext.Current.GetApplicationType()));
        }

        public void Initialize(string[] filesToUpload)
        {
            this.FilesToUpload = filesToUpload;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            SiteSettings siteSettings = ConfigurationManager.GetInstance().GetSiteSettings();
            List<ExplorerLocation> explorerLocations = ConfigurationManager.GetInstance().GetExplorerLocations(ApplicationContext.Current.GetApplicationType());
            List<Folder> folders = ConfigurationManager.GetInstance().GetFoldersByExplorerLocations(explorerLocations, false);

            UploadLocation.InitializeForm(siteSettings, folders, true, null);
            this.OKButtonSelected += new EventHandler(UploadFilesForm_OKButtonSelected);
            this.SetUploadFilesText();
        }

        protected void UploadFilesForm_OKButtonSelected(object sender, EventArgs e)
        {
            if (UploadLocation.SelectedFolder == null)
            {
                MessageBox.Show(Languages.Translate("Please select a folder to upload"));
            }

            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(UploadLocation.SelectedFolder.SiteSettingID);
            IServiceManager serviceManagerFactory = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            string sourceFolder = ConfigurationManager.GetInstance().CreateATempFolder();

            foreach (string filePath in this.FilesToUpload)
            {
                Sobiens.Connectors.Entities.UploadItem uploadItem = new Entities.UploadItem();
                uploadItem.FilePath = filePath;
                uploadItem.Folder = UploadLocation.SelectedFolder;
                uploadItem.FieldInformations = new Dictionary<string, object>();

                Sobiens.Connectors.Common.ApplicationContext.Current.UploadFile(siteSetting, uploadItem, null, false, false, Upload_Success, Upload_Failed);
            }
        }

        public void Upload_Success(object sender, EventArgs e)
        {

        }

        public void Upload_Failed(object sender, EventArgs e)
        {
        }

        private void SetUploadFilesText()
        {
            StringBuilder filesTexts = new StringBuilder();
            foreach (string fileName in this.FilesToUpload)
            {
                FileInfo fi = new FileInfo(fileName);
                filesTexts.Append(fi.Name + Environment.NewLine);
            }
            FilesTextBlock.Text = filesTexts.ToString();
        }
    }
}
