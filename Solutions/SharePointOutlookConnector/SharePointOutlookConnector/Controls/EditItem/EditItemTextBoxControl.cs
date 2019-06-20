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
    public partial class EditItemTextBoxControl : EditItemControl
    {
        public EditItemTextBoxControl()
        {
            InitializeComponent();
        }

        public override void InitializeControl(EUField field, EUFolder rootFolder, EUListSetting listSetting, EUListItem listItem)
        {
            base.InitializeControl(field, rootFolder, listSetting, listItem);
            textBox1.Text = this.GetValueFromListItemOrDefault();
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

        void emailFieldMappingControl1_MappingChange(EUEmailFields selectedEmailMappingField)
        {
            EmailMappingField = selectedEmailMappingField;
            if (selectedEmailMappingField !=  EUEmailFields.NotSelected)
                textBox1.Enabled = false;
            else
                textBox1.Enabled = true;
        }

        public override string Value
        {
            get
            {
                return textBox1.Text;
            }
        }

        public override bool IsValid
        {
            get
            {
                errorProvider1.Clear();
                if (Field.Required == true && textBox1.Text == String.Empty)
                {
                    errorProvider1.SetError(textBox1, "You must specify a value for this required field.");
                    return false;
                }
                return true;
            }
        }
        public override void SetEmailMappingField()
        {
            emailFieldMappingControl1.SelectedEmailMappingField = EmailMappingField;
        }

    }
}
