using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
                QueryMediator.EnqueueRequests(DateTime.Now);
                QueryMediator.PerformRequests(false);
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
