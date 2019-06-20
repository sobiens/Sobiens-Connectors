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

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class AlertAddForm : Form
    {
        public EUSiteSetting siteSetting = null;
        public string webUrl = String.Empty;
        public string listName = String.Empty;
        public string listID = String.Empty;
        public AlertAddForm()
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
            siteSetting = _siteSetting;
        }

        public void Initialize()
        {
            List<EUList> lists = SharePointManager.GetLists(webUrl, siteSetting);
            foreach (EUList list in lists)
            {
                ListsComboBox.Items.Add(list);
            }
            if (ListsComboBox.Items.Count > -1)
                ListsComboBox.SelectedIndex = 0;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (ListsComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a list");
                return;
            }
            listName = ((EUList)ListsComboBox.SelectedItem).Title;
            listID = ((EUList)ListsComboBox.SelectedItem).ID;
            this.DialogResult = DialogResult.OK;
        }
    }
}
