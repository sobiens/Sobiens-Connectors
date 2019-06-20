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
    public partial class TaskListLocationForm : HostControl
    {
        private Guid SelectedSiteSettingID { get; set; }
        private Folder SelectedFolder { get; set; }
        private string SelectedFolderPath { get; set; }
        private string SelectedFolderType { get; set; }
        private SiteSettings SiteSettings { get; set; }

        public TaskListLocationForm()
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

            TaskListLocation workflowLocation = this.Tag as TaskListLocation;
            this.SelectedSiteSettingID = workflowLocation.SiteSettingID;

            if (workflowLocation.ApplicationTypes.Contains(ApplicationTypes.General) == true)
            {
                GeneralATCheckBox.IsChecked = true;
            }

            if (workflowLocation.ApplicationTypes.Contains(ApplicationTypes.Excel) == true)
            {
                ExcelATCheckBox.IsChecked = true;
            }

            if (workflowLocation.ApplicationTypes.Contains(ApplicationTypes.Outlook) == true)
            {
                OutlookATCheckBox.IsChecked = true;
            }

            if (workflowLocation.ApplicationTypes.Contains(ApplicationTypes.Word) == true)
            {
                WordATCheckBox.IsChecked = true;
            }

            if (SelectedSiteSettingID != null && SelectedSiteSettingID != Guid.Empty)
            {
                SiteSettingComboBox.SelectedValue = SelectedSiteSettingID;
            }
            else
            {
                if (SiteSettingComboBox.Items.Count > 0)
                    SiteSettingComboBox.SelectedIndex = 0;
            }

            if (workflowLocation.Folder != null)
            {
                SelectedFolderLabel.Content = workflowLocation.Folder.GetUrl();
            }

            TaskListNameTextBox.Text = workflowLocation.Name;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            this.ParentWindow.OKButtonSelected += new EventHandler(ParentWindow_OKButtonSelected);
        }

        protected void ParentWindow_OKButtonSelected(object sender, EventArgs e)
        {
            TaskListLocation workflowLocation = (TaskListLocation)this.Tag;
            workflowLocation.SiteSettingID = (Guid)SiteSettingComboBox.SelectedValue;
            workflowLocation.Name = TaskListNameTextBox.Text;

            workflowLocation.ApplicationTypes = new List<ApplicationTypes>();
            if (GeneralATCheckBox.IsChecked == true)
            {
                workflowLocation.ApplicationTypes.Add(ApplicationTypes.General);
            }
            if (WordATCheckBox.IsChecked == true)
            {
                workflowLocation.ApplicationTypes.Add(ApplicationTypes.Word);
            }
            if (ExcelATCheckBox.IsChecked == true)
            {
                workflowLocation.ApplicationTypes.Add(ApplicationTypes.Excel);
            }
            if (OutlookATCheckBox.IsChecked == true)
            {
                workflowLocation.ApplicationTypes.Add(ApplicationTypes.Outlook);
            }

            if (workflowLocation.ShowAll == true)
            {
                SiteSetting siteSetting = (SiteSetting)SiteSettingComboBox.SelectedItem;
                workflowLocation.Folder = new SPWeb(siteSetting.Url, siteSetting.Url, siteSetting.ID, String.Empty, siteSetting.Url, siteSetting.Url);
                workflowLocation.BasicFolderDefinition = workflowLocation.Folder.GetBasicFolderDefinition();
                workflowLocation.Folder.Selected = true;
            }
        }

        private void SelectLocationButton_Click(object sender, RoutedEventArgs e)
        {
            TaskListLocation workflowLocation = (TaskListLocation)this.Tag;

            SiteSetting siteSetting = (SiteSetting)SiteSettingComboBox.SelectedItem;
            Folder rootFolder = null;


            SiteContentSelections siteContentSelections = new SiteContentSelections();
                rootFolder = new SPWeb(siteSetting.Url, siteSetting.Url, siteSetting.ID, String.Empty, siteSetting.Url, siteSetting.Url);
                rootFolder.PublicFolder = true;
                siteContentSelections.InitializeForm(siteSetting, rootFolder, true, new int[]{107});

                if (siteContentSelections.ShowDialog(this.ParentWindow, "Site Content Selections") == true)
                {
                    workflowLocation.Folder = siteContentSelections.SelectedFolder;
                    workflowLocation.BasicFolderDefinition = workflowLocation.Folder.GetBasicFolderDefinition();
                    SelectedFolderLabel.Content = workflowLocation.Folder.GetUrl();
                }
            //    if (workflowLocation.Folder == null)
            //    {
            //    }
            //else
            //{
            //    rootFolder = workflowLocation.Folder;
            //    siteContentSelections.InitializeForm(siteSetting, rootFolder, true, new int[] { 107 });
            //    if (siteContentSelections.ShowDialog(this.ParentWindow, "Site Content Selections") == true)
            //    {
            //        workflowLocation.Folder = siteContentSelections.SelectedFolder;
            //        workflowLocation.BasicFolderDefinition = workflowLocation.Folder.GetBasicFolderDefinition();
            //        SelectedFolderLabel.Content = workflowLocation.Folder.GetUrl();
            //    }
            //}

        }

        private void LoadSiteSettings()
        {
            SiteSettingComboBox.ItemsSource = this.SiteSettings;
        }
    }
}
