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
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common.Threading;
using Sobiens.Connectors.Entities.Workflows;
using System.Threading;
using System.Windows.Threading;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Common;
using Sobiens.WPF.Controls.UserControls;

namespace Sobiens.Connectors.WPF.Controls.Workflow
{
    /// <summary>
    /// Interaction logic for TaskListView.xaml
    /// </summary>
    public partial class TaskListView : HostControl
    {
        public TaskListLocation TaskListLocation { get; set; }
        public SiteSetting SiteSetting { get; set; }
        public bool IsDataLoaded { get; set; }
        private Guid? _UniqueID;
        public Guid UniqueID
        {
            get
            {
                if (_UniqueID.HasValue == false)
                {
                    _UniqueID = Guid.NewGuid();
                }

                return _UniqueID.Value;
            }
        }

        public TaskListView()
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.TasksListView.ContextMenu = new ContextMenu();

            List<ListBoxItemExt> items = new List<ListBoxItemExt>();
            items.Add(new ListBoxItemExt() { Content = Languages.Translate("My Tasks"), IsChecked = true });
            AssignedToFilterableHeader.Initialize(items, Languages.Translate("AssignedTo"));
            AssignedToFilterableHeader.FiltersChanged += new Sobiens.WPF.Controls.UserControls.FilterableHeader_FiltersChanged(AssignedToFilterableHeader_FiltersChanged);

            List<ListBoxItemExt> statusItems = new List<ListBoxItemExt>();
            statusItems.Add(new ListBoxItemExt() { Content = Languages.Translate("Not Started"), IsChecked=true });
            statusItems.Add(new ListBoxItemExt() { Content = Languages.Translate("In Progress"), IsChecked = true });
            statusItems.Add(new ListBoxItemExt() { Content = Languages.Translate("Completed"), IsChecked = false });
            statusItems.Add(new ListBoxItemExt() { Content = Languages.Translate("Deferred"), IsChecked = true });
            statusItems.Add(new ListBoxItemExt() { Content = Languages.Translate("Waiting on someone else"), IsChecked = true });
            StatusFilterableHeader.Initialize(statusItems, Languages.Translate("Status"));
            StatusFilterableHeader.FiltersChanged += new Sobiens.WPF.Controls.UserControls.FilterableHeader_FiltersChanged(StatusFilterableHeader_FiltersChanged);

            List<ListBoxItemExt> priorityItems = new List<ListBoxItemExt>();
            priorityItems.Add(new ListBoxItemExt() { Content = Languages.Translate("(1) High"), IsChecked = true });
            priorityItems.Add(new ListBoxItemExt() { Content = Languages.Translate("(2) Normal"), IsChecked = true });
            priorityItems.Add(new ListBoxItemExt() { Content = Languages.Translate("(3) Low"), IsChecked = true });
            PriorityFilterableHeader.Initialize(priorityItems, Languages.Translate("Priority"));
            PriorityFilterableHeader.FiltersChanged += new Sobiens.WPF.Controls.UserControls.FilterableHeader_FiltersChanged(PriorityFilterableHeader_FiltersChanged);

            this.RefreshControls(false);
        }

        void PriorityFilterableHeader_FiltersChanged(Sobiens.WPF.Controls.UserControls.FilterableHeader filterableHeader)
        {
            RefreshControls(true);
        }

        void StatusFilterableHeader_FiltersChanged(Sobiens.WPF.Controls.UserControls.FilterableHeader filterableHeader)
        {
            RefreshControls(true);
        }

        void AssignedToFilterableHeader_FiltersChanged(Sobiens.WPF.Controls.UserControls.FilterableHeader filterableHeader)
        {
            RefreshControls(true);
        }

        public void Initialize(SiteSetting siteSetting, TaskListLocation taskListLocation)
        {
            this.IsDataLoaded = false;
            this.SiteSetting = siteSetting;
            this.TaskListLocation = taskListLocation;
        }

