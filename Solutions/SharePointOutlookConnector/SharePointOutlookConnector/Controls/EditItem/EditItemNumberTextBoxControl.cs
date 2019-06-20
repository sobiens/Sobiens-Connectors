using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector.Controls.EditItem
{
    public partial class EditItemNumberTextBoxControl : EditItemControl
    {
        public EditItemNumberTextBoxControl()
        {
            InitializeComponent();
        }

        public override void InitializeControl(EUField field, EUFolder rootFolder, EUListSetting listSetting, EUListItem listItem)
        {
            base.InitializeControl(field, rootFolder, listSetting, listItem);
            numericUpDown1.DecimalPlaces = field.Decimals;
            numericUpDown1.Minimum = field.Min;
            numericUpDown1.Maximum = field.Max;
            numericUpDown1.CausesValidation = false;
            string value = this.GetValueFromListItemOrDefault();
            decimal decimalValue;
            if (decimal.TryParse(value, out decimalValue) == true)
            {
                try
                {
                    numericUpDown1.Value = decimalValue;
                }
                catch (Exception ex) { }
            }
            if (listItem == null)
            {
                emailFieldMappingControl1.InitializeForm(field, rootFolder, listSetting, listItem);
                emailFieldMappingControl1.MappingChange += new EmailFieldMappingControl_MappingChange(emailFieldMappingControl1_MappingChange);
            }
            else
            {
                emailFieldMappingControl1.Visible = false;
            }
            SetEmailMappingField();
            _IsLoaded = true;
        }
        public override void SetEmailMappingField()
        {
            emailFieldMappingControl1.SelectedEmailMappingField = EmailMappingField;
        }

        void emailFieldMappingControl1_MappingChange(EUEmailFields selectedEmailMappingField)
        {
            EmailMappingField = selectedEmailMappingField;
            if (selectedEmailMappingField != EUEmailFields.NotSelected)
                numericUpDown1.Enabled = false;
            else
                numericUpDown1.Enabled = true;
        }

        public override string Value
        {
            get
            {
                return numericUpDown1.Value.ToString();
            }
        }

        public override bool IsValid
        {
            get
            {
                errorProvider1.Clear();
                /*
                if (Field.Required == true && numericUpDown1.Value == String.Empty)
                {
                    errorProvider1.SetError(textBox1, "You must specify a value for this required field.");
                    return false;
                }
                 */ 
                return true;
            }
        }

    }
}
