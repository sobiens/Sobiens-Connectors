namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class ListItemVersionsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ListItemVersionsDataGridView = new System.Windows.Forms.DataGridView();
            this.VersionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.URLColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedByColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CommentsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rollbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.ListItemVersionsDataGridView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListItemVersionsDataGridView
            // 
            this.ListItemVersionsDataGridView.AllowDrop = true;
            this.ListItemVersionsDataGridView.AllowUserToAddRows = false;
            this.ListItemVersionsDataGridView.AllowUserToDeleteRows = false;
            this.ListItemVersionsDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.ListItemVersionsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListItemVersionsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VersionColumn,
            this.URLColumn,
            this.SizeColumn,
            this.CreatedColumn,
            this.CreatedByColumn,
            this.CommentsColumn});
            this.ListItemVersionsDataGridView.ContextMenuStrip = this.contextMenuStrip1;
            this.ListItemVersionsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListItemVersionsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.ListItemVersionsDataGridView.Name = "ListItemVersionsDataGridView";
            this.ListItemVersionsDataGridView.RowHeadersVisible = false;
            this.ListItemVersionsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ListItemVersionsDataGridView.Size = new System.Drawing.Size(560, 221);
            this.ListItemVersionsDataGridView.TabIndex = 2;
            // 
            // VersionColumn
            // 
            this.VersionColumn.HeaderText = "Version";
            this.VersionColumn.Name = "VersionColumn";
            this.VersionColumn.ReadOnly = true;
            // 
            // URLColumn
            // 
            this.URLColumn.HeaderText = "URL";
            this.URLColumn.Name = "URLColumn";
            this.URLColumn.Visible = false;
            // 
            // SizeColumn
            // 
            this.SizeColumn.HeaderText = "Size";
            this.SizeColumn.Name = "SizeColumn";
            // 
            // CreatedColumn
            // 
            this.CreatedColumn.HeaderText = "Created";
            this.CreatedColumn.Name = "CreatedColumn";
            // 
            // CreatedByColumn
            // 
            this.CreatedByColumn.HeaderText = "CreatedBy";
            this.CreatedByColumn.Name = "CreatedByColumn";
            // 
            // CommentsColumn
            // 
            this.CommentsColumn.HeaderText = "Comments";
            this.CommentsColumn.Name = "CommentsColumn";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.rollbackToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // rollbackToolStripMenuItem
            // 
            this.rollbackToolStripMenuItem.Name = "rollbackToolStripMenuItem";
            this.rollbackToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rollbackToolStripMenuItem.Text = "Rollback";
            this.rollbackToolStripMenuItem.Click += new System.EventHandler(this.rollbackToolStripMenuItem_Click);
            // 
            // ListItemVersionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 221);
            this.Controls.Add(this.ListItemVersionsDataGridView);
            this.Name = "ListItemVersionsForm";
            this.Text = "ListItemVersionsForm";
            ((System.ComponentModel.ISupportInitialize)(this.ListItemVersionsDataGridView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ListItemVersionsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn URLColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SizeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedByColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CommentsColumn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rollbackToolStripMenuItem;
    }
}