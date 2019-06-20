using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL;
using Sobiens.Office.SharePointOutlookConnector.Forms;
using System.Diagnostics;

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class SettingsForm : Form
    {
        private EUSettings Settings { get; set; }
        private EUListSetting SelectedListSetting = null;
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Settings.UploadAutomatically = UploadAutomaticallyCheckBox.Checked;
            Settings.SaveAsWord = WordPlusAttachmentsRadioButton.Checked;
            Settings.DetailedLogMode = DetailedLogCheckBox.Checked;
            EUSettingsManager.GetInstance().Settings = Settings;
            EUSettingsManager.GetInstance().SaveSettings();
            DialogResult = DialogResult.OK;
        }

        public void SetSelectedListSetting(string webURL, string rootFolderPath, string listName)
        {
            Settings = EUSettingsManager.GetInstance().Settings;
            if (Settings == null)
                Settings = new EUSettings();
            EUListSetting listSetting = null;
            foreach (EUListSetting tempListSetting in Settings.ListSettings)
            {
                if (tempListSetting.RootFolderPath == rootFolderPath)
                {
                    listSetting = tempListSetting;
                }
            }
            if (listSetting == null)
            {
                listSetting = new EUListSetting();
                listSetting.RootFolderPath = rootFolderPath;
                listSetting.WebURL = webURL;
                listSetting.ListName = listName;
                Settings.ListSettings.Add(listSetting);
            }
            EmailMappingControl.BindEmailMapping(listSetting);
            SelectedListSetting = listSetting;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            if (Settings == null)
                Settings = EUSettingsManager.GetInstance().Settings;
            if (Settings == null)
            {
                Settings = new EUSettings();
                return;
            }
            if (Settings.SaveAsWord == true)
            {
                WordPlusAttachmentsRadioButton.Checked = true;
            }
            else
            {
                EmailRadioButton.Checked = true;
            }
            UploadAutomaticallyCheckBox.Checked = Settings.UploadAutomatically;
            DetailedLogCheckBox.Checked = Settings.DetailedLogMode;
            LoadListSettings();
            LoadSites();
            if (SelectedListSetting == null)
            {
                EmailMappingControl.BindEmailMapping(Settings.DefaultListSetting);
            }
            else
            {
                EmailMappingControl.BindEmailMapping(SelectedListSetting);
                EmailMappingSettingsComboBox.SelectedItem = SelectedListSetting;
                SettingsTabControl.SelectedTab = ListSettingTabPage;
            }
        }

        private void LoadListSettings()
        {
            EmailMappingSettingsComboBox.Items.Clear();
            EmailMappingSettingsComboBox.Items.Add(Settings.DefaultListSetting);
            foreach (EUListSetting listSetting in Settings.ListSettings)
            {
                EmailMappingSettingsComboBox.Items.Add(listSetting);
            }
            EmailMappingSettingsComboBox.SelectedIndex = 0;
        }


        private void LoadSites()
        {
            SitesListBox.Items.Clear();
            foreach (EUSiteSetting siteSetting in Settings.SiteSettings)
            {
                SitesListBox.Items.Add(siteSetting);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            EUSiteSetting siteSetting = new EUSiteSetting();
            SiteSettingForm siteSettingForm = new SiteSettingForm();
            siteSettingForm.InitializeForm(siteSetting);
            if (siteSettingForm.ShowDialog() == DialogResult.OK)
            {
                Settings.SiteSettings.Add(siteSetting);
                LoadSites();
            }
        }

        private void SitesListBox_DoubleClick(object sender, EventArgs e)
        {
            if (SitesListBox.SelectedItem == null)
                return;
            EUSiteSetting siteSetting = (EUSiteSetting)SitesListBox.SelectedItem;
            SiteSettingForm siteSettingForm = new SiteSettingForm();
            siteSettingForm.InitializeForm(siteSetting);
            if (siteSettingForm.ShowDialog() == DialogResult.OK)
            {
                LoadSites();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (SitesListBox.SelectedItem == null)
                return;
            EUSiteSetting siteSetting = (EUSiteSetting)SitesListBox.SelectedItem;
            for (int i = Settings.SiteSettings.Count-1; i >-1 ;i-- )
            {
                if (Settings.SiteSettings[i] == siteSetting)
                    Settings.SiteSettings.RemoveAt(i);
            }
            LoadSites();
        }

        private void EmailMappingSettingsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EmailMappingSettingsComboBox.SelectedItem != null)
            {
                EUListSetting listSetting = (EUListSetting)EmailMappingSettingsComboBox.SelectedItem;
                EmailMappingControl.BindEmailMapping(listSetting);
            }
        }

        private void DeleteEmailMappingSettingButton_Click(object sender, EventArgs e)
        {
            EUListSetting listSetting = EmailMappingSettingsComboBox.SelectedItem as EUListSetting;
            if(listSetting == null)
                return;
            if (listSetting.WebURL == null || listSetting.WebURL == String.Empty)
            {
                MessageBox.Show("You can not delete default settings");
                return;
            }
            for (int i = Settings.ListSettings.Count - 1; i > -1; i--)
            {
                if (Settings.ListSettings[i].WebURL == listSetting.WebURL)
                    Settings.ListSettings.RemoveAt(i);
            }
            LoadListSettings();
            EmailMappingSettingsComboBox.SelectedIndex = 0;

        }

        private void ShowLogsButton_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", EUSettingsManager.GetInstance().GetLogFolder());
        }
    }
}
