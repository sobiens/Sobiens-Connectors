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
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.Controls.EditItem;

namespace Sobiens.Office.SharePointOutlookConnector.Forms
{
    public partial class EmailMappingFieldSelectionForm : Form
    {
        private EUFieldCollection Fields { get; set; }
        public EUEmailFields SelectedField ;
        public EmailMappingFieldSelectionForm()
        {
            InitializeComponent();
        }

        public void InitializeForm()
        {
            foreach (EUEmailFields name in Enum.GetValues(typeof(EUEmailFields)))
            {
                if (name != EUEmailFields.BCC) // JOEL JEFFERY 20110714 - filter out pesky BCC fields
                    FieldsComboBox.Items.Add(name);
            }
            if (FieldsComboBox.Items.Count > 0)
                FieldsComboBox.SelectedIndex = 0;
        }


        private void OkButton_Click(object sender, EventArgs e)
        {
            if(FieldsComboBox.SelectedItem != null)
            {
                SelectedField = (EUEmailFields)FieldsComboBox.SelectedItem;
            }
            else
            {
//                SelectedField = null;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
