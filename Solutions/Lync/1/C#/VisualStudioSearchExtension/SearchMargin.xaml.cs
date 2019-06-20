//++
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
// Module Name:
//
//  SearchMargin.xaml.cs
//    
// Abstract:
//
//  The code behind the search margin. It is mostly boilerplate for implementing the IWpfTextViewMargin interface.
//  
//--
using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

using Microsoft.VisualStudio.Text.Editor;
using Microsoft.Lync.Controls;
using System.Windows;


namespace VisualStudioSearchExtension
{
    public partial class SearchMargin : StackPanel, IWpfTextViewMargin
    {
        internal const string MarginName = "LyncMargin";
        private bool _isDisposed = false;
        private DoubleAnimation _show;
        private DoubleAnimation _hide;

        public string ContextualApplicationData
        {
            get
            {
                return search.ContextualInformation.ApplicationData;
            }
            set
            {
                ConversationContextualInfo conversationContextualInfo = new ConversationContextualInfo();
                conversationContextualInfo.ApplicationData = value;
                conversationContextualInfo.ApplicationId = search.ContextualInformation.ApplicationId;
                conversationContextualInfo.ContextualLink = search.ContextualInformation.ContextualLink;
                conversationContextualInfo.Subject = search.ContextualInformation.Subject;

                search.ContextualInformation = conversationContextualInfo;

                System.Diagnostics.Debug.WriteLine(string.Format("Setting APPDATAA******************************\n{0}", value));
            }
        }

        public SearchMargin()
        {
            InitializeComponent();
            this.Width = 0;
            _show = new DoubleAnimation(0, 300, TimeSpan.FromMilliseconds(300));
            _hide = new DoubleAnimation(300, 0, TimeSpan.FromMilliseconds(300));
            this.Loaded += new RoutedEventHandler(SearchMargin_Loaded);
        }

        void SearchMargin_Loaded(object sender, RoutedEventArgs e)
        {
            this.PreviewKeyUp += new System.Windows.Input.KeyEventHandler(SearchMargin_PreviewKeyUp);
            ((FrameworkElement)this.Parent).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(SearchMargin_PreviewKeyDown);
        }

        void SearchMargin_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
            }
        }

        void SearchMargin_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
            }
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(MarginName);
            }
        }

        public void Show()
        {
            this.BeginAnimation(StackPanel.WidthProperty, _show);
        }
        public void Hide()
        {
            _hide.From = this.ActualWidth;
            _hide.Duration = TimeSpan.FromMilliseconds(this.ActualWidth);
            this.BeginAnimation(StackPanel.WidthProperty, _hide);
        }


        #region IWpfTextViewMargin Members

        public System.Windows.FrameworkElement VisualElement
        {
            // Since this margin inheirits StackPanel, this is the object which renders
            // the margin.
            get
            {
                ThrowIfDisposed();
                return this;
            }
        }

        #endregion

        #region ITextViewMargin Members

        public double MarginSize
        {
            // Since this is a veritcal margin, its height will be bound to the height of the text view.
            // Therefore, its size is its width.
            get
            {
                ThrowIfDisposed();
                return this.ActualWidth;
            }
        }

        public bool Enabled
        {
            // The margin should always be enabled if it is shown
            get
            {
                ThrowIfDisposed();
                return this.ActualWidth > 0;
            }
        }

        /// <summary>
        /// Returns an instance of the margin if this is the margin that has been requested.
        /// </summary>
        /// <param name="marginName">The name of the margin requested</param>
        /// <returns>An instance of LyncMargin or null</returns>
        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return (marginName == SearchMargin.MarginName) ? (IWpfTextViewMargin)this : null;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }
        #endregion
    }
}
