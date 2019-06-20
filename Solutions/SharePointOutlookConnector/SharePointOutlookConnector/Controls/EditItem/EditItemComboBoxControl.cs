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
using EmailUploader.BLL;

namespace Sobiens.Office.SharePointOutlookConnector.Controls.EditItem
{
    public delegate void LoadChoiceItemsHandler();

    public partial class EditItemComboBoxControl : EditItemControl
    {
        public EditItemComboBoxControl()
        {
            InitializeComponent();
        }

        private List<ChoiceDataItem> ChoiceItems = null;

        private void LoadChoiceItemsInvoke()
        {
            try
            {
                this.Invoke(new LoadChoiceItemsHandler(LoadChoiceItems), null);
            }
            catch (Exception ex)
            {
                LogManager.LogException("LoadChoiceItemsInvoke", ex);
            }
        }
        private void LoadChoiceItems()
        {
            string value = this.GetValueFromListItemOrDefault();
            if (Field.Type == EUFieldTypes.Lookup)
            {
                value = value.Split(new string[]{";#"}, StringSplitOptions.None)[0];
            }
            foreach (ChoiceDataItem dataItem in ChoiceItems)
            {
                comboBox1.Items.Add(dataItem);
                if (dataItem.Value == value)
                    comboBox1.SelectedItem = dataItem;
            }
            _IsLoaded = true;
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

        void emailFieldMappingControl1_MappingChange(EUEmailFields selectedEmailMappingField)
        {
            EmailMappingField = selectedEmailMappingField;
            if (selectedEmailMappingField != EUEmailFields.NotSelected)
                comboBox1.Enabled = false;
            else
                comboBox1.Enabled = true;
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
                if (comboBox1.SelectedItem == null)
                    return String.Empty;
                if(Field.Type == EUFieldTypes.Lookup)
                    return ((ChoiceDataItem)comboBox1.SelectedItem).Value+";#";
                else
                    return ((ChoiceDataItem)comboBox1.SelectedItem).Value;
            }
        }

        public override bool IsValid
        {
            get
            {
                errorProvider1.Clear();
                if (Field.Required == true && comboBox1.SelectedItem == null)
                {
                    errorProvider1.SetError(comboBox1, "You must specify a value for this required field.");
                    return false;
                }
                return true;
            }
        }

    }
}
