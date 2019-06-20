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
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for SiteSettingForm.xaml
    /// </summary>
    public partial class ProjectLoadForm : HostControl
    {
        public ProjectLoadForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.InitializeForm();
        }

        private void InitializeForm()
        {
            this.OKButtonSelected +=new EventHandler(SiteSettingForm_OKButtonSelected);
            List<QueryProjectObject> queryProjects = ConfigurationManager.GetInstance().GetProjects();
            foreach(QueryProjectObject queryProject in queryProjects){
                ProjectsListBox.Items.Add(queryProject);
            }
        }

        private void SiteSettingForm_OKButtonSelected(object sender, EventArgs e)
        {
            if (ProjectsListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a project to load.");
                return;
            }
            this.Tag = ProjectsListBox.SelectedItem;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            ((Window)this.Parent).DialogResult = false;
        }
    }
}
