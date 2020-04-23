using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }
        private Action ActionEvent { get; set; }
        private bool IsOpened = false;
        private static LoadingWindow _Instance = null;
        private static LoadingWindow Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new LoadingWindow();

                return _Instance;
            }
        }
        public static void SetMessage(string message)
        {
            Instance.Dispatcher.Invoke((Action)(() =>
            {
                Instance.label1.Content = message;
            }));
        }

        public void ExecuteAction(string message, Action actionEvent)
        {
            this.ActionEvent = actionEvent;
            SetMessage(message);
        }

        public static void ShowDialog(string message, Action actionEvent)
        {
            try
            {
                Instance.ExecuteAction(message, actionEvent);
                Instance.IsOpened = false;
                Instance.ShowDialog();
            }
            catch (Exception ex) {
                int x = 3;
            }
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (this.IsOpened == true)
                return;

            this.IsOpened = true;
            Thread thread = new Thread(new ThreadStart(ThreadStartMethod));
            thread.Start();
        }

        private void ThreadStartMethod()
        {
            try
            {
                this.ActionEvent();
            }
            catch(Exception ex)
            {

            }
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.Close();
            }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}
