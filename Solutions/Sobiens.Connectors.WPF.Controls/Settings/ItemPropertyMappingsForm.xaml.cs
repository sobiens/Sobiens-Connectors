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
using System.Reflection;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.WPF.Controls.Settings
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class ItemPropertyMappingsForm : HostControl
    {
        private SiteSettings SiteSettings { get; set; }
        private ApplicationTypes ApplicationType { get; set; }
        private Folder _SelectedFolder = null;
        private Folder SelectedFolder
        {
            get
            {
                if (GeneralLocationTypeRadioButton.IsChecked == true)
                {
                    return null;
                }
                else
                {
                    return _SelectedFolder;
                }
            }
            set
            {
                _SelectedFolder = value;
            }
        }

        public FolderSettings FolderSettings { get; private set; }

        private ApplicationTypes SelectedApplicationType
        {
            get
            {
                if (OutlookApplicationRadioButton.IsChecked == true)
                {
                    return ApplicationTypes.Outlook;
                }
                else if (ExcelApplicationRadioButton.IsChecked == true)
                {
                    return ApplicationTypes.Excel;
                }

                return ApplicationTypes.Word;
            }
        }


        public ItemPropertyMappingsForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ItemPropertyMappings_Loaded);
        }

        void ItemPropertyMappings_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(ItemPropertyMappings_Loaded);
            this.LoadSiteSettings();
            if (this.ApplicationType == null)
            {
                this.ApplicationType = ApplicationTypes.Word;
            }

            if (this.ApplicationType == ApplicationTypes.Excel)
            {
                ExcelApplicationRadioButton.IsChecked = true;
            }
            else if (this.ApplicationType == ApplicationTypes.Outlook)
            {
                OutlookApplicationRadioButton.IsChecked = true;
            }
            else if (this.ApplicationType == ApplicationTypes.Word)
            {
                WordApplicationRadioButton.IsChecked = true;
            }

            if (this.SelectedFolder == null)
            {
                GeneralLocationTypeRadioButton.IsChecked = true;
            }
            else
            {
                SpecificLocationTypeRadioButton.IsChecked = true;
                this.SaveLocationTextBox.Text = this.SelectedFolder.Title;
                this.LoadContentTypes(this.SelectedFolder);
            }

            LocationTypeRadioButton_Checked(null, null);
            this.RefreshMappingListView();
        }


        public void Initialize(SiteSettings siteSettings, Folder selectedFolder, ApplicationTypes applicationType, FolderSettings folderSettings)
        {
            this.ApplicationType = applicationType;
            this.FolderSettings = folderSettings ;
            this.SelectedFolder = selectedFolder;
            this.SiteSettings = siteSettings;
        }

        private void LoadSiteSettings()
        {
            SiteSettingComboBox.ItemsSource = this.SiteSettings;

            if (SiteSettingComboBox.Items.Count > 0)
            {
                SiteSettingComboBox.SelectedIndex = 0;
            }
        }


        private void LoadContentTypes(Folder selectedFolder)
        {
            this.ShowLoadingStatus(Languages.Translate("Loading content type..."));
            SiteSetting siteSetting = this.SiteSettings[selectedFolder.SiteSettingID];
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            List<ContentType> contentTypes = serviceManager.GetContentTypes(siteSetting, selectedFolder, false);

            //ContentTypeComboBox.ItemsSource = contentTypes;
            foreach (ContentType c in contentTypes)
            {
                ContentType transaled = c;
                transaled.Name = Languages.Translate(c.Name);
                ContentTypeComboBox.Items.Add(c);
            }

            if (ContentTypeComboBox.Items.Count > 0)
            {
                ContentTypeComboBox.SelectedIndex = 0;
            }

            this.HideLoadingStatus(Languages.Translate("Ready"));
        }

        private void RefreshMappingListView()
        {
            string selectedContentTypeID = string.Empty;
            if (ContentTypeComboBox.SelectedValue != null)
            {
                selectedContentTypeID = ((ContentType)ContentTypeComboBox.SelectedValue).ID;
            }

            MappingsListView.Items.Clear();
            ItemPropertyMappings mappings;
            if (SpecificLocationTypeRadioButton.IsChecked == true && string.IsNullOrEmpty(selectedContentTypeID) == false)
            {
                mappings = this.FolderSettings.GetFolderSettings(this.SelectedApplicationType).GetItemPropertyMappings(selectedContentTypeID);
            }
            else
            {
                mappings = this.FolderSettings.GetFolderSettings(this.SelectedApplicationType).GetItemPropertyMappings();
            }

            foreach (ItemPropertyMapping itemPropertyMapping in mappings)
            {
                MappingsListView.Items.Add(itemPropertyMapping);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContentType selectedContentType ;
            if (GeneralLocationTypeRadioButton.IsChecked == true)
            {
                selectedContentType = null;
            }
            else
            {
                selectedContentType = ContentTypeComboBox.SelectedValue as ContentType;
                if (selectedContentType == null)
                {
                    MessageBox.Show(Languages.Translate("Please select a content type or use general location instead of specific"));
                }
            }

            ItemPropertyMappingForm itemPropertyMapping = new ItemPropertyMappingForm();
            itemPropertyMapping.Initialize(ConfigurationManager.GetInstance().GetApplicationItemProperties(this.SelectedApplicationType), selectedContentType);

            if (itemPropertyMapping.ShowDialog(this.ParentWindow, Languages.Translate("Add Mapping"),300,350) == true)
            {
                ItemPropertyMapping newItemPropertyMapping = new ItemPropertyMapping();
                newItemPropertyMapping.ID = Guid.NewGuid();
                if (selectedContentType != null)
                {
                    newItemPropertyMapping.ContentTypeID = selectedContentType.ID;
                }

                newItemPropertyMapping.ApplicationPropertyName = itemPropertyMapping.SelectedApplicationPropertyID;
                newItemPropertyMapping.ServicePropertyName = itemPropertyMapping.SelectedServicePropertyID;

                FolderSetting folderSetting;
                if (this.SelectedFolder == null)
                {
                    folderSetting = this.FolderSettings.GetDefaultFolderSetting(this.SelectedApplicationType);
                    folderSetting.ItemPropertyMappings.Add(newItemPropertyMapping);
                }
                else
                {
                    folderSetting = this.FolderSettings.GetFolderSetting(this.SelectedApplicationType, this.SelectedFolder);
                    if (folderSetting != null)
                    {
                        folderSetting.ItemPropertyMappings.Add(newItemPropertyMapping);
                    }
                    else
                    {
                        folderSetting = new FolderSetting();
                        folderSetting.ApplicationType = this.SelectedApplicationType;
                        folderSetting.BasicFolderDefinition = this.SelectedFolder.GetBasicFolderDefinition();
                        folderSetting.Folder = this.SelectedFolder;
                        folderSetting.ItemPropertyMappings.Add(newItemPropertyMapping);
                        this.FolderSettings.Add(folderSetting);
                    }
                }

                this.RefreshMappingListView();
            }
        }

        private void MappingsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ItemPropertyMapping itemPropertyMapping = (ItemPropertyMapping)MappingsListView.SelectedItem;
            FolderSetting folderSetting;
            itemPropertyMapping = this.FolderSettings.GetItemPropertyMapping(itemPropertyMapping.ID, out folderSetting);

            EditItem(itemPropertyMapping, folderSetting);
        }

        private void EditItem(ItemPropertyMapping itemPropertyMapping, FolderSetting folderSetting)
        {
            ContentType selectedContentType = null;

            if (folderSetting.Folder != null)
            {
                //SiteSetting siteSetting = this.SiteSettings[folderSetting.Folder.SiteSettingID];//JD
                ISiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(folderSetting.Folder.SiteSettingID);
                IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
                List<ContentType> contentTypes = serviceManager.GetContentTypes(siteSetting, folderSetting.Folder, false);

                selectedContentType = contentTypes.Single(t => t.ID.Equals(itemPropertyMapping.ContentTypeID));
            }

            ItemPropertyMappingForm itemPropertyMappingForm = new ItemPropertyMappingForm();
            itemPropertyMappingForm.Initialize(ApplicationContext.Current.GetApplicationFields(null), selectedContentType, itemPropertyMapping.ApplicationPropertyName, itemPropertyMapping.ServicePropertyName);

            if (itemPropertyMappingForm.ShowDialog(this.ParentWindow, Languages.Translate("Add Mapping"),200,300) == true)
            {
                itemPropertyMapping.ApplicationPropertyName = itemPropertyMappingForm.SelectedApplicationPropertyID;
                itemPropertyMapping.ServicePropertyName = itemPropertyMappingForm.SelectedServicePropertyID;
                this.RefreshMappingListView();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Guid id = (Guid)((Button)e.Source).Tag;
            FolderSetting folderSetting;
            ItemPropertyMapping itemPropertyMapping = this.FolderSettings.GetItemPropertyMapping(id, out folderSetting);
            EditItem(itemPropertyMapping, folderSetting);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Guid id = (Guid)((Button)e.Source).Tag;
            this.FolderSettings.DeleteItemPropertyMapping(id);
            RefreshMappingListView();
        }

        private void ContentTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.RefreshMappingListView();
        }

        private void LocationTypeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (GeneralLocationTypeRadioButton.IsChecked == true)
            {
                SiteSettingComboBox.Visibility = System.Windows.Visibility.Hidden;
                DescriptionLabel.Visibility = System.Windows.Visibility.Hidden;
                URLLabel.Visibility = System.Windows.Visibility.Hidden;
                SaveLocationTextBox.Visibility = System.Windows.Visibility.Hidden;
                SelectLocationButton.Visibility = System.Windows.Visibility.Hidden;
                ContentTypeLabel.Visibility = System.Windows.Visibility.Hidden;
                ContentTypeComboBox.Visibility = System.Windows.Visibility.Hidden;
                MappingsListView.Height = 333;
                AddButton.Margin = new Thickness(491, 70, 0, 0);
                MappingsListView.Margin = new Thickness(12, 100, 0, 0);
            }
            else
            {
                SiteSettingComboBox.Visibility = System.Windows.Visibility.Visible;
                DescriptionLabel.Visibility = System.Windows.Visibility.Visible;
                URLLabel.Visibility = System.Windows.Visibility.Visible;
                SaveLocationTextBox.Visibility = System.Windows.Visibility.Visible;
                SelectLocationButton.Visibility = System.Windows.Visibility.Visible;
                ContentTypeLabel.Visibility = System.Windows.Visibility.Visible;
                ContentTypeComboBox.Visibility = System.Windows.Visibility.Visible;
                MappingsListView.Height = 250;
                AddButton.Margin = new Thickness(491, 152, 0, 0);
                MappingsListView.Margin = new Thickness(12, 183, 0, 0);
            }

            this.RefreshMappingListView();
        }

        private void ApplicationRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RefreshMappingListView();
        }

        private void SiteSettingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
                this.SaveLocationTextBox.Text = this.SelectedFolder.Title;
                this.LoadContentTypes(this.SelectedFolder);
            }

        }

    }
}
