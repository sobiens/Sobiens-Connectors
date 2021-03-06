﻿using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sobiens.Connectors.UI.Service
{
    public class QueryRunner
    {
        public QueryRunner()
        {
        }

        public RunnerState State
        {
            get;
            set;
        }

        public DateTime? TriggerTime
        {
            get;
            set;
        }

        public SyncTask Task
        {
            get;
            set;
        }
        BackgroundWorker backgroundWorker = new BackgroundWorker();

        public void RunAsync()
        {
            State = RunnerState.InProgress;
            TriggerTime = DateTime.Now;

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerAsync(this);

            //RequestHandler rh = new RequestHandler(this.RunRequest);
            //rh.BeginInvoke(new AsyncCallback(EndRequest), this);
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime processStartDate = DateTime.Now;
            DateTime? lastProcessStartDate = null;
            QueryRunner queryRunner = (QueryRunner)e.Argument;
            try
            {
                //SyncTaskStatus syncTaskStatus = SyncTasksManager.GetInstance().GetLastSyncTaskStatus(queryRunner.Task);
                if (queryRunner.Task.LastSuccessfullyCompletedStartDate != DateTime.MinValue && queryRunner.Task.LastSuccessfullyCompletedStartDate != DateTime.MaxValue)
                    lastProcessStartDate = queryRunner.Task.LastSuccessfullyCompletedStartDate;
                if (queryRunner.Task as SyncTaskListItemsCopy != null)
                {
                    SyncTaskListItemsCopy syncTaskListItemsCopy = queryRunner.Task as SyncTaskListItemsCopy;
                    SyncTasksManager.GetInstance().SaveProcessStatus(syncTaskListItemsCopy.ProcessID, "Exporting items", null, null);
                    SyncTasksManager.GetInstance().ExportSyncTaskItems(syncTaskListItemsCopy, true, true, true, queryRunner.backgroundWorker, lastProcessStartDate, 0);
                    SyncTasksManager.GetInstance().SaveProcessStatus(syncTaskListItemsCopy.ProcessID, "Processing export items", null, null);
                    SyncTasksManager.GetInstance().ProcessSyncTaskExportFiles(syncTaskListItemsCopy, queryRunner.backgroundWorker);
                    SyncTasksManager.GetInstance().SaveProcessStatus(syncTaskListItemsCopy.ProcessID, "Importing items", null, null);
                    SyncTasksManager.GetInstance().ImportSyncTaskItems(syncTaskListItemsCopy, syncTaskListItemsCopy.ShouldSkipUpdates, new string[] { }, queryRunner.backgroundWorker);
                }
                else if (queryRunner.Task as SyncTaskSchemaCopy != null)
                {
                    SyncTaskSchemaCopy syncTaskSchemaCopy = queryRunner.Task as SyncTaskSchemaCopy;
                    SyncTasksManager.GetInstance().SyncSchema(queryRunner.backgroundWorker, syncTaskSchemaCopy.SourceObjects, syncTaskSchemaCopy.DestinationObject);
                }

            SyncTasksManager.GetInstance().SaveProcessStatus(queryRunner.Task.ProcessID, "Completed", null, queryRunner.Task.LastRunStartDate);
                SyncTasksManager.GetInstance().SaveSyncTaskStatus(queryRunner.Task, processStartDate, DateTime.Now, true, string.Empty);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, "QueryRunner");
                SyncTasksManager.GetInstance().SaveProcessStatus(queryRunner.Task.ProcessID, "Failed, check log", null, null);
            }

            queryRunner.State = RunnerState.Complete;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        public void Cancel()
        {
            backgroundWorker.CancelAsync();
            State = RunnerState.Cancelled;
            //Request.GwTransport.Interrupt();
        }
    }

    public enum RunnerState { New, InProgress, Complete, Cancelled }
}
