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
using Sobiens.Connectors.WPF.Controls;
using Microsoft.Lync.Model.Extensibility;
using Sobiens.Connectors.Entities.Interfaces;
using System.Diagnostics;

namespace SharePointLyncConnector.WPF
{
    /// <summary>
    /// Interaction logic for SSWindow.xaml
    /// </summary>
    public partial class SSWindow : Window
    {
        private Microsoft.Lync.Model.LyncClient _LyncClient;
        public SSWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ConnectorExplorer hierarchyNavigator = new ConnectorExplorer();
            hierarchyNavigator.ShowDialog(this, "test");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ContextMenu contextMenu = new System.Windows.Controls.ContextMenu();
            //List<MenuItem> menuItems = new List<MenuItem>();

            MenuItem sendFileItem = new MenuItem();
            sendFileItem.Header = "Send File";
            sendFileItem.Click += new RoutedEventHandler(sendFileItem_Click);
            contextMenu.Items.Add(sendFileItem);

            MenuItem messageFileLinkItem = new MenuItem();
            messageFileLinkItem.Header = "Message File Link";
            messageFileLinkItem.Click += new RoutedEventHandler(messageFileLinkItem_Click);
            contextMenu.Items.Add(messageFileLinkItem);

            MenuItem shareFileItem = new MenuItem();
            shareFileItem.Header = "Share File";
            shareFileItem.Click += new RoutedEventHandler(shareFileItem_Click);
            contextMenu.Items.Add(shareFileItem);

            connectorExplorer1.ContextMenu = contextMenu;
        }

        void sendFileItem_Click(object sender, RoutedEventArgs e)
        {
            IItem item = ((MenuItem)e.Source).Tag as IItem;
            string contacts = Environment.GetCommandLineArgs()[2];
            contacts = contacts.Substring(contacts.IndexOf("<") + 1).TrimEnd(new char[] { '>' });
            _LyncClient = Microsoft.Lync.Model.LyncClient.GetClient();
            List<string> c = new List<string>();
            c.Add(contacts);
            Microsoft.Lync.Model.Extensibility.Automation automation = Microsoft.Lync.Model.LyncClient.GetAutomation();

            Dictionary<AutomationModalitySettings, object> _ModalitySettings = new Dictionary<AutomationModalitySettings, object>();
            _ModalitySettings.Add(AutomationModalitySettings.FirstInstantMessage, "Hi Serkant, please have a look at this:");
            _ModalitySettings.Add(AutomationModalitySettings.Subject, "Sharing File");
            _ModalitySettings.Add(AutomationModalitySettings.SendFirstInstantMessageImmediately, true);
            //_ModalitySettings.Add(AutomationModalitySettings.FilePathToTransfer, @"c:\temp\test.txt");
            _ModalitySettings.Add(AutomationModalitySettings.FilePathToTransfer, item.URL);


            IAsyncResult res = automation.BeginStartConversation(AutomationModalities.InstantMessage | AutomationModalities.FileTransfer, c, _ModalitySettings, null, null);

            automation.EndStartConversation(res);
        }

        void shareFileItem_Click(object sender, RoutedEventArgs e)
        {
            IItem item = ((MenuItem)e.Source).Tag as IItem;
            string extension = item.URL.Substring(item.URL.LastIndexOf('.') + 1);

            Process process = new Process();
            switch (extension.ToLower())
            {
                case "dotx":
                    process.StartInfo.FileName = "winword.exe";
                    process.StartInfo.Arguments = "/t " + item.URL;
                    break;
                case "doc":
                case "docx":
                    process.StartInfo.FileName = "winword.exe";
                    process.StartInfo.Arguments = item.URL;
                    break;
                case "xls":
                case "csv":
                    process.StartInfo.FileName = "excel.exe";
                    process.StartInfo.Arguments = "/r" + item.URL;
                    break;
                default:
                    process.StartInfo.FileName = "iexplore.exe";
                    process.StartInfo.Arguments = "-nomerge " + item.URL;
                    break;
            }
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();
            while (process.MainWindowHandle == IntPtr.Zero)
            {
                System.Threading.Thread.Sleep(100);
            }

            string contacts = Environment.GetCommandLineArgs()[2];
            contacts = contacts.Substring(contacts.IndexOf("<") + 1).TrimEnd(new char[] { '>' });
            _LyncClient = Microsoft.Lync.Model.LyncClient.GetClient();
            List<string> c = new List<string>();
            c.Add(contacts);

            Microsoft.Lync.Model.Extensibility.Automation automation = Microsoft.Lync.Model.LyncClient.GetAutomation();
            AutomationModalities _ChosenMode = AutomationModalities.ApplicationSharing | AutomationModalities.InstantMessage;

            Dictionary<AutomationModalitySettings, object> _ModalitySettings = new Dictionary<AutomationModalitySettings, object>();
            // Add the process Id to the modality settings for the conversation.
            _ModalitySettings.Add(AutomationModalitySettings.SharedProcess, process.Id);

            // Add the main window handle to the modality settings for the conversation.
            _ModalitySettings.Add(AutomationModalitySettings.SharedWindow, process.MainWindowHandle);

            // Adds text to toast and local user IMWindow text-entry control.
            _ModalitySettings.Add(AutomationModalitySettings.FirstInstantMessage, "Hello Serkant. I would like to share my first running process with you.");
            _ModalitySettings.Add(AutomationModalitySettings.SendFirstInstantMessageImmediately, true);

            IAsyncResult res = automation.BeginStartConversation(_ChosenMode, c, _ModalitySettings, null, null);
            automation.EndStartConversation(res);
        }

        void messageFileLinkItem_Click(object sender, RoutedEventArgs e)
        {
            IItem item = ((MenuItem)e.Source).Tag as IItem;
            string contacts = Environment.GetCommandLineArgs()[2];
            contacts = contacts.Substring(contacts.IndexOf("<") + 1).TrimEnd(new char[] { '>' });
            _LyncClient = Microsoft.Lync.Model.LyncClient.GetClient();
            List<string> c = new List<string>();
            c.Add(contacts);
            Microsoft.Lync.Model.Extensibility.Automation automation = Microsoft.Lync.Model.LyncClient.GetAutomation();

            Dictionary<AutomationModalitySettings, object> _ModalitySettings = new Dictionary<AutomationModalitySettings, object>();
            _ModalitySettings.Add(AutomationModalitySettings.FirstInstantMessage, "Hi Serkant, please have a look at this:");
            _ModalitySettings.Add(AutomationModalitySettings.Subject, "Sharing Link");
            _ModalitySettings.Add(AutomationModalitySettings.SendFirstInstantMessageImmediately, true);
            _ModalitySettings.Add(AutomationModalitySettings.HyperLink, item.URL);

            IAsyncResult res = automation.BeginStartConversation(AutomationModalities.InstantMessage, c, _ModalitySettings, null, null);

            automation.EndStartConversation(res);
        }

    }
}
