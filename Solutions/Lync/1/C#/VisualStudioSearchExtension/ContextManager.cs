//++
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
// Module Name:
//
//  ConversationManager.cs
//    
// Abstract:
//
//  The ConversationManager keeps track of the state of a conversation and its access to the contents of the text view.
//  Communication with the the other endpoints of the conversation is done through Communicator's BeginSendContextData method.
//  This is used to send the text to the Communicator plugin and receive information about edits back from them.
//  This class also includes the method to extract the text formatting from the text view and store it as XAML.
//  
//--
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace VisualStudioSearchExtension
{
    class ContextManager
    {
        private static class ContextManagerHelper
        {
            public static Dictionary<string, ContextManager> _managers;
            public static string _contextualApplicationGuid;

            static ContextManagerHelper()
            {
                _managers = new Dictionary<string, ContextManager>();
                LyncClient.GetClient().ConversationManager.ConversationAdded += HandleConversationAdded;
                LyncClient.GetClient().ConversationManager.ConversationRemoved += HandleConversationRemoved;
            }

            private static void HandleConversationAdded(object sender, ConversationManagerEventArgs e)
            {
                try
                {
                    string s = e.Conversation.GetApplicationData(_contextualApplicationGuid);
                    string id = s.Split('\n')[3];
                    _managers[id].HandleConversationStarted(e.Conversation);
                }
                catch (ItemNotFoundException)
                {
                    //Probably means that a conversation was started by something else. This is safe to ignore.
                }
            }

            private static void HandleConversationRemoved(object sender, ConversationManagerEventArgs e)
            {
                RemoveConversation(e.Conversation, true);
            }

            public static void RemoveConversation(Conversation conversation, bool callback)
            {
                foreach (string id in _managers.Keys)
                {
                    ContextManager manager = _managers[id];
                    if (manager._conversation == conversation)
                    {
                        if (callback)
                        {
                            manager._searchMargin.Dispatcher.Invoke(manager._conversationEndedCallback);
                        }
                        _managers.Remove(id);
                        break;
                    }
                }
            }
        }

        //For generating the XAML. Having defaults set as the most common setting makes the XAML smaller.
        private static readonly Brush DefaultColor = new SolidColorBrush(Colors.Black);
        private static readonly double DefaultSize = 13;
        private static readonly FontFamily DefaultFont = new FontFamily("Consolas");

        //These constants and a few utility functions are also in the Communicator plugin. If this wasn't a sample, they would be in a
        //shared utility class.
        private const string ContentType = "text/plain";
        private const string ServerID = "server";
        private const char RecipientSeparator = ';';
        private const string GetTextCommand = "get text";
        private const string TextCommand = "text";
        private const string PutTextCommand = "put text";
        private const string GetEditRightsCommand = "get editing";
        private const string ReleaseEditRightsCommand = "release editing";
        private const string EditRightsCommand = "editing";
        private const string IdentifySelfCommand = "self";

        private IWpfTextView _view;
        private SnapshotSpan _span;
        private string _filename;
        private ITextDocument _doc;
        private string _formattedText;
        private string _id;
        private Conversation _conversation;
        private SearchMargin _searchMargin;
        private Action _conversationStartedCallback;
        private Action _conversationEndedCallback;
        //Made an integer to support Interlocked.Exchange, which doesn't allow bools. Conceptually, 1=true and 0=false.
        private int _editLocked;
        private string _editRightsHolder;

        public bool Started
        {
            get
            {
                return _conversation != null;
            }
        }

        public ContextManager(IWpfTextView view, SnapshotSpan span, Action conversationStartedCallback, Action conversationEndedCallback)
        {
            _view = view;
            _span = span;
            _conversationStartedCallback = conversationStartedCallback;
            _conversationEndedCallback = conversationEndedCallback;
            _searchMargin = _view.Properties.GetProperty<SearchMargin>(SearchMargin.MarginName);
            _editLocked = 0;
            _editRightsHolder = "";
            if (ContextManagerHelper._contextualApplicationGuid == null)
            {
                ContextManagerHelper._contextualApplicationGuid = _searchMargin.search.ContextualInformation.ApplicationId;
            }
        }    

        private void HandleConversationStarted(Conversation conversation)
        {
            _conversation = conversation;
            _conversation.ContextDataSent += HandleContextSentOrReceived;
            _conversation.ContextDataReceived += HandleContextSentOrReceived;

            _searchMargin.Dispatcher.Invoke(_conversationStartedCallback);
        }

        //Use the header to make sure this context data was intended for this client, then process it.
        private void HandleContextSentOrReceived(object sender, ContextEventArgs e)
        {
            //The first line is the sender, the second is the recipient list, the third is the command, and everything following that is the body
            //Sample:
            //sip:tester1@example.com
            //server
            //get text
            //
            string[] lines = e.ContextData.Split('\n');

            //Make sure enough lines are present and that we this was not sent by us.
            if (lines.Length >= 3 && lines[0] != ServerID)
            {
                //Multiple recipients are separated by semicolons. Make sure we are one of them.
                string[] dests = lines[1].Split(RecipientSeparator);
                if (dests.Contains(ServerID))
                {
                    //The last parameter is everything that follows the third line break.
                    int lineBreaks = lines.Length > 3 ? 3 : 2;
                    ProcessCommand(lines[0], lines[2], e.ContextData.Substring(lines[0].Length + lines[1].Length + lines[2].Length + lineBreaks));
                }
            }
        }

        private void ProcessCommand(string sender, string command, string body)
        {
            if (command == GetTextCommand)
            {
                SendData(sender, TextCommand, _formattedText);

                //Send the identify self command to the client that is running on the same instance of Communicator, so
                //it knows that it is allowed to send edits back to the server.
                if (sender == _conversation.SelfParticipant.Contact.Uri)
                {
                    SendData(_conversation.SelfParticipant.Contact.Uri, IdentifySelfCommand, string.Empty);
                }
            }
            else if (command == PutTextCommand)
            {
                if (sender == _conversation.SelfParticipant.Contact.Uri)
                {
                    _searchMargin.Dispatcher.Invoke(new Action<string>(ChangeText), body);
                }
            }
            else if (command == GetEditRightsCommand)
            {
                //Makes sure only one client can get the rights. Exchange is atomic. Because of this, if two threads try to swap 1 into
                //_editLocked at the same time, one will happen first, and only that one will return 0. The other will return 1, so this
                //line makes sure that in the case of two simultaneous requests, only one succeeds.
                if (0 == System.Threading.Interlocked.Exchange(ref _editLocked, 1))
                {
                    _editRightsHolder = sender;
                }

                SendData(AllParticipants(), EditRightsCommand, _editRightsHolder);
            }
            else if (command == ReleaseEditRightsCommand)
            {
                if (sender == _editRightsHolder)
                {
                    _editRightsHolder = string.Empty;
                    _editLocked = 0;
                }

                SendData(AllParticipants(), EditRightsCommand, _editRightsHolder);
            }
        }

        private void SendData(string recipient, string command, string body)
        {
            _conversation.BeginSendContextData(ContextManagerHelper._contextualApplicationGuid, ContentType, string.Format("{0}\n{1}\n{2}\n{3}\n", ServerID, recipient, command, body), null, null);
        }

        private string AllParticipants()
        {
            return string.Join(";", from c in _conversation.Participants select c.Contact.Uri);
        }

        private void ChangeText(string newText)
        {
            //Tagging the edit with this manager as the tag allows the edit to be identified by the event handler for text buffer change.
            ITextEdit edit = _view.TextBuffer.CreateEdit(EditOptions.None, null, this);
            edit.Replace(_span, newText);
            edit.Apply();

            //Since the text changed length, the span needs to be updated.
            _span = new SnapshotSpan(_view.TextBuffer.CurrentSnapshot, _span.Start.Position, newText.Length);

            //Update the clients. They don't have formatting info yet.
            _formattedText = GetTextAsXaml(_view, _span.Start, _span.End);
            SendData(AllParticipants(), TextCommand, _formattedText);
        }

        public string GetContextString()
        {
            //As needed, get the various pieces of info sent to the client
            //First is the file name of the document
            if (_doc == null)
            {
                if (_view.TextBuffer.Properties.TryGetProperty<ITextDocument>(typeof(ITextDocument), out _doc))
                {
                    _filename = System.IO.Path.GetFileName(_doc.FilePath);
                }
                else
                {
                    _filename = "NONE";
                }
            }

            //This registers with conversation listener so that it can find this manager when the converstaion is started.
            if (_id == null)
            {
                _id = System.Guid.NewGuid().ToString();
                ContextManagerHelper._managers.Add(_id, this);
            }

            //Get the formatted text.
            if (_formattedText == null)
            {
                _formattedText = GetTextAsXaml(_view, _span.Start, _span.End);
            }

            //Send the file name, the line number, the document type, manager's id as contextual info.
            //It will look something like:
            //File.cs
            //1
            //CSharp
            //d0d57bfe-3cd3-498b-afcf-4bee7dbaabc3
            return _filename + "\n" + (_span.Start.GetContainingLine().LineNumber + 1) + "\n" + _view.TextDataModel.DocumentBuffer.ContentType + "\n" + _id;
        }

        //Should be called when something is finished with the ContextManager. This will deregister it if it has not been started, allowing GC.
        public bool Done()
        {
            if (Started)
            {
                return false;
            }
            else
            {
                //false prvents the callback from being called.
                ContextManagerHelper.RemoveConversation(_conversation, false);
                return true;
            }
        }

        private static string GetTextAsXaml(IWpfTextView view, SnapshotPoint startPoint, SnapshotPoint endPoint)
        {
            int firstLine = startPoint.GetContainingLine().LineNumber;
            int lastLine = endPoint.GetContainingLine().LineNumber;

            //Formatted text will be added to this FlowDocument, from where it can be read out as XAML.
            FlowDocument document = new FlowDocument();
            document.Foreground = DefaultColor;
            document.FontSize = DefaultSize;
            document.FontFamily = DefaultFont;


            for (int i = firstLine; i <= lastLine; i++)
            {
                ITextSnapshotLine visualLine = view.VisualSnapshot.GetLineFromLineNumber(i);
                Collection<IFormattedLine> formattedLines = view.FormattedLineSource.FormatLineInVisualBuffer(visualLine);
                foreach (IFormattedLine formattedLine in formattedLines)
                {
                    TextRunProperties currentFormatting = null;

                    string textToAdd = string.Empty;
                    Paragraph paragraph = new Paragraph();

                    int start = visualLine.Start.Position;
                    int end = visualLine.End.Position;

                    //Since the formatter works in lines, skipping the unselected parts at the ends of the first and last lines has
                    //to be done manually.
                    if (visualLine.LineNumber == firstLine)
                    {
                        start = startPoint;
                    }
                    if (visualLine.LineNumber == lastLine)
                    {
                        end = endPoint;
                    }

                    for (int j = start; j < end; j++)
                    {
                        //Formatting is read out one character at a time.
                        SnapshotPoint currentCharacter = new SnapshotPoint(formattedLine.Snapshot, j);
                        TextRunProperties formatting = formattedLine.GetCharacterFormatting(currentCharacter);

                        if (formatting != currentFormatting)
                        {
                            //When the formatting changes, if any text has been seen, a new run is needed. Add the text that has ben seen,
                            //and then clear the variable since the text has already been added.
                            if (!string.IsNullOrEmpty(textToAdd))
                            {
                                paragraph.Inlines.Add(MakeInline(textToAdd, currentFormatting));
                                textToAdd = string.Empty;
                            }

                            currentFormatting = formatting;
                        }

                        //Expand tabs to spaces, since the Silverlight RichTextBox can only display tabs as 2 spaces.
                        if (currentCharacter.GetChar() == '\t')
                        {
                            textToAdd += "".PadRight(view.FormattedLineSource.TabSize, ' ');
                        }
                        else
                        {
                            textToAdd += currentCharacter.GetChar();
                        }
                    }

                    //Add remaining text at end of line.
                    if (!string.IsNullOrEmpty(textToAdd))
                    {
                        paragraph.Inlines.Add(MakeInline(textToAdd, currentFormatting));
                    }

                    document.Blocks.Add(paragraph);
                }
            }

            //Get the data out as XAML.
            TextRange tr = new TextRange(document.ContentStart, document.ContentEnd);
            MemoryStream ms = new MemoryStream();
            tr.Save(ms, System.Windows.DataFormats.Xaml);
            string xaml = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            //By default, the output includes attributes on the section element that Silverlight can't handle.
            //This isn't particularly clean, but the attributes need to be removed before beings sent to the client.
            const string SectionTagRegex =  "<Section [^>]*>";
            const string NewSectionTag = "<Section xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">";
            xaml = Regex.Replace(xaml, SectionTagRegex, NewSectionTag);
            return xaml;
        }

        private static Inline MakeInline(string text, TextRunProperties formatting)
        {
            Run run = new Run(text);
            run.Foreground = formatting.ForegroundBrush;
            run.FontSize = formatting.FontRenderingEmSize;
            run.FontFamily = formatting.Typeface.FontFamily;
            if (formatting.TextDecorations.Count > 0)
            {
                run.TextDecorations = formatting.TextDecorations;
            }
            return run;
        }
    }
}
