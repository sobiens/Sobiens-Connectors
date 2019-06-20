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
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.WPF.Controls.Settings
{
    /// <summary>
    /// Interaction logic for SiteSettingForm.xaml
    /// </summary>
    public partial class SiteSettingForm : HostControl
    {
        public SiteSettingForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.InitializeForm();
        }

        private void InitializeForm()
        {
            this.OKButtonSelected +=new EventHandler(SiteSettingForm_OKButtonSelected);
            //TypeComboBox.FillItemsByEnum(typeof(SiteSettingTypes));

            List<object> list = new List<object>();
            list.Add(SiteSettingTypes.SharePoint);
            TypeComboBox.ItemsSource = list;

            SetCredentialControls();
        }

        private void SetCredentialControls()
        {
            if (DefaultCredentialCheckBox.IsChecked == true)
            {
                UserTextBox.IsEnabled = false;
                PasswordTextBox.IsEnabled = false;
            }
            else
            {
                UserTextBox.IsEnabled = true;
                PasswordTextBox.IsEnabled = true;
            }
        }

        private void DefaultCredentialRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetCredentialControls();
        }

        private void CustomCredentialRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetCredentialControls();
        }

        private void SiteSettingForm_OKButtonSelected(object sender, EventArgs e)
        {
            SiteSetting siteSetting = (SiteSetting)this.Tag;
            LoadingWindow loadingWindow = new LoadingWindow();
            loadingWindow.Show(Languages.Translate("Checking connection..."));
            if (ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).CheckConnection(siteSetting) == false)
            {
                this.IsValid = false;
                loadingWindow.Close();
                MessageBox.Show(Languages.Translate("Checking connection failed. Please correct the entries."));
                return;
            }
            loadingWindow.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            ((Window)this.Parent).DialogResult = false;
        }

        private void DefaultCredentialCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetCredentialControls();
        }

        private void DefaultCredentialCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SetCredentialControls();
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SiteSettingTypes siteSettingType = (SiteSettingTypes)TypeComboBox.SelectedValue;
            if (siteSettingType == SiteSettingTypes.SharePoint)
            {
                ClaimAuthenticationCheckBox.Visibility = System.Windows.Visibility.Visible;
                AdvancedSharePointSetting.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ClaimAuthenticationCheckBox.Visibility = System.Windows.Visibility.Hidden;
                AdvancedSharePointSetting.Visibility = System.Windows.Visibility.Hidden;
            }

            if (siteSettingType != SiteSettingTypes.GMail)
            {
                URLTextBox.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                URLTextBox.Visibility = System.Windows.Visibility.Hidden;
            }
            
        }
    }
}
