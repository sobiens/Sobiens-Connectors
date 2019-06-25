using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.CRM;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for CodeWizardForm.xaml
    /// </summary>
    public partial class MMSCopyWizardForm : HostControl
    {
        private int CurrentTabIndex = 0;
        private SPWeb SourceWeb = null;
        private SPWeb DestinationWeb = null;
        private ISiteSetting DestinationSiteSetting = null;

        public MMSCopyWizardForm()
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
            this.OKButtonSelected += SyncDataWizardForm_OKButtonSelected;
        }

        private void SyncDataWizardForm_OKButtonSelected(object sender, EventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;

        }

        private void SourceSelectButton_Click(object sender, RoutedEventArgs e)
        {
            //SyncTask s = new SyncTask();
            //s.SourceQueryResultMapping.Mappings[0].QueryResult.
            SelectEntityForm selectEntityForm = new SelectEntityForm();
            selectEntityForm.Initialize(new Type[] { typeof(SPWeb) });
            //HostControl hc = ((HostControl)this.Parent);

            if (selectEntityForm.ShowDialog(null, "Select an object to sync from") == true)
            {
                SourceWeb = (SPWeb)selectEntityForm.SelectedObject;
                ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(SourceWeb.SiteSettingID);
                
                SourceSelectButton.Content = SourceWeb.GetPath();
                SourceTermObjectSelectorControl.Initialize(siteSetting);
            }
        }
        public void Initialize()
        {
        }

        private void MigrateButton_Click(object sender, RoutedEventArgs e)
        {
            //SyncBackgroundWorker.RunWorkerAsync(new object[] { syncTask, shouldImportListItems, shouldExportListItems, shouldImportDocuments, shouldExportDocuments, shouldSkipUpdates, includeVersionsLimit, excludeFields });

            /*
            SyncTask syncTask = (SyncTask)SyncTasksComboBox.SelectedItem;
            bool shouldImportListItems = ImportListItemsCheckBox.IsChecked == true ? true : false;
            bool shouldExportListItems = ExportListItemsCheckBox.IsChecked == true ? true : false;
            bool shouldImportDocuments = ImportDocumentsCheckBox.IsChecked == true ? true : false;
            bool shouldExportDocuments = ExportDocumentsCheckBox.IsChecked == true ? true : false;
            bool shouldSkipUpdates = SkipUpdateCheckBox.IsChecked == true ? true : false;
            string[] excludeFields = ExcludeFieldsTextBox.Text.Split(new string[] { ";" }, StringSplitOptions.None);

            int includeVersionsLimit = 0;
            if (LimitVersionsCheckBox.IsChecked == true)
            {
                string limitVersions = LimitVersionsTextBox.Text;
                if (int.TryParse(limitVersions, out includeVersionsLimit) == true)
                {
                }
            }
            */
            //SyncBackgroundWorker.RunWorkerAsync(new object[] { syncTask, shouldImportListItems, shouldExportListItems, shouldImportDocuments, shouldExportDocuments, shouldSkipUpdates, includeVersionsLimit, excludeFields });

            /*
            SyncDataWizardForm syncDataWizardForm = new SyncDataWizardForm();
            syncDataWizardForm.Initialize(syncTasks);
            if (syncDataWizardForm.ShowDialog(this.ParentWindow, "Data Import Wizard", false, true) == true)
            {
            }
            */
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            e.Handled = true;

            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }

        private void SourceNextButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentTabIndex = 1;
            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }



        private void MappingBackButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentTabIndex = 1;
            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }

        private void ScheduleBackButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentTabIndex = 2;
            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }


        private void DestinationSelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectEntityForm selectEntityForm = new SelectEntityForm();
            selectEntityForm.Initialize(new Type[] { typeof(SPWeb) });
            if (selectEntityForm.ShowDialog(this.ParentWindow, "Select an object to sync to") == true)
            {
                DestinationWeb = (SPWeb)selectEntityForm.SelectedObject;
                ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(DestinationWeb.SiteSettingID);
                DestinationSelectButton.Content = DestinationWeb.GetPath();
                SourceSelectButton.Content = SourceWeb.GetPath();
                SourceTermObjectSelectorControl.Initialize(siteSetting);
            }
        }


        private void HostControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DestinationNextButton_Click(object sender, RoutedEventArgs e)
        {
            object sourceObject = SourceTermObjectSelectorControl.SelectedObject;
            object destinationObject = DestinationTermObjectSelectorControl.SelectedObject;

            if (sourceObject == null)
            {
                MessageBox.Show("Please select a term from source list");
                return;
            }

            if (destinationObject == null)
            {
                MessageBox.Show("Please select a term from destination list");
                return;
            }

            if(sourceObject.GetType() != destinationObject.GetType())
            {
                MessageBox.Show("Please select the same object type from source and destiny");
                return;
            }



            CurrentTabIndex = 2;
            WizardTabControl.SelectedIndex = CurrentTabIndex;

        }

        private void DestinationBackButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentTabIndex = 0;
            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }
    }
}
