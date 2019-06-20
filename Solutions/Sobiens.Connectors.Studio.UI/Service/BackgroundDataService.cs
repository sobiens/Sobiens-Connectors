using Sobiens.Connectors.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Sobiens.Connectors.UI.Service
{
    public class BackgroundDataService
    {
        private System.Timers.Timer timer;
        private Thread worker;
        private static DateTime LastQueuePerformed = DateTime.Now;
        private static BackgroundDataService _Instance = null;

        public static BackgroundDataService GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new BackgroundDataService();
            }
            return _Instance;
        }

        public void OnStart(string[] args)
        {
            Logger.Info("Starting service ...", "BackgroundDataService");
            Logger.Info("Creating Service context ...", "BackgroundDataService");

            timer = new System.Timers.Timer(30000);
            timer.Elapsed += new ElapsedEventHandler(EnqueueRequests);
            timer.Enabled = true;

            worker = new Thread(new ThreadStart(PerformRequests));
            worker.Start();

            Logger.Info("Service started and timer activated.", "BackgroundDataService");
        }

        public void OnStop()
        {
            Logger.Info("Stopping service ...", "BackgroundDataService");
            timer.Enabled = false;
            Logger.Info("Timer disabled, waiting for pending requests to complete ...", "BackgroundDataService");

            int outstanding = QueryMediator.GetQueuedRequests().Length;
            Logger.Info(String.Format("No of enqued requests is {0}, waiting to terminate ...", outstanding), "BackgroundDataService");

            QueryMediator.CancelRequests();
            worker.Abort();

            Logger.Info("Service successfully terminated.", "BackgroundDataService");
        }

        private void EnqueueRequests(object sender, ElapsedEventArgs e)
        {
            try
            {
                QueryMediator.EnqueueRequests(e.SignalTime);
            }
            catch (Exception exc)
            {
                Logger.Error(exc, "BackgroundDataService");
            }
        }

        private void PerformRequests()
        {
            Logger.Info("Worker thread for processing request queue has started.", "BackgroundDataService");

            while (true)
            {
                try
                {
                    if ((DateTime.Now - LastQueuePerformed).TotalSeconds > 10)
                    {
                        Logger.Info("Performing requests OLEYYYY...", "BackgroundDataService");
                        LastQueuePerformed = DateTime.Now;
                    }

                    QueryMediator.PerformRequests();
                    QueryMediator.TimeOutRequests();
                    Thread.Sleep(30000);
                }
                catch (ThreadAbortException)
                {
                    Logger.Info("Worker thread for processing request queue is terminated.", "BackgroundDataService");
                    break;
                }
                catch (Exception exc)
                {
                    Logger.Info("An error occured while performing requests:. " + exc.Message, "BackgroundDataService");
                    Logger.Error(exc, "BackgroundDataService");
                }
            }
        }

    }
}
