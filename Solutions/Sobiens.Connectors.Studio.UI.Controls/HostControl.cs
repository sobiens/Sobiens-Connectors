using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    public class HostControl : UserControl
    {
        private bool isInitialized = false;
        public HostWindow ParentWindow
        {
            get
            {
                return FindControls.FindParent<HostWindow>(this);
            }
        }

        private bool _IsValid;
        public virtual bool IsValid
        {
            get
            {
                return this._IsValid;
            }
            set
            {
                this._IsValid = value;
            }
        }

        public event EventHandler OKButtonSelected;
        public event EventHandler CancelButtonSelected;
        protected virtual void OnLoad() { }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (isInitialized == false)
            {
                if (this.ParentWindow != null)
                {
                    this.ParentWindow.OKButtonSelected += new EventHandler(ParentWindow_OKButtonSelected);
                    this.ParentWindow.CancelButtonSelected += new EventHandler(ParentWindow_CancelButtonSelected);
                }

                OnLoad();
                isInitialized = true;
            }
        }

        void ParentWindow_CancelButtonSelected(object sender, EventArgs e)
        {
            if (this.CancelButtonSelected != null)
            {
                this.CancelButtonSelected(sender, e);
            }
        }

        void ParentWindow_OKButtonSelected(object sender, EventArgs e)
        {
            if (this.OKButtonSelected != null)
            {
                this.OKButtonSelected(sender, e);
            }
        }

        public bool? ShowDialog(Window owner, string title, double? height, double? width, bool showActionButtons, bool showLogo)
        {
            return UIHelper.ShowDialog(owner, this, title, height, width, showActionButtons, showLogo);
        }

        public bool? ShowDialog(Window owner, string title, bool showActionButtons, bool showLogo)
        {
            return UIHelper.ShowDialog(owner, this, title, null, null, showActionButtons, showLogo);
        }

        public bool? ShowDialog(Window owner, string title)
        {
            return UIHelper.ShowDialog(owner, this, title, null, null, true, true);
        }

        public bool? ShowDialog(Window owner, string title, double? height, double? width)
        {
            return UIHelper.ShowDialog(owner, this, title, height, width, true, true);
        }

        public void Close(bool? dialogResult)
        {
            this.ParentWindow.DialogResult = dialogResult;
        }

        public void ShowLoadingStatus()
        {
            ParentWindow.ShowLoadingStatus();
        }
        public void ShowLoadingStatus(string message)
        {
            ParentWindow.ShowLoadingStatus(message);
        }
        public void HideLoadingStatus()
        {
            ParentWindow.HideLoadingStatus();
        }
        public void HideLoadingStatus(string message)
        {
            ParentWindow.HideLoadingStatus(message);
        }

        public void SetStatusText(string message)
        {
            ParentWindow.SetStatusText(message);
        }

        public void SetErrorMessage(string message)
        {
            ParentWindow.SetErrorMessage(message);
        }

    }
}
