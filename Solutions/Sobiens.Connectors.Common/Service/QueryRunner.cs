﻿using Sobiens.Connectors.Entities;
using System;
using System.ComponentModel;

namespace Sobiens.Connectors.Common.Service
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
            SyncTaskStatus syncTaskStatus = SyncTasksManager.GetInstance().GetLastSyncTaskStatus(queryRunner.Task);
            if (syncTaskStatus != null)
                lastProcessStartDate = syncTaskStatus.StartTime;
            SyncTasksManager.GetInstance().ExportSyncTaskItems(queryRunner.Task, true, true, true, queryRunner.backgroundWorker, lastProcessStartDate, 0);
            SyncTasksManager.GetInstance().ProcessSyncTaskExportFiles(queryRunner.Task, queryRunner.backgroundWorker);
            SyncTasksManager.GetInstance().ImportSyncTaskItems(queryRunner.Task, queryRunner.Task.ShouldSkipUpdates, new string[] { }, queryRunner.backgroundWorker);
            SyncTasksManager.GetInstance().SaveSyncTaskStatus(queryRunner.Task, processStartDate, DateTime.Now, true, string.Empty);
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
