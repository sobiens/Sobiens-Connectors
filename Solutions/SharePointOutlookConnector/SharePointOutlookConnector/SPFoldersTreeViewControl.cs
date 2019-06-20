using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using EmailUploader.BLL.Entities;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.IO;
using Microsoft.Office.Interop.Outlook;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities.Gmail;

namespace Sobiens.Office.SharePointOutlookConnector
{

    public delegate void SPFoldersTreeViewControlAfter_Select(ISPCFolder folder);
    public partial class SPFoldersTreeViewControl : UserControl
    {
        // JOEL JEFFERY 20110711
        /// <summary>
        /// Occurs when [upload failed].
        /// </summary>
        public event EventHandler UploadFailed;

        // JON SILVER JULY 2011
        /// <summary>
        /// Occurs when [upload succeeded].
        /// </summary>
        public event EventHandler UploadSucceeded;
        private bool addedEventHandler = false;
        private List<Outlook.MailItem> emailItems;

        public event SPFoldersTreeViewControlAfter_Select After_Select;
        public Outlook.Application Application = null;
        private const int LoadingNodeTagValue = -1;
        private const int RootNodeTagValue = -2;
        private TreeNode SelectedNode = null;

        // JON SILVER JULY 2011
        // used for drag/drop operations - necessary to save state for context menu operations
        private bool MoveNotCopy = false;
        private TreeNode DragDropTargetNode;
        private DragEventArgs DragDropArgs;

        // JOEL JEFFERY 20110711
        /// <summary>
        /// Handles the UploadFailed event of the SharePointOutlookConnector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SharePointOutlookConnector_UploadFailed(object sender, EventArgs e)
        {
            refreshOutlookExplorerView();
            emailItems.Clear();
        }

        // JOEL JEFFERY 20110712
        // JON SILVER JULY 2011
        /// <summary>
        /// Handles the UploadSucceeded event of the SharePointOutlookConnector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SharePointOutlookConnector_UploadSucceeded(object sender, EventArgs e)
        {
            if (MoveNotCopy)
                foreach (Outlook.MailItem mailItem in emailItems)
                    mailItem.Delete();
            refreshOutlookExplorerView();
            emailItems.Clear();
            if (DragDropTargetNode != null)
                this.Invoke(new MethodInvoker(delegate { 
                    refreshNode(DragDropTargetNode);
                    After_Select((ISPCFolder)DragDropTargetNode.Tag);
                }));
        }

