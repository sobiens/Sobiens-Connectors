using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.CRM;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for QueryResultControl.xaml
    /// </summary>
    public partial class WorkflowCopyForm : HostControl
    {
        public string WorkflowName = null;
        public Guid WorkflowId = Guid.Empty;
        public Folder SourceFolder = null;
        public Folder TargetFolder = null;
        public ISiteSetting SourceSiteSetting = null;
        public ISiteSetting TargetSiteSetting = null;

        public void Initialize(ISiteSetting siteSetting, string workflowName, Guid workflowId, Folder folder)
        {
            SourceSiteSetting = siteSetting;
            WorkflowName = workflowName;
            WorkflowId = workflowId;
            SourceFolder = folder;
        }

        public WorkflowCopyForm()
        {
            InitializeComponent();
        }

        private void SourceSelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectEntityForm selectEntityForm = new SelectEntityForm();
            selectEntityForm.Initialize(new Type[] { typeof(SPWeb) });

            if (selectEntityForm.ShowDialog(null, "Select an object to copy to") == true)
            {
                TargetFolder = selectEntityForm.SelectedObject;
                TargetSiteSetting = ApplicationContext.Current.GetSiteSetting(TargetFolder.SiteSettingID);
                List<Folder> folders = ApplicationContext.Current.GetSubFolders(TargetSiteSetting, TargetFolder, null, string.Empty);

                AssociatedListComboBox.Items.Clear();
                AssociatedListComboBox.DisplayMemberPath = "Title";
                AssociatedListComboBox.ItemsSource = folders;

                TaskListComboBox.Items.Clear();
                TaskListComboBox.DisplayMemberPath = "Title";
                TaskListComboBox.ItemsSource = folders;

                HistoryListComboBox.Items.Clear();
                HistoryListComboBox.DisplayMemberPath = "Title";
                HistoryListComboBox.ItemsSource = folders;

            }
        }




        protected override void OnLoad()
        {
            base.OnLoad();
            this.InitializeForm();
        }

        private void InitializeForm()
        {
            this.OKButtonSelected += WorkflowCopyForm_OKButtonSelected; ;
        }

        private void WorkflowCopyForm_OKButtonSelected(object sender, EventArgs e)
        {
            Folder associatedList = (Folder)AssociatedListComboBox.SelectedItem;
            Folder taskList = (Folder)TaskListComboBox.SelectedItem;
            Folder historyList = (Folder)HistoryListComboBox.SelectedItem;
            this.IsValid = false;
            if(associatedList == null)
            {
                MessageBox.Show("Please select associated list!");
                return;
            }
            if (taskList == null)
            {
                MessageBox.Show("Please select task list!");
                return;
            }
            if (historyList == null)
            {
                MessageBox.Show("Please select history list!");
                return;
            }

            Services.SharePoint.SharePointService.CopyWorkflow(SourceSiteSetting, new Guid(SourceFolder.UniqueIdentifier), WorkflowId, TargetSiteSetting, new Guid(associatedList.UniqueIdentifier), new Guid(taskList.UniqueIdentifier), new Guid(historyList.UniqueIdentifier));
            this.IsValid = true;

        }
    }
}
