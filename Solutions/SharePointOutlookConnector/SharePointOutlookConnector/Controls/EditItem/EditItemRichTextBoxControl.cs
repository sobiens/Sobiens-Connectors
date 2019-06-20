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
    public partial class EditItemRichTextBoxControl : EditItemControl
    {
        public EditItemRichTextBoxControl()
        {
            InitializeComponent();
        }

        public override void InitializeControl(EUField field, EUFolder rootFolder, EUListSetting listSetting, EUListItem listItem)
        {
            base.InitializeControl(field, rootFolder, listSetting, listItem);
            this.Height = field.NumLines * 15;
            richTextBox1.Text = this.GetValueFromListItemOrDefault().Replace("  ", Environment.NewLine);
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
                richTextBox1.Enabled = false;
            else
                richTextBox1.Enabled = true;
        }

        public override string Value
        {
            get
            {
                return richTextBox1.Text;
            }
        }

        public override bool IsValid
        {
            get
            {
                errorProvider1.Clear();
                if (Field.Required == true && richTextBox1.Text == String.Empty)
                {
                    errorProvider1.SetError(richTextBox1, "You must specify a value for this required field.");
                    return false;
                }
                return true;
            }
        }

    }
}
