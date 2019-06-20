using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL;
using System.Diagnostics;

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class AlertMaintenanceForm : Form
    {
        private string webUrl = String.Empty;
        private EUSiteSetting siteSetting = null;
        public AlertMaintenanceForm()
        {
            InitializeComponent();
        }

        private void AlertMaintenanceForm_Load(object sender, EventArgs e)
        {
            Initialize();
        }


        public void Initialize(string _webUrl, EUSiteSetting _siteSetting)
        {
            webUrl = _webUrl;
            siteSetting= _siteSetting;
        }

        public void Initialize()
        {
            AlertsListView.Items.Clear();
            List<EUAlert> alerts = AlertManager.GetAlerts(siteSetting, webUrl);
            foreach (EUAlert alert in alerts)
            {
                ListViewItem item = AlertsListView.Items.Add(alert.Title.Replace("[SPOutlookConnector]",""));
                if(alert.AlertFrequency == "Immediate")
                    item.Group = AlertsListView.Groups["ImmediateListViewGroup"];
                else if(alert.AlertFrequency == "Daily")
                    item.Group = AlertsListView.Groups["DailyListViewGroup"];
                else if(alert.AlertFrequency == "Weekly")
                    item.Group = AlertsListView.Groups["WeeklyListViewGroup"];
                item.Tag = alert;
            }
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            AlertAddForm alertAddForm = new AlertAddForm();
            alertAddForm.Initialize(webUrl, siteSetting);
            alertAddForm.ShowDialog();
            if (alertAddForm.DialogResult == DialogResult.OK)
            {
                AlertEditForm alertEditForm = new AlertEditForm();
                EUAlert alert = new EUAlert();
                alert.Title = alertAddForm.listName + " Alert";
                alert.ListID = alertAddForm.listID;
                alertEditForm.Initialize(siteSetting,  webUrl, alertAddForm.listName, alertAddForm.listID, alert);
                alertEditForm.ShowDialog();
                if (alertEditForm.DialogResult == DialogResult.OK)
                {
                    Initialize();
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EUAlert alert = (EUAlert)AlertsListView.SelectedItems[0].Tag;
            if (alert.Title.IndexOf("[SPOutlookConnector]") == -1)
            {
                string alertLink = webUrl + "/_layouts/SubEdit.aspx?Alert={" + alert.ID + "}&List={" + alert.ListID + "}";
                Process.Start("IExplore.exe", alertLink);
            }
            else
            {
                AlertEditForm alertEditForm = new AlertEditForm();
                alertEditForm.Initialize(siteSetting, webUrl, alert.ListName, alert.ListID, alert);
                alertEditForm.ShowDialog();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EUAlert alert = (EUAlert)AlertsListView.SelectedItems[0].Tag;
            AlertManager.DeleteAlert(siteSetting, webUrl, alert.ID);
            AlertsListView.SelectedItems[0].Remove();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (AlertsListView.SelectedItems.Count > 0)
            {
                deleteToolStripMenuItem.Enabled = true;
                editToolStripMenuItem.Enabled = true;
            }
            else
            {
                deleteToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
            }
        }
    }
}
