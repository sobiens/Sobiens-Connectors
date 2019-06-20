using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class ToogleButtonExt : UserControl
    {
        [DefaultValue(false)]
        public bool IsChecked { get; set; }
        public string CheckedText { get; set; }
        public string UnCheckedText { get; set; }
        public Image Image
        {
            get
            {
                return ToogleButton.Image;
            }
            set
            {
                ToogleButton.Image = value;
            }
        }
        public event System.EventHandler ButtonClick;

        public ToogleButtonExt()
        {
            InitializeComponent();
            this.SetVisualState();
        }


        private void ToogleButton_Click(object sender, EventArgs e)
        {
            if (IsChecked == true)
            {
                this.IsChecked = false;
            }
            else
            {
                this.IsChecked = true;
            }
            this.SetVisualState();
            if (ButtonClick != null)
                ButtonClick(sender, e);
//            ToogleButton_MouseHover(null, null);
        }

        private void SetVisualState()
        {
            if (IsChecked == true)
            {
                ToogleButton.BackColor = Color.Orange;
            }
            else
            {
                ToogleButton.BackColor = Color.Gray;
            }
        }

        private void ToogleButton_MouseHover(object sender, EventArgs e)
        {
            if (IsChecked == true)
                ToogleButtonToolTip.SetToolTip(ToogleButton, CheckedText);
            else
                ToogleButtonToolTip.SetToolTip(ToogleButton, UnCheckedText);
        }

        private void ToogleButton_MouseLeave(object sender, EventArgs e)
        {
            ToogleButtonToolTip.Hide(this);
        }

        private void ToogleButtonExt_Load(object sender, EventArgs e)
        {
            this.SetVisualState();
        }
    }
}
