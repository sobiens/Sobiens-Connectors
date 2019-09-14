using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.CRM;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.SQLServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for SyncSchemaListWizardForm.xaml
    /// </summary>
    public partial class SyncSchemaWizardForm : HostControl
    {

        private int CurrentTabIndex = 0;
        private SyncTaskSchemaCopy SyncTask = null;

        public SyncSchemaWizardForm()
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

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            e.Handled = true;

            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }

        private void SourceNextButton_Click(object sender, RoutedEventArgs e)
        {
            List<Folder> selectedObjects = SourceSelectEntityForm.SourceDataFoldersSelector.SelectedObjects;
            if(selectedObjects.Count == 0)
            {
                MessageBox.Show("Please select a source object!");
            }
            else
            {
                CurrentTabIndex = 1;
                WizardTabControl.SelectedIndex = CurrentTabIndex;
            }
        }

        private void HostControl_Loaded(object sender, RoutedEventArgs e)
        {
            SourceSelectEntityForm.Initialize(new Type[] { typeof(SPWeb), typeof(SPList), typeof(CRMEntity), typeof(SQLTable) });
            DestinationSelectEntityForm.Initialize(new Type[] { typeof(SPWeb), typeof(CRMEntity), typeof(SQLTable) });
        }

        private void DestinationBackButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentTabIndex = 0;
            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }

        private void DestinationNextButton_Click(object sender, RoutedEventArgs e)
        {
            List<Folder> destinationSelectedObjects = DestinationSelectEntityForm.SourceDataFoldersSelector.SelectedObjects;
            if (destinationSelectedObjects.Count == 0)
            {
                MessageBox.Show("Please select a destination object!");
                return;
            }
            List<Folder> sourceSelectedObjects = SourceSelectEntityForm.SourceDataFoldersSelector.SelectedObjects;


            SyncTask = SyncTaskSchemaCopy.NewSyncTask();
            SyncTask.SourceObjects = sourceSelectedObjects;
            SyncTask.DestinationObject = destinationSelectedObjects[0];
            SyncTask.IncludeData = CopyDataCheckBox.IsChecked.HasValue == true ? CopyDataCheckBox.IsChecked.Value : false;

            SyncTask.ScheduleInterval = 0;
            SyncTask.Scheduled = true;
            SyncTasksManager.GetInstance().AddSyncTask(SyncTask);
            SyncTasksManager.GetInstance().SaveSyncTasks();
            MessageBox.Show("Synchronization has started.");
            this.Close(true);

            SyncTaskProgressForm form = new SyncTaskProgressForm();
            form.SyncTask = SyncTask;
            form.IsCompleted = false;
            form.ShowDialog(null, "Progress", false, true);
        }
    }
}
