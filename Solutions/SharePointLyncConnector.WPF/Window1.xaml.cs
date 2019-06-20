using System;
using System.Collections.Generic;
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
using Microsoft.Lync.Model.Conversation;
using Microsoft.Lync.Model.Extensibility;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Win32;

namespace SharePointLyncConnector.WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private string myRemoteParticipantUri;
        Microsoft.Lync.Model.Conversation.Conversation _Conversation = null;
        Microsoft.Lync.Model.LyncClient _LyncClient;
        public Window1()
        {
            InitializeComponent();
        }

        public void StartIMConversation(string participantUri)
        {
            _LyncClient.ConversationManager.ConversationAdded += ConversationsManager_ConversationAdded;
            myRemoteParticipantUri = participantUri;
            _Conversation = _LyncClient.ConversationManager.AddConversation();
        }

        // Callback method for the BeginSendContextData method.
        public void SendContextDataCallback(IAsyncResult res)
        {
            ConversationWindow _conversationWindow = ((Automation)res.AsyncState).EndStartConversation(res);
            InstantMessageModality instantMessage = (InstantMessageModality) _conversationWindow.Conversation.Modalities[ModalityTypes.InstantMessage];
            instantMessage.BeginSendMessage("hello sdx - success!", SendCMessageCallback, null);
        }

        // Callback method for the BeginSendContextData method.
        public void StartConversationCallback2(IAsyncResult res)
        {
            //Process[] RunningProcesses = Process.GetProcesses();
        }

        public void SendCMessageCallback(IAsyncResult res)
        {
            MessageBox.Show("Message sent");
        }

        /// Handles ConversationAdded state change event raised on ConversationsManager
        /// <param name="source">ConversationsManager: The source of the event</param>
        /// <param name="data">ConversationsManagerEventArgs The event data. The incoming Conversation is obtained here</param>
        void ConversationsManager_ConversationAdded(Object source, ConversationManagerEventArgs data)
        {
            // Register for Conversation state changed events.
            data.Conversation.ParticipantAdded += Conversation_ParticipantAdded;
            //data.Conversation.StateChanged += Conversation_ConversationChangedEvent;

            // Add a remote participant.
            data.Conversation.AddParticipant(_LyncClient.ContactManager.GetContactByUri(this.myRemoteParticipantUri));

        }

        /// <summary>
        /// ParticipantAdded callback handles ParticpantAdded event raised by Conversation
        /// </summary>
        /// <param name="source">Conversation Source conversation.</param>
        /// <param name="data">ParticpantCollectionEventArgs Event data</param>
        void Conversation_ParticipantAdded(Object source, ParticipantCollectionChangedEventArgs data)
        {

            if (data.Participant.IsSelf == false)
            {
                if (((Conversation)source).Modalities.ContainsKey(ModalityTypes.InstantMessage))
                {
                    //((InstantMessageModality)data.Participant.Modalities[ModalityTypes.InstantMessage]).InstantMessageReceived += myInstantMessageModality_MessageReceived;
                    //((InstantMessageModality)data.Participant.Modalities[ModalityTypes.InstantMessage]).IsTypingChanged += myInstantMessageModality_ComposingChanged;
                }
            }
        }

        //send message
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string contacts = Environment.GetCommandLineArgs()[2];
                contacts = contacts.Substring(contacts.IndexOf("<") + 1).TrimEnd(new char[] { '>' });
                _LyncClient = Microsoft.Lync.Model.LyncClient.GetClient();
                List<string> c = new List<string>();
                c.Add(contacts);
                Microsoft.Lync.Model.Extensibility.Automation automation = Microsoft.Lync.Model.LyncClient.GetAutomation();
                
                IAsyncResult res = automation.BeginStartConversation(Microsoft.Lync.Model.Extensibility.AutomationModalities.InstantMessage, c, null, SendContextDataCallback, automation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err:" + ex.Message);
            }
        }

        //send message
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            foreach(Conversation conversation in _LyncClient.ConversationManager.Conversations)
            {
                InstantMessageModality instantMessage = (InstantMessageModality) conversation.Modalities[ModalityTypes.InstantMessage];
                Dictionary<InstantMessageContentType, string> contents = new Dictionary<InstantMessageContentType, string>();
                contents.Add(InstantMessageContentType.PlainText, "http://www.google.com");
                //instantMessage.BeginSendMessage("hello sdx - success!", SendCMessageCallback, null);
                instantMessage.BeginSendMessage(contents, SendCMessageCallback, null);
            }
        }

        //send file
        private void button3_Click(object sender, RoutedEventArgs e)
        {
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
            _ModalitySettings.Add(AutomationModalitySettings.FilePathToTransfer, @"c:\temp\test.txt");


            IAsyncResult res = automation.BeginStartConversation(AutomationModalities.InstantMessage | AutomationModalities.FileTransfer, c, _ModalitySettings, null, null);

            automation.EndStartConversation(res);
        }

        private static string GetDefaultBrowserPath()
        {
            string key = @"htmlfile\shell\open\command";
            RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(key, false);
            // get default browser path
            return ((string)registryKey.GetValue(null, null)).Split('"')[1];
        }

        //share browser
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string browser = GetDefaultBrowserPath();
            Process process = new Process();
            process.StartInfo.FileName = "iexplore.exe";
            process.StartInfo.Arguments = "-nomerge http://www.google.com";
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

            IAsyncResult res = automation.BeginStartConversation(_ChosenMode, c, _ModalitySettings, StartConversationCallback2, null);

            //automation.EndStartConversation(res);
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
