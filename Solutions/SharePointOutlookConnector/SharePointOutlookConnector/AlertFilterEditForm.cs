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
    public partial class AlertFilterEditForm : Form
    {
        private string webUrl = String.Empty;
        private string listName = String.Empty;
        private string listID = String.Empty;
        private EUAlert CurrentAlert = null;
        private EUCamlFilters OrGroup = null;
        public AlertFilterEditForm()
        {
            InitializeComponent();
        }

        private void AlertMaintenanceForm_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        public void Initialize(string _webUrl, string _listName, string _listID, EUCamlFilters orGroup, EUAlert _alert, List<EUField> fields)
        {
            webUrl = _webUrl;
            listName = _listName;
            listID = _listID;
            CurrentAlert = _alert;
            OrGroup = orGroup;
            FieldsComboBox.Items.Clear();
            foreach (EUField field in fields)
            {
                FieldsComboBox.Items.Add(field);
            }
            FieldsComboBox.SelectedIndex = 0;
            FieldsComboBox.Focus();
        }

        public void Initialize()
        {
            SelectedListNameLabel.Text = listName;

            foreach (EUCamlFilterTypes filterType in Enum.GetValues(typeof(EUCamlFilterTypes)))
            {
                OperationComboBox.Items.Add(filterType);
            }
            OperationComboBox.SelectedIndex = 0;
        }

        private void CancelSaveFilterButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void SaveFilterButton_Click(object sender, EventArgs e)
        {
            string fieldName = ((EUField)FieldsComboBox.SelectedItem).Name;
            EUCamlFilterTypes operationType = (EUCamlFilterTypes)OperationComboBox.SelectedItem;
            string value = FilterValueTextBox.Text;
            if (OrGroup == null)
            {
                OrGroup = new EUCamlFilters();
                CurrentAlert.OrGroups.Add(OrGroup);
            }
            OrGroup.Add(new EUCamlFilter(fieldName, EUFieldTypes.Text, operationType, false, value));
            this.DialogResult = DialogResult.OK;
        }

        private void FieldsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
