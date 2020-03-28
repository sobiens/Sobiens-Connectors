using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Service;
using Sobiens.Connectors.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace Sobiens.Connectors.Studio.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
            base.OnStartup(e);
            if (System.Configuration.ConfigurationManager.AppSettings["RunAs"] == "RunOnlyScheduledTasks")
            {
                Logger.Info("Running as only schedule tasks", "Application");
                ApplicationContext.SetApplicationManager(new Sobiens.Connectors.UI.SPCamlStudioApplicationManager(null));
                Logger.Info("Initializing application context ", "Application");
                ApplicationContext.Current.Initialize();

                Logger.Info("Enqueueing requests", "Application");
                QueryMediator.EnqueueRequests(DateTime.Now);

                Logger.Info("Performing requests", "Application");
                QueryMediator.PerformRequests(false);

                Logger.Info("Shutting down application", "Application");
                this.Shutdown();
            }
        }


        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //log(e);
            //Logger.Error(e.Exception, "Application");
            e.Handled = true;
        }
    }
}
