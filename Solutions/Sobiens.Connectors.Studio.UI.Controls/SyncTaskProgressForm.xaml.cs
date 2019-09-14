using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Service;
using Sobiens.Connectors.Common.SLExcelUtility;
using Sobiens.Connectors.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for CodeWizardForm.xaml
    /// </summary>
    public partial class SyncTaskProgressForm : HostControl
    {
        public BackgroundWorker worker = new BackgroundWorker();
        public SyncTask SyncTask { get; set; }
        public bool IsCompleted { get; set; }
        public SyncTaskProgressForm()
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
            this.OKButtonSelected += SyncTaskListForm_OKButtonSelected;
        }

        private void SyncTaskListForm_OKButtonSelected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                RefreshButton_Click(null, null);
                System.Threading.Thread.Sleep(500);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            SLExcelData processedData = SyncTasksManager.GetInstance().GetProcessData(SyncTask.ID, SyncTask.ProcessID, IsCompleted);

            if (processedData == null)
            {
                SyncTask syncTask = SyncTasksManager.GetInstance().SyncTasks.Where(t=>t.ProcessID == SyncTask.ProcessID).FirstOrDefault();
                if(syncTask != null)
                {
                    List<ProgressItem> progressItems = new List<ProgressItem>();
                    progressItems.Add(new ProgressItem()
                    {
                        Action = "Synchronization",
                        //Message = SyncTask.DestinationListName,
                        Status = syncTask.Status,
                        Path = ""
                    });

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        TasksDataGrid.ItemsSource = null;
                        TasksDataGrid.ItemsSource = progressItems;
                    }));
                }
            }
            else
            {
                try
                {
                    List<ProgressItem> progressItems = new List<ProgressItem>();
                    foreach (List<string> datarow in processedData.DataRows)
                    {
                        progressItems.Add(new ProgressItem()
                        {
                            Action = datarow[0],
                            Message = datarow[2],
                            Status = datarow[1],
                            Path = datarow[4]
                        });
                    }

                    int totalCount = progressItems.Count;
                    int completedCount = progressItems.Where(t => t.Status.Equals("Completed", StringComparison.InvariantCultureIgnoreCase) == true).Count();
                    int completionPercentage = (completedCount * 100) / totalCount;
                    progressItems.OrderByDescending(t => t.Status);

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        StatusProgressBar.Value = completionPercentage;
                        StatusTextBlock.Text = completedCount + "/" + totalCount;

                        TasksDataGrid.ItemsSource = null;
                        TasksDataGrid.ItemsSource = progressItems;
                    }));
                }
                catch (Exception ex)
                {
                }
            }
        }
    }

    public class ProgressItem
    {
        public string Path { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
