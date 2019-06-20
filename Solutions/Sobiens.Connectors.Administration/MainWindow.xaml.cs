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
using Sobiens.Connectors.WPF.Controls.Settings;
using Sobiens.Connectors.WPF.Controls;
using Sobiens.Connectors.Administration.Controls;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Services.Settings;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.WPF.Controls.Search;
using Sobiens.Connectors.Entities.Documents;

namespace Sobiens.Connectors.Administration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private List<AppConfiguration> AppConfigurations
        {
            get;
            set;
        }

        private AppConfiguration AppConfiguration
        {
            get;
            set;
        }

        private ConnectorExplorerState ExplorerState
        {
            get;
            set;
        }

        private void LoadConfigurations()
        {
            ConfigurationsManager configurationsManager = new ConfigurationsManager();
            AppConfigurations = configurationsManager.GetConfigurations();

            RefreshAppConfigurationsListBox();
        }

        private void RefreshAppConfigurationsListBox()
        {
            AppConfigurationComboBox.Items.Clear();
            foreach (AppConfiguration configuration in AppConfigurations)
            {
                AppConfigurationComboBox.Items.Add(configuration);
            }
        }

        private void RefreshGeneralConfigurationsListBox()
        {
            GeneralConfigurationListBox.Items.Clear();
            foreach (SiteSetting siteSetting in this.AppConfiguration.SiteSettings)
            {
                GeneralConfigurationListBox.Items.Add(siteSetting);
            }
        }

        private void RefreshWordTemplateListBox()
        {
            WordTemplatesListBox.Items.Clear();
            foreach (DocumentTemplate documentTemplate in AppConfiguration.DocumentTemplates.GetDocumentTemplates(ApplicationTypes.Word) )
            {
                WordTemplatesListBox.Items.Add(documentTemplate);
            }
        }

        private void RefreshExcelTemplateListBox()
        {
            ExcelTemplateListBox.Items.Clear();
            foreach (DocumentTemplate documentTemplate in AppConfiguration.DocumentTemplates.GetDocumentTemplates(ApplicationTypes.Excel))
            {
                ExcelTemplateListBox.Items.Add(documentTemplate);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting siteSetting = new SiteSetting();
            siteSetting.ID = Guid.NewGuid();
            SiteSettingForm siteSettingForm = new SiteSettingForm();
            siteSettingForm.BindControls(siteSetting);
            if (siteSettingForm.ShowDialog(this, "Site Setting") == true)
            {
                this.AppConfiguration.SiteSettings.Add(siteSetting);
                RefreshGeneralConfigurationsListBox();
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            GeneralConfigurationListBox.Items.RemoveAt(GeneralConfigurationListBox.SelectedIndex);
            SPWeb web = (SPWeb) GeneralConfigurationListBox.SelectedItem;
            ExplorerState.Folders.Remove(web);
        }

        private void AddTemplateButton_Click(object sender, RoutedEventArgs e)
        {
//            WordDocumentTemplate documentTemplate = new WordDocumentTemplate();
//            DocumentTemplateForm documentTemplateForm = new DocumentTemplateForm();
//            documentTemplateForm.BindControls(documentTemplate);
//            documentTemplateForm.Initialize(null, string.Empty, this.AppConfiguration.SiteSettings);
//            if (documentTemplateForm.ShowDialog(this, "Word Template") == true)
//            {
////                AppConfiguration.WordConfigurations.Templates.Add(documentTemplate);
//                RefreshWordTemplateListBox();
//            }
        }

        private void DeleteTemplateButton_Click(object sender, RoutedEventArgs e)
        {
//            WordDocumentTemplate documentTemplate = (WordDocumentTemplate)WordTemplatesListBox.SelectedItem;
////            AppConfiguration.WordConfigurations.Templates.Remove(documentTemplate);
//            RefreshWordTemplateListBox();
        }

        private void WordTemplatesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //WordDocumentTemplate documentTemplate = (WordDocumentTemplate)WordTemplatesListBox.SelectedItem;
            //DocumentTemplateForm documentTemplateForm = new DocumentTemplateForm();
            //documentTemplateForm.BindControls(documentTemplate);
            //documentTemplateForm.Initialize(documentTemplate.TargetFolder, documentTemplate.ContentTypeID, this.AppConfiguration.SiteSettings);
            //if (documentTemplateForm.ShowDialog(this, "Word Template") == true)
            //{
            //}
        }

        private void ExcelTemplateListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //ExcelDocumentTemplate documentTemplate = (ExcelDocumentTemplate)ExcelTemplateListBox.SelectedItem;
            //DocumentTemplateForm documentTemplateForm = new DocumentTemplateForm();
            //documentTemplateForm.BindControls(documentTemplate);
            //if (documentTemplateForm.ShowDialog(this, "Excel Template") == true)
            //{
            //}
        }

        private void AddExcelTemplateButton_Click(object sender, RoutedEventArgs e)
        {
//            ExcelEntity.ExcelDocumentTemplate documentTemplate = new ExcelEntity.ExcelDocumentTemplate();
//            DocumentTemplateForm documentTemplateForm = new DocumentTemplateForm();
//            documentTemplateForm.BindControls(documentTemplate);
//            if (documentTemplateForm.ShowDialog(this, "Excel Template") == true)
//            {
////                AppConfiguration.ExcelConfigurations.Templates.Add(documentTemplate);
//                RefreshExcelTemplateListBox();
//            }
        }

        private void DeleteExcelTemplateButton_Click(object sender, RoutedEventArgs e)
        {
//            ExcelEntity.ExcelDocumentTemplate documentTemplate = (ExcelEntity.ExcelDocumentTemplate)ExcelTemplateListBox.SelectedItem;
////            AppConfiguration.ExcelConfigurations.Templates.Remove(documentTemplate);
//            RefreshExcelTemplateListBox();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationsManager configurationsManager = new ConfigurationsManager();
            configurationsManager.SaveAppConfiguration(AppConfiguration);
            configurationsManager.SaveExplorerState(AppConfiguration, ExplorerState);
            MessageBox.Show("Configurations saved successfully.");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConfigurations();
        }

        private void ConfigurationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.AppConfiguration = AppConfigurationComboBox.SelectedItem as AppConfiguration;
            if (this.AppConfiguration == null)
                return;

            ConfigurationsManager configurationsManager = new ConfigurationsManager();
            Sobiens.Connectors.Common.ConfigurationManager.GetInstance().AdministrativeConfiguration = new Entities.Settings.AppConfiguration();
            Sobiens.Connectors.Common.ConfigurationManager.GetInstance().Configuration = this.AppConfiguration;
            ExplorerState = configurationsManager.GetExplorerState(this.AppConfiguration);

            RefreshGeneralConfigurationsListBox();
            RefreshExcelTemplateListBox();
            RefreshWordTemplateListBox();
        }

        private void AddConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            AppConfiguration configuration = new AppConfiguration();
            ConfigurationEditForm configurationEditForm = new ConfigurationEditForm();
            configurationEditForm.BindControls(configuration);
            if (configurationEditForm.ShowDialog(this, "Add Configuration") == true)
            {
                AppConfigurations.Add(configuration);
                ConfigurationsManager configurationsManager = new ConfigurationsManager();
                ConnectorExplorerState connectorExplorerState = new ConnectorExplorerState();
                configurationsManager.SaveExplorerState(configuration, connectorExplorerState);
                RefreshAppConfigurationsListBox();
            }
        }

        private void GeneralConfigurationListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SiteSetting siteSetting = (SiteSetting)(GeneralConfigurationListBox.SelectedItem as SiteSetting).Clone();
            
            SiteSettingForm siteSettingForm = new SiteSettingForm();
            siteSettingForm.BindControls(siteSetting);
            if (siteSettingForm.ShowDialog(this, "Site Setting") == true)
            {
                this.AppConfiguration.SiteSettings[siteSetting.ID] = siteSetting;
                RefreshGeneralConfigurationsListBox();
            }
        }

        private void DeleteConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            AppConfiguration configuration = AppConfigurationComboBox.SelectedItem as AppConfiguration;
            ConfigurationsManager configurationsManager = new ConfigurationsManager();
            configurationsManager.DeleteConfiguration(configuration);
            LoadConfigurations();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ConnectorExplorer ce = new ConnectorExplorer();
            //UserControl1 ce = new UserControl1();
            ce.ShowDialog(this, "Add Configuration");
        }

        private void ExplorerEditButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting siteSetting = (SiteSetting)GeneralConfigurationListBox.SelectedItem;
            SPWeb web = new SPWeb(siteSetting.Url, siteSetting.Url, siteSetting.ID, String.Empty, siteSetting.Url, siteSetting.Url);
            web.PublicFolder = true;
            SiteContentSelections siteContentSelections = new SiteContentSelections();
            siteContentSelections.InitializeForm(siteSetting, web, false, null);
            if (siteContentSelections.ShowDialog(this, "Site Content Selections") == true)
            {
                ExplorerState.Folders.Add(web);
                RefreshGeneralConfigurationsListBox();
            }
        }
    }
}
