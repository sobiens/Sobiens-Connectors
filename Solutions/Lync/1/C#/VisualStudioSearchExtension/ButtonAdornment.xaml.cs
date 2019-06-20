//++
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
// Module Name:
//
//  ButtonAdornment.xaml.cs
//    
// Abstract:
//
//  The ButtonAdornment follows selection. When it is clicked, it causes the margin to be shown.
//  
//--
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;


namespace VisualStudioSearchExtension
{
    public partial class ButtonAdornment : StackPanel
    {
        internal const string adornmentLayerName = "ButtonAdornmentLayer";
        private static readonly Brush EditLockedBackgroundBrush = new SolidColorBrush(Colors.Pink);

        private IAdornmentLayer _layer;
        private IWpfTextView _view;
        private ContextManager _manager;
        private Brush _oldBrush;

        //The value of ViewSearchMargin only needs to be loaded once, but this needs to happen after the constructor. This allows that.
        private SearchMargin _viewSearchMargin;
        private SearchMargin ViewSearchMargin
        {
            get
            {
                return _viewSearchMargin ?? (_viewSearchMargin = _view.Properties.GetProperty<SearchMargin>(SearchMargin.MarginName));
            }
        }

        public ButtonAdornment(IWpfTextView view)
        {
            InitializeComponent();
            _view = view;
            _view.Selection.SelectionChanged += HandleSelectionChanged;
            _view.TextBuffer.Changing += HandleTextBufferChanging;
            _layer = view.GetAdornmentLayer(adornmentLayerName);
        }

        private void HandleTextBufferChanging(object sender, TextContentChangingEventArgs e)
        {
            //Prevent the text from changing while the margin is visible, since the text needs to be loaded when the
            //margin is shown, and this keeps it consistent.
            //The second disjunct makes sure that a conversation is currently happening. It also makes sure that edits are not tagged
            //with the context manager. Edits made by the manager in response to a client sending edited code back should not be cancelled,
            //and will be tagged with the manager. Edits made by the user directly have no tag and need to be cancelled while a conversation
            //is happening to prevent the concurrent modification.
            if (ViewSearchMargin.ActualWidth > 0 || (_manager != null && _manager.Started && e.EditTag != _manager))
            {
                e.Cancel();
            }
        }

        //Show the margin when the button is clicked. Also, get the contextual information ready for a converstaion to be started.
        private void HandleButtonClick(object sender, RoutedEventArgs e)
        {
            _layer.RemoveAdornment(this);
            ContextManager cm = new ContextManager(_view, new SnapshotSpan(_view.Selection.Start.Position, _view.Selection.End.Position), HandleConversationStarted, HandleConversationEnded);
            ViewSearchMargin.ContextualApplicationData = cm.GetContextString();
            _manager = cm;
            ViewSearchMargin.Show();
            ViewSearchMargin.Focus();
        }

        private void HandleConversationStarted()
        {
            ViewSearchMargin.Hide();
            //Set the background to show that edits cannot be made.
            _view.Selection.Clear();
            _oldBrush = _view.Background;
            _view.Background = EditLockedBackgroundBrush;
        }

        private void HandleConversationEnded()
        {
            _view.Background = _oldBrush;
            _manager = null;
            PositionAdornment();
        }

        void HandleSelectionChanged(object sender, EventArgs e)
        {
            if (_manager == null || !_manager.Started)
            {
                PositionAdornment();
                ViewSearchMargin.Hide();
                if (_manager != null)
                {
                    _manager.Done();
                    _manager = null;
                }
            }
        }

        private void PositionAdornment()
        {
            SnapshotSpan selection = new SnapshotSpan(_view.Selection.Start.Position, _view.Selection.End.Position);

            if (selection.IsEmpty)
            {
                _layer.RemoveAdornment(this);
            }
            else
            {
                //For a forwards selection, put the button at the bottom right.
                if (!_view.Selection.IsReversed)
                {
                    Geometry g = _view.TextViewLines.GetLineMarkerGeometry(new SnapshotSpan(_view.TextSnapshot, _view.Selection.End.Position.Position - 1, 1));
                    if (g == null)
                    {
                        g = _view.TextViewLines.GetMarkerGeometry(selection);
                    }
                    Canvas.SetLeft(this, g.Bounds.Right);
                    Canvas.SetTop(this, g.Bounds.Bottom);
                }
                //For a backwards selection, put the button at the top left, but not off of the viewport.
                else
                {
                    Geometry g = _view.TextViewLines.GetLineMarkerGeometry(new SnapshotSpan(_view.Selection.Start.Position, 1));
                    Canvas.SetLeft(this, Math.Max(g.Bounds.Left - ActualWidth, _view.ViewportLeft));
                    Canvas.SetTop(this, Math.Max(g.Bounds.Top - ActualHeight, _view.ViewportTop));
                }

                if (Parent == null)
                {
                    _layer.AddAdornment(AdornmentPositioningBehavior.OwnerControlled, null, null, this, null);
                }

            }
        }
    }
}
