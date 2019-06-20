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
using Sobiens.Office.SharePointOutlookConnector.BLL;

namespace Sobiens.Office.SharePointOutlookConnector.Controls.EditItem
{
    public partial class EditItemCheckedListBoxControl : EditItemControl
    {
        public EditItemCheckedListBoxControl()
        {
            InitializeComponent();
        }

        private List<ChoiceDataItem> ChoiceItems = null;

        private void LoadChoiceItemsInvoke()
        {
            this.Invoke(new LoadChoiceItemsHandler(LoadChoiceItems), null);
        }

        private void LoadChoiceItems()
        {
            string value = this.GetValueFromListItemOrDefault();
            List<string> selectedValues = new List<string>();
            if (Field.Type == EUFieldTypes.Lookup)
            {
                string[] values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

                for (int i = 0; i < values.Length; i = i + 2)
                {
                    selectedValues.Add(values[i]);
                }
            }
            else
            {
                string[] values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

                for (int i = 0; i < values.Length; i++)
                {
                    if(values[i] != String.Empty)
                        selectedValues.Add(values[i]);
                }
            }

            foreach (ChoiceDataItem dataItem in ChoiceItems)
            {
                bool isChecked = false;
                if (selectedValues.Contains(dataItem.Value) == true)
                    isChecked = true;
                checkedListBox1.Items.Add(dataItem, isChecked);
            }
            _IsLoaded = true;
        }

        public override void InitializeControl(EUField field, EUFolder rootFolder, EUListSetting listSetting, EUListItem listItem)
        {
            base.InitializeControl(field, rootFolder, listSetting, listItem);

            if (field.Type == EUFieldTypes.Choice)
            {
                ChoiceItems = field.ChoiceItems;
                LoadChoiceItems();
            }
            else if (field.Type == EUFieldTypes.Lookup)
            {
                backgroundWorker1.RunWorkerAsync();
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
        }

        public override void SetEmailMappingField()
        {
            if (Field.Type == EUFieldTypes.Lookup)
            {
                emailFieldMappingControl1.SelectedEmailMappingField = EUEmailFields.NotSelected;
                emailFieldMappingControl1.Visible = false;
            }
            else
            {
                emailFieldMappingControl1.SelectedEmailMappingField = EmailMappingField;
            }
        }

        void emailFieldMappingControl1_MappingChange(EUEmailFields selectedEmailMappingField)
        {
            EmailMappingField = selectedEmailMappingField;
            if (selectedEmailMappingField != EUEmailFields.NotSelected )
                checkedListBox1.Enabled = false;
            else
                checkedListBox1.Enabled = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ChoiceItems = SharePointManager.GetListItems(RootFolder.SiteSetting, Field.ShowField, RootFolder.WebUrl, Field.List.ToString());
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadChoiceItemsInvoke();
        }

        public override string Value
        {
            get
            {
                string value = String.Empty;
                foreach (object item in checkedListBox1.CheckedItems)
                {
                    value += ((ChoiceDataItem)item).Value + ";#;#";
                }
                return value;
            }
        }

        public override bool IsValid
        {
            get
            {
                errorProvider1.Clear();
                if (Field.Required == true && checkedListBox1.CheckedItems.Count == 0)
                {
                    errorProvider1.SetError(checkedListBox1, "You must specify a value for this required field.");
                    return false;
                }
                return true;
            }
        }
    }
}
