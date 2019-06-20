namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class SharePointListViewControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SharePointListViewControl));
            this.LibraryContentDataGridView = new System.Windows.Forms.DataGridView();
            this.ExtensionImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.FilePathColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ListItemContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attachToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asHyperlinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asAnAttachmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.approveRejectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoCheckOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.versionHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ViewsComboBox = new System.Windows.Forms.ComboBox();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.PagingLabel = new System.Windows.Forms.Label();
            this.ChangeViewBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LoadingPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LibraryContentDataGridView)).BeginInit();
            this.ListItemContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LibraryContentDataGridView
            // 
            this.LibraryContentDataGridView.AllowDrop = true;
            this.LibraryContentDataGridView.AllowUserToAddRows = false;
            this.LibraryContentDataGridView.AllowUserToDeleteRows = false;
            this.LibraryContentDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LibraryContentDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.LibraryContentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LibraryContentDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExtensionImageColumn,
            this.FilePathColumn,
            this.TitleColumn});
            this.LibraryContentDataGridView.ContextMenuStrip = this.ListItemContextMenuStrip;
            this.LibraryContentDataGridView.Location = new System.Drawing.Point(3, 20);
            this.LibraryContentDataGridView.Name = "LibraryContentDataGridView";
            this.LibraryContentDataGridView.RowHeadersVisible = false;
            this.LibraryContentDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LibraryContentDataGridView.Size = new System.Drawing.Size(279, 131);
            this.LibraryContentDataGridView.TabIndex = 1;
            this.LibraryContentDataGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.LibraryContentDataGridView_ColumnHeaderMouseClick);
            this.LibraryContentDataGridView.SelectionChanged += new System.EventHandler(this.LibraryContentDataGridView_SelectionChanged);
            this.LibraryContentDataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.LibraryContentDataGridView_DragDrop);
            this.LibraryContentDataGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.LibraryContentDataGridView_DragEnter);
            this.LibraryContentDataGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LibraryContentDataGridView_MouseMove);
            this.LibraryContentDataGridView.Resize += new System.EventHandler(this.LibraryContentDataGridView_Resize);
            // 
            // ExtensionImageColumn
            // 
            this.ExtensionImageColumn.HeaderText = "";
            this.ExtensionImageColumn.Name = "ExtensionImageColumn";
            this.ExtensionImageColumn.ReadOnly = true;
            this.ExtensionImageColumn.Width = 30;
            // 
            // FilePathColumn
            // 
            this.FilePathColumn.HeaderText = "";
            this.FilePathColumn.Name = "FilePathColumn";
            this.FilePathColumn.Visible = false;
            // 
            // TitleColumn
            // 
            this.TitleColumn.HeaderText = "Name";
            this.TitleColumn.Name = "TitleColumn";
            // 
            // ListItemContextMenuStrip
            // 
            this.ListItemContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.attachToolStripMenuItem,
            this.editPropertiesToolStripMenuItem,
            this.toolStripSeparator1,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator2,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator3,
            this.approveRejectToolStripMenuItem,
            this.checkInToolStripMenuItem,
            this.undoCheckOutToolStripMenuItem,
            this.checkOutToolStripMenuItem,
            this.toolStripSeparator4,
            this.versionHistoryToolStripMenuItem});
            this.ListItemContextMenuStrip.Name = "ListItemContextMenuStrip";
            this.ListItemContextMenuStrip.Size = new System.Drawing.Size(164, 270);
            this.ListItemContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ListItemContextMenuStrip_Opening);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.OPEN;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // attachToolStripMenuItem
            // 
            this.attachToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asHyperlinkToolStripMenuItem,
            this.asAnAttachmentToolStripMenuItem});
            this.attachToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.ATTACH;
            this.attachToolStripMenuItem.Name = "attachToolStripMenuItem";
            this.attachToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.attachToolStripMenuItem.Text = "Attach";
            // 
            // asHyperlinkToolStripMenuItem
            // 
            this.asHyperlinkToolStripMenuItem.Name = "asHyperlinkToolStripMenuItem";
            this.asHyperlinkToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.asHyperlinkToolStripMenuItem.Text = "as a hyperlink";
            this.asHyperlinkToolStripMenuItem.Click += new System.EventHandler(this.asHyperlinkToolStripMenuItem_Click);
            // 
            // asAnAttachmentToolStripMenuItem
            // 
            this.asAnAttachmentToolStripMenuItem.Name = "asAnAttachmentToolStripMenuItem";
            this.asAnAttachmentToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.asAnAttachmentToolStripMenuItem.Text = "as an attachment";
            this.asAnAttachmentToolStripMenuItem.Click += new System.EventHandler(this.asAnAttachmentToolStripMenuItem_Click);
            // 
            // editPropertiesToolStripMenuItem
            // 
            this.editPropertiesToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.EDIT;
            this.editPropertiesToolStripMenuItem.Name = "editPropertiesToolStripMenuItem";
            this.editPropertiesToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.editPropertiesToolStripMenuItem.Text = "Edit Properties";
            this.editPropertiesToolStripMenuItem.Click += new System.EventHandler(this.editPropertiesToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.COPY;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.PASTE;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(160, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.DELETE;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(160, 6);
            // 
            // approveRejectToolStripMenuItem
            // 
            this.approveRejectToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.APPRJ;
            this.approveRejectToolStripMenuItem.Name = "approveRejectToolStripMenuItem";
            this.approveRejectToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.approveRejectToolStripMenuItem.Text = "Approve/Reject";
            this.approveRejectToolStripMenuItem.Click += new System.EventHandler(this.approveRejectToolStripMenuItem_Click);
            // 
            // checkInToolStripMenuItem
            // 
            this.checkInToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.CHECKIN;
            this.checkInToolStripMenuItem.Name = "checkInToolStripMenuItem";
            this.checkInToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.checkInToolStripMenuItem.Text = "Check In";
            this.checkInToolStripMenuItem.Click += new System.EventHandler(this.checkInToolStripMenuItem_Click);
            // 
            // undoCheckOutToolStripMenuItem
            // 
            this.undoCheckOutToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.UNCHKOUT;
            this.undoCheckOutToolStripMenuItem.Name = "undoCheckOutToolStripMenuItem";
            this.undoCheckOutToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.undoCheckOutToolStripMenuItem.Text = "Undo Check Out";
            this.undoCheckOutToolStripMenuItem.Click += new System.EventHandler(this.undoCheckOutToolStripMenuItem_Click);
            // 
            // checkOutToolStripMenuItem
            // 
            this.checkOutToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.CHECKOUT;
            this.checkOutToolStripMenuItem.Name = "checkOutToolStripMenuItem";
            this.checkOutToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.checkOutToolStripMenuItem.Text = "Check Out";
            this.checkOutToolStripMenuItem.Click += new System.EventHandler(this.checkOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(160, 6);
            // 
            // versionHistoryToolStripMenuItem
            // 
            this.versionHistoryToolStripMenuItem.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.VERSIONS;
            this.versionHistoryToolStripMenuItem.Name = "versionHistoryToolStripMenuItem";
            this.versionHistoryToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.versionHistoryToolStripMenuItem.Text = "Version History";
            this.versionHistoryToolStripMenuItem.Click += new System.EventHandler(this.versionHistoryToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.gif");
            this.imageList1.Images.SetKeyName(1, "itgen.gif");
            this.imageList1.Images.SetKeyName(2, "stsicon.gif");
            // 
            // ViewsComboBox
            // 
            this.ViewsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ViewsComboBox.FormattingEnabled = true;
            this.ViewsComboBox.Location = new System.Drawing.Point(4, -2);
            this.ViewsComboBox.Name = "ViewsComboBox";
            this.ViewsComboBox.Size = new System.Drawing.Size(158, 21);
            this.ViewsComboBox.TabIndex = 2;
            this.ViewsComboBox.SelectedIndexChanged += new System.EventHandler(this.ViewsComboBox_SelectedIndexChanged);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PreviousButton.Enabled = false;
            this.PreviousButton.Location = new System.Drawing.Point(4, 154);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(75, 23);
            this.PreviousButton.TabIndex = 3;
            this.PreviousButton.Text = "Previous";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NextButton.Enabled = false;
            this.NextButton.Location = new System.Drawing.Point(131, 154);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 23);
            this.NextButton.TabIndex = 4;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // PagingLabel
            // 
            this.PagingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PagingLabel.AutoSize = true;
            this.PagingLabel.Location = new System.Drawing.Point(83, 159);
            this.PagingLabel.Name = "PagingLabel";
            this.PagingLabel.Size = new System.Drawing.Size(34, 13);
            this.PagingLabel.TabIndex = 5;
            this.PagingLabel.Text = "1-100";
            // 
            // ChangeViewBackgroundWorker
            // 
            this.ChangeViewBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ChangeViewBackgroundWorker_DoWork);
            this.ChangeViewBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ChangeViewBackgroundWorker_RunWorkerCompleted);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.FILTER;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(165, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // LoadingPictureBox
            // 
            this.LoadingPictureBox.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.ajax_loader;
            this.LoadingPictureBox.Location = new System.Drawing.Point(179, 0);
            this.LoadingPictureBox.Name = "LoadingPictureBox";
            this.LoadingPictureBox.Size = new System.Drawing.Size(16, 16);
            this.LoadingPictureBox.TabIndex = 6;
            this.LoadingPictureBox.TabStop = false;
            this.LoadingPictureBox.Visible = false;
            // 
            // SharePointListViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ViewsComboBox);
            this.Controls.Add(this.PagingLabel);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.LibraryContentDataGridView);
            this.Controls.Add(this.PreviousButton);
            this.Controls.Add(this.LoadingPictureBox);
            this.Name = "SharePointListViewControl";
            this.Size = new System.Drawing.Size(285, 180);
            ((System.ComponentModel.ISupportInitialize)(this.LibraryContentDataGridView)).EndInit();
            this.ListItemContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip ListItemContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem approveRejectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem versionHistoryToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem attachToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asHyperlinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asAnAttachmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoCheckOutToolStripMenuItem;
        private System.Windows.Forms.ComboBox ViewsComboBox;
        private System.Windows.Forms.Button PreviousButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Label PagingLabel;
        private System.Windows.Forms.PictureBox LoadingPictureBox;
        private System.ComponentModel.BackgroundWorker ChangeViewBackgroundWorker;
        public System.Windows.Forms.DataGridView LibraryContentDataGridView;
        private System.Windows.Forms.DataGridViewImageColumn ExtensionImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilePathColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleColumn;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
