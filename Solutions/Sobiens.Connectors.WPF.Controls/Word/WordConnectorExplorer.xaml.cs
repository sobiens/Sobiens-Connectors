using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Services.SharePoint;
using Sobiens.Connectors.WPF.Controls.Settings;
using System.Collections.ObjectModel;
using System.IO;
using Sobiens.WPF.Controls.Classes;
using Sobiens.Connectors.Entities.Documents;
using Sobiens.Connectors.Entities.Interfaces;
using System.Windows.Threading;
using System.Threading;

namespace Sobiens.Connectors.WPF.Controls.Word
{
    /// <summary>
    /// Interaction logic for ConnectorExplorer.xaml
    /// </summary>
    public partial class WordConnectorExplorer : UserControl, IConnectorMainView
    {
        public object Inspector { get; set; }
        public DateTime InitializedDate { get; set; }
        public IWorkflowExplorer WorkflowExplorer
        {
            get
            {
                return workflowExplorer;
            }
        }

        public IDocumentTemplateSelection DocumentTemplateSelection
        {
            get
            {
                return DocumentTemplateSelector;
            }
        }

        public ISearchExplorer SearchExplorer
        {
            get
            {
                return searchExplorer1;
            }
        }

        public IConnectorExplorer ConnectorExplorer
        {
            get
            {
                return connectorExplorer1;
            }
        }

        public IStatusBar StatusBar
        {
            get
            {
                return StatusBarControl;
            }
        }

        public WordConnectorExplorer()
        {
            InitializeComponent();
            this.Resources.MergedDictionaries.Add(Sobiens.Connectors.Entities.Languages.Dict);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void RefreshControls()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                ItemsManager.ConnectorMainViewInitialize(this.DocumentTemplateSelection, this.SearchExplorer, this.ConnectorExplorer, this.WorkflowExplorer, tabControl1, WorkflowTabItem, EmptyConfigurationTabItem, TemplateTabItem, SearchTabItem, NavigatorTabItem, this.Inspector, this.StatusBarControl);
            }));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsForm sf = new SettingsForm();
            AppConfiguration configuration = ConfigurationManager.GetInstance().Configuration;
            sf.BindControls(configuration);
            if (sf.ShowDialog(null, Languages.Translate("Configuration")) == true)
            {
                ConfigurationManager.GetInstance().SaveAppConfiguration();
                ItemsManager.ConnectorMainViewInitialize(this.DocumentTemplateSelection, this.SearchExplorer, this.ConnectorExplorer, this.WorkflowExplorer, tabControl1, WorkflowTabItem, EmptyConfigurationTabItem, TemplateTabItem, SearchTabItem, NavigatorTabItem, this.Inspector, this.StatusBarControl);
                ItemsManager.ConnectorMainViewRefreshControls(this.DocumentTemplateSelection, this.SearchExplorer, this.ConnectorExplorer, this.WorkflowExplorer, tabControl1, WorkflowTabItem, TemplateTabItem, SearchTabItem, NavigatorTabItem);
            }
            else
            {
                ConfigurationManager.GetInstance().LoadConfiguration();
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            About aboutBox = new About();
            aboutBox.ShowDialog(null, Languages.Translate("About Sobiens Office Connector"));
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                ItemsManager.ConnectorMainViewRefreshControls(this.DocumentTemplateSelection, this.SearchExplorer, this.ConnectorExplorer, this.WorkflowExplorer, tabControl1, WorkflowTabItem, TemplateTabItem, SearchTabItem, NavigatorTabItem);
            }
        }

    }
}
