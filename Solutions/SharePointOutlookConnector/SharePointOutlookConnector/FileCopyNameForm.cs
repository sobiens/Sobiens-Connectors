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

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class FileCopyNameForm : Form
    {
        public FileCopyNameForm()
        {
            InitializeComponent();
        }
        private ISPCItem SelectedListItem;
        private ISPCFolder SelectedFolder;
        public void Initialize(ISPCItem selectedListItem, ISPCFolder selectedFolder)
        {
            SelectedListItem = selectedListItem;
            SelectedFolder = selectedFolder;
            FileNameTextBox.Text = selectedListItem.Title;
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            ErrorLabel.Visible = false;

            string newFileName = FileNameTextBox.Text;
            IOutlookConnector connector = OutlookConnector.GetConnector(SelectedFolder.SiteSetting);

            if (connector.CheckFileExistency(SelectedFolder, SelectedListItem, newFileName) == true)
            {
                errorProvider1.SetError(FileNameTextBox, "There is a file with the same name already!");
                ErrorLabel.Visible = true;
                return;
            }
            connector.CopyFile(SelectedFolder, SelectedListItem, newFileName);
            this.DialogResult = DialogResult.OK;
        }
    }
}
