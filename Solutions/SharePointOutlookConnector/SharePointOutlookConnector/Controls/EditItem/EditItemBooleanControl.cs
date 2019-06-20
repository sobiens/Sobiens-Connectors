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
    public partial class EditItemBooleanControl : EditItemControl
    {
        public EditItemBooleanControl()
        {
            InitializeComponent();
        }

        public override void InitializeControl(EUField field, EUFolder rootFolder, EUListSetting listSetting, EUListItem listItem)
        {
            base.InitializeControl(field, rootFolder, listSetting, listItem);
            string value = this.GetValueFromListItemOrDefault();
            bool booleanValue;
            if (bool.TryParse(value, out booleanValue) == true)
            {
                checkBox1.Checked = booleanValue;
            }
            else if (value == "1")
            {
                checkBox1.Checked = true;
            }
            else if (value == "0")
            {
                checkBox1.Checked = false;
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
            emailFieldMappingControl1.SelectedEmailMappingField = EUEmailFields.NotSelected;
            emailFieldMappingControl1.Visible = false;
//            emailFieldMappingControl1.SelectedEmailMappingField = EmailMappingField;
        }

        void emailFieldMappingControl1_MappingChange(EUEmailFields selectedEmailMappingField)
        {
            EmailMappingField = selectedEmailMappingField;
            if (selectedEmailMappingField != EUEmailFields.NotSelected)
                checkBox1.Enabled = false;
            else
                checkBox1.Enabled = true;
        }

        public override string Value
        {
            get
            {
                return (checkBox1.Checked==true?"1":"0");
            }
        }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

    }
}
