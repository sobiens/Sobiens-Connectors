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
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities.Workflows;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.WPF.Controls.Workflow
{
    /// <summary>
    /// Interaction logic for SiteSettingForm.xaml
    /// </summary>
    public partial class ItemWorkflowForm : HostControl
    {
        public ItemWorkflowForm()
        {
            InitializeComponent();
        }

        private ISiteSetting SiteSetting { get; set; }
        private IItem Item { get; set; }
        private WorkflowData WorkflowData { get; set; }

        public void Initialize(ISiteSetting siteSetting, IItem item)
        {
            this.SiteSetting = siteSetting;
            this.Item = item;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.InitializeForm();
        }

        private void InitializeForm()
        {
            this.WorkflowData = ServiceManagerFactory.GetServiceManager(this.SiteSetting.SiteSettingType).GetWorkflowDataForItem(this.SiteSetting, this.Item);
            WorkflowListView.ItemsSource = this.WorkflowData.TemplateData.WorkflowTemplates;

            CollectionViewSource cvs = new CollectionViewSource();
            cvs.Source = this.WorkflowData.ActiveWorkflowsData.Workflows;
            cvs.GroupDescriptions.Add(new PropertyGroupDescription("ActivenessGroupName"));
            ActiveWorkflowsListView.ItemsSource = cvs.View;
        }

        private bool CanTheWorkflowBeInitiated(string templateId)
        {
            var query = from wf in this.WorkflowData.ActiveWorkflowsData.Workflows
                        where wf.TemplateId.Equals(templateId, StringComparison.InvariantCultureIgnoreCase) == true
                        && wf.InternalState == "2"
                        select wf;

            return query.Count() > 0 ? false : true;
        }

        private void WorkflowListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WorkflowTemplate workflow = (WorkflowTemplate)WorkflowListView.SelectedItem;
            if (CanTheWorkflowBeInitiated(workflow.TemplateId) == false)
            {
                MessageBox.Show(Languages.Translate("There is already a workflow instance running with the same workflow template"));
                return;
            }


            ServiceManagerFactory.GetServiceManager(this.SiteSetting.SiteSettingType).StartWorkflow(this.SiteSetting, this.Item, workflow);
        }
    }
}
