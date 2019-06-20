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
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.Workflows;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Threading;
using System.Windows.Threading;
using System.Threading;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.WPF.Controls.Workflow;

namespace Sobiens.Connectors.WPF.Controls
{
    /// <summary>
    /// Interaction logic for WorkflowExplorer.xaml
    /// </summary>
    public partial class WorkflowExplorer : HostControl, IWorkflowExplorer
    {

        public bool HasAnythingToDisplay
        {
            get
            {
                foreach (WorkflowConfiguration workflowConfiguration in this.WorkflowConfigurations)
                {
                    List<TaskListLocation> taskListLocations = workflowConfiguration.TaskListLocations[ApplicationContext.Current.GetApplicationType()];
                    if (taskListLocations != null && taskListLocations.Count > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public IConnectorExplorer ConnectorExplorer { get; private set; }
        private List<WorkflowConfiguration> WorkflowConfigurations { get; set; }
        private SiteSettings SiteSettings { get; set; }
        public bool IsDataLoaded { get; set; }

        public WorkflowExplorer()
        {
            InitializeComponent();
        }

        public void SetConnectorExplorer(IConnectorExplorer connectorExplorer)
        {
            this.ConnectorExplorer = connectorExplorer;
        }

        public void Initialize(SiteSettings siteSettings, List<WorkflowConfiguration> workflowConfigurations)
        {
            this.IsDataLoaded = false;
            this.SiteSettings = siteSettings;
            this.WorkflowConfigurations = workflowConfigurations;
        }

        public void RefreshControls()
        {
            if (this.IsDataLoaded == false)
            {
                FillTaskListViews();

                this.IsDataLoaded = true;
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
        }

        private void FillTaskListViews()
        {
            List<Task> tasks = new List<Task>();
            List<TaskListLocation> mainTaskListLocations = new List<TaskListLocation>();
            foreach (WorkflowConfiguration workflowConfiguration in this.WorkflowConfigurations)
            {
                List<TaskListLocation> taskListLocations = workflowConfiguration.TaskListLocations[ApplicationContext.Current.GetApplicationType()];
                if (taskListLocations != null)
                {
                    mainTaskListLocations.AddRange(taskListLocations);
                }
            }

            TasksGrid.Children.Clear();
            foreach (TaskListLocation taskListLocation in mainTaskListLocations)
            {
                var bc = new BrushConverter();
                SiteSetting siteSetting = this.SiteSettings[taskListLocation.SiteSettingID];
                Expander expander = new Expander();
                expander.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                Label label = new Label();
                label.Background = new SolidColorBrush(Colors.Aqua);
                label.Content = taskListLocation.Name;
                label.BorderThickness = new Thickness(1);
                label.BorderBrush = (Brush)bc.ConvertFrom("#FF0080FF");
                label.FontWeight = FontWeights.Bold;
                //label.Width = "{Binding RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type Expander}}, Path=ActualWidth}";
                expander.Header = label;
                expander.Expanded += new RoutedEventHandler(expander_Expanded);
                TaskListView taskListView = new TaskListView();
                taskListView.Initialize(siteSetting, taskListLocation);
                taskListView.Margin = new Thickness(0, 0, 0, 0);
                expander.Content = taskListView;
                TasksGrid.Children.Add(expander);
            }
        }

        void expander_Expanded(object sender, RoutedEventArgs e)
        {
            Expander expander = (Expander)sender;
            if (expander.IsExpanded == true)
            {
                //TaskListView taskListView = (TaskListView)expander.Content;
                //taskListView.RefreshControls(false);
            }
        }
    }
}
