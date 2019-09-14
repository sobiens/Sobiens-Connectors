using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for CodeWizardForm.xaml
    /// </summary>
    public partial class SyncTaskListForm : HostControl
    {
        public SyncTaskListForm()
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
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshButton_Click(null, null);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryDatePicker.SelectedDate.HasValue == false)
                HistoryDatePicker.SelectedDate = DateTime.Now;

            List<SyncTask> synctasks = SyncTasksManager.GetInstance().SyncTasks;
            TasksDataGrid.ItemsSource = null;
            TasksDataGrid.ItemsSource = synctasks;

            List<SyncTask> pastSynctasks = SyncTasksManager.GetInstance().GetSyncTaskHistories(HistoryDatePicker.SelectedDate.Value);
            PastTasksDataGrid.ItemsSource = null;
            PastTasksDataGrid.ItemsSource = pastSynctasks;


        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            List<SyncTask> synctasks = new List<SyncTask>();
            foreach(object synchTask in TasksDataGrid.SelectedItems) {
                SyncTask _synchTask = (SyncTask)synchTask;
                SyncTasksManager.GetInstance().SyncTasks.Remove(_synchTask);
            }

            SyncTasksManager.GetInstance().SaveSyncTasks();
            RefreshButton_Click(null, null);
        }

        private void TasksDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SyncTask syncTask = TasksDataGrid.SelectedItem as SyncTask;
            if (syncTask == null)
                return;

            SyncTaskProgressForm form = new SyncTaskProgressForm();
            form.SyncTask = syncTask;
            form.IsCompleted = false;
            form.ShowDialog(this.ParentWindow, "Progress", false, true);
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = ConfigurationManager.GetInstance().GetSyncTasksFolder();
            System.Diagnostics.Process.Start(folderPath);
        }

        private void PastTasksDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SyncTask syncTask = PastTasksDataGrid.SelectedItem as SyncTask;
            if (syncTask == null)
                return;

            SyncTaskProgressForm form = new SyncTaskProgressForm();
            form.SyncTask = syncTask;
            form.IsCompleted = true;
            form.ShowDialog(this.ParentWindow, "Progress", false, true);
        }

        private void PastTasksDataGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void HistoryDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshButton_Click(null, null);
        }

        private void ProgressButton_Click(object sender, RoutedEventArgs e)
        {
            string taskId = ((Button)e.Source).Tag.ToString();
            SyncTask syncTask = SyncTasksManager.GetInstance().SyncTasks.Where(t => t.ID.ToString().Equals(taskId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (syncTask == null)
            {
                MessageBox.Show("Task not found");
                return;
            }

            SyncTaskProgressForm form = new SyncTaskProgressForm();
            form.SyncTask = syncTask;
            form.IsCompleted = false;
            form.ShowDialog(this.ParentWindow, "Progress", false, true);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string taskId = ((Button)e.Source).Tag.ToString();
            SyncTask syncTask = SyncTasksManager.GetInstance().SyncTasks.Where(t => t.ID.ToString().Equals(taskId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (syncTask == null)
            {
                MessageBox.Show("Task not found");
                return;
            }

            SyncCopyListWizardForm syncCopyListWizardForm = new SyncCopyListWizardForm();
            syncCopyListWizardForm.Initialize((SyncTaskListItemsCopy)syncTask);
            if (syncCopyListWizardForm.ShowDialog(this.ParentWindow, "Data Import Wizard", false, true) == true)
            {
            }
        }
    }
}
