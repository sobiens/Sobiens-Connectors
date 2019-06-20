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
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.WPF.Controls.Settings;

namespace Sobiens.Connectors.WPF.Controls.EditItems
{
    /// <summary>
    /// Interaction logic for FileExistConfirmationForm.xaml
    /// </summary>
    public partial class ConfigurationImportForm : HostControl
    {
        public bool ShowFileExistsErrorMessage = false;
        public string NewFileName = string.Empty;
        private SiteSetting SelectedSiteSetting = null;

        public ConfigurationImportForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(FileExistConfirmationForm_Loaded);
        }

        protected void FileExistConfirmationForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(FileExistConfirmationForm_Loaded);

            ExternalAdministrationConfiguration config = ConfigurationManager.GetInstance().ExternalAdministrationConfiguration;
            if (config != null)
            {
                ConfigurationURLTextBox.Text = config.Url;
                this.SelectedSiteSetting = config.SiteSetting;
                SiteSettingTextBox.Text = config.SiteSetting.ToString();
            }

            this.OKButtonSelected += new EventHandler(FileCopyNameForm_OKButtonSelected);
        }

        protected void FileCopyNameForm_OKButtonSelected(object sender, EventArgs e)
        {
            ////http://sobiens31.sharepoint.com/SiteAssets/co.aspx?ConfigId=2edc4953-0bb4-4bd4-8a20-77cc8c3d8f09
            ////ss.Password = "Trinity100";
            ////ss.Url = "http://sobiens31.sharepoint.com";
            ////ss.Username = "serkants@sobiens31.onmicrosoft.com";

            string url = ConfigurationURLTextBox.Text;
            string administrativeConfigurationFilePath = ConfigurationManager.GetInstance().GetAdministrativeConfigurationFilePath();
            if (ServiceManagerFactory.GetServiceManager(SelectedSiteSetting.SiteSettingType).DownloadAdministrativeConfiguration(SelectedSiteSetting, url, administrativeConfigurationFilePath) == true)
            {
                ExternalAdministrationConfiguration config = new ExternalAdministrationConfiguration();
                config.SiteSetting = SelectedSiteSetting;
                config.Url = url;
                ConfigurationManager.GetInstance().ExternalAdministrationConfiguration = config;
                ConfigurationManager.GetInstance().SaveExternalAdministrationConfiguration();
            }
            else
            {
                this.SetErrorMessage("An error occured while importing configuration");
                this.IsValid = false;
            }
        }

        private void SiteSettingButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting siteSetting = new SiteSetting();
            if (SelectedSiteSetting != null)
            {
                siteSetting = (SiteSetting)SelectedSiteSetting.Clone();
            }
            else
            {
                siteSetting.ID = Guid.NewGuid();
            }
            SiteSettingForm siteSettingForm = new SiteSettingForm();
            siteSettingForm.BindControls(siteSetting);
            if (siteSettingForm.ShowDialog(this.ParentWindow,Languages.Translate("Site Settings")) == true)
            {
                SelectedSiteSetting = siteSetting;
                SiteSettingTextBox.Text = siteSetting.ToString();
            }
        }

    }
}
