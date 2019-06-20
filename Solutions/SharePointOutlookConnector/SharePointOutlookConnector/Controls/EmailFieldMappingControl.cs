using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL;

namespace Sobiens.Office.SharePointOutlookConnector.Controls
{
    public delegate void EmailFieldMappingControl_MappingChange(EUEmailFields selectedEmailMappingField);

    public partial class EmailFieldMappingControl : UserControl
    {
        public event EmailFieldMappingControl_MappingChange MappingChange;
        private bool PreventEventTrigering = false;
        private EUFieldCollection fields = null;
        public EUFieldCollection Fields
        {
            get
            {
                if (fields == null)
                    fields = SharePointManager.GetFields(RootFolder.SiteSetting, RootFolder.WebUrl, RootFolder.ListName);
                return fields;
            }
        }
        public EUFolder RootFolder { get; set; }
        public EUEmailFields _SelectedEmailMappingField = EUEmailFields.NotSelected;
        public EUEmailFields SelectedEmailMappingField
        {
            get
            {
                return _SelectedEmailMappingField;
            }
            set
            {
                _SelectedEmailMappingField = value;
                PreventEventTrigering = true;
                //switch (_SelectedEmailMappingField)
                //{
                //    //case EUEmailFields.BCC:
                //    //    setMappingField("BCC", false);
                //    //    break;
                //    case EUEmailFields.NotSelected:
                //        setMappingField("Not Selected", false);
                //        break;
                //    default:
                //        setMappingField(value.ToString(), true);
                //        break;
                //}

                if (_SelectedEmailMappingField != EUEmailFields.NotSelected)
                {
                    MappingFieldCheckBox.Text = _SelectedEmailMappingField.ToString();
                    MappingFieldCheckBox.Checked = true;
                    toolTip1.SetToolTip(MappingFieldCheckBox, _SelectedEmailMappingField.ToString());
                }
                else
                {
                    MappingFieldCheckBox.Text = "Not Selected";
                    MappingFieldCheckBox.Checked = false;
                    toolTip1.SetToolTip(MappingFieldCheckBox, "Not Selected");
                }
                if (MappingChange != null)
                    MappingChange(_SelectedEmailMappingField);
                PreventEventTrigering = false;
            }
        }

        /// <summary>
        /// Sets the mapping field.
        /// </summary>
        /// <param name="field">The field.</param>
        private void setMappingField(string field, bool check)
        {
            MappingFieldCheckBox.Text = field;
            MappingFieldCheckBox.Checked = check;
            toolTip1.SetToolTip(MappingFieldCheckBox, field);
        }

        public EmailFieldMappingControl()
        {
            InitializeComponent();
        }
        public void InitializeForm(EUField field, EUFolder rootFolder, EUListSetting listSetting, EUListItem listItem)
        {
            RootFolder = rootFolder;
            //Fields = fields;
        }

        private void MappingFieldCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (PreventEventTrigering == true)
                return;
            if (MappingFieldCheckBox.Checked == true)
            {
                EmailMappingFieldSelectionForm emailMappingFieldSelectionForm = new EmailMappingFieldSelectionForm();
                emailMappingFieldSelectionForm.InitializeForm();
                emailMappingFieldSelectionForm.ShowDialog();
                if (emailMappingFieldSelectionForm.DialogResult == DialogResult.OK)
                {
                    SelectedEmailMappingField = emailMappingFieldSelectionForm.SelectedField;
                }
            }
            else
            {
                SelectedEmailMappingField = EUEmailFields.NotSelected;
            }
        }
    }
}
