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

namespace Sobiens.Connectors.WPF.Controls.Settings
{
    /// <summary>
    /// Interaction logic for DocumentTemplateForm.xaml
    /// </summary>
    public partial class DocumentTemplateForm : HostControl
    {
        private string SelectedContentTypeID { get; set; }
        private Folder SelectedFolder { get; set; }
        private SiteSettings SiteSettings { get; set; }
        private string TemplatePath { get; set; }
        private bool hasTemplateDocumentChanged = false;

        public DocumentTemplateForm()
        {
            InitializeComponent();
        }

        public void Initialize(Folder selectedFolder, string selectedContentTypeID, SiteSettings siteSettings)
        {
            this.SelectedFolder = selectedFolder;
            this.SelectedContentTypeID = selectedContentTypeID;
            this.SiteSettings = siteSettings;

            /*
            if (this.SelectedFolder != null)
            {
                this.SaveLocationTextBox.Text = this.SelectedFolder.Title;
                this.LoadContentTypes(this.SelectedFolder);
            }
             */ 

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            this.ParentWindow.OKButtonSelected += new EventHandler(ParentWindow_OKButtonSelected);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            DocumentTemplate documentTemplate = (DocumentTemplate)this.Tag;
            TitleTextBox.Text = documentTemplate.Title;
            DescriptionTextBox.Text = documentTemplate.Description;
            pictureSelection.Initialize(documentTemplate.ImageID);
            TemplatePath = documentTemplate.TemplatePath;
            if (documentTemplate.ApplicationType == ApplicationTypes.Word)
            {
                WordTemplateApplicationRadioButton.IsChecked = true;
            }
            else if (documentTemplate.ApplicationType == ApplicationTypes.Outlook)
            {
                OutlookTemplateApplicationRadioButton.IsChecked = true;
            }
            else
            {
                ExcelTemplateApplicationRadioButton.IsChecked = true;
            }

        }

        protected void ParentWindow_OKButtonSelected(object sender, EventArgs e)
        {
            DocumentTemplate documentTemplate = (DocumentTemplate)this.Tag;
            documentTemplate.Title = TitleTextBox.Text;
            documentTemplate.Description = DescriptionTextBox.Text;
            documentTemplate.ImageID = pictureSelection.ImageID;
            SC_Image image = ImageManager.GetInstance().Images.GetImageByID(pictureSelection.ImageID);
            documentTemplate.ImagePath = image.ImagePath;                
            documentTemplate.TemplatePath = TemplatePath;
            if (WordTemplateApplicationRadioButton.IsChecked == true)
            {
                documentTemplate.ApplicationType = ApplicationTypes.Word;
            }
            else if (OutlookTemplateApplicationRadioButton.IsChecked == true)
            {
                documentTemplate.ApplicationType = ApplicationTypes.Outlook;
            }
            else
            {
                documentTemplate.ApplicationType = ApplicationTypes.Excel;
            }
//            documentTemplate.TargetFolder = this.SelectedFolder;
//            documentTemplate.ContentTypeID = ContentTypeComboBox.SelectedValue.ToString();
        }

        private void SelectLocationButton_Click(object sender, RoutedEventArgs e)
        {
            List<Folder> folders = new List<Folder>();
            foreach (SiteSetting siteSetting in this.SiteSettings)
            {
                SPWeb web = new SPWeb(siteSetting.Url, siteSetting.Url, siteSetting.ID, siteSetting.Url, siteSetting.Url, siteSetting.Url);
                web.PublicFolder = true;

                folders.Add(web);
            }
            SiteContentSelections siteContentSelections = new SiteContentSelections();
            siteContentSelections.InitializeForm(this.SiteSettings, folders, true, null);
            if (siteContentSelections.ShowDialog(this.ParentWindow, Languages.Translate("Save Location Selections")) == true)
            {
                this.SelectedFolder = siteContentSelections.SelectedFolder;
//                this.SaveLocationTextBox.Text = this.SelectedFolder.Title;
//                this.LoadContentTypes(this.SelectedFolder);
            }
        }
        /*
        private void LoadContentTypes(Folder selectedFolder)
        {
            SiteSetting siteSetting = this.SiteSettings[this.SelectedFolder.SiteSettingID];
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            List<ContentType> contentTypes = serviceManager.GetContentTypes(siteSetting, this.SelectedFolder, false);

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
        }
        */

        private void ContentTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SelectTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog(this.ParentWindow) == true)
            {
                TemplatePath = openFileDialog.FileName;
                FileInfo fi = new FileInfo(TemplatePath);
                TemplatePathLabel.Content = fi.Name;
                hasTemplateDocumentChanged = true;
            }

        }
    }
}
