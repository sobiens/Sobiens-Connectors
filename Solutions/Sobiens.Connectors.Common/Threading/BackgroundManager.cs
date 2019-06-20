#if General
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.Common.Threading
{
    public class BackgroundManager
    {
        private static BackgroundManager instance = null;
        
        private int activeThreadCount = 0;
        private BackgroundManager()
        {
            queuedBackgroundWorker = new QueuedBackgroundWorker();
            queuedBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_DoWork);
            queuedBackgroundWorker.RunWorkerCompleted += new Sobiens.Connectors.Common.Threading.RunWorkerCompletedWithUserStateEventHandler(backgroundWorker_RunWorkerCompleted);

            threadPool = new ThreadPool(2, 8, "BackgroundManager ThreadPool for ");
            threadPool.Start();
        }

        public static BackgroundManager GetInstance()
        {
            if (instance == null)
            {
                instance = new BackgroundManager();
            }
            return instance;
        }


        ~BackgroundManager()
        {
            //TODO: We need to wait here until we properly cancel all request, save them etc.
            queuedBackgroundWorker.CancelAllAsync();

            //TODO: Not sure if we really need to wait here for threadpool. Maybe ".Stop" would be better
            threadPool.Stop();
        }

        /// <summary>
        /// QueuedBackgroundWorker is used for Data processing. These operations are data modifying operations that should be execured in order.
        /// </summary>
        QueuedBackgroundWorker queuedBackgroundWorker;

        /// <summary>
        /// ThreadPool is used for Information gathering. These operations are used to get information or do async operations that does not rely on execution order.
        /// </summary>
        ThreadPool threadPool;

        public bool AddWorkItem(WorkItem workItem)
        {
            string message = string.Format("WorkItem has been added LogCategory:{0} Title:{1} WorkItemType:{2}", workItem.LogCategory, workItem.Title, workItem.WorkItemType.ToString());
            Logger.Warning(message, ApplicationContext.Current.GetApplicationType().ToString());
            activeThreadCount++;
            switch (workItem.WorkItemType)
            {
                case WorkItem.WorkItemTypeEnum.NonCriticalWorkItem:
                    queuedBackgroundWorker.RunWorkerAsync(workItem);
                    break;
                case WorkItem.WorkItemTypeEnum.CriticalWorkItem:
                    threadPool.PostRequest(new WorkRequestDelegate(workItem.CallbackFunction), workItem.CallbackData);
                    break;
            }
            return true;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkItem args = (WorkItem)e.Argument;
            string message = string.Format("WorkItem is being processed LogCategory:{0} Title:{1} WorkItemType:{2}", args.LogCategory, args.Title, args.WorkItemType.ToString());
            Logger.Warning(message, ApplicationContext.Current.GetApplicationType().ToString());

            int percentage = 100 / activeThreadCount;
            ApplicationContext.Current.SetStatusBar(args.Title, percentage);
            if (queuedBackgroundWorker.IsCancellationPending(args))
            {
                e.Cancel = true;
            }
            else
            {
                try
                {
                    //TODO: correct second parameter below
                    args.CallbackFunction(args.CallbackData, DateTime.Now);
                }
                catch (Exception ex)
                {
                    message = string.Format("An error occured in WorkItem LogCategory:{0} Title:{1} WorkItemType:{2}", args.LogCategory, args.Title, args.WorkItemType.ToString());
                    Exception exc = new Exception(message, ex);
                    Logger.Error(exc, ApplicationContext.Current.GetApplicationType().ToString());
                }
            }

            e.Result = args;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgsWithUserState e)
        {
            if (e.Cancelled)
            {
                //Operation was cancelled
            }
            else if (e.Error != null)
            {
                //Asynchronous operation failed
            }
            else
            {
                //Success
            }
            activeThreadCount--;
            if (activeThreadCount == 0)
            {
                ApplicationContext.Current.SetStatusBar(Languages.Translate("Ready"), 100);
            }
        }
    }
}
#endif
