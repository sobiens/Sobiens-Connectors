using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class ToogleButton : Button
    {
        [DefaultValue(false)]
        public bool IsChecked { get; set; }
        public ToogleButton()
        {
            InitializeComponent();
            this.Click += new EventHandler(ToogleButton_Click);
            this.MouseHover += new EventHandler(ToogleButton_MouseHover);
        }

        void ToogleButton_MouseHover(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ToogleButton_Click(object sender, EventArgs e)
        {
            if (IsChecked == true)
            {
                this.IsChecked = false;
                this.BackColor = Color.OrangeRed;
            }
            else
            {
                this.IsChecked = true;
                this.BackColor = Color.Gray;
            }

        }

    }
}
