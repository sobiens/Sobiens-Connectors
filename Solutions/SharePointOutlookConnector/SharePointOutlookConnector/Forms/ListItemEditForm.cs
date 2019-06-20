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
    public partial class ListItemEditForm : Form
    {
        private EUFolder Folder { get; set; }
        private EUListItem ListItem { get; set; }
        private EUListSetting ListSetting { get; set; }
        private List<EditItemControl> EditItemControls = new List<EditItemControl>();
        public EUFieldInformations FieldInformations = null;
        public ListItemEditForm()
        {
            InitializeComponent();
        }

        private void GenerateEditForm(EUFolder folder, EUListItem listItem, string contentTypeID)
        {
            List<EUField> editableFields = SharePointManager.GetContentType(folder.SiteSetting, folder.WebUrl, folder.ListName, contentTypeID).Fields.GetEditableFields();

            for (int i = EditControlsPanel.Controls.Count - 1; i > -1; i--)
            {
                if(EditControlsPanel.Controls[i].Tag is EUField)
                    EditControlsPanel.Controls.RemoveAt(i);
            }

            int height = 30;
            EditItemControls = new List<EditItemControl>();
            for (int i = 0; i < editableFields.Count; i++)
            {
                EUField field = editableFields[i];
                EditItemControl editControl = GetEditItemControl(field, folder, listItem);

                Label fieldNameLabel = new Label();
                fieldNameLabel.Text = field.DisplayName;
                fieldNameLabel.Location = new Point(10, height);
                fieldNameLabel.Tag = field;
                toolTip1.SetToolTip(fieldNameLabel, field.Description);
                EditControlsPanel.Controls.Add(fieldNameLabel);

                editControl.Location = new Point(200, height);
                editControl.Width = 400;
                editControl.Tag = field;
                toolTip1.SetToolTip(editControl, field.Description);
                EditControlsPanel.Controls.Add(editControl);
                height = height + editControl.Height +5;
                EditItemControls.Add(editControl);
            }
        }

        public void InitializeForm(EUFolder folder, EUListItem listItem)
        {
            Folder = folder;
            ListItem = listItem;

            ListSetting = EUSettingsManager.GetInstance().GetListSetting(folder.WebUrl.TrimEnd(new char[] { '/' }) + "/" + folder.FolderPath.TrimStart(new char[] { '/' }));
            
            List<EUContentType> contentTypes = SharePointManager.GetContentTypes(folder.SiteSetting, folder.WebUrl, folder.ListName);
            foreach (EUContentType contentType in contentTypes)
            {
                if(contentType.Name.ToLower() != "folder")
                    ContentTypeComboBox.Items.Add(contentType);
            }
            if (ContentTypeComboBox.Items.Count > 0)
            {
                if (listItem != null)
                {
                    for (int i = 0; i < ContentTypeComboBox.Items.Count; i++)
                    {
                        if (listItem.ContentTypeName == ((EUContentType)ContentTypeComboBox.Items[i]).Name)
                        {
                            ContentTypeComboBox.SelectedIndex = i;
                        }
                    }
                }
                if(ContentTypeComboBox.SelectedIndex<0)
                    ContentTypeComboBox.SelectedIndex = 0;
            }
            EUContentType selectedContentType = (EUContentType)ContentTypeComboBox.SelectedItem;
            if (ContentTypeComboBox.Items.Count == 1)
            {
                SelectedContentTypeLabel.Text = selectedContentType.Name;
                SelectedContentTypeLabel.Location= new Point(200,SelectedContentTypeLabel.Location.Y);
                ContentTypeComboBox.Visible = false;
            }
            else
            {
                SelectedContentTypeLabel.Visible = false;
            }
        }

        public bool IsValid
        {
            get
            {
                foreach (EditItemControl editItemControl in EditItemControls)
                {
                    if (editItemControl.IsValid == false)
                        return false;
                }
                return true;
            }
        }

        public bool IsLoaded
        {
            get
            {
                foreach (EditItemControl editItemControl in EditItemControls)
                {
                    if (editItemControl.IsLoaded == false)
                        return false;
                }
                return true;
            }
        }

        public EditItemControl GetEditItemControl(EUField field, EUFolder folder, EUListItem listItem)
        {
            EditItemControl editControl = new EditItemTextBoxControl();
            switch (field.Type)
            {
                case EUFieldTypes.Note:
                    if (field.RichText == false)
                        editControl = new EditItemMultiLineTextBoxControl();
                    else
                        editControl = new EditItemRichTextBoxControl();
                    break;
                case EUFieldTypes.Boolean:
                    editControl = new EditItemBooleanControl();
                    break;
                case EUFieldTypes.DateTime:
                    editControl = new EditItemDateTimeControl();
                    break;
                case EUFieldTypes.Number:
                    editControl = new EditItemNumberTextBoxControl();
                    break;
                case EUFieldTypes.Lookup:
                    if(field.Mult == true)
                        editControl = new EditItemCheckedListBoxControl();
                    else
                        editControl = new EditItemComboBoxControl();
                    break;
                case EUFieldTypes.Choice:
                    if(field.Mult == true)
                        editControl = new EditItemCheckedListBoxControl();
                    else
                        editControl = new EditItemComboBoxControl();
                    break;
                default:
                    break;
            }
            editControl.InitializeControl(field, folder, ListSetting, listItem);
            return editControl;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (IsLoaded == false)
            {
                MessageBox.Show("Please wait controls are being loaded...");
                return;
            }
            if (IsValid == true)
            {
                EUFieldInformations fieldInformations = new EUFieldInformations();
                fieldInformations.ContentType = (EUContentType)ContentTypeComboBox.SelectedItem;
                foreach (EditItemControl editItemControl in EditItemControls)
                {
                    EUFieldInformation fieldInfo = new EUFieldInformation();
                    fieldInfo.Id = editItemControl.Field.ID;
                    fieldInfo.InternalName = editItemControl.Field.Name;
                    fieldInfo.DisplayName = editItemControl.Field.DisplayName;
                    fieldInfo.Value = editItemControl.Value;
                    fieldInfo.EmailField = editItemControl.EmailMappingField;
                    switch (editItemControl.Field.Type)
                    {
                        case EUFieldTypes.Note:
                            fieldInfo.Type = Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.FieldType.Note;
                            break;
                        case EUFieldTypes.Text:
                            fieldInfo.Type = Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.FieldType.Text;
                            fieldInfo.Value = (editItemControl.Value.Length > editItemControl.Field.MaxLength ? editItemControl.Value.Substring(0, editItemControl.Field.MaxLength) : editItemControl.Value);
                            break;
                        case EUFieldTypes.DateTime:
                            fieldInfo.Type = Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.FieldType.DateTime;
                            break;
                        case EUFieldTypes.Lookup:
                            fieldInfo.Type = Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.FieldType.Lookup;
                            break;
                        case EUFieldTypes.Choice:
                            fieldInfo.Type = Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.FieldType.Choice;
                            break;
                    }
                    fieldInformations.Add(fieldInfo);
                }
                FieldInformations = fieldInformations;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void ContentTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EUContentType contentType = (EUContentType)ContentTypeComboBox.SelectedItem;
            GenerateEditForm(Folder, ListItem, contentType.ID);
        }
    }
}
