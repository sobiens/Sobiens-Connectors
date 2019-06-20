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
using Sobiens.Connectors.Entities.Interfaces;
using System.Windows.Threading;
using System.Threading;

namespace Sobiens.Connectors.WPF.Controls
{
    /// <summary>
    /// Interaction logic for StatusBar.xaml
    /// </summary>
    public partial class StatusBar : UserControl, IStatusBar
    {
        public StatusBar()
        {
            InitializeComponent();
        }

        public void SetStatusBar(string text, int percentage)
        {
            StatusBarTextBlock.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                StatusBarTextBlock.Text = text;
                if (percentage == 100)
                {
                    StatusBarProgressBar.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    StatusBarProgressBar.Visibility = System.Windows.Visibility.Visible;
                    StatusBarProgressBar.Value = percentage;
                    StatusBarProgressBar.UpdateLayout();
                }
            }));
        }
    }
}
