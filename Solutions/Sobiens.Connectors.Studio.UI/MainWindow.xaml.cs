using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Studio.UI.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sobiens.Connectors.Studio.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ISPCamlStudio
    {
        public IQueriesPanel QueriesPanel { get { return this.queriesPanel; } }
        public IQueryDesignerToolbar QueryDesignerToolbar { get { return this.queryDesignerToolbar; } }
        public IServerObjectExplorer ServerObjectExplorer { get { return this.serverObjectExplorer; } }

        public MainWindow()
        {
            InitializeComponent();

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string fileVersion = fvi.FileVersion;
            System.Console.WriteLine("App version:" + fileVersion);

            System.Console.WriteLine("Site provisioning has started");
            //LoadingWindow lw = new LoadingWindow();
            //lw.ShowDialog();
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void serverObjectExplorer_After_Select(object parentFolder, object folder)
        {
            ApplicationContext.Current.SPCamlStudio.QueryDesignerToolbar.ValidateButtonEnabilities();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Title = "Sobiens Studio " + fvi.FileVersion;
            ApplicationVersionLabel.Content= "Sobiens Studio " + fvi.FileVersion;

            ApplicationContext.SetApplicationManager(new Sobiens.Connectors.UI.SPCamlStudioApplicationManager(this));
            ApplicationContext.Current.Initialize();
            FeatureHelpManager.ShowNotSeenFeatureHelpDialog();
            StartBackgrounProcessButton_Click(null, null);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
        }

        private void SyncDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Current.ShowSyncDataWizard();
        }
        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Current.Save();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //            OpenFileDialog openFileDialog = new OpenFileDialog();
            //            if (openFileDialog.ShowDialog() == true)
            ApplicationContext.Current.Load();
        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://tutorials.sobiens.com/solutions/sobiens-studio/");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Connectors.UI.Service.BackgroundDataService.GetInstance().OnStop();
            }
            catch(Exception ex)
            {

            }
        }

        private void StartBackgrounProcessButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartBackgrounProcessButton.Content.ToString() == "Start service")
            {
                Connectors.UI.Service.BackgroundDataService.GetInstance().OnStart(null);
                StartBackgrounProcessButton.Content = "Stop service";
            }
            else
            {
                Connectors.UI.Service.BackgroundDataService.GetInstance().OnStop();
                StartBackgrounProcessButton.Content = "Start service";
            }
        }

        private void ScheduledJobsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SyncTaskListForm stlf = new SyncTaskListForm();
            if (stlf.ShowDialog(null, "Scheduled Jobs") == true)
            {
            }

        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    Application.Current.MainWindow.DragMove();
                }
        }

        private void AdjustWindowSize()
        {
            RestoreButton.Visibility = Visibility.Hidden;
            MaximizeButton.Visibility = Visibility.Hidden;
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaximizeButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                RestoreButton.Visibility = Visibility.Visible;
            }
        }

        public void ReportProgress(int? percentage, string message)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                if(percentage==0 || percentage ==100)
                {
                    LoadingImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    LoadingImage.Visibility = Visibility.Visible;
                }
                if (percentage.HasValue == true)
                {
                    StatusProgressBar.Value = percentage.Value;
                    StatusProgressBar.InvalidateVisual();
                }

                StatusLabel.Content = message;
                StatusLabel.InvalidateVisual();
            }));
        }

        private void StatusLabel_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SyncTaskListForm stlf = new SyncTaskListForm();
            if (stlf.ShowDialog(null, "Scheduled Jobs") == true)
            {
            }
        }
    }
}
