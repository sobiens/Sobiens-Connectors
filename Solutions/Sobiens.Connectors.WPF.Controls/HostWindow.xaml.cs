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
using System.Windows.Shapes;

namespace Sobiens.Connectors.WPF.Controls
{
    /// <summary>
    /// Interaction logic for HostWindow.xaml
    /// </summary>
    public partial class HostWindow : Window
    {
        public HostWindow()
        {
            InitializeComponent();
            this.Resources.MergedDictionaries.Add(Sobiens.Connectors.Entities.Languages.Dict);
        }

        private HostControl HostControl
        {
            get{
                return ((HostControl)this.MainPanel.Children[0]);
            }
        }

        public bool ShowActionButtons
        {
            get;
            set;
        }

        public bool ShowLogo
        {
            get;
            set;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (this.ShowActionButtons == false)
            {
                BottomGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (this.ShowLogo == false)
            {
                TopGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public event EventHandler OKButtonSelected;
        public event EventHandler CancelButtonSelected;
        public void ShowLoadingStatus()
        {
            this.ShowLoadingStatus(string.Empty);
        }
        public void ShowLoadingStatus(string message)
        {
            LoadingImage.Visibility = System.Windows.Visibility.Visible;
            LoadingImage.UpdateLayout();
            this.SetStatusText(message);
        }
        public void HideLoadingStatus()
        {
            this.HideLoadingStatus(string.Empty);
        }
        public void HideLoadingStatus(string message)
        {
            LoadingImage.Visibility = System.Windows.Visibility.Hidden;
            this.SetStatusText(message);
        }

        public void SetStatusText(string message)
        {
            StatusTextBox.Content = message;
            StatusTextBox.Foreground = Brushes.Black;
            StatusTextBox.UpdateLayout();
        }

        public void SetErrorMessage(string message)
        {
            StatusTextBox.Content = message;
            StatusTextBox.Foreground = Brushes.Red;
            StatusTextBox.UpdateLayout();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (OKButtonSelected != null)
            {
                OKButtonSelected(sender, e);
            }

            if (this.HostControl.IsValid == true)
            {
                this.DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (CancelButtonSelected != null)
            {
                CancelButtonSelected(sender, e);
            }

            this.DialogResult = false;
        }

        public void SetHostControl(UserControl control)
        {
            this.MainPanel.Children.Clear();
            this.MainPanel.Children.Add(control);
        }

    }
}
