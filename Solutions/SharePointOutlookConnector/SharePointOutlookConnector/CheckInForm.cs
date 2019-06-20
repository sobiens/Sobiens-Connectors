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

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class CheckInForm : Form
    {
        public EUListItem SelectedItem = null;
        public CheckInForm()
        {
            InitializeComponent();
        }

        private void SetControlEnability()
        {
            if (PublishRadioButton.Checked == true)
            {
                YesRadioButton.Enabled = false;
                NoRadioButton.Enabled = false;
                NoRadioButton.Checked = true;
            }
            else
            {
                YesRadioButton.Enabled = true;
                NoRadioButton.Enabled = true;
            }
        }

        private void DraftRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetControlEnability();
        }

        private void PublishRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetControlEnability();
        }

        private void OverwriteRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetControlEnability();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            EUCheckinTypes checkinType;
            if (DraftRadioButton.Checked == true)
            {
                checkinType = EUCheckinTypes.MinorCheckIn;
            }
            else if (PublishRadioButton.Checked == true)
            {
                checkinType = EUCheckinTypes.MajorCheckIn;
            }
            else
            {
                checkinType = EUCheckinTypes.OverwriteCheckIn;
            }

            SharePointManager.CheckInFile(SelectedItem.SiteSetting, SelectedItem.WebURL, SelectedItem.URL, CommentsTextBox.Text, checkinType);
            if (YesRadioButton.Checked == true)
            {
                SharePointManager.CheckOutFile(SelectedItem.SiteSetting, SelectedItem.WebURL, SelectedItem.URL);
            }
            DialogResult = DialogResult.OK;
        }

        private void CheckInForm_Load(object sender, EventArgs e)
        {
            DraftRadioButton.Text = SelectedItem.GetMajorVersion() + "." + (SelectedItem.GetMinorVersion() + 1) + " Minor version (draft)";
            PublishRadioButton.Text = (SelectedItem.GetMajorVersion()+1) + ".0 Major version (publish)";
            OverwriteRadioButton.Text = SelectedItem.GetMajorVersion() + "." + SelectedItem.GetMinorVersion() + " Overwrite the current minor version";
        }
    }
}