        // JON SILVER JUNE 2011 - Ugly workaround for "explorer not refreshing after item move" bug
        /// <summary>
        /// Refreshes the outlook explorer view.
        /// </summary>
        private void refreshOutlookExplorerView()
        {
            Outlook.MAPIFolder oFolder = Application.ActiveExplorer().CurrentFolder;
            Application.ActiveExplorer().ClearSelection();
            Application.ActiveExplorer().CurrentFolder = Application.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderDrafts);
            Application.ActiveExplorer().CurrentFolder = Application.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderDeletedItems);
            Application.ActiveExplorer().CurrentFolder = oFolder;
        }

        public SPFoldersTreeViewControl()
        {
            InitializeComponent();
        }

        public void Initialize(Outlook.Application _Application)
        {
            Application = _Application;
        }

        public void SaveState()
        {
            EUSettingsManager.GetInstance().SaveSharePointTreeViewState(SPFoldersTreeView);
        }

        private Inspector GetCurrentInspactor()
        {
            return GetCurrentSharePointExplorerPane().Inspector;
        }

        private SharePointExplorerPane GetCurrentSharePointExplorerPane()
        {
            return this.Parent.Parent.Parent as SharePointExplorerPane;
        }


        public void DeleteState()
        {
            EUSettingsManager.GetInstance().ClearSharePointTreeViewState();
        }

        private void SPFoldersTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            this.SaveState();
        }

        private void SPFoldersTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            refreshNode(e.Node);
        }

        // JOEL JEFFERY 20110712
        /// <summary>
        /// Refreshes the node.
        /// </summary>
        /// <param name="node">The node.</param>
        private void refreshNode(TreeNode node)
        {
            node.Nodes.Clear();
            //if (e.Node.Nodes.Count == 1)
            {
                this.Refresh();
                //if (IsLoadingNode(e.Node.Nodes[0]) == true)
                {
                    ISPCFolder folder = (ISPCFolder)node.Tag;
                    IOutlookConnector connector = OutlookConnector.GetConnector(folder.SiteSetting);
                    List<ISPCFolder> subFolders = connector.GetSubFolders(folder);
                    SPFoldersTreeView.BeginUpdate();
                    foreach (ISPCFolder subFolder in subFolders)
                    {
                        TreeNode folderNode = node.Nodes.Add(subFolder.Title);
                        folderNode.Tag = subFolder;
                        SetTreeNodeImage(folderNode);
                        AddLoadingNode(folderNode);
                    }
                    SPFoldersTreeView.EndUpdate();

                    //if (IsLoadingNode(e.Node.Nodes[0]) == true) 
                    //    e.Node.Nodes.RemoveAt(0);
                }
            }
            this.SaveState();
        }

        private void SPFoldersTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //refreshNode(e.Node);
            if (SPFoldersTreeView.SelectedNode == null)
                return;
            if (SelectedNode != null)
                SelectedNode.BackColor = Color.White;
            SelectedNode = SPFoldersTreeView.SelectedNode;
            SelectedNode.BackColor = Color.LightGray;
            if (After_Select == null)
                return;

            TreeNode tempNode = SPFoldersTreeView.SelectedNode;
            ISPCFolder folder = SPFoldersTreeView.SelectedNode.Tag as ISPCFolder;
            /*
            if (spObject as EUFolder != null)
            {
                folder = (EUFolder)spObject;
            }
            else if (spObject as EUList != null)
            {
                folder = ((EUList)spObject).RootFolder;
            }
             */
            After_Select(folder);
        }

        private TreeNode FindFromText(TreeNodeCollection nodes, string text)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text == text)
                    return node;
            }
            return null;
        }

        public void ExpandNodes(List<EUTreeNode> euNodes)
        {
            ExpandNodes(euNodes, SPFoldersTreeView.Nodes);
        }

        public void ExpandNodes(List<EUTreeNode> euNodes, TreeNodeCollection nodes)
        {
            foreach (EUTreeNode nodex in euNodes)
            {
                if (nodex.IsExpanded == false)
                    continue;
                TreeNode node = FindFromText(nodes, nodex.Text);
                if (node == null)
                    continue;
                node.Expand();
                ExpandNodes(nodex.Nodes, node.Nodes);
            }
        }

        public void FillNodes(List<EUTreeNode> euNodes)
        {
            FillNodes(euNodes, SPFoldersTreeView.Nodes);
        }

        public void FillNodes(List<EUTreeNode> euNodes, TreeNodeCollection nodes)
        {
            foreach (EUTreeNode nodex in euNodes)
            {
                TreeNode newNode = new TreeNode();
                newNode.Text = nodex.Text;
                newNode.Tag = nodex.Tag;
                nodes.Add(newNode);
                SetTreeNodeImage(newNode);
                if (nodex.IsExpanded == true)
                {
                    newNode.Expand();
                }
                FillNodes(nodex.Nodes, newNode.Nodes);
            }
        }
        private bool IsRootNode(TreeNode node)
        {
            if (node.Tag.GetType() == typeof(int) && ((int)node.Tag) == RootNodeTagValue)
                return true;
            return false;
        }
        private bool IsLoadingNode(TreeNode node)
        {
            if (node.Tag.GetType() == typeof(int) && ((int)node.Tag) == LoadingNodeTagValue)
                return true;
            return false;
        }
        private void AddLoadingNode(TreeNode node)
        {
            TreeNode childNode = new TreeNode("Loading...");
            childNode.Tag = LoadingNodeTagValue;
            node.Nodes.Add(childNode);
        }
        public void ClearAllNodes()
        {
            SPFoldersTreeView.Nodes.Clear();
            if (EUSettingsManager.GetInstance().Settings == null || EUSettingsManager.GetInstance().Settings.SiteSettings == null)
            {
                return;
            }
            foreach (EUSiteSetting siteSetting in EUSettingsManager.GetInstance().Settings.SiteSettings)
            {
                IOutlookConnector connector = OutlookConnector.GetConnector(siteSetting);
                ISPCFolder rootFolder = connector.GetRootFolder(siteSetting);
                TreeNode rootNode = new TreeNode(siteSetting.ToString());
                rootNode.Tag = rootFolder;
                SPFoldersTreeView.Nodes.Add(rootNode);
                SetTreeNodeImage(rootNode);
                AddLoadingNode(rootNode);
            }
        }

        private void LoadWebs(TreeNode node, string webUrl, EUSiteSetting siteSetting)
        {
            SPFoldersTreeView.BeginUpdate();
            List<EUWeb> webs = SharePointManager.GetWebs(webUrl, siteSetting);
            foreach (EUWeb web in webs)
            {
                TreeNode webNode = node.Nodes.Add(web.Title);
                webNode.Tag = web;
                SetTreeNodeImage(webNode);
                AddLoadingNode(webNode);
            }
            SPFoldersTreeView.EndUpdate();
        }

        private void LoadLists(TreeNode node, string webUrl, EUSiteSetting siteSetting)
        {
            SPFoldersTreeView.BeginUpdate();
            List<EUList> lists = SharePointManager.GetLists(webUrl, siteSetting);
            foreach (EUList list in lists)
            {
                if (
                    (list.ServerTemplate == 101 || list.ServerTemplate == 100 || list.BaseType == 1) //or BaseType == 1 - JOEL JEFFERY 20110708
                    && list.Hidden == false
                    )
                {
                    TreeNode listNode = node.Nodes.Add(list.Title);
                    listNode.Tag = list;
                    SetTreeNodeImage(listNode);
                    AddLoadingNode(listNode);

                    //                    TreeNode rootFolderNode = listNode.Nodes.Add(list.RootFolder.Title);
                    //                    rootFolderNode.Tag = list.RootFolder;
                    //                    SetTreeNodeImage(rootFolderNode);
                    //                    AddLoadingNode(rootFolderNode);
                    if (list.ServerTemplate == 101)
                    {
                    }
                }
            }
            SPFoldersTreeView.EndUpdate();
        }
        /*
        private void LoadFolders(TreeNode node, int serverTemplate, string webUrl, string listName, string folderPath, string rootFolderPath, EUSiteSetting siteSetting)
        {
            SPFoldersTreeView.BeginUpdate();
            List<EUFolder> folders = SharePointManager.GetFolders(serverTemplate, webUrl, listName, folderPath, rootFolderPath, siteSetting);
            foreach (EUFolder folder in folders)
            {
                TreeNode folderNode = node.Nodes.Add(folder.Title);
                folderNode.Tag = folder;
                SetTreeNodeImage(folderNode);
                AddLoadingNode(folderNode);
            }
            SPFoldersTreeView.EndUpdate();
        }
         */

        private void SetTreeNodeImage(TreeNode node)
        {
            object spObject = node.Tag;
            if (spObject as EUWeb != null)
            {
                node.SelectedImageKey = "stsicon.gif";
                node.ImageKey = "stsicon.gif";
            }
            else if (spObject as EUList != null)
            {
                node.SelectedImageKey = "itgen.gif";
                node.ImageKey = "itgen.gif";
            }
            else if (spObject as EUFolder != null)
            {
                node.SelectedImageKey = "folder.gif";
                node.ImageKey = "folder.gif";
            }
            else if (spObject as GFolder != null && node.Parent == null)
            {
                node.SelectedImageKey = "google-docs.png";
                node.ImageKey = "google-docs.png";
            }
        }
        protected TreeNode FindTreeNode(TreeNode rootNode, int x, int y)
        {
            TreeNode aNode = rootNode;

            Point pt = new Point(x, y);
            pt = PointToClient(pt);

            while (aNode != null)
            {
                if (aNode.Bounds.Contains(pt))
                {
                    return aNode;
                }
                aNode = aNode.NextVisibleNode;
            }

            return null;
        }

        private void SPFoldersTreeView_DragDrop(object sender, DragEventArgs e)
        {
            Point pos = SPFoldersTreeView.PointToClient(new Point(e.X, e.Y));
            DragDropTargetNode = SPFoldersTreeView.GetNodeAt(pos);
            DragDropArgs = e;

            // JON SILVER JUNE 2011
            MoveNotCopy = false;

            if (DragDropArgs.Effect == DragDropEffects.Move)// && !EUSettingsManager.GetInstance().Settings.SaveAsWord)
            {
                contextMenuStrip1.Show(this, pos);
                return;
            }

            CopyOrMoveEmailToFolder();
            // JON SILVER JUNE 2011
        }

        // JOEL JEFFERY 20110708
        /// <summary>
        /// Copies or moves the email to folder.
        /// </summary>
        private void CopyOrMoveEmailToFolder()
        {
            CopyOrMoveEmailToFolder(SaveFormatOverride.None);
        }

        /// <summary>
        /// Copies or moves the email to folder.
        /// </summary>
        /// <param name="format">The format.</param>
        private void CopyOrMoveEmailToFolder(SaveFormatOverride format) // JOEL JEFFERY 20110708 added SaveFormatOverride format
        {
            if (DragDropTargetNode == null)
                return;

            EUFolder dragedFolder = DragDropTargetNode.Tag as EUFolder;

            if (dragedFolder == null)
                return;

            SharePointListViewControl sharePointListViewControl = GetCurrentSharePointExplorerPane().sharePointListViewControl;
            if (sharePointListViewControl.SelectedFolder == null || sharePointListViewControl.SelectedFolder.UniqueIdentifier != dragedFolder.UniqueIdentifier || sharePointListViewControl.SelectedFolder.Title != dragedFolder.Title)
                sharePointListViewControl = null;

            emailItems = new List<MailItem>();

            if (DragDropArgs.Data.GetDataPresent("RenPrivateSourceFolder") == false) // if it's not a folder???
            {
                // JON SILVER SAYS... I've never been able to get this bit to fire... therefore under exactly what circumstances would e.Data.GetDataPresent("RenPrivateSourceFolder") == false ??
                emailItems.Add(Application.ActiveExplorer().Selection[1] as Outlook.MailItem);
            }
            else
            {
                // JON SILVER SAYS... we always seem to end up here regardless of what's dropped
                for (int i = 0; i < Application.ActiveExplorer().Selection.Count; i++)
                {
                    Outlook.MailItem item = Application.ActiveExplorer().Selection[i + 1] as Outlook.MailItem;
                    emailItems.Add(item);
                }
            }

            bool isListItemAndAttachmentMode = false;
            EUFieldInformations fieldInformations = null;
            EUFieldCollection fields = null;
            if (dragedFolder as EUFolder != null && ((EUFolder)dragedFolder).IsDocumentLibrary == false)
            {
                if (((EUFolder)dragedFolder).EnableAttachments == false)
                {
                    MessageBox.Show("In order to upload email, you need to enable attachment feature in this list.");
                    return;
                }
                isListItemAndAttachmentMode = true;
            }

            List<EUEmailUploadFile> emailUploadFiles = CommonManager.GetEmailUploadFiles(emailItems, DragDropArgs, isListItemAndAttachmentMode, format);

            // JON SILVER JUNE 2011
            if (!addedEventHandler)
            {
                EUEmailManager.UploadFailed += new EventHandler(SharePointOutlookConnector_UploadFailed);
                EUEmailManager.UploadSucceeded += new EventHandler(SharePointOutlookConnector_UploadSucceeded);
                addedEventHandler = true;
            }

            // JOEL JEFFERY 20110712
            try
            {
                // JON SILVER JUNE 2011
                EUEmailManager.UploadEmail(sharePointListViewControl, dragedFolder, DragDropArgs, emailUploadFiles, isListItemAndAttachmentMode);
            }
            catch (System.Exception ex)
            {
                if (UploadFailed != null)
                    UploadFailed(this, new EventArgs());
                throw ex;
            }
        }


        /// <summary>
        /// Handles the DragEnter event of the SPFoldersTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        private void SPFoldersTreeView_DragEnter(object sender, DragEventArgs e)
        {
            // JON SILVER JUNE 2011
            if ((e.KeyState & 2) == 2) //right mouse button
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.All;
            }
            // JON SILVER JUNE 2011

            // WAS
            // e.Effect = DragDropEffects.All;
        }

        private void FolderTreeviewContextMenuStrip_Opened(object sender, EventArgs e)
        {

        }

        private void FolderTreeviewContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            EUList list = SPFoldersTreeView.SelectedNode.Tag as EUList;
            if (list == null)
                listSettingsToolStripMenuItem.Enabled = false;
            else
                listSettingsToolStripMenuItem.Enabled = true;
        }

        private void setAsEmailAttachmentFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SPFoldersTreeView.SelectedNode == null)
            {
                MessageBox.Show("Please select a folder first.");
            }
            object spObject = SPFoldersTreeView.SelectedNode.Tag;
            if (spObject as EUFolder != null || spObject as EUList != null)
            {
                EUFolder folder = (EUFolder)spObject;
                EUSettingsManager.GetInstance().Settings.EmailAttachmentRootFolderUrl = folder.RootFolderPath;
                EUSettingsManager.GetInstance().Settings.EmailAttachmentRootWebUrl = folder.SiteSetting.Url;
                EUSettingsManager.GetInstance().Settings.EmailAttachmentFolderUrl = folder.FolderPath;
                EUSettingsManager.GetInstance().Settings.EmailAttachmentWebUrl = folder.WebUrl;
                EUSettingsManager.GetInstance().Settings.EmailAttachmentListName = folder.ListName;
                EUSettingsManager.GetInstance().SaveSettings();
                MessageBox.Show("You have successfuly saved your email attachment folder.");
            }
            else
            {
                MessageBox.Show("Please select a SharePoint folder first.");
            }
        }

        private void alertsMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EUFolder folder = SPFoldersTreeView.SelectedNode.Tag as EUFolder;
            EUList list = SPFoldersTreeView.SelectedNode.Tag as EUList;
            EUWeb web = SPFoldersTreeView.SelectedNode.Tag as EUWeb;
            EUSiteSetting siteSetting = null;
            string webUrl = String.Empty;
            if (folder != null)
            {
                webUrl = folder.WebUrl;
                siteSetting = folder.SiteSetting;
            }
            else if (list != null)
            {
                webUrl = list.WebUrl;
                siteSetting = list.SiteSetting;
            }
            else if (web != null)
            {
                webUrl = web.Url;
                siteSetting = web.SiteSetting;
            }
            if (AlertManager.CheckSobiensAlertServiceEnability(siteSetting, webUrl) == false)
            {
                SobiensAlertServiceDisabledForm sobiensAlertServiceDisabledForm = new SobiensAlertServiceDisabledForm();
                sobiensAlertServiceDisabledForm.ShowDialog();
                return;
            }

            AlertMaintenanceForm alertMaintenanceForm = new AlertMaintenanceForm();
            alertMaintenanceForm.Initialize(webUrl, siteSetting);
            alertMaintenanceForm.ShowDialog();
        }

        private void listSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EUList selectedList = SPFoldersTreeView.SelectedNode.Tag as EUList;

            SettingsForm settingsForm = new SettingsForm();
            settingsForm.SetSelectedListSetting(selectedList.WebUrl, selectedList.WebUrl.TrimEnd(new char[] { '/' }) + "/" + selectedList.FolderPath.TrimStart(new char[] { '/' }), selectedList.Title);
            settingsForm.ShowDialog();
        }

        // JON SILVER JUNE 2011
        // Goes with context menu created using designer
        /// <summary>
        /// Handles the Click event of the cancelToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Cancel");
            // Essentially, cancel means do nothing
        }

        // JOEL JEFFERY 20110708
        /// <summary>
        /// Handles the Click event of the copyItemsAsFilesStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void copyItemsAsFilesStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveNotCopy = false;
            CopyOrMoveEmailToFolder(SaveFormatOverride.Word);
        }

        // JOEL JEFFERY 20110708
        /// <summary>
        /// Handles the Click event of the copyItemsAsMsgToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void copyItemsAsMsgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveNotCopy = false;
            CopyOrMoveEmailToFolder(SaveFormatOverride.Email);
        }

        // JOEL JEFFERY 20110708
        /// <summary>
        /// Handles the Click event of the moveItemsAsMsgToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void moveItemsAsMsgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveNotCopy = true;
            CopyOrMoveEmailToFolder(SaveFormatOverride.Email);
        }

        // JOEL JEFFERY 20110712
        /// <summary>
        /// Handles the NodeMouseDoubleClick event of the SPFoldersTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        private void SPFoldersTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            refreshNode(e.Node);
        }

    }
}
