using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector.Forms
{
    public partial class SiteSettingForm : Form
    {
        private EUSiteSetting SiteSetting { get; set; }
        public SiteSettingForm()
        {
            InitializeComponent();
        }

        public void InitializeForm(EUSiteSetting siteSetting)
        {
            SiteSetting = siteSetting;
            if (siteSetting.SiteSettingType == EUSiteSettingTypes.FileSystem)
                FileSystemRadioButton.Checked = true;
            else if (siteSetting.SiteSettingType == EUSiteSettingTypes.GMail)
                GMailRadioButton.Checked = true;
            else
                SharePointRadioButton.Checked = true;

            UrlTextBox.Text = siteSetting.Url;
            UserTextBox.Text = siteSetting.User;
            PasswordTextBox.Text = siteSetting.Password;
            if (siteSetting.UseDefaultCredential == true)
            {
                DefaultCredentialRadioButton.Checked = true;
            }
            else
            {
                CustomCredentialRadioButton.Checked = true;
            }

            SetCredentialControls();
        }

        private void SetCredentialControls()
        {
            if (DefaultCredentialRadioButton.Checked == true)
            {
                UserTextBox.Enabled = false;
                PasswordTextBox.Enabled = false;
            }
            else
            {
                UserTextBox.Enabled = true;
                PasswordTextBox.Enabled = true;
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

        private void OkButton_Click(object sender, EventArgs e)
        {
            SiteSetting.Url = UrlTextBox.Text;
            if (SiteSetting.Url.EndsWith("/") == true)
            {
                SiteSetting.Url = SiteSetting.Url.TrimEnd('/');
            }
            if (SharePointRadioButton.Checked == true)
                SiteSetting.SiteSettingType = EUSiteSettingTypes.SharePoint;
            else if(FileSystemRadioButton.Checked == true)
                SiteSetting.SiteSettingType = EUSiteSettingTypes.FileSystem;
            else
                SiteSetting.SiteSettingType = EUSiteSettingTypes.GMail;
            SiteSetting.User = UserTextBox.Text;
            SiteSetting.Password = PasswordTextBox.Text;
            SiteSetting.UseDefaultCredential = DefaultCredentialRadioButton.Checked;
            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void GMailRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (GMailRadioButton.Checked == true)
            {
                UsernameLabel.Text = "User Name:";
                UrlTextBox.Enabled = false;
            }
            else
            {
                UsernameLabel.Text = "Domain\\User:";
                UrlTextBox.Enabled = true;
            }
        }
    }
}
