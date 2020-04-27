using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

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

        public void _SetMessage(int percentage, string message)
        {
            label1.Content = message;
            //label1.InvalidateVisual();
            StatusProgressBar.Value = percentage;
            //StatusProgressBar.InvalidateVisual();
        }

        public static void SetMessage(int percentage, string message)
        {
            Instance.Dispatcher.Invoke((Action)(() =>
            {
                Instance._SetMessage(percentage, message);
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
                Instance.WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
                int d = 4;
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

        private void StatusProgressBar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StatusProgressBar.Value = 70;
        }
    }
}
