﻿using Sobiens.Connectors.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sobiens.Connectors.Common.Service
{
    public static class QueryMediator
    {
        private static List<QueryRunner> QueuedRequests = new List<QueryRunner>();
        private static object lockObject = new object();
        private static DateTime LastQueuePerformed = DateTime.Now;

        public static void EnqueueRequests(DateTime referenceTime)
        {
            var time = referenceTime.TimeOfDay;
            //List<SyncTask> synctasks = SyncTasksManager.GetInstance().GetTestListItemSyncTasks().ToList();
            Logger.Info("Total synctask count:" + SyncTasksManager.GetInstance().SyncTasks.Count, "EnqueueRequests");
            List<SyncTask> synctasks = SyncTasksManager.GetInstance().SyncTasks.Where(t => t.Scheduled == true).ToList();
            Logger.Info("Total scheduled synctask count:" + synctasks.Count, "EnqueueRequests");

            foreach (var synctask in synctasks)
            {
                QueryRunner queryRunner = QueuedRequests.Where(t => t.Task.ID == synctask.ID).FirstOrDefault();
                if (queryRunner == null)
                {
                    Logger.Info("Queued request synctask.ProcessID:" + synctask.ProcessID, "EnqueueRequests");
                    SyncTasksManager.GetInstance().SaveProcessStatus(synctask.ProcessID, "Queued", referenceTime, null);
                    EnqueAction(synctask);
                }
                else if(queryRunner.Task.LastRunStartDate.AddSeconds(synctask.ScheduleInterval) < DateTime.Now && queryRunner.State == RunnerState.Complete)
                {
                    Logger.Info("Removing queued request synctask.ProcessID:" + synctask.ProcessID, "EnqueueRequests");
                    QueuedRequests.Remove(queryRunner);
                    /*
                    SyncTaskStatus syncTaskStatus = SyncTasksManager.GetInstance().GetLastSyncTaskStatus(synctask);
                    if(syncTaskStatus.Successful == true)
                    {
                        synctask.LastSuccessfullyCompletedStartDate = synctask.LastRunStartDate;
                        SyncTasksManager.GetInstance().SaveSyncTasks();
                    }
                    */
                    SyncTasksManager.GetInstance().SaveProcessStatus(synctask.ProcessID, "Completed", referenceTime, null);
                    EnqueAction(synctask);
                }

            }
        }

        public static void PerformRequests(bool isAsync)
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
                    Logger.Info("Performing request synctask ProcessID:" + runner.Task.ProcessID, "EnqueueRequests");
                    SyncTasksManager.GetInstance().SaveProcessStatus(runner.Task.ProcessID, "Started", null, null);

                    //logger.Info(String.Format("Performing request for {0} RetriedCount:{1}...", runner.Request.TargetMeter.ToLogString(), runner.RetryCount));
                    if (isAsync == true)
                    {
                        Logger.Info("Running synctask as async ProcessID:" + runner.Task.ProcessID, "EnqueueRequests");
                        runner.RunAsync();
                    }
                    else
                    {
                        Logger.Info("Running synctask as sync ProcessID:" + runner.Task.ProcessID, "EnqueueRequests");
                        runner.RunSync();
                    }
                }
            }
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
        }

        private static void EnqueAction(SyncTask task)
        {
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

        /*
        private static void QueryStateChanged(object sender, QueryStateArgs args)
        {
            if (args.Response == null)
                return;

            try
            {
                if (args.Success)
                {
                    //logger.Error("SSX request has been completed succesfully.");
                    if (args.Response == null)
                    {
                        //logger.Error("Request appears to have completed successfully, however the response is null!");
                    }
                    if (args.Response.Skip == true)
                    {
                        //logger.Error("Request appears does not need to be processed, most likely data already exist");
                    }
                    else
                    {
                        //IResponseHandler responseHandler = ResponseHandlerFactory.GetResponseHandler(args.Response);
                        //responseHandler.HandleResponse();
                    }
                    Dequeue(((QueryRunner)sender));
                }
                else
                {
                    //logger.Error("SSX request has not been completed.");
                    //logger.Error(args.Error);
                    QueryRunner queryRunner = (QueryRunner)sender;
                    Dequeue(queryRunner);
                }
            }
            catch (Exception exc)
            {
                //logger.Error(exc);
                QueryRunner queryRunner = (QueryRunner)sender;
                Dequeue(queryRunner);
            }
            finally
            {
            }
        }

        private static void QueryStateChangedSync(object sender, QueryStateArgs args)
        {
            try
            {
                if (args.Success)
                {
                    //IResponseHandler responseHandler = ResponseHandlerFactory.GetResponseHandler(args.Response);
                    //responseHandler.HandleResponse();
                }
                else
                {
                    //logger.Error(args.Error);
                }
            }
            catch (Exception exc)
            {
                //logger.Error(exc);
            }
        }
        */
    }
}
