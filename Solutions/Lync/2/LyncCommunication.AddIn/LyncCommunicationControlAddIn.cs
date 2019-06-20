// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LyncCommunicationControlAddIn.cs" company="Microsoft Corporation">
//   Copyright © Microsoft Corporation.  All Rights Reserved.
// </copyright>
// <summary>
//   Add-In components for the a Lync contact presence field
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LyncCommunicationAddIn
{
    using Microsoft.Dynamics.Framework.UI.Extensibility;
    using Microsoft.Dynamics.Framework.UI.Extensibility.WinForms;
    using Microsoft.Lync.Controls;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Forms.Integration;
    using HorizontalAlignment = System.Windows.HorizontalAlignment;

    /// <summary>Control Add-in class that provides an input field with Presence indication</summary>
    [ControlAddInExport("LyncCommunicationControlAddIn")]
    public class LyncCommunicationControlAddIn : StringControlAddInBase
    {
        #region private fields

        /// <summary>
        /// The control for the RTC
        /// </summary>
        private LyncCommunicationControl lyncCommunicationControl;

        /// <summary>
        /// The control that hosts the presence field and is returned to the RTC
        /// </summary>
        private ElementHost presenceHost;

        /// <summary>
        /// Value indicating that the presence contact has changed and a new Sip URI can be saved.
        /// </summary>Save
        private bool presenceContactChanged;

        /// <summary>
        /// Value indicating whether App code wants to allow contact search
        /// </summary>
        private bool searchAllowed;

        /// <summary>
        /// Value indicating that ths stying shall be optimized for Factboxes
        /// </summary>
        private AddInControlContext addInControlContext;

        /// <summary>
        /// Value defining the size of th ePresence indicator
        /// 0: Hidden photo, small indicator, 1: small phote, 2: large photo
        /// </summary>
        private int presenceIndicatorSize = 1;
        #endregion

        /// <summary>Gets a value indicating whether the NAV field shall show a caption.</summary>
        public override bool AllowCaptionControl
        {
            get
            {
                return true;
            }
        }

        /// <summary>Gets a value indicating whether HasValueChanged.</summary>
        public override bool HasValueChanged
        {
            get
            {
                return this.presenceContactChanged;
            }
        }

        /// <summary>Gets or sets a value vor the value. This is expected to be e SIP URI</summary>
        public override string Value
        {
            get
            {
                return this.lyncCommunicationControl.PresenceAddress;
            }
            set
            {
                this.SetSipAddress(value);
            }
        }

        #region V7 custom interface for CAL developers
        /// <summary>
        /// Event will be fired when the AddIn is ready for communication through its API
        /// </summary>
        [ApplicationVisible]
        public event MethodInvoker AddInReady;

        /// <summary>
        /// Gets or sets a value indicating whether search funcionality shall be enabled
        /// </summary>
        [ApplicationVisible]
        public bool AllowSearch
        {
            get
            {
                return this.searchAllowed;
            }

            set
            {
                this.searchAllowed = value;
                this.UpdatePresenceFieldStates();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether search funcionality shall be enabled
        /// </summary>
        [ApplicationVisible]
        public int PresenceIndicatorSize
        {
            get
            {
                return this.presenceIndicatorSize;
            }

            set
            {
                this.presenceIndicatorSize = value;
                this.UpdatePresenceIndicatorSize();
            }
        }

        /// <summary>
        /// Adjusts the control for usage in taskpage / factboxes
        /// </summary>
        /// <param name="context">The context.</param>
        [ApplicationVisible]
        public void SetAddInControlContext(AddInControlContext context)
        {
            this.addInControlContext = context;
            switch (this.addInControlContext)
            {
                case AddInControlContext.TaskPage:
                    this.PresenceIndicatorSize = 1;
                    break;
                default:
                    this.PresenceIndicatorSize = 0;
                    break;
            }            
        }

        /// <summary>
        /// CAL method: sets the search candidates
        /// </summary>
        /// <param name="candidates">The search candidates</param>
        [ApplicationVisible]
        public void SetSearchCandidates(StringCollection candidates)
        {
            if (!this.SearchCandidatesAreEqual(candidates))
            {
                this.lyncCommunicationControl.SetSearchCandidates(candidates);
                this.SearchCandidates = new StringCollection();
                if (candidates != null)
                {
                    foreach (var candidate in candidates)
                    {
                        this.SearchCandidates.Add(candidate);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the currentlyt set SearchCandidates.
        /// </summary>
        public StringCollection SearchCandidates { get; private set; }

        /// <summary>
        /// CAL method: sets the SIP Address
        /// </summary>
        /// <param name="sipAddress">The Sip Uri</param>
        [ApplicationVisible]
        public void SetSipAddress(string sipAddress)
        {
            if (!string.Equals(this.lyncCommunicationControl.PresenceAddress, sipAddress, StringComparison.CurrentCulture))
            {
                this.lyncCommunicationControl.ContactChanged -= this.OnPresenceContactChanged;
                this.lyncCommunicationControl.PresenceAddress = sipAddress;
                this.presenceContactChanged = false;
                this.UpdatePresenceIndicatorSize();
                this.lyncCommunicationControl.ContactChanged += this.OnPresenceContactChanged;
            }
        }

        #endregion

        /// <summary>
        /// Creates the control for use in NAV RTC
        /// </summary>
        /// <returns>The control inbstance</returns>
        protected override Control CreateControl()
        {
            this.searchAllowed = true;
            this.lyncCommunicationControl = new LyncCommunicationControl();
            this.lyncCommunicationControl.ContactChanged += this.OnPresenceContactChanged;
            this.presenceHost = new ElementHost
            {
                Child = this.lyncCommunicationControl,
                AutoSize = false,
            };

            // NOTE: This means that the context menu for the host control would be dissabled and if by any
            //       chance we host another control in the same host control, the control would not be able to use
            //       the host control context menu... thus it should declare one of its own
            var contextMenuStrip = this.presenceHost.ContextMenuStrip ?? (this.presenceHost.ContextMenuStrip = new ContextMenuStrip());
            contextMenuStrip.Opening += (sender, e) => { e.Cancel = true; };

            this.presenceHost.EnabledChanged += delegate { this.lyncCommunicationControl.CanEnable = this.presenceHost.Enabled; };
            this.presenceHost.ParentChanged += delegate
            {
                if (this.AddInReady != null)
                {
                    this.AddInReady();
                }
            };
            return this.presenceHost;
        }

        /// <summary>
        /// Release allocated resources
        /// </summary>
        /// <param name="disposing">Disposing</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                this.lyncCommunicationControl.ContactChanged -= this.OnPresenceContactChanged;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Called if the Editable state has changed
        /// </summary>
        /// <param name="editable">The New editable state</param>
        protected override void OnEditableChanged(bool editable)
        {
            base.OnEditableChanged(editable);
            this.UpdatePresenceFieldStates();
        }

        #region private tools
        /// <summary>
        /// Updates the states of the Presence field
        /// </summary>
        private void UpdatePresenceFieldStates()
        {
            this.lyncCommunicationControl.IsSearchAllowed = this.searchAllowed && (this.Site == null || this.Site.Editable);
            this.lyncCommunicationControl.IsReadOnly = (this.Site != null) && !this.Site.Editable;
        }

        /// <summary>
        /// Updates the states of the Presence field
        /// </summary>
        private void UpdatePresenceIndicatorSize()
        {
            PhotoDisplayMode mode;
            int height; 
            if (this.presenceIndicatorSize <= 0)
            {
                mode = PhotoDisplayMode.Hidden;
                height = 21;
            }
            else if (this.presenceIndicatorSize == 1)
            {
                mode = PhotoDisplayMode.Small;
                height = 36;
            }
            else
            {
                mode = PhotoDisplayMode.Large;
                height = 52;
            }

            this.lyncCommunicationControl.PhotoDisplayMode = mode;
            switch (this.addInControlContext)
            {
                case AddInControlContext.TaskPage:
                    this.lyncCommunicationControl.LayoutRoot.HorizontalAlignment = HorizontalAlignment.Stretch;
                    this.lyncCommunicationControl.InputHelper.Visibility = Visibility.Visible;
                    this.lyncCommunicationControl.DisplayName.Padding = new Thickness(8,0,8,0);
                    break;
                default:
                    this.lyncCommunicationControl.LayoutRoot.HorizontalAlignment = HorizontalAlignment.Right;
                    this.lyncCommunicationControl.InputHelper.Visibility = Visibility.Collapsed;
                    this.lyncCommunicationControl.DisplayName.Padding = new Thickness(0, 0, 0, 0);
                    break;
            }

            var sizeY = new DisplaySize(height, height, height);
            this.ApplySize(DisplaySize.Default, sizeY);
        }

        /// <summary>
        /// Compares twho sets of search candidates
        /// </summary>
        /// <param name="candidates">Ne search candidates</param>
        /// <returns>Boolean value indicating whether new search candidates have been passed in</returns>
        private bool SearchCandidatesAreEqual(ICollection candidates)
        {
            if (((this.SearchCandidates == null) || (this.SearchCandidates.Count == 0))  && ((candidates == null) || (candidates.Count == 0)))
            {
                return true;
            }

            if (((this.SearchCandidates == null) && (candidates != null)) || ((this.SearchCandidates != null) && (candidates == null)))
            {
                return false;
            }

            if (this.SearchCandidates.Count != candidates.Count)
            {
                return false;
            }

            return !candidates.Cast<string>().Where((t, i) => t != this.SearchCandidates[i]).Any();
        }

        /// <summary>
        /// Contact in presence indicator has changed
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Event arguments</param>
        private void OnPresenceContactChanged(object sender, EventArgs e)
        {
            this.presenceContactChanged = true;
            if (this.Site != null)
            {
                this.Site.SaveValue();
            }
        }
        #endregion
    }
}
