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

namespace Sobiens.Connectors.WPF.Controls.Selectors
{
    /// <summary>
    /// Interaction logic for DocumentTemplateForm.xaml
    /// </summary>
    public partial class DocumentTemplateLocationSelectionForm : HostControl
    {
        private List<DocumentTemplateMapping> Mappings { get; set; }
        public DocumentTemplateMapping SelectedDocumentTemplateMapping = null;
        public Folder SelectedFolder = null;
        public DocumentTemplateLocationSelectionForm()
        {
            InitializeComponent();
        }

        public void Initialize(List<DocumentTemplateMapping> mappings)
        {
            this.Mappings = mappings;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            this.ParentWindow.OKButtonSelected += new EventHandler(ParentWindow_OKButtonSelected);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            LocationsComboBox.ItemsSource = Mappings;
        }

        protected void ParentWindow_OKButtonSelected(object sender, EventArgs e)
        {
            if (LocationsComboBox.SelectedItem == null)
            {
                MessageBox.Show(Languages.Translate("Please select a location"));
                return;
            }

            SelectedDocumentTemplateMapping = LocationsComboBox.SelectedItem as DocumentTemplateMapping;
        }

        private void SelectFromSubFoldersButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentTemplateMapping documentTemplateMapping = LocationsComboBox.SelectedItem as DocumentTemplateMapping;

            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(documentTemplateMapping.Folder.SiteSettingID);
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            Folder folder = serviceManager.GetFolder(siteSetting, documentTemplateMapping.Folder);
            SiteContentSelections siteContentSelections = new SiteContentSelections();
            siteContentSelections.InitializeForm(siteSetting, folder, true, null);
            if (siteContentSelections.ShowDialog(this.ParentWindow, Languages.Translate("Save Location Selection")) == true)
            {
                this.SelectedFolder = siteContentSelections.SelectedFolder;
                this.SelectedFolderLabel.Content = this.SelectedFolder.Title;
            }

        }

        private void LocationsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DocumentTemplateMapping documentTemplateMapping = LocationsComboBox.SelectedItem as DocumentTemplateMapping;
            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(documentTemplateMapping.Folder.SiteSettingID);
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            Folder folder = serviceManager.GetFolder(siteSetting, documentTemplateMapping.Folder);
            this.SelectedFolder = folder;
            this.SelectedFolderLabel.Content = this.SelectedFolder.Title;

            SelectFromSubFoldersButton.Visibility = documentTemplateMapping.AllowToSelectSubFolders == true ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

    }
}
