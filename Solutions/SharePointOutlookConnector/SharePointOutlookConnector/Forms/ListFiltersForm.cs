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
    public partial class ListFiltersForm : Form
    {
        private EUFieldCollection Fields { get; set; }
        public EUCamlFilters CustomFilters = new EUCamlFilters();
        public ListFiltersForm()
        {
            InitializeComponent();
        }

        private void BindGrid(EUCamlFilters filters)
        {
            ListFiltersDataGridView.Rows.Clear();
            foreach (EUCamlFilter filter in CustomFilters)
            {
                int index = ListFiltersDataGridView.Rows.Add();
                ListFiltersDataGridView.Rows[index].Tag = filter;
                ListFiltersDataGridView.Rows[index].Cells["FieldNameColumn"].Value = filter.FieldName;
                ListFiltersDataGridView.Rows[index].Cells["FieldTypeColumn"].Value = filter.FieldType;
                ListFiltersDataGridView.Rows[index].Cells["FilterTypeColumn"].Value = filter.FilterType;
                ListFiltersDataGridView.Rows[index].Cells["FilterValueColumn"].Value = filter.FilterValue;
            }
        }

        public void InitializeForm(EUFieldCollection fields, EUCamlFilters filters)
        {
            Fields = fields;
            foreach (EUField field in Fields)
            {
                if (field.Type == EUFieldTypes.Choice
                    || field.Type == EUFieldTypes.Lookup
                    || field.Type == EUFieldTypes.Number
                    || field.Type == EUFieldTypes.Text)
                {
                    FieldsComboBox.Items.Add(field);
                }
            }
            if (FieldsComboBox.Items.Count > 0)
                FieldsComboBox.SelectedIndex = 0;
            if (filters != null)
            {
                CustomFilters = filters.Clone();
            }
            BindGrid(CustomFilters);
        }


        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void AddFilterButton_Click(object sender, EventArgs e)
        {
            string fieldName = ((EUField)FieldsComboBox.SelectedItem).Name;
            string filterValue = FilterValueTextBox.Text;
            EUCamlFilter filter = new EUCamlFilter(fieldName, EUFieldTypes.Text, EUCamlFilterTypes.Equals, false, filterValue);
            CustomFilters.Add(filter);
            BindGrid(CustomFilters);
        }

        private void DeleteFilterButton_Click(object sender, EventArgs e)
        {
            if (ListFiltersDataGridView.SelectedRows.Count > 0)
            {
                EUCamlFilter filter = (EUCamlFilter)ListFiltersDataGridView.SelectedRows[0].Tag;
                CustomFilters.Remove(filter);
                BindGrid(CustomFilters);
            }
        }
    }
}
