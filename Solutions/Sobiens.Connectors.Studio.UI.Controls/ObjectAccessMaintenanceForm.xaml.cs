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
    public partial class ObjectAccessMaintenanceForm : HostControl
    {
        private string ObjectType = string.Empty;
        private string ObjectName = string.Empty;
        private string ObjectId = string.Empty;
        private ISiteSetting SiteSetting = null;
        public ObjectAccessMaintenanceForm()
        {
            InitializeComponent();
        }

        public void Initialize(ISiteSetting siteSetting, string objectName, string objectType, string objectId)
        {
            this.SiteSetting = siteSetting;
            this.ObjectName = objectName;
            this.ObjectType = objectType;
            this.ObjectId = objectId;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.InitializeForm();
        }

        private void InitializeForm()
        {
            TypeTextBox.Text = this.ObjectType;
            NameTextBox.Text = this.ObjectName;

            try
            {
                Services.CRM.CRMService crmService = new Services.CRM.CRMService();
                string[] values = crmService.GetCurrentUserDetails(SiteSetting);
                Guid userId = new Guid(values[0]);
                //crmService.GetUserObjectAccessDetails(SiteSetting, new Guid(ObjectId), userId);
                crmService.GrantUserObjectAccess(SiteSetting, new Guid(ObjectId), "userquery", userId, 135069719);
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occured:" + ex.Message);
                Logger.Error(ex, "GrantUserObjectAccess");
            }

            this.OKButtonSelected +=new EventHandler(SiteSettingForm_OKButtonSelected);
        }

        private void SiteSettingForm_OKButtonSelected(object sender, EventArgs e)
        {
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            ((Window)this.Parent).DialogResult = false;
        }
    }
}
