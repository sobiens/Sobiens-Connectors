using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for CodeWizardForm.xaml
    /// </summary>
    public partial class SyncDataWizardForm : HostControl
    {
        BackgroundWorker SyncBackgroundWorker = new BackgroundWorker();
        public SyncDataWizardForm()
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
            SyncBackgroundWorker.DoWork += SyncBackgroundWorker_DoWork;
            SyncBackgroundWorker.ProgressChanged += SyncBackgroundWorker_ProgressChanged;
            SyncBackgroundWorker.RunWorkerCompleted += SyncBackgroundWorker_RunWorkerCompleted;
            SyncBackgroundWorker.WorkerReportsProgress = true;
        }

        private void SyncBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void SyncBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SyncDataProgressBar.Value = e.ProgressPercentage;
            //SyncDataProgressBar.InvalidateVisual();
            //SyncDataProgressBar.UpdateLayout();
            ProgressTextBox.Text = e.ProgressPercentage + "% " + e.UserState.ToString();
            //ProgressTextBox.InvalidateVisual();
            //ProgressTextBox.UpdateLayout();
        }

        private void SyncBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arguments = (object[])e.Argument;

            SyncTask syncTask = (SyncTask)arguments[0];
            bool shouldImportListItems = (bool)arguments[1];
            bool shouldExportListItems = (bool)arguments[2];
            bool shouldImportDocuments = (bool)arguments[3];
            bool shouldExportDocuments = (bool)arguments[4];
            bool shouldSkipUpdates = (bool)arguments[5];
            int includeVersionsLimit = (int)arguments[6];
            string[] excludeFields = (string[])arguments[7];

            try
            {
                SyncTasksManager.GetInstance().ExportSyncTaskItems(syncTask, shouldExportListItems, shouldExportDocuments, shouldImportListItems, SyncBackgroundWorker, null, includeVersionsLimit);

                if (shouldImportListItems == true)
                {
                    SyncTasksManager.GetInstance().ProcessSyncTaskExportFiles(syncTask, SyncBackgroundWorker);
                    SyncTasksManager.GetInstance().ImportSyncTaskItems(syncTask, shouldSkipUpdates, excludeFields, SyncBackgroundWorker);
                }

                string folderPath = ConfigurationManager.GetInstance().GetSyncTaskFolder(syncTask);
                System.Diagnostics.Process.Start("explorer.exe", folderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured:" + ex.Message + Environment.NewLine + "StackTrace:" + ex.StackTrace);
                Logger.Error(ex, "ProcessSyncTaskExport");
            }

        }

        private void SyncDataWizardForm_OKButtonSelected(object sender, EventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;

        }

        public void Initialize()
        {
            SyncTasksComboBox.Items.Clear();
            SyncTasksComboBox.DisplayMemberPath = "Name";
            SyncTasksComboBox.ItemsSource = SyncTasksManager.GetInstance().SyncTasks;
        }

        public void Initialize(List<SyncTask> syncTasks)
        {
            SyncTasksComboBox.Items.Clear();
            SyncTasksComboBox.DisplayMemberPath = "Name";
            SyncTasksComboBox.ItemsSource = syncTasks;
        }

        private void MigrateButton_Click(object sender, RoutedEventArgs e)
        {
            SyncTask syncTask = (SyncTask)SyncTasksComboBox.SelectedItem;
            bool shouldImportListItems = ImportListItemsCheckBox.IsChecked == true ? true : false;
            bool shouldExportListItems = ExportListItemsCheckBox.IsChecked == true ? true : false;
            bool shouldImportDocuments = ImportDocumentsCheckBox.IsChecked == true ? true : false;
            bool shouldExportDocuments = ExportDocumentsCheckBox.IsChecked == true ? true : false;
            bool shouldSkipUpdates = SkipUpdateCheckBox.IsChecked == true ? true : false;
            string[] excludeFields = ExcludeFieldsTextBox.Text.Split(new string[] { ";" }, StringSplitOptions.None);

            int includeVersionsLimit = 0;
            if(LimitVersionsCheckBox.IsChecked == true)
            {
                string limitVersions = LimitVersionsTextBox.Text;
                if (int.TryParse(limitVersions, out includeVersionsLimit)==true)
                {
                }
            }

            SyncBackgroundWorker.RunWorkerAsync(new object[] { syncTask, shouldImportListItems, shouldExportListItems, shouldImportDocuments, shouldExportDocuments, shouldSkipUpdates, includeVersionsLimit, excludeFields });
        }
    }
}
