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

namespace Sobiens.Connectors.WPF.Controls.EditItems
{
    /// <summary>
    /// Interaction logic for FileExistConfirmationForm.xaml
    /// </summary>
    public partial class FileCopyNameForm : HostControl
    {
        public bool ShowFileExistsErrorMessage = false;
        public string NewFileName = string.Empty;

        public FileCopyNameForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(FileExistConfirmationForm_Loaded);
        }


        public void Initialize(bool showFileExistsErrorMessage, string newFileName)
        {
            this.ShowFileExistsErrorMessage = showFileExistsErrorMessage;
            this.NewFileName = newFileName;
        }

        void FileExistConfirmationForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(FileExistConfirmationForm_Loaded);
            ErrorLabel.Visibility = this.ShowFileExistsErrorMessage == true ? Visibility.Visible : Visibility.Hidden;
            FileNameTextBox.Text = this.NewFileName;
            this.OKButtonSelected += new EventHandler(FileCopyNameForm_OKButtonSelected);
        }

        void FileCopyNameForm_OKButtonSelected(object sender, EventArgs e)
        {
            this.NewFileName = FileNameTextBox.Text;
        }

    }
}
