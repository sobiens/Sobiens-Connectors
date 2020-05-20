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

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for HostWindow.xaml
    /// </summary>
    public partial class HostWindow : Window
    {
        public HostWindow()
        {
            InitializeComponent();
            //this.Resources.MergedDictionaries.Add(Sobiens.Connectors.Entities.Languages.Dict);
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
            MainGrid.RowDefinitions.Clear();

            if (this.ShowLogo == false)
            {
                TopGrid.Visibility = System.Windows.Visibility.Collapsed;
                MainPanel.SetValue(Grid.RowProperty, 0);
            }
            else
            {
                MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(55, GridUnitType.Pixel) });
                TopGrid.SetValue(Grid.RowProperty, 0);
                MainPanel.SetValue(Grid.RowProperty, 1);
            }
            MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100, GridUnitType.Star) });

            if (this.ShowActionButtons == false)
            {
                BottomGrid.Visibility = System.Windows.Visibility.Collapsed;
                OKButton.Visibility = Visibility.Hidden;
                CancelButton.Visibility = Visibility.Hidden;
            }
            else
            {
                MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(70, GridUnitType.Pixel) });
                BottomGrid.SetValue(Grid.RowProperty, (this.ShowLogo == false ? 1 : 2));
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
            BottomGrid.Visibility = System.Windows.Visibility.Visible;
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
            BottomGrid.Visibility = System.Windows.Visibility.Visible;
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
                try
                {
                    this.DialogResult = true;
                }
                catch (Exception ex) { }
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
            control.VerticalContentAlignment = VerticalAlignment.Stretch;
            control.HorizontalContentAlignment = HorizontalAlignment.Stretch ;
            control.VerticalAlignment = VerticalAlignment.Stretch;
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            double height = this.Height;
            if (this.Height == 0)
                height = SystemParameters.WorkArea.Height;

            if (this.ShowLogo == true) {
                height = height - 55;
            }

            if (this.ShowActionButtons == false)
            {
                height = height - 70;
            }
            control.Height = height;

            this.MainPanel.Children.Add(control);
        }

    }
}
