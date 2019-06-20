namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class SPFoldersTreeViewControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPFoldersTreeViewControl));
            this.SPFoldersTreeView = new System.Windows.Forms.TreeView();
            this.FolderTreeviewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setAsEmailAttachmentFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alertsMaintenanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyItemsAsFilesStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyItemsAsMsgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveItemsAsMsgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderTreeviewContextMenuStrip.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SPFoldersTreeView
            // 
            this.SPFoldersTreeView.AllowDrop = true;
            this.SPFoldersTreeView.ContextMenuStrip = this.FolderTreeviewContextMenuStrip;
            this.SPFoldersTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SPFoldersTreeView.ImageIndex = 0;
            this.SPFoldersTreeView.ImageList = this.imageList1;
            this.SPFoldersTreeView.Location = new System.Drawing.Point(0, 0);
            this.SPFoldersTreeView.Name = "SPFoldersTreeView";
            this.SPFoldersTreeView.SelectedImageIndex = 0;
            this.SPFoldersTreeView.Size = new System.Drawing.Size(150, 150);
            this.SPFoldersTreeView.TabIndex = 0;
            this.SPFoldersTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.SPFoldersTreeView_AfterCollapse);
            this.SPFoldersTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.SPFoldersTreeView_AfterExpand);
            this.SPFoldersTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SPFoldersTreeView_AfterSelect);
            this.SPFoldersTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.SPFoldersTreeView_NodeMouseDoubleClick);
            this.SPFoldersTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.SPFoldersTreeView_DragDrop);
            this.SPFoldersTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.SPFoldersTreeView_DragEnter);
            // 
            // FolderTreeviewContextMenuStrip
            // 
            this.FolderTreeviewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setAsEmailAttachmentFolderToolStripMenuItem,
            this.alertsMaintenanceToolStripMenuItem,
            this.listSettingsToolStripMenuItem});
            this.FolderTreeviewContextMenuStrip.Name = "contextMenuStrip1";
            this.FolderTreeviewContextMenuStrip.Size = new System.Drawing.Size(235, 70);
            this.FolderTreeviewContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.FolderTreeviewContextMenuStrip_Opening);
            this.FolderTreeviewContextMenuStrip.Opened += new System.EventHandler(this.FolderTreeviewContextMenuStrip_Opened);
            // 
            // setAsEmailAttachmentFolderToolStripMenuItem
            // 
            this.setAsEmailAttachmentFolderToolStripMenuItem.Name = "setAsEmailAttachmentFolderToolStripMenuItem";
            this.setAsEmailAttachmentFolderToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.setAsEmailAttachmentFolderToolStripMenuItem.Text = "Set as email attachment folder";
            this.setAsEmailAttachmentFolderToolStripMenuItem.Click += new System.EventHandler(this.setAsEmailAttachmentFolderToolStripMenuItem_Click);
            // 
            // alertsMaintenanceToolStripMenuItem
            // 
            this.alertsMaintenanceToolStripMenuItem.Name = "alertsMaintenanceToolStripMenuItem";
            this.alertsMaintenanceToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.alertsMaintenanceToolStripMenuItem.Text = "Alerts Maintenance";
            this.alertsMaintenanceToolStripMenuItem.Click += new System.EventHandler(this.alertsMaintenanceToolStripMenuItem_Click);
            // 
            // listSettingsToolStripMenuItem
            // 
            this.listSettingsToolStripMenuItem.Name = "listSettingsToolStripMenuItem";
            this.listSettingsToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.listSettingsToolStripMenuItem.Text = "List Settings";
            this.listSettingsToolStripMenuItem.Click += new System.EventHandler(this.listSettingsToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.gif");
            this.imageList1.Images.SetKeyName(1, "itgen.gif");
            this.imageList1.Images.SetKeyName(2, "stsicon.gif");
            this.imageList1.Images.SetKeyName(3, "google-docs.png");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyItemsAsMsgToolStripMenuItem,
            this.moveItemsAsMsgToolStripMenuItem,
            this.copyItemsAsFilesStripMenuItem,
            this.cancelToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(188, 114);
            // 
            // copyItemsAsFilesStripMenuItem
            // 
            this.copyItemsAsFilesStripMenuItem.Name = "copyItemsAsFilesStripMenuItem";
            this.copyItemsAsFilesStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.copyItemsAsFilesStripMenuItem.Text = "Copy item(s) as files";
            this.copyItemsAsFilesStripMenuItem.Click += new System.EventHandler(this.copyItemsAsFilesStripMenuItem_Click);
            // 
            // copyItemsAsMsgToolStripMenuItem
            // 
            this.copyItemsAsMsgToolStripMenuItem.Name = "copyItemsAsMsgToolStripMenuItem";
            this.copyItemsAsMsgToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.copyItemsAsMsgToolStripMenuItem.Text = "Copy item(s) as .msg";
            this.copyItemsAsMsgToolStripMenuItem.Click += new System.EventHandler(this.copyItemsAsMsgToolStripMenuItem_Click);
            // 
            // moveItemsAsMsgToolStripMenuItem
            // 
            this.moveItemsAsMsgToolStripMenuItem.Name = "moveItemsAsMsgToolStripMenuItem";
            this.moveItemsAsMsgToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.moveItemsAsMsgToolStripMenuItem.Text = "Move item(s) as .msg";
            this.moveItemsAsMsgToolStripMenuItem.Click += new System.EventHandler(this.moveItemsAsMsgToolStripMenuItem_Click);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.cancelToolStripMenuItem.Text = "Cancel";
            this.cancelToolStripMenuItem.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
            // 
            // SPFoldersTreeViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SPFoldersTreeView);
            this.Name = "SPFoldersTreeViewControl";
            this.FolderTreeviewContextMenuStrip.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView SPFoldersTreeView;
        private System.Windows.Forms.ContextMenuStrip FolderTreeviewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem setAsEmailAttachmentFolderToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem alertsMaintenanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listSettingsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyItemsAsFilesStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyItemsAsMsgToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveItemsAsMsgToolStripMenuItem;
    }
}