        public void RefreshControls(bool forceToReload)
        {
            if (this.IsDataLoaded == false || forceToReload == true)
            {
                RefreshImage.Visibility = System.Windows.Visibility.Collapsed;
                LoadingImage.Visibility = System.Windows.Visibility.Visible;
                List<string> assignedTos = new List<string>();
                if (AssignedToFilterableHeader.FilterItems != null)
                {
                    assignedTos = AssignedToFilterableHeader.NotSelectedItemTexts;
                }

                List<string> statuses = new List<string>();
                if (StatusFilterableHeader.FilterItems != null)
                {
                    statuses = StatusFilterableHeader.NotSelectedItemTexts;
                }

                List<string> priorities = new List<string>();
                if (PriorityFilterableHeader.FilterItems != null)
                {
                    priorities = PriorityFilterableHeader.NotSelectedItemTexts;
                }

                object[] args = new object[] { this.SiteSetting, this.TaskListLocation, this.TasksListView, assignedTos, statuses, priorities, LoadingImage, RefreshImage };
                WorkItem workItem = new WorkItem(Languages.Translate("Populating tasks"));
                workItem.CallbackFunction = new WorkRequestDelegate(LoadTasks_Callback);
                workItem.CallbackData = args;
                workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
                BackgroundManager.GetInstance().AddWorkItem(workItem);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.RefreshControls(true);
        }

        private void LoadTasks_Callback(object _item, DateTime dateTime)
        {
            object[] args = ((object[])_item);
            SiteSetting siteSetting = (SiteSetting)args[0];
            TaskListLocation taskListLocation = (TaskListLocation)args[1];
            ListView tasksListView = (ListView)args[2];
            List<string> assignedTos = (List<string>)args[3];
            List<string> statuses = (List<string>)args[4];
            List<string> priorities = (List<string>)args[5];
            Image loadingImage = (Image)args[6];
            Image refreshImage = (Image)args[7];

            List<Task> tasks = new List<Task>();

            CamlFilters customFilters = new CamlFilters();
            customFilters.IsOr = false;
            if (assignedTos.Count == 0)
            {
                customFilters.Add(new CamlFilter("AssignedTo", FieldTypes.Number, CamlFilterTypes.Equals, "[Me]"));
            }
            foreach (string filterValue in statuses)
            {
                customFilters.Add(new CamlFilter("Status", FieldTypes.Choice, CamlFilterTypes.NotEqual, filterValue));
            }
            foreach (string filterValue in priorities)
            {
                customFilters.Add(new CamlFilter("Priority", FieldTypes.Choice, CamlFilterTypes.NotEqual, filterValue));
            }

            int itemCount;
            string listItemCollectionPositionNext = String.Empty;

            List<IItem> items = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetListItems(siteSetting, taskListLocation.Folder, null, string.Empty, true, 0, string.Empty, customFilters, false, out listItemCollectionPositionNext, out itemCount);
            if (items.Count == 0)
            {
                Task emptyTask = new Task();
                emptyTask.Title = Languages.Translate("No items to display");
                tasks.Add(emptyTask);
            }
            foreach (IItem item in items)
            {
                Task task = new Task();
                task.SiteSettingID = siteSetting.ID;
                task.ID = item.GetID();
                task.Title = item.Title;
                if (item.Properties.ContainsKey("ows_Priority") == true)
                    task.Priority = item.Properties["ows_Priority"];
                if (item.Properties.ContainsKey("ows_Status") == true)
                    task.Status = item.Properties["ows_Status"];
                if (item.Properties.ContainsKey("ows_AssignedTo") == true)
                {
                    string assignedTo =item.Properties["ows_AssignedTo"];
                    if(string.IsNullOrEmpty(assignedTo) == false)
                    {
                        string[] assignedToArray = assignedTo.Split(new string[]{";#"}, StringSplitOptions.None);
                        task.AssignedToID = int.Parse(assignedToArray[0]);
                        task.AssignedTo = assignedToArray[1];
                    }
                }

                if (item.Properties.ContainsKey("ows_WorkflowLink") == true)
                {
                    string workflowLink = item.Properties["ows_WorkflowLink"];
                    if (string.IsNullOrEmpty(workflowLink) == false)
                    {
                        int titleStartIndex = workflowLink.IndexOf(",");
                        task.RelatedContentUrl = workflowLink.Substring(0, titleStartIndex);
                        task.RelatedContentTitle = workflowLink.Substring(titleStartIndex + 1);
                    }
                }
                

                task.ListUrl = taskListLocation.Folder.GetUrl();
                tasks.Add(task);
            }

            this.TasksListView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                CollectionViewSource cvs = new CollectionViewSource();
                cvs.Source = tasks;
                cvs.GroupDescriptions.Add(new PropertyGroupDescription("ListUrl"));

                this.TasksListView.ItemsSource = cvs.View;

                refreshImage.Visibility = System.Windows.Visibility.Visible;
                loadingImage.Visibility = System.Windows.Visibility.Collapsed;
            }));

            this.IsDataLoaded = true;
        }

        private void TasksListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TasksListView.SelectedItem == null)
                return;

            Task task = (Task)TasksListView.SelectedItem;
            if (string.IsNullOrEmpty(task.ID) == true)
                return;

            string url = task.ListUrl + "/DispForm.aspx?ID=" + task.ID + "&IsDlg=1";
            BrowserExplorer browserExplorer = new BrowserExplorer();
            browserExplorer.Initialize(this.SiteSetting, url);
            if (browserExplorer.ShowDialog(null, Languages.Translate("Edit Task"), 600, 800, false, false) == true)
            {
                this.RefreshControls(true);
            }
        }

        private void TasksListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (TasksListView.SelectedItem == null)
                return;

            Task task = (Task)TasksListView.SelectedItem;
            if (string.IsNullOrEmpty(task.ID) == true)
                return;

            ContextMenuManager.Instance.FillContextMenuItems(TasksListView.ContextMenu, this.SiteSetting, task, null, TaskListLocation.Folder);
        }
    }
}
