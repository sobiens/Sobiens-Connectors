namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class SharePointExplorerPane
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SharePointExplorerPane));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.TopPanel = new System.Windows.Forms.Panel();
            this.socActionBar1 = new Sobiens.Office.SharePointOutlookConnector.SOCActionBar();
            this.ListItemContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.approveRejectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.versionHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sharePointListViewControl = new Sobiens.Office.SharePointOutlookConnector.SharePointListViewControl();
            this.FolderTreeviewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.SPFoldersTreeView = new Sobiens.Office.SharePointOutlookConnector.SPFoldersTreeViewControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.TopPanel.SuspendLayout();
            this.ListItemContextMenuStrip.SuspendLayout();
            this.FolderTreeviewSplitContainer.Panel1.SuspendLayout();
            this.FolderTreeviewSplitContainer.Panel2.SuspendLayout();
            this.FolderTreeviewSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.gif");
            this.imageList1.Images.SetKeyName(1, "itgen.gif");
            this.imageList1.Images.SetKeyName(2, "stsicon.gif");
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.socActionBar1);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(210, 29);
            this.TopPanel.TabIndex = 3;
            // 
            // socActionBar1
            // 
            this.socActionBar1.Location = new System.Drawing.Point(3, 3);
            this.socActionBar1.Name = "socActionBar1";
            this.socActionBar1.Size = new System.Drawing.Size(189, 26);
            this.socActionBar1.TabIndex = 5;
            this.socActionBar1.ListView_CheckedChanged += new System.EventHandler(this.socActionBar1_ListView_CheckedChanged);
            this.socActionBar1.Properties_CheckedChanged += new System.EventHandler(this.socActionBar1_Properties_CheckedChanged);
            this.socActionBar1.RefreshButtonClick += new System.EventHandler(this.socActionBar1_RefreshButtonClick);
            this.socActionBar1.Hierarchy_CheckedChanged += new System.EventHandler(this.socActionBar1_Hierarchy_CheckedChanged);
            // 
            // ListItemContextMenuStrip
            // 
            this.ListItemContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.editPropertiesToolStripMenuItem,
            this.toolStripSeparator1,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator2,
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.toolStripSeparator3,
            this.approveRejectToolStripMenuItem,
            this.checkInToolStripMenuItem,
            this.checkOutToolStripMenuItem,
            this.toolStripSeparator4,
            this.versionHistoryToolStripMenuItem});
            this.ListItemContextMenuStrip.Name = "ListItemContextMenuStrip";
            this.ListItemContextMenuStrip.Size = new System.Drawing.Size(162, 270);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.OPEN;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // editPropertiesToolStripMenuItem
            // 
            this.editPropertiesToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.EDIT;
            this.editPropertiesToolStripMenuItem.Name = "editPropertiesToolStripMenuItem";
            this.editPropertiesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.editPropertiesToolStripMenuItem.Text = "Edit Properties";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.CUT;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.COPY;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.PASTE;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(158, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.DELETE;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.EDITICON;
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(158, 6);
            // 
            // approveRejectToolStripMenuItem
            // 
            this.approveRejectToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.APPRJ;
            this.approveRejectToolStripMenuItem.Name = "approveRejectToolStripMenuItem";
            this.approveRejectToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.approveRejectToolStripMenuItem.Text = "Approve/Reject";
            // 
            // checkInToolStripMenuItem
            // 
            this.checkInToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.CHECKIN;
            this.checkInToolStripMenuItem.Name = "checkInToolStripMenuItem";
            this.checkInToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.checkInToolStripMenuItem.Text = "Check In";
            // 
            // checkOutToolStripMenuItem
            // 
            this.checkOutToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.CHECKOUT;
            this.checkOutToolStripMenuItem.Name = "checkOutToolStripMenuItem";
            this.checkOutToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.checkOutToolStripMenuItem.Text = "Check Out";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(158, 6);
            // 
            // versionHistoryToolStripMenuItem
            // 
            this.versionHistoryToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.VERSIONS;
            this.versionHistoryToolStripMenuItem.Name = "versionHistoryToolStripMenuItem";
            this.versionHistoryToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.versionHistoryToolStripMenuItem.Text = "Version History";
            // 
            // sharePointListViewControl
            // 
            this.sharePointListViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sharePointListViewControl.Location = new System.Drawing.Point(0, 0);
            this.sharePointListViewControl.Name = "sharePointListViewControl";
            this.sharePointListViewControl.Size = new System.Drawing.Size(210, 264);
            this.sharePointListViewControl.TabIndex = 1;
            this.sharePointListViewControl.SelectionChanged += new System.EventHandler(this.sharePointListViewControl_SelectionChanged);
            this.sharePointListViewControl.EditListItemSelected += new System.EventHandler(this.sharePointListViewControl_EditListItemSelected);
            // 
            // FolderTreeviewSplitContainer
            // 
            this.FolderTreeviewSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FolderTreeviewSplitContainer.Location = new System.Drawing.Point(0, 29);
            this.FolderTreeviewSplitContainer.Name = "FolderTreeviewSplitContainer";
            this.FolderTreeviewSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // FolderTreeviewSplitContainer.Panel1
            // 
            this.FolderTreeviewSplitContainer.Panel1.Controls.Add(this.SPFoldersTreeView);
            // 
            // FolderTreeviewSplitContainer.Panel2
            // 
            this.FolderTreeviewSplitContainer.Panel2.Controls.Add(this.sharePointListViewControl);
            this.FolderTreeviewSplitContainer.Size = new System.Drawing.Size(210, 530);
            this.FolderTreeviewSplitContainer.SplitterDistance = 262;
            this.FolderTreeviewSplitContainer.TabIndex = 7;
            // 
            // SPFoldersTreeView
            // 
            this.SPFoldersTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SPFoldersTreeView.Location = new System.Drawing.Point(0, 0);
            this.SPFoldersTreeView.Name = "SPFoldersTreeView";
            this.SPFoldersTreeView.Size = new System.Drawing.Size(210, 262);
            this.SPFoldersTreeView.TabIndex = 2;
            this.SPFoldersTreeView.After_Select += new Sobiens.Office.SharePointOutlookConnector.SPFoldersTreeViewControlAfter_Select(this.SPFoldersTreeView_After_Select);
            // 
            // SharePointExplorerPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FolderTreeviewSplitContainer);
            this.Controls.Add(this.TopPanel);
            this.Name = "SharePointExplorerPane";
            this.Size = new System.Drawing.Size(210, 559);
            this.Load += new System.EventHandler(this.SharePointExplorerPane_Load);
            this.TopPanel.ResumeLayout(false);
            this.ListItemContextMenuStrip.ResumeLayout(false);
            this.FolderTreeviewSplitContainer.Panel1.ResumeLayout(false);
            this.FolderTreeviewSplitContainer.Panel2.ResumeLayout(false);
            this.FolderTreeviewSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer FolderTreeviewSplitContainer;
        private System.Windows.Forms.ContextMenuStrip ListItemContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem approveRejectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem versionHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkInToolStripMenuItem;
        private SPFoldersTreeViewControl SPFoldersTreeView;
        private SOCActionBar socActionBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        public SharePointListViewControl sharePointListViewControl;
    }
}
