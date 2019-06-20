using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using System.IO;
using Sobiens.Office.SharePointOutlookConnector.BLL;
using System.Collections;
using EmailUploader.BLL;
using EmailUploader.BLL.Entities;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using System.Xml;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class SharePointExplorerPane : UserControl
    {
        public Outlook.Application Application = null;
        public Inspector Inspector = null;
        public SharePointExplorerPane()
        {
            InitializeComponent();
        }

        private void SharePointExplorerPane_Load(object sender, EventArgs e)
        {
            try
            {

                if (EUSettingsManager.GetInstance().Settings == null)
                {
                    MessageBox.Show("You need to configure settings first.");
                    SettingsForm settingsControl = new SettingsForm();
                    settingsControl.ShowDialog();
                }
                this.SPFoldersTreeView.Initialize(Application);
                this.sharePointListViewControl.Initialize(Application);
                List<EUTreeNode> euNodes = EUSettingsManager.GetInstance().LoadSPTreeview();
                if (euNodes == null)
                {
                    SPFoldersTreeView.ClearAllNodes();
                }
                else
                {
                    SPFoldersTreeView.FillNodes(euNodes);
                    SPFoldersTreeView.ExpandNodes(euNodes);
                }
                socActionBar1.Application = Application;
            }
            catch (System.Exception ex)
            {
                LogManager.Log("An error occured:" + ex.Message, EULogModes.Normal);
                MessageBox.Show("An error occured:" + ex.Message);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            //Point pos = SPFoldersTreeView.PointToClient(new Point(e.X, e.Y));
            //TreeNode targetNode = SPFoldersTreeView.GetNodeAt(pos);
            //if (targetNode == null)
            //    return;
            //EUFolder dragedFolder = targetNode.Tag as EUFolder;
            //if (dragedFolder == null)
            //    return;
            //EUList dragedList = targetNode.Parent.Tag as EUList;
            //EUWeb dragedWeb = targetNode.Parent.Parent.Tag as EUWeb;

        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
        }

        private void socActionBar1_RefreshButtonClick(object sender, EventArgs e)
        {
            SPFoldersTreeView.DeleteState();
            SPFoldersTreeView.ClearAllNodes();
        }

        private void sharePointListViewControl_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void SPFoldersTreeView_After_Select(ISPCFolder folder)
        {
            if (socActionBar1.IsListViewVisible == false)
                return;
            sharePointListViewControl.SelectFolder(folder);
        }

        private void socActionBar1_Properties_CheckedChanged(object sender, EventArgs e)
        {
            SetPanelVisibilities();
        }

        private void socActionBar1_ListView_CheckedChanged(object sender, EventArgs e)
        {
            SetPanelVisibilities();
        }

        private void socActionBar1_Hierarchy_CheckedChanged(object sender, EventArgs e)
        {
            SetPanelVisibilities();
        }

        private void SetPanelVisibilities()
        {
            if (socActionBar1.IsHierarchyVisible == false && socActionBar1.IsListViewVisible == false)
            {
                FolderTreeviewSplitContainer.Visible = false;
                return;
            }
            else
            {
                FolderTreeviewSplitContainer.Visible = true;
            }
            FolderTreeviewSplitContainer.Panel1Collapsed = !socActionBar1.IsHierarchyVisible;
            if (socActionBar1.IsListViewVisible == false)
            {
                FolderTreeviewSplitContainer.Panel2Collapsed = true;
                return;
            }
            else
            {
                FolderTreeviewSplitContainer.Panel2Collapsed = false;
            }
        }

        private void listItemPropertyGrid_Leave(object sender, EventArgs e)
        {
            SetPanelVisibilities();
        }

        private void sharePointListViewControl_EditListItemSelected(object sender, EventArgs e)
        {
        }
    }
}
