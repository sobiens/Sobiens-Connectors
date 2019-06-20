using System.Windows.Forms;
namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class SOCActionBar
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
            this.SobiensLogoPictureBox = new System.Windows.Forms.PictureBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.ListViewToogleButton = new Sobiens.Office.SharePointOutlookConnector.ToogleButtonExt();
            this.FolderTreeviewDisplayButton = new Sobiens.Office.SharePointOutlookConnector.ToogleButtonExt();
            ((System.ComponentModel.ISupportInitialize)(this.SobiensLogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SobiensLogoPictureBox
            // 
            this.SobiensLogoPictureBox.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.Sobiens_20x20;
            this.SobiensLogoPictureBox.Location = new System.Drawing.Point(2, 0);
            this.SobiensLogoPictureBox.Name = "SobiensLogoPictureBox";
            this.SobiensLogoPictureBox.Size = new System.Drawing.Size(20, 20);
            this.SobiensLogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SobiensLogoPictureBox.TabIndex = 7;
            this.SobiensLogoPictureBox.TabStop = false;
            this.SobiensLogoPictureBox.Click += new System.EventHandler(this.SobiensLogoPictureBox_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshButton.BackColor = System.Drawing.SystemColors.Control;
            this.RefreshButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.REFRESH;
            this.RefreshButton.Location = new System.Drawing.Point(113, 3);
            this.RefreshButton.Margin = new System.Windows.Forms.Padding(0);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(20, 20);
            this.RefreshButton.TabIndex = 6;
            this.RefreshButton.UseVisualStyleBackColor = false;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton1_Click);
            // 
            // SettingsButton
            // 
            this.SettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsButton.BackColor = System.Drawing.SystemColors.Control;
            this.SettingsButton.BackgroundImage = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.SettingsButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.SETTINGS;
            this.SettingsButton.Location = new System.Drawing.Point(138, 3);
            this.SettingsButton.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(20, 20);
            this.SettingsButton.TabIndex = 5;
            this.SettingsButton.UseVisualStyleBackColor = false;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // ListViewToogleButton
            // 
            this.ListViewToogleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ListViewToogleButton.BackColor = System.Drawing.SystemColors.Control;
            this.ListViewToogleButton.CheckedText = "Hide Folder Items";
            this.ListViewToogleButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.LIST;
            this.ListViewToogleButton.IsChecked = true;
            this.ListViewToogleButton.Location = new System.Drawing.Point(187, 3);
            this.ListViewToogleButton.Name = "ListViewToogleButton";
            this.ListViewToogleButton.Size = new System.Drawing.Size(20, 20);
            this.ListViewToogleButton.TabIndex = 10;
            this.ListViewToogleButton.UnCheckedText = "Show Folder Items";
            this.ListViewToogleButton.ButtonClick += new System.EventHandler(this.ListViewToogleButton_ButtonClick);
            // 
            // FolderTreeviewDisplayButton
            // 
            this.FolderTreeviewDisplayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderTreeviewDisplayButton.BackColor = System.Drawing.SystemColors.Control;
            this.FolderTreeviewDisplayButton.CheckedText = "Hide Hierarchy";
            this.FolderTreeviewDisplayButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.Hiearchy;
            this.FolderTreeviewDisplayButton.IsChecked = true;
            this.FolderTreeviewDisplayButton.Location = new System.Drawing.Point(161, 3);
            this.FolderTreeviewDisplayButton.Name = "FolderTreeviewDisplayButton";
            this.FolderTreeviewDisplayButton.Size = new System.Drawing.Size(20, 20);
            this.FolderTreeviewDisplayButton.TabIndex = 8;
            this.FolderTreeviewDisplayButton.UnCheckedText = "Show Hierarchy";
            this.FolderTreeviewDisplayButton.ButtonClick += new System.EventHandler(this.FolderTreeviewDisplayButton_ButtonClick);
            // 
            // SOCActionBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ListViewToogleButton);
            this.Controls.Add(this.FolderTreeviewDisplayButton);
            this.Controls.Add(this.SobiensLogoPictureBox);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.SettingsButton);
            this.Name = "SOCActionBar";
            this.Size = new System.Drawing.Size(217, 26);
            ((System.ComponentModel.ISupportInitialize)(this.SobiensLogoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ToogleButtonExt FolderTreeviewDisplayButton;
        private System.Windows.Forms.PictureBox SobiensLogoPictureBox;
        private Button RefreshButton;
        private Button SettingsButton;
        private ToogleButtonExt ListViewToogleButton;
    }
}
