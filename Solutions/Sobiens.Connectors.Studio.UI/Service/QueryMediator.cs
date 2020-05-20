using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.UI.Service
{
    public static class QueryMediator
    {
        private static List<QueryRunner> QueuedRequests = new List<QueryRunner>();
        private static object lockObject = new object();
        private static DateTime LastQueuePerformed = DateTime.Now;
        private static bool IsEnqueueInProgress = false;

        public static void EnqueueRequests(DateTime referenceTime)
        {
            if (IsEnqueueInProgress == true)
                return;

            IsEnqueueInProgress = true;
            try
            {
                //ApplicationContext.Current.ReportProgress(null, "Checking schedules...");

                var time = referenceTime.TimeOfDay;
                //List<SyncTask> synctasks = SyncTasksManager.GetInstance().GetTestListItemSyncTasks().ToList();
                IEnumerable<SyncTask> synctasks = SyncTasksManager.GetInstance().SyncTasks.Where(t => t.Scheduled == true);

                foreach (var synctask in synctasks)
                {
                    QueryRunner queryRunner = QueuedRequests.Where(t => t.Task.ID == synctask.ID).FirstOrDefault();
                    if (queryRunner == null)
                    {
                        SyncTasksManager.GetInstance().SaveProcessStatus(synctask.ProcessID, "Queued", referenceTime, null);
                        EnqueAction(synctask);
                    }
                    else if (synctask.ScheduleInterval == 0 && queryRunner.State == RunnerState.Complete)
                    {
                        QueuedRequests.Remove(queryRunner);
                        SyncTasksManager.GetInstance().AddSyncTaskHistory(synctask);
                        SyncTasksManager.GetInstance().SaveProcessStatus(synctask.ProcessID, "Completed", referenceTime, null);
                        SyncTasksManager.GetInstance().RemoveProcess(synctask.ProcessID);
                    }
                    else if (queryRunner.Task.LastRunStartDate.AddSeconds(synctask.ScheduleInterval) < DateTime.Now && queryRunner.State == RunnerState.Complete)
                    {
                        SyncTasksManager.GetInstance().AddSyncTaskHistory(synctask);
                        QueuedRequests.Remove(queryRunner);
                        SyncTasksManager.GetInstance().SaveProcessStatus(synctask.ProcessID, "Queued", referenceTime, null);
                        EnqueAction(synctask);
                    }
                }

                ReportProgress();
            }
            catch(Exception ex)
            {

            }

            IsEnqueueInProgress = false;
        }

        public static void ReportProgress()
        {
            int totalCount = QueuedRequests.Count;
            int inProgressCount = GetInProgressRequests().Length;
            string message = "Progressing " + inProgressCount + "/" + totalCount + " ...";
            if (totalCount > 0)
            {
                int barValue = 100 / (2 * totalCount);
                int percentage = inProgressCount * barValue;
                if (percentage == 0)
                    percentage = 10;
                ApplicationContext.Current.ReportProgress(percentage, message);
            }
            else
                ApplicationContext.Current.ReportProgress(100, "Done");
        }

        public static void PerformRequests()
        {
            lock (lockObject)
            {
                IEnumerable<QueryRunner> normal = from QueryRunner runner in QueuedRequests where runner.State == RunnerState.New select runner;
                if ((DateTime.Now - LastQueuePerformed).TotalSeconds > 10)
                {
                    //logger.Info(String.Format("Performing request for QueuedRequests.Count:{0}...", QueuedRequests.Count));
                    //logger.Info(String.Format("Performing request for High.Count:{0}...", high.Count()));
                    //logger.Info(String.Format("Performing request for Normal.Count:{0}...", normal.Count()));
                    LastQueuePerformed = DateTime.Now;
                }

                foreach (QueryRunner runner in normal)
                {
                    bool isTaskInProgress = (from QueryRunner inProgressRunner in QueuedRequests
                                              where runner.Task.ID == inProgressRunner.Task.ID && inProgressRunner.State == RunnerState.InProgress
                                              select runner).Count() > 0 ? true : false;
                    if (isTaskInProgress == true)
                        continue;

                    SyncTasksManager.GetInstance().SaveProcessStatus(runner.Task.ProcessID, "Started", null, null);
                    //logger.Info(String.Format("Performing request for {0} RetriedCount:{1}...", runner.Request.TargetMeter.ToLogString(), runner.RetryCount));
                    runner.RunAsync();
                }
            }

            ReportProgress();
        }

        public static void TimeOutRequests()
        {
            lock (lockObject)
            {
                foreach (QueryRunner runner in QueuedRequests)
                {
                    /*
                    if (runner.TriggerTime.HasValue && DateTime.Now.Subtract(runner.TriggerTime.Value).TotalMilliseconds > runner.TimeOut && runner.State != RunnerState.Cancelled)
                    {
                        //logger.Warn("Cancelling request {0} due to timeout", runner.Request);
                        runner.Cancel();
                    }
                    */
                }
            }

            ReportProgress();
        }

        public static void CancelRequests() 
        {
            lock (lockObject)
            {
                int removedItems = QueuedRequests.Count;
                foreach (QueryRunner runner in QueuedRequests)
                {
                    if (runner.State == RunnerState.InProgress)
                    {
                        runner.Cancel();
                    }
                }
                QueuedRequests.Clear();
                //logger.Info(String.Format("Removed {0} requests from the queue...", removedItems));
            }

            ReportProgress();
        }

        private static void EnqueAction(SyncTask task)
        {
            //string previousSyncTaskStatusFilePath = ConfigurationManager.GetInstance().GetSyncTaskStatusFilePath(task);
            task.ProcessID = Guid.NewGuid();
            /*
            if (System.IO.File.Exists(previousSyncTaskStatusFilePath) == true)
            {
                string currentSyncTaskStatusFilePath = ConfigurationManager.GetInstance().GetSyncTaskStatusFilePath(task);
                System.IO.File.Copy(previousSyncTaskStatusFilePath, currentSyncTaskStatusFilePath);
            }
            */
            Enque(task);
        }

        private static void Enque(SyncTask task){
            lock (lockObject)
            {
                var runner = new QueryRunner() {
                    Task = task
                };
                //runner.RetryCount = request.RetryCount;
                //runner.OnStateChange += QueryStateChanged;
                QueuedRequests.Add(runner);
            }

            ReportProgress();
        }

        private static void Dequeue(QueryRunner runner){
            lock (lockObject)
            {
                //logger.Debug("Dequeing request {0}", runner.Request);
                runner.State = RunnerState.Complete;
                if(QueuedRequests.Contains(runner) == true)
                    QueuedRequests.Remove(runner);
                //logger.Debug("Dequeued request {0}", runner.Request);
            }
            //logger.Debug("Dequeued1 request {0}", runner.Request);

            ReportProgress();
        }

        public static QueryRunner[] GetQueuedRequests()
        {
            lock (lockObject)
            {
                var qr = new QueryRunner[QueuedRequests.Count];
                QueuedRequests.CopyTo(qr);
                return qr;
            }
        }

        public static QueryRunner[] GetInProgressRequests()
        {
            lock (lockObject)
            {
                return (from QueryRunner inProgressRunner in QueuedRequests
                        where inProgressRunner.State == RunnerState.InProgress
                        select inProgressRunner).ToArray();
            }
        }

    }
}
