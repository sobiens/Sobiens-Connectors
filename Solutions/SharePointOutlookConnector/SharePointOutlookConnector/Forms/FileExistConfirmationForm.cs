using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector.Forms
{
    public partial class FileExistConfirmationForm : Form
    {
        public FileExistDialogResults FileExistDialogResult = FileExistDialogResults.NotSelected;
        public bool DoThisForNextConflicts = false;
        public string NewFileName { get; set; }

        private void SetVariablesAndClose(FileExistDialogResults fileExistDialogResult)
        {
            FileExistDialogResult = fileExistDialogResult;
            DoThisForNextConflicts = DoThisForNextConflictsCheckBox.Checked;
            NewFileName = FileNameTextBox.Text;
            
            this.Close();
        }

        public FileExistConfirmationForm()
        {
            InitializeComponent();
        }

        public FileExistConfirmationForm(string existFilePath)
        {
            InitializeComponent();

            string name = existFilePath.Substring(existFilePath.LastIndexOf("/")+1);

            Replace_FileLocationLabel.Text = "[Outlook Item] " + name;
            Replace_FileNameLabel.Text = name;

            FileNameTextBox.Text = name;

            DontCopy_FileLocationLabel.Text = existFilePath;
            DontCopy_FileNameLabel.Text = name;
        }

        private void ReplaceButton_Click(object sender, EventArgs e)
        {
            SetVariablesAndClose(FileExistDialogResults.CopyAndReplace);
        }

        private void DontCopyButton_Click(object sender, EventArgs e)
        {
            SetVariablesAndClose(FileExistDialogResults.Cancel);
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            SetVariablesAndClose(FileExistDialogResults.Copy);
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            SetVariablesAndClose(FileExistDialogResults.Skip);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            SetVariablesAndClose(FileExistDialogResults.Cancel);
        }

        private void FileExistConfirmationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FileExistDialogResult == FileExistDialogResults.NotSelected)
            {
                SetVariablesAndClose(FileExistDialogResults.Skip);
            }
        }
    }
}
