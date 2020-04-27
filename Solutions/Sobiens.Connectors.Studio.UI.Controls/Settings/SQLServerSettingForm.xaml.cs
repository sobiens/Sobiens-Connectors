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
using Sobiens.Connectors.Entities.Cryptography;

namespace Sobiens.Connectors.Studio.UI.Controls.Settings
{
    /// <summary>
    /// Interaction logic for SiteSettingForm.xaml
    /// </summary>
    public partial class SQLServerSettingForm : HostControl
    {
        public SQLServerSettingForm()
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

            SetCredentialControls();
            URLTextBox.Focus();
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
            SiteSetting siteSetting = (SiteSetting)((SiteSetting)this.Tag).Clone();
            if (string.IsNullOrEmpty(siteSetting.Password) == false)
                siteSetting.Password = AesOperation.EncryptString(siteSetting.Password);
            LoadingWindow.ShowDialog(Languages.Translate("Checking connection..."), delegate ()
            {
                if (ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).CheckConnection(siteSetting) == false)
                {
                    this.IsValid = false;
                    MessageBox.Show(Languages.Translate("Checking connection failed. Please correct the entries."));
                    return;
                }
                this.IsValid = true;
            });
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

    }
}
