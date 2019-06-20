using Microsoft.Win32;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Threading;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Studio.UI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
        //private bool IsLogFileInUse = false;
        //private TextWriter oldOut = System.Console.Out;
        //private FileStream ostrm = null;
        //private StreamWriter writer = null;

        public MainWindow()
        {
            InitializeComponent();
            //SyncTasksManager.GetInstance().GetTestDocumentSyncTasks();

            //string configurationFolderPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["LogFolderPath"]);
            //if (string.IsNullOrEmpty(configurationFolderPath) == false)
            //{
            //    string fileName = DateTime.Now.ToString("yyyy-MM-dd-hh-mm") + ".txt";
            //    try
            //    {
            //        ostrm = new FileStream(configurationFolderPath + "\\" + fileName, FileMode.OpenOrCreate, FileAccess.Write);
            //        writer = new StreamWriter(ostrm);
            //        writer.AutoFlush = true;
            //        System.Console.SetOut(writer);
            //        IsLogFileInUse = true;
            //        System.Console.WriteLine("Log file has been set as " + configurationFolderPath);
            //    }
            //    catch (Exception e)
            //    {
            //        System.Console.WriteLine("Cannot open " + fileName + " for writing");
            //        System.Console.WriteLine(e.Message);
            //    }
            //}
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string fileVersion = fvi.FileVersion;
            System.Console.WriteLine("App version:" + fileVersion);

            System.Console.WriteLine("Site provisioning has started");

            /*

                        string modelBase64 = @"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";

                        byte[] bytes = Convert.FromBase64String(modelBase64);
                        byte[] uncompressed = Decompress(bytes);

                        string edmx = Encoding.UTF8.GetString(uncompressed);
                        File.WriteAllText(@"c:\temp\edmx.xml", edmx);
                        */
        }

        static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];

                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }

                    while (count > 0);
                    return memory.ToArray();
                }
            }
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

            ApplicationContext.SetApplicationManager(new Sobiens.Connectors.UI.SPCamlStudioApplicationManager(this));
            ApplicationContext.Current.Initialize();
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
            //if (IsLogFileInUse == true)
            //{
            //    System.Console.SetOut(oldOut);
            //    writer.Close();
            //    ostrm.Close();
            //}

            System.Console.WriteLine("Site provisioning has ended");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
            //stlf.Initialize();
            if (stlf.ShowDialog(null, "Scheduled Jobs") == true)
            {
            }

        }
    }
}
