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
    public partial class SOCActionBar : UserControl
    {
        public SOCActionBar()
        {
            InitializeComponent();
        }
        public Microsoft.Office.Interop.Outlook.Application Application = null;

        public event System.EventHandler RefreshButtonClick;
        public event System.EventHandler Hierarchy_CheckedChanged;
        public event System.EventHandler ListView_CheckedChanged;
        public event System.EventHandler Properties_CheckedChanged;


        public bool IsHierarchyVisible
        {
            get
            {
                return FolderTreeviewDisplayButton.IsChecked;
            }
        }

        public bool IsListViewVisible
        {
            get
            {
                return ListViewToogleButton.IsChecked;
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm settingsControl = new SettingsForm();
            settingsControl.ShowDialog();
            if (settingsControl.DialogResult == DialogResult.OK)
            {
                RefreshButtonClick(sender, e);
            }
        }

        private void RefreshButton1_Click(object sender, EventArgs e)
        {
            if (RefreshButtonClick != null)
                RefreshButtonClick(sender, e);
        }

        private void SobiensLogoPictureBox_Click(object sender, EventArgs e)
        {
            SobiensAboutBox sobiensAboutBox = new SobiensAboutBox();
            sobiensAboutBox.ShowDialog();
        }

        private void PropertiesToogleButton_ButtonClick(object sender, EventArgs e)
        {
            if (Properties_CheckedChanged != null)
                Properties_CheckedChanged(sender, e);
        }

        private void ListViewToogleButton_ButtonClick(object sender, EventArgs e)
        {
            if (ListView_CheckedChanged != null)
                ListView_CheckedChanged(sender, e);
        }

        private void FolderTreeviewDisplayButton_ButtonClick(object sender, EventArgs e)
        {
            if (Hierarchy_CheckedChanged != null)
                Hierarchy_CheckedChanged(sender, e);
        }
    }
}
