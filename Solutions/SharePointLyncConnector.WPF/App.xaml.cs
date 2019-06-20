using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using Sobiens.Connectors.WPF.Controls;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.WordConnector;
using Sobiens.Connectors.Entities;

namespace SharePointLyncConnector.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            UploadFilesForm uploadFilesForm = new UploadFilesForm();
            IConnectorMainView connectorExplorer = uploadFilesForm as IConnectorMainView;

            ApplicationContext.SetApplicationManager(new GeneralConnectorManager(connectorExplorer));
            ConfigurationManager.GetInstance().DownloadAdministrationXml(RefreshControls);

            uploadFilesForm.Initialize(new string[] { e.Args[0].ToString() });
            uploadFilesForm.ShowDialog(null, Languages.Translate("Send To Office Connector"));
            this.Shutdown();
        }

        private void RefreshControls()
        {
            ApplicationContext.Current.RefreshControlsFromConfiguration();
        }

    }
}
