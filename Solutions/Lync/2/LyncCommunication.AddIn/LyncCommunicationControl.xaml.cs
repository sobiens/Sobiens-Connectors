// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LyncCommunicationControl.xaml.cs" company="Microsoft Corporation">
//   Copyright © Microsoft Corporation.  All Rights Reserved.
// </copyright>
// <summary>
//   Add-In components for the a Lync contact presence field
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LyncCommunicationAddIn
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Lync.Controls;
    using Microsoft.Lync.Controls.Internal;
    using Microsoft.Lync.Controls.Internal.Model;
    using Microsoft.Lync.Internal.Utilities.Helpers;
    using MenuItem = System.Windows.Controls.MenuItem;

    /// <summary>
    /// Interaction logic for LyncCommunicationControl.xaml
    /// </summary>
    public partial class LyncCommunicationControl : UserControl //, IDisposable
    {
        /// <summary>
        /// The current list of search candidates
        /// </summary>
        private List<string> searchCandidates;

        /// <summary>
        /// Dependency Property for the IsSearchAllowed property
        /// </summary>
        public static readonly DependencyProperty IsSearchAllowedProperty = DependencyProperty.Register("IsSearchAllowed", typeof(bool), typeof(LyncCommunicationControl), new PropertyMetadata(true, null));

        /// <summary>
        /// Dependency Property for the CanEnable property
        /// </summary>
        public static readonly DependencyProperty CanEnableProperty = DependencyProperty.Register("CanEnable", typeof(bool), typeof(LyncCommunicationControl), new PropertyMetadata(true, null));

        /// <summary>
        /// Dependency Property for the IsReadOnly property
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(LyncCommunicationControl), new PropertyMetadata(false, null));

        /// <summary>
        /// Initializes a new instance of the <see cref="LyncCommunicationControl"/> class.
        /// </summary>
        public LyncCommunicationControl()
        {
            //this.AccessibleName = "Lync communication control";
            this.InitializeComponent();

            this.PreviewKeyDown += this.OnLyncFieldPreviewKeyDown;
            this.KeyDown += this.OnLyncFieldKeyDown;
            this.PreviewMouseDown += delegate (object sender, MouseButtonEventArgs args)
                {
                    this.Focus();
                    if ((args.ChangedButton == MouseButton.Right) && this.IsSearchAllowed)
                    {
                        args.Handled = true;
                    }
                };

            var prop = DependencyPropertyDescriptor.FromProperty(ContactBase.SourceProperty, typeof(PresenceIndicator));
            prop.AddValueChanged(this.PresenceIndicator, this.OnPresenceIndicatorSourceChanged);

            prop = DependencyPropertyDescriptor.FromProperty(UCBase.IsSignedInProperty, typeof(PresenceIndicator));
            prop.AddValueChanged(this.PresenceIndicator, this.OnPresenceIndicatorIsSignedInChanged);

            this.SearchPopup.Opened += this.OnSearchPopupOpened;
            
            this.InitInputHelperContextMenu();
            this.UpdateEnabledSignedInState();
        } 

        /// <summary>
        /// Event fires if presence contact has changed
        /// </summary>
        public event EventHandler ContactChanged;

        /// <summary>
        /// Finds out whether the input string is a sip URI
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Boolean value indicating whether 'input' is a sip URI</returns>
        public static bool IsSipUri(string input)
        {
            return !string.IsNullOrEmpty(input) && (input.StartsWith("sip:", true, CultureInfo.CurrentCulture) || input.StartsWith("tel:", true, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Gets the root grid in order to change horizontal alignment
        /// </summary>
        public FrameworkElement LayoutRoot
        {
            get
            {
                return this.LayoutGrid;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can show enabled / based on the IsSigned in state
        /// </summary>
        public bool CanEnable
        {
            get
            {
                return (bool) this.GetValue(CanEnableProperty);
            }
            set
            {
                this.SetValue(CanEnableProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether search shall be supported
        /// </summary>
        public bool IsSearchAllowed
        {
            get
            {
                return (bool)this.GetValue(IsSearchAllowedProperty);
            }

            set
            {
                this.SetValue(IsSearchAllowedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether presence address can be changed by the control
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return (bool)this.GetValue(IsReadOnlyProperty);
            }

            set
            {
                this.SetValue(IsReadOnlyProperty, value);
                this.InitInputHelperContextMenu();
            }
        }

        /// <summary>
        /// Gets a value indicating whether user is signed in.
        /// </summary>
        public bool IsSignedIn
        {
            get
            {
                return this.PresenceIndicator.IsSignedIn;
            }
        }

        /// <summary>
        /// Gets or sets sets the Photo display mode for the Presence Indicator and by that also
        /// selects the Lync Icon for respective display size.
        /// </summary>
        public PhotoDisplayMode PhotoDisplayMode
        {
            get
            {
                return this.PresenceIndicator.PhotoDisplayMode;
            } 

            set
            {
                this.PresenceIndicator.PhotoDisplayMode = value;
                this.LyncIconImage.BeginInit();
                ImageSource imageSource;
                switch (value)
                {
                    case PhotoDisplayMode.Large:
                        imageSource = this.LoadResourceImage("Lync48x48.png");
                        this.LyncIconImage.Source = imageSource;
                        break;

                    case PhotoDisplayMode.Small:
                        imageSource = this.LoadResourceImage("Lync32x32.png");
                        this.LyncIconImage.Source = imageSource;
                        break;

                    case PhotoDisplayMode.Hidden:
                        imageSource = this.LoadResourceImage("Lync16x16.png");
                        this.LyncIconImage.Source = imageSource;
                        break;
                }
                this.LyncIconImage.EndInit();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the presence indicator has a valid contact
        /// </summary>
        public bool CanShowContactCard
        {
            get
            {
                return !string.IsNullOrEmpty(this.PresenceAddress) && this.PresenceIndicator.ContactPopupCard != null;
            }
        }

        /// <summary>
        /// Loads a resource PNG image (EmbeddedResource) by adding the default namespace before the given resource name
        /// </summary>
        /// <param name="resourceName">The resource name</param>
        /// <returns>The image source loaded with the resource image.</returns>
        private ImageSource LoadResourceImage(string resourceName)
        {
            resourceName = this.GetType().Namespace + '.' + resourceName;
            var resourceStream = this.GetType().Assembly.GetManifestResourceStream(resourceName);
            if (resourceStream != null)
            {
                var bitmapDecoder = new PngBitmapDecoder(resourceStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                var imageSource = bitmapDecoder.Frames[0];
                return imageSource;
            }
            return null;
        }

        /// <summary>
        /// Assumes that search candidates are comma seperated in this string
        /// If this is a sip address ()
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>List of search string candidates</returns>
        public static Collection<string> ParseSearchCandidates(string input)
        {
            var result = new Collection<string>();
            if (!string.IsNullOrEmpty(input))
            {
                result.AddRange(input.Split(','));
            }

            return result;
        }

        /// <summary>
        /// Search popup event handler
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Event arguments</param>
        private void OnSearchPopupOpened(object sender, EventArgs e)
        {
            var bl = this.PointToScreen(new Point(0, this.ActualHeight));
            // better would be to use the current screen ...
            this.SearchPopup.MaxHeight = SystemParameters.PrimaryScreenHeight - bl.Y - 10;
            this.SearchPopup.MaxWidth = SystemParameters.PrimaryScreenWidth - bl.X - 10;
            this.contactSearchBox.SearchInputBox.Focus();
            this.contactSearchBox.SearchResultList.PreviewMouseDoubleClick -= this.OnSearchResultListDoubleClicked;
            this.contactSearchBox.SearchResultList.PreviewMouseDoubleClick += this.OnSearchResultListDoubleClicked;
            this.contactSearchBox.SearchResultList.PreviewKeyDown -= this.OnSearchResultListPreviewKeyDown;
            this.contactSearchBox.SearchResultList.PreviewKeyDown += this.OnSearchResultListPreviewKeyDown;
        }

        /// <summary>
        /// Processes key down for Enter and Space to accept a selection.
        /// Also prevents issuing the default communication action, which the base class would do.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void OnSearchResultListPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (((e.Key == Key.Space) || (e.Key == Key.Return)) && (this.contactSearchBox.SearchResultList.SelectedItem != null))
            {
                this.UseSelectedContactSearchResult();
                e.Handled = true;
            }
        }

        /// <summary>
        /// DoubleClick is used to accepts a selected Contact
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void OnSearchResultListDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            this.UseSelectedContactSearchResult();
        }

        /// <summary>
        /// Uses the contact in the search result for the communication contact
        /// </summary>
        private void UseSelectedContactSearchResult()
        {
            if (this.IsReadOnly)
            {
                return;
            }

            var resultItem = this.contactSearchBox.SearchResultList.SelectedItem as SearchResult;
            if (resultItem != null && (resultItem.Contact != null) &&
                (resultItem.Contact.ContactType == ContactType.Person || resultItem.Contact.ContactType == ContactType.Telephone || resultItem.Contact.ContactType == ContactType.Unknown))
            {
                var uri = resultItem.Contact.Uri;
                if ((this.PresenceIndicator.Source == null) ||
                    (!string.Equals(
                        this.PresenceIndicator.Source.ToString(), uri, StringComparison.CurrentCultureIgnoreCase)))
                {
                    this.PresenceIndicator.Source = uri;
                }

                this.SearchPopup.IsOpen = false;
                this.Focus();
            }
        }

        /// <summary>
        /// Keydown preview event handler
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Event arguments</param>
        private void OnLyncFieldPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.CloseSearchPopup();
                    this.CloseContextMenu();
                    this.CloseContactCardPopup();
                    this.Focus();
                    e.Handled = true;
                    break;

                case Key.F3:
                    if (this.IsSearchAllowed)
                    {
                        this.OpenSearchPopup(string.Empty);
                        e.Handled = true;
                    }
                    break;

                case Key.System:
                    if (e.SystemKey == Key.Down)
                    {
                        e.Handled = this.OpenContactCardPopup();
                    }
                    break;

                case Key.Space:
                    if (!this.InputHelper.IsFocused)
                    {
                        e.Handled = this.OpenContactCardPopup();
                    }
                    break;

                case Key.F4:
                    e.Handled = this.OpenContactCardPopup();
                    break;
            }
        }

        /// <summary>
        /// Keydown event handler
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Event arguments</param>
        private void OnLyncFieldKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (this.SearchPopup.IsOpen)
                    {
                        e.Handled = true;
                    }
                    break;

                case Key.Apps:
                    this.CloseContactCardPopup(true);
                    this.CloseSearchPopup();
                    break;
            }
        }

        /// <summary>
        /// Opens the contact card popup
        /// Does: Closed -> Non expanded, or Non expanded -> Expanded 
        /// </summary>
        /// <returns>true if the contact card has been shown</returns>
        private bool OpenContactCardPopup()
        {
            if (this.CanShowContactCard)
            {
                this.CloseSearchPopup();
                if (!this.PresenceIndicator.ContactPopupCard.IsOpen)
                {
                    var hoverOnTarget = this.PresenceIndicator.ContactPopupCard.HoverOnTarget;
                    if (hoverOnTarget != null)
                    {
                        FrameworkTools.InvokeEventHandlers(
                            hoverOnTarget,
                            Mouse.MouseMoveEvent,
                            new MouseEventArgs(Mouse.PrimaryDevice, 0));
                    }
                }
                else
                {
                    var clickTarget = this.PresenceIndicator.ContactPopupCard.SingleClickActionTarget;
                    if (clickTarget != null)
                    {
                        FrameworkTools.InvokeEventHandlers(clickTarget, ButtonBase.ClickEvent);
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Closes the contact card popup
        /// Does: Expanded -> Non expanded, or Non expanded -> closed 
        /// </summary>
        /// <param name="closeAll">
        /// If tru alwys closes completely
        /// </param>
        private void CloseContactCardPopup(bool closeAll = false)
        {
            var popup = this.PresenceIndicator.ContactPopupCard;
            if ((popup == null) || !popup.IsOpen || popup.IsPinned)
            {
                return;
            }

            // issue force close event by faking a sign off event
            popup.ForEachVisualChild(delegate(DependencyObject visualChild)
                {
                    var button = visualChild as SplitButton;
                    if (button != null)
                    {
                        button.IsMenuOpen = false;
                    }
                });

            if (closeAll || !popup.IsExpanded)
            {
                var hoverOffTarget = this.PresenceIndicator.ContactPopupCard.HoverOffTarget;
                if (hoverOffTarget != null)
                {
                    FrameworkTools.InvokeEventHandlers(
                        hoverOffTarget,
                        Mouse.MouseLeaveEvent,
                        new MouseEventArgs(Mouse.PrimaryDevice, 0));
                }
            }

            popup.SelectedTabIndex = 0;
            popup.IsExpanded = false;
            popup.IsFloating = false;
            popup.IsPinned = false;
            popup.UpdateLayout();
        }

        /// <summary>
        /// Opens the search context menu
        /// </summary>
        private void OpenContextMenu()
        {
            this.CloseContactCardPopup(true);
            this.CloseSearchPopup();
            this.InputHelperContextMenu.PlacementTarget = this.InputHelper;
            this.InputHelperContextMenu.Placement = PlacementMode.Bottom;
            this.InputHelperContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Closes the search context menu
        /// </summary>
        private void CloseContextMenu()
        {
            this.InputHelperContextMenu.IsOpen = false;
        }

        /// <summary>
        /// Opens the search popup
        /// </summary>
        /// <param name="searchInput">
        /// The search Input.
        /// </param>
        private void OpenSearchPopup(string searchInput)
        {
            this.CloseContactCardPopup(true);
            this.SearchPopup.IsOpen = true;
            // needed to assure value is always updated
            this.contactSearchBox.SearchInputBox.SearchTextInput = string.Empty;
            this.contactSearchBox.SearchInputBox.SearchTextInput = searchInput;            
        }

        /// <summary>
        /// Closes the search popup
        /// </summary>
        private void CloseSearchPopup()
        {
            if (this.SearchPopup.IsOpen)
            {
                this.SearchPopup.IsOpen = false;
                this.Focus();
            }
        }

        /// <summary>
        /// Updates the enabled states and the visiblity of the presence indicator based on the signed in state
        /// </summary>
        private void UpdateEnabledSignedInState()
        {
            this.IsEnabled = this.CanEnable && this.PresenceIndicator.IsSignedIn;
            if (this.PresenceIndicator.IsSignedIn)
            {
                this.PresenceIndicator.Visibility = Visibility.Visible;
                this.DisplayName.Visibility = Visibility.Visible;
                this.SignInError.Visibility = Visibility.Collapsed;
                this.LyncIcon.Visibility = Visibility.Hidden;                
            }
            else
            {
                this.PresenceIndicator.Visibility = Visibility.Hidden;
                this.DisplayName.Visibility = Visibility.Collapsed;
                this.SignInError.Text = StringResources.SignIntoLync;
                this.SignInError.Visibility = Visibility.Visible;
                this.LyncIcon.Visibility = Visibility.Visible;
            }
            
            this.Focusable = this.PresenceIndicator.IsSignedIn;
            this.InitInputHelperContextMenu();
        }

        /// <summary>
        /// Contact in presence indicator has changed
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Event arguments</param>
        private void OnPresenceIndicatorSourceChanged(object sender, EventArgs e)
        {
            if (this.ContactChanged != null)
            {
                this.ContactChanged(this, e);
            }
        }

        /// <summary>
        /// Contact in presence indicator has changed
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Event arguments</param>
        private void OnPresenceIndicatorIsSignedInChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledSignedInState();
        }

        /// <summary>
        /// Search event handler
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void OnSearchMenuItemClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                this.OpenSearchPopup(menuItem.Header.ToString());
            }
        }

        /// <summary>
        /// Search menu item event handler
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void OnCustomSearchMenuClick(object sender, RoutedEventArgs e)
        {
            this.OpenSearchPopup(string.Empty);
        }

        /// <summary>
        /// Button click event handler
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void OnInputHelperButtonClick(object sender, RoutedEventArgs e)
        {
            this.OpenContextMenu();
        }

        /// <summary>
        /// Open contact contact card menu item event handler
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void OnOpenContactDetailsMenuClick(object sender, RoutedEventArgs e)
        {
            this.OpenContactCardPopup();
        }        

        /// <summary>
        /// Remove contact menu item event handler
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void OnRemoveContactMenuClick(object sender, RoutedEventArgs e)
        {
            if (!this.IsReadOnly)
            {
                this.PresenceIndicator.Source = null;
            }
        }

        /// <summary>
        /// Dependency Property has changed
        /// </summary>
        /// <param name="e">
        /// The event arguments
        /// </param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == CanEnableProperty)
            {
                this.UpdateEnabledSignedInState();
            }
        }

        /// <summary>
        /// Gets or sets the Presence Sip URI
        /// </summary>
        public string PresenceAddress
        {
            get
            {
                return this.PresenceIndicator.Source != null ? this.PresenceIndicator.Source.ToString() : null;
            }

            set
            {
                this.PresenceIndicator.Source = value;
            }
        }

        /// <summary>
        /// Sets the search candidates and initializes the input helper menu with search candidates
        /// </summary>
        public void SetSearchCandidates()
        {
           this.SetSearchCandidates(null);
        }

        /// <summary>
        /// Sets the search candidates and initializes the input helper menu with search candidates
        /// </summary>
        /// <param name="candidates">List of search candidates</param>
        public void SetSearchCandidates(StringCollection candidates)
        {
            var count = candidates == null ? 0 : candidates.Count;
            var candidateArray = new string[count];
            if (candidates != null)
            {
                candidates.CopyTo(candidateArray, 0);
            }
            this.searchCandidates = new List<string>(candidateArray);
            this.InitInputHelperContextMenu();            
        }
        
        /// <summary>
        /// Initializes the input helper menu with search candidates
        /// </summary>
        private void InitInputHelperContextMenu()
        {
            // update labels from resources
            this.LookupMenuHeaderText.Text = StringResources.MenuItem_LookupMenuHeader;
            this.LookupMenuHeader.Visibility = this.IsSearchAllowed ? Visibility.Visible : Visibility.Collapsed; 
            this.CustomSearch.Header = StringResources.MenuItem_CustomSearch;
            this.CustomSearch.Visibility = this.IsSearchAllowed ? Visibility.Visible : Visibility.Collapsed;
            this.ContactMenuHeaderText.Text = StringResources.MenuItem_ContactMenuHeader;            
            this.ShowContact.Header = StringResources.MenuItem_ShowContact;
            this.RemoveContact.Header = StringResources.MenuItem_RemoveContact;
            this.RemoveContact.IsEnabled = !this.IsReadOnly;
            
            // delete all search Items before Seperator            
            while ((this.InputHelperContextMenu.Items.Count > 1) && 
                  (this.InputHelperContextMenu.Items[1] != this.SearchContextMenuSeparator))
            {
                this.InputHelperContextMenu.Items.RemoveAt(1);
            }

            // add search suggestion menu items
            this.SearchContextMenuSeparator.Visibility = Visibility.Collapsed;                
            if (this.searchCandidates == null)
            {
                return;
            }

            var menuCandidates =
                this.searchCandidates.Where(candidate => (candidate.Trim().Length > 0));
            if (!this.IsSearchAllowed || !menuCandidates.Any())
            {
                return;
            }

            this.SearchContextMenuSeparator.Visibility = Visibility.Visible;
            foreach (var menuItem in menuCandidates.Select(candidate => new MenuItem { Header = candidate.Trim(), }))
            {
                menuItem.Click += this.OnSearchMenuItemClick;
                this.InputHelperContextMenu.Items.Insert(1, menuItem);
            }
            this.SearchContextMenuSeparator.Visibility = Visibility.Visible;
        }
    }
}