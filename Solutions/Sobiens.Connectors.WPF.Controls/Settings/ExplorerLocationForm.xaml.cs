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

namespace Sobiens.Connectors.WPF.Controls.Settings
{
    /// <summary>
    /// Interaction logic for DocumentTemplateForm.xaml
    /// </summary>
    public partial class ExplorerLocationForm : HostControl
    {
        private Guid SelectedSiteSettingID { get; set; }
        private Folder SelectedFolder { get; set; }
        private string SelectedFolderPath { get; set; }
        private string SelectedFolderType { get; set; }
        private SiteSettings SiteSettings { get; set; }

        public ExplorerLocationForm()
        {
            InitializeComponent();
        }

        public void Initialize(SiteSettings siteSettings)
        {
            this.SiteSettings = siteSettings;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            LoadSiteSettings();

            ExplorerLocation explorerLocation = this.Tag as ExplorerLocation;
            this.SelectedSiteSettingID = explorerLocation.SiteSettingID;

            if (explorerLocation.ApplicationTypes.Contains(ApplicationTypes.General) == true)
            {
                GeneralATCheckBox.IsChecked = true;
            }

            if (explorerLocation.ApplicationTypes.Contains(ApplicationTypes.Excel) == true)
            {
                ExcelATCheckBox.IsChecked = true;
            }

            if (explorerLocation.ApplicationTypes.Contains(ApplicationTypes.Outlook) == true)
            {
                OutlookATCheckBox.IsChecked = true;
            }
            
            if (explorerLocation.ApplicationTypes.Contains(ApplicationTypes.Word) == true)
            {
                WordATCheckBox.IsChecked = true;
            }

            if (explorerLocation.ShowAll == true)
            {
                ShowAllRadioButton.IsChecked = true;
            }
            else
            {
                ShowSelectedRadioButton.IsChecked = true;
            }

            this.AllowToSelectSubFoldersCheckBox.IsChecked = explorerLocation.AllowToSelectSubfolders;


            if (SelectedSiteSettingID != null && SelectedSiteSettingID != Guid.Empty)
            {
                SiteSettingComboBox.SelectedValue = SelectedSiteSettingID;
            }
            else
            {
                if (SiteSettingComboBox.Items.Count > 0)
                    SiteSettingComboBox.SelectedIndex = 0;
            }

            ShowAllRadioButton_Checked(null, null);

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
            ExplorerLocation explorerLocation = (ExplorerLocation)this.Tag;
            explorerLocation.SiteSettingID = (Guid)SiteSettingComboBox.SelectedValue;
            explorerLocation.AllowToSelectSubfolders = AllowToSelectSubFoldersCheckBox.IsChecked.Value;
            explorerLocation.ShowAll = ShowAllRadioButton.IsChecked == true ? true : false;
            explorerLocation.ApplicationTypes = new List<ApplicationTypes>();
            if (GeneralATCheckBox.IsChecked == true)
            {
                explorerLocation.ApplicationTypes.Add(ApplicationTypes.General);
            }
            if (WordATCheckBox.IsChecked == true)
            {
                explorerLocation.ApplicationTypes.Add(ApplicationTypes.Word);
            }
            if (ExcelATCheckBox.IsChecked == true)
            {
                explorerLocation.ApplicationTypes.Add(ApplicationTypes.Excel);
            }
            if (OutlookATCheckBox.IsChecked == true)
            {
                explorerLocation.ApplicationTypes.Add(ApplicationTypes.Outlook);
            }

            if (explorerLocation.ShowAll == true)
            {
                SiteSetting siteSetting = (SiteSetting)SiteSettingComboBox.SelectedItem;
                explorerLocation.Folder = new SPWeb(siteSetting.Url, siteSetting.Url, siteSetting.ID, String.Empty, siteSetting.Url, siteSetting.Url);
                explorerLocation.BasicFolderDefinition = explorerLocation.Folder.GetBasicFolderDefinition();
                explorerLocation.Folder.Selected = true;
            }
        }

        private void SelectLocationButton_Click(object sender, RoutedEventArgs e)
        {
            ExplorerLocation explorerLocation = (ExplorerLocation)this.Tag;
            SiteSetting siteSetting = (SiteSetting)SiteSettingComboBox.SelectedItem;
            Folder rootFolder = null;

            SiteContentSelections siteContentSelections = new SiteContentSelections();
            if (explorerLocation.Folder == null)
            {
                rootFolder = new SPWeb(siteSetting.Url, siteSetting.Url, siteSetting.ID, String.Empty, siteSetting.Url, siteSetting.Url);
                rootFolder.PublicFolder = true;
                //siteContentSelections.InitializeForm(siteSetting, rootFolder, false, null);
                siteContentSelections.InitializeForm(siteSetting, rootFolder, true, null);

                if (siteContentSelections.ShowDialog(this.ParentWindow, Languages.Translate("Site Content Selections")) == true)
                {
                    explorerLocation.Folder = siteContentSelections.SelectedFolder;
                    explorerLocation.BasicFolderDefinition = explorerLocation.Folder.GetBasicFolderDefinition();
                }
            }
            else
            {
                rootFolder = explorerLocation.Folder;
                //siteContentSelections.InitializeForm(siteSetting, rootFolder, false, null);
                siteContentSelections.InitializeForm(siteSetting, rootFolder, true, null);

                if (siteContentSelections.ShowDialog(this.ParentWindow, Languages.Translate("Site Content Selections")) == true)
                {
                    explorerLocation.Folder = siteContentSelections.SelectedFolder;
                    explorerLocation.BasicFolderDefinition = explorerLocation.Folder.GetBasicFolderDefinition();
                }
            }

        }

        private void LoadSiteSettings()
        {
            SiteSettingComboBox.ItemsSource = this.SiteSettings;
        }

        private void ShowAllRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            AllowToSelectSubFoldersCheckBox.Visibility = (ShowAllRadioButton.IsChecked == true ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
            LocationsLabel.Visibility = (ShowAllRadioButton.IsChecked == true ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
            SelectLocationButton.Visibility = (ShowAllRadioButton.IsChecked == true ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
        }
    }
}
