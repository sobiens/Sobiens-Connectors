namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class AlertMaintenanceForm
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Frequency: Immediate", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Frequency: Daily", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Frequency: Weekly", System.Windows.Forms.HorizontalAlignment.Left);
            this.CancelButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.AlertsListView = new System.Windows.Forms.ListView();
            this.TitleColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.BackColor = System.Drawing.Color.Transparent;
            this.CancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.CancelButton.Location = new System.Drawing.Point(583, 399);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(79, 35);
            this.CancelButton.TabIndex = 21;
            this.CancelButton.Text = "Close";
            this.CancelButton.UseVisualStyleBackColor = false;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // NewButton
            // 
            this.NewButton.BackColor = System.Drawing.Color.Transparent;
            this.NewButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.NewButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.NewButton.Location = new System.Drawing.Point(12, 74);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(79, 35);
            this.NewButton.TabIndex = 19;
            this.NewButton.Text = "New";
            this.NewButton.UseVisualStyleBackColor = false;
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.Head;
            this.pictureBox1.Location = new System.Drawing.Point(-1, -4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(668, 72);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // AlertsListView
            // 
            this.AlertsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TitleColumnHeader});
            this.AlertsListView.ContextMenuStrip = this.contextMenuStrip1;
            listViewGroup1.Header = "Frequency: Immediate";
            listViewGroup1.Name = "ImmediateListViewGroup";
            listViewGroup2.Header = "Frequency: Daily";
            listViewGroup2.Name = "DailyListViewGroup";
            listViewGroup3.Header = "Frequency: Weekly";
            listViewGroup3.Name = "WeeklyListViewGroup";
            this.AlertsListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.AlertsListView.Location = new System.Drawing.Point(12, 115);
            this.AlertsListView.Name = "AlertsListView";
            this.AlertsListView.Size = new System.Drawing.Size(633, 278);
            this.AlertsListView.TabIndex = 22;
            this.AlertsListView.UseCompatibleStateImageBehavior = false;
            this.AlertsListView.View = System.Windows.Forms.View.Details;
            // 
            // TitleColumnHeader
            // 
            this.TitleColumnHeader.Text = "Title";
            this.TitleColumnHeader.Width = 624;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(117, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // AlertMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(669, 436);
            this.Controls.Add(this.AlertsListView);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AlertMaintenanceForm";
            this.Text = "Alerts Maintenance";
            this.Load += new System.EventHandler(this.AlertMaintenanceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.ListView AlertsListView;
        private System.Windows.Forms.ColumnHeader TitleColumnHeader;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}