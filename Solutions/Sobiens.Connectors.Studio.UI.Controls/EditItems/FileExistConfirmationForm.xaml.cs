using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.Studio.UI.Controls.EditItems
{
    /// <summary>
    /// Interaction logic for FileExistConfirmationForm.xaml
    /// </summary>
    public partial class FileExistConfirmationForm : HostControl
    {
        public FileExistDialogResults FileExistDialogResult = FileExistDialogResults.NotSelected;
        public bool DoThisForNextConflicts = false;
        public string NewFileName { get; set; }

        public string ExistFilePath { get; private set; }

        public FileExistConfirmationForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(FileExistConfirmationForm_Loaded);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.SetControlValues();
        }

        void FileExistConfirmationForm_CancelButtonSelected(object sender, EventArgs e)
        {
            if (FileExistDialogResult == FileExistDialogResults.NotSelected)
            {
                SetVariablesAndClose(FileExistDialogResults.Skip);
            }
        }

        public void Initialize(string existFilePath)
        {
            this.ExistFilePath = existFilePath;
        }

        void FileExistConfirmationForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(FileExistConfirmationForm_Loaded);
            this.CancelButtonSelected += new EventHandler(FileExistConfirmationForm_CancelButtonSelected);
        }

        private void SetControlValues()
        {
            string name = this.ExistFilePath.Substring(this.ExistFilePath.LastIndexOf("/") + 1);

            Replace_FileLocationLabel.Content = "[Item] " + name;
            Replace_FileNameLabel.Content = name;

            FileNameTextBox.Text = name;

            DontCopy_FileLocationLabel.Content = this.ExistFilePath;
            DontCopy_FileNameLabel.Content = name;
        }

        private void SetVariablesAndClose(FileExistDialogResults fileExistDialogResult)
        {
            FileExistDialogResult = fileExistDialogResult;
            DoThisForNextConflicts = DoThisForNextConflictsCheckBox.IsChecked.Value;
            NewFileName = FileNameTextBox.Text;

            this.Close(true);
        }

        private void ReplaceButton_Click(object sender, RoutedEventArgs e)
        {
            SetVariablesAndClose(FileExistDialogResults.CopyAndReplace);
        }

        private void DontCopyButton_Click(object sender, RoutedEventArgs e)
        {
            SetVariablesAndClose(FileExistDialogResults.Cancel);
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            SetVariablesAndClose(FileExistDialogResults.Copy);
        }

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            SetVariablesAndClose(FileExistDialogResults.Skip);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SetVariablesAndClose(FileExistDialogResults.Cancel);
        }
    }
}
