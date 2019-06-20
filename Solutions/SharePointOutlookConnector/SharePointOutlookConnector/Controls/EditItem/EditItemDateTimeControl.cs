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
using System.Globalization;

namespace Sobiens.Office.SharePointOutlookConnector.Controls.EditItem
{
    public partial class EditItemDateTimeControl : EditItemControl
    {
        public EditItemDateTimeControl()
        {
            InitializeComponent();
        }

        public override void InitializeControl(EUField field, EUFolder rootFolder, EUListSetting listSetting, EUListItem listItem)
        {
            base.InitializeControl(field, rootFolder, listSetting, listItem);
            string value = this.GetValueFromListItemOrDefault();
            //            DateTime dateValue;
            string pattern = "yyyy-MM-ddTHH:mm:ssZ";
            //            string pattern = "u";
            if (value != String.Empty)
            {
                dateTimePicker1.Value = DateTime.ParseExact(value, pattern, CultureInfo.InvariantCulture); ;
            }
            else
            {
                dateTimePicker1.Checked = false;
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
                dateTimePicker1.Enabled = false;
            else
                dateTimePicker1.Enabled = true;
        }

        public override string Value
        {
            get
            {
                if (dateTimePicker1.Checked == true)
                    return dateTimePicker1.Value.ToString("yyyy-MM-ddTHH:mm:ssZ");
                else
                    return String.Empty;
            }
        }

        public override bool IsValid
        {
            get
            {
                errorProvider1.Clear();
                if (Field.Required == true && dateTimePicker1.Checked == false)
                {
                    errorProvider1.SetError(dateTimePicker1, "You must specify a value for this required field.");
                    return false;
                }
                return true;
            }
        }

    }
}
