using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Sobiens.Connectors.WPF.Controls.Settings;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common;
using System.Diagnostics;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.Documents;
using Sobiens.Connectors.WPF.Controls.EditItems;

namespace Sobiens.Connectors.WPF.Controls.Settings
{
    public partial class SettingsForm : HostControl
    {
        private AppConfiguration Configuration { get; set; }

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void LoadListSettings()
        {
            /*
            EmailMappingSettingsComboBox.Items.Clear();
            EmailMappingSettingsComboBox.Items.Add(Settings.DefaultListSetting);
            foreach (ListSetting listSetting in Settings.ListSettings)
            {
                EmailMappingSettingsComboBox.Items.Add(listSetting);
            }
            EmailMappingSettingsComboBox.SelectedIndex = 0;
             */ 
        }


        private void LoadSites()
        {
            SitesListBox.Items.Clear();
            foreach (SiteSetting siteSetting in Configuration.SiteSettings)
            {
                SitesListBox.Items.Add(siteSetting);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting siteSetting = new SiteSetting();
            siteSetting.ID = Guid.NewGuid();
            SiteSettingForm siteSettingForm = new SiteSettingForm();
            siteSettingForm.BindControls(siteSetting);
            if (siteSettingForm.ShowDialog(this.ParentWindow, Languages.Translate("Site Settings")) == true)
            {
                Configuration.SiteSettings.Add(siteSetting);
                ExplorerLocation explorerLocation = new ExplorerLocation();
                explorerLocation.ID = Guid.NewGuid();
                explorerLocation.ApplicationTypes.Add(ApplicationTypes.Excel);
                explorerLocation.ApplicationTypes.Add(ApplicationTypes.Outlook);
                explorerLocation.ApplicationTypes.Add(ApplicationTypes.Word);
                explorerLocation.ApplicationTypes.Add(ApplicationTypes.General);
                explorerLocation.ShowAll = true;
                explorerLocation.SiteSettingID = siteSetting.ID;
                explorerLocation.Folder = ApplicationContext.Current.GetRootFolder(siteSetting);
                explorerLocation.Folder.Selected = true;
                explorerLocation.BasicFolderDefinition = explorerLocation.Folder.GetBasicFolderDefinition();
                Configuration.ExplorerConfiguration.ExplorerLocations.Add(explorerLocation);
                LoadSites();
                RefreshExplorerLocationsListBox();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SitesListBox.SelectedItem == null)
                return;
            ISiteSetting siteSetting = (SiteSetting)SitesListBox.SelectedItem;
            Configuration.SiteSettings.Remove(siteSetting.ID);
            LoadSites();
        }

        private void ShowLogsButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("IExplore.exe", ConfigurationManager.GetInstance().GetLogFolder());
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString(),"Settings");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Configuration == null)
                Configuration = ConfigurationManager.GetInstance().Configuration;
            if (Configuration == null)
            {
                Configuration = new AppConfiguration();
                return;
            }
            if (Configuration.OutlookConfigurations.SaveAsWord == true)
            {
                WordPlusAttachmentsRadioButton.IsChecked = true;
            }
            else
            {
                EmailRadioButton.IsChecked = true;
            }
            UploadAutomaticallyCheckBox.IsChecked = Configuration.OutlookConfigurations.UploadAutomatically;
            DetailedLogCheckBox.IsChecked = Configuration.DetailedLogMode;
            if (Configuration.OutlookConfigurations.DefaultAttachmentSaveFolder != null && string.IsNullOrEmpty(Configuration.OutlookConfigurations.DefaultAttachmentSaveFolder.FolderUrl) != true)
            {
                string folderName = Configuration.OutlookConfigurations.DefaultAttachmentSaveFolder.FolderUrl;
                folderName = folderName.Substring(folderName.IndexOf("/")+1);
                this.SaveLocationTextBox.Text = folderName;
            }

            DontAskSaveAttachmentLocationCheckBox.IsChecked = Configuration.OutlookConfigurations.DontAskSaveAttachmentLocation;
            LoadListSettings();
            LoadSites();
            RefreshTemplateMappingsListBox();
            RefreshTemplateListBox();
            RefreshExplorerLocationsListBox();
            RefreshTaskListLocationsListBox();
        }

        private void SitesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SiteSetting siteSetting = (SiteSetting)SitesListBox.SelectedItem;
            SiteSettingForm siteSettingForm = new SiteSettingForm();
            siteSettingForm.BindControls(siteSetting);
            if (siteSettingForm.ShowDialog(this.ParentWindow, "Site Setting") == true)
            {
                LoadSites();
            }

        }

        private void RefreshTaskListLocationsListBox()
        {
            TaskListLocationListBox.Items.Clear();
            foreach (TaskListLocation taskListLocation in Configuration.WorkflowConfiguration.TaskListLocations)
            {
                TaskListLocationListBox.Items.Add(taskListLocation);
            }
        }

        private void RefreshExplorerLocationsListBox()
        {
            ExplorerLocationListBox.Items.Clear();
            foreach (ExplorerLocation explorerLocation in Configuration.ExplorerConfiguration.ExplorerLocations)
            {
                ExplorerLocationListBox.Items.Add(explorerLocation);
            }
        }

        private void RefreshTemplateListBox()
        {
            TemplatesListBox.Items.Clear();
            foreach (DocumentTemplate documentTemplate in Configuration.DocumentTemplates)
            {
                TemplatesListBox.Items.Add(documentTemplate);
            }
        }

        private void RefreshTemplateMappingsListBox()
        {
            TemplateMappingsListBox.Items.Clear();
            foreach (DocumentTemplateMapping documentTemplateMapping in Configuration.DocumentTemplateMappings)
            {
                TemplateMappingsListBox.Items.Add(documentTemplateMapping);
            }
        }

        private void TemplatesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DocumentTemplate documentTemplate = (DocumentTemplate)TemplatesListBox.SelectedItem;
            DocumentTemplateForm documentTemplateForm = new DocumentTemplateForm();
            documentTemplateForm.BindControls(documentTemplate);

            if (documentTemplateForm.ShowDialog(this.ParentWindow, "Edit Template") == true)
            {
                RefreshTemplateListBox();
            }

        }

        private void AddTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentTemplate documentTemplate = new DocumentTemplate();
            documentTemplate.ID = Guid.NewGuid();
            documentTemplate.ApplicationType = ApplicationTypes.Word;
            DocumentTemplateForm documentTemplateForm = new DocumentTemplateForm();
            documentTemplateForm.BindControls(documentTemplate);
            if (documentTemplateForm.ShowDialog(this.ParentWindow, "New Template") == true)
            {
                Configuration.DocumentTemplates.Add(documentTemplate);
                RefreshTemplateListBox();
            }
        }

        private void DeleteTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentTemplate documentTemplate = (DocumentTemplate)TemplatesListBox.SelectedItem;
            Configuration.DocumentTemplates.Remove(documentTemplate);
            RefreshTemplateListBox();
        }


        private void SelectLocationButton_Click(object sender, RoutedEventArgs e)
        {
            List<Folder> folders = new List<Folder>();
            foreach (SiteSetting siteSetting in Configuration.SiteSettings)
            {
                SPWeb web = new SPWeb(siteSetting.Url, siteSetting.Url, siteSetting.ID, siteSetting.Url, siteSetting.Url, siteSetting.Url);

                web.PublicFolder = true;
                folders.Add(web);
            }
            SiteContentSelections siteContentSelections = new SiteContentSelections();
            siteContentSelections.InitializeForm(Configuration.SiteSettings, folders, true, null);
            if (siteContentSelections.ShowDialog(this.ParentWindow, Languages.Translate("Save Location Selections")) == true)
            {
                Configuration.OutlookConfigurations.DefaultAttachmentSaveFolder = siteContentSelections.SelectedFolder.GetBasicFolderDefinition();
                this.SaveLocationTextBox.Text = siteContentSelections.SelectedFolder.Title;
            }
        }

        private void DontAskSaveAttachmentLocationCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Configuration.OutlookConfigurations.DontAskSaveAttachmentLocation = DontAskSaveAttachmentLocationCheckBox.IsChecked.Value;
        }

        private void WordPlusAttachmentsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Configuration.OutlookConfigurations.SaveAsWord = true;
        }

        private void EmailRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Configuration.OutlookConfigurations.SaveAsWord = false;
        }

        private void UploadAutomaticallyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Configuration.OutlookConfigurations.UploadAutomatically = UploadAutomaticallyCheckBox.IsChecked.Value;
        }

        private void UploadAutomaticallyCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Configuration.OutlookConfigurations.UploadAutomatically = UploadAutomaticallyCheckBox.IsChecked.Value;
        }

        private void DontAskSaveAttachmentLocationCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Configuration.OutlookConfigurations.DontAskSaveAttachmentLocation = DontAskSaveAttachmentLocationCheckBox.IsChecked.Value;
        }

        private void DetailedLogCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Configuration.DetailedLogMode = DetailedLogCheckBox.IsChecked.Value;
        }

        private void DetailedLogCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Configuration.DetailedLogMode = DetailedLogCheckBox.IsChecked.Value;
        }

        private void NewTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentTemplate documentTemplate = new DocumentTemplate();
            documentTemplate.ID = Guid.NewGuid();
            documentTemplate.ApplicationType = ApplicationTypes.Word;
            DocumentTemplateForm documentTemplateForm = new DocumentTemplateForm();
            documentTemplateForm.BindControls(documentTemplate);
            if (documentTemplateForm.ShowDialog(this.ParentWindow, "New Template") == true)
            {
                Configuration.DocumentTemplates.Add(documentTemplate);
                RefreshTemplateListBox();
            }
        }

        private void AddExplorerLocationButton_Click(object sender, RoutedEventArgs e)
        {
            ExplorerLocation explorerLocation = new ExplorerLocation();
            explorerLocation.ID = Guid.NewGuid();

            ExplorerLocationForm explorerLocationForm = new ExplorerLocationForm();

            explorerLocationForm.Initialize(Configuration.SiteSettings);
            explorerLocationForm.BindControls(explorerLocation);
            if (explorerLocationForm.ShowDialog(null, "New Explorer Location") == true)
            {
                Configuration.ExplorerConfiguration.ExplorerLocations.Add(explorerLocation);
                RefreshExplorerLocationsListBox();
            }
        }

        private void DeleteExplorerLocationButton_Click(object sender, RoutedEventArgs e)
        {
            ExplorerLocation explorerLocation = (ExplorerLocation)ExplorerLocationListBox.SelectedItem;
            Configuration.ExplorerConfiguration.ExplorerLocations.Remove(explorerLocation);
            RefreshExplorerLocationsListBox();
        }

        private void ExplorerLocationListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ExplorerLocationListBox.SelectedItem == null)
                return;

            ExplorerLocation explorerLocation = (ExplorerLocation)ExplorerLocationListBox.SelectedItem;
            ExplorerLocationForm explorerLocationForm = new ExplorerLocationForm();

            explorerLocationForm.Initialize(Configuration.SiteSettings);
            explorerLocationForm.BindControls(explorerLocation);
            if (explorerLocationForm.ShowDialog(null, Languages.Translate("Edit Explorer Location")) == true)
            {
                RefreshExplorerLocationsListBox();
            }
        }

        private void TaskListLocationListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TaskListLocationListBox.SelectedItem == null)
                return;
            TaskListLocation taskListLocation = (TaskListLocation)TaskListLocationListBox.SelectedItem;
            TaskListLocationForm taskListLocationForm = new TaskListLocationForm();

            taskListLocationForm.Initialize(Configuration.SiteSettings);
            taskListLocationForm.BindControls(taskListLocation);
            if (taskListLocationForm.ShowDialog(null, "Edit Task List Location") == true)
            {
                RefreshTaskListLocationsListBox();
            }
        }

        private void AddTaskListLocationButton_Click(object sender, RoutedEventArgs e)
        {
            TaskListLocation taskListLocation = new TaskListLocation();
            taskListLocation.ID = Guid.NewGuid();
            taskListLocation.ApplicationTypes.Add(ApplicationTypes.Excel);
            taskListLocation.ApplicationTypes.Add(ApplicationTypes.General);
            taskListLocation.ApplicationTypes.Add(ApplicationTypes.Outlook);
            taskListLocation.ApplicationTypes.Add(ApplicationTypes.Word);

            TaskListLocationForm taskListLocationForm = new TaskListLocationForm();

            taskListLocationForm.Initialize(Configuration.SiteSettings);
            taskListLocationForm.BindControls(taskListLocation);
            if (taskListLocationForm.ShowDialog(null, "New Task List Location") == true)
            {
                Configuration.WorkflowConfiguration.TaskListLocations.Add(taskListLocation);
                RefreshTaskListLocationsListBox();
            }
        }

        private void DeleteTaskListLocationButton_Click(object sender, RoutedEventArgs e)
        {
            TaskListLocation taskListLocation = (TaskListLocation)TaskListLocationListBox.SelectedItem;
            Configuration.WorkflowConfiguration.TaskListLocations.Remove(taskListLocation);
            RefreshTaskListLocationsListBox();
        }

        private void NewTemplateMappingButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentTemplateMapping documentTemplateMapping = new DocumentTemplateMapping();
            DocumentTemplateMappingForm documentTemplateMappingForm = new DocumentTemplateMappingForm();
            documentTemplateMappingForm.Initialize(Configuration.DocumentTemplates.GetDocumentTemplates(ApplicationTypes.Word), Configuration.SiteSettings);
            documentTemplateMappingForm.BindControls(documentTemplateMapping);
            if (documentTemplateMappingForm.ShowDialog(null, Languages.Translate("New Word Template Mapping")) == true)
            {
                Configuration.DocumentTemplateMappings.Add(documentTemplateMapping);
                RefreshTemplateMappingsListBox();
            }
        }

        private void DeleteTemplateMappingButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentTemplateMapping documentTemplateMapping = (DocumentTemplateMapping)TemplateMappingsListBox.SelectedItem;
            Configuration.DocumentTemplateMappings.Remove(documentTemplateMapping);
            RefreshTemplateMappingsListBox();
        }

        private void TemplateMappingsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DocumentTemplateMapping documentTemplateMapping = TemplateMappingsListBox.SelectedValue as DocumentTemplateMapping;
            if (documentTemplateMapping == null)
            {
                MessageBox.Show("Please select a template");
                return;
            }

            DocumentTemplateMappingForm documentTemplateMappingForm = new DocumentTemplateMappingForm();
            documentTemplateMappingForm.Initialize(Configuration.DocumentTemplates, Configuration.SiteSettings);
            documentTemplateMappingForm.BindControls(documentTemplateMapping);
            if (documentTemplateMappingForm.ShowDialog(null, "Edit Template Mapping") == true)
            {
                RefreshTemplateMappingsListBox();
            }
        }


        private void ConfigurationImportButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationImportForm x = new ConfigurationImportForm();
            x.ShowDialog(this.ParentWindow, "Configuration Import");
        }
    }
}
