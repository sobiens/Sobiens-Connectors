namespace Sobiens.Office.SharePointOutlookConnector.Forms
{
    partial class SiteSettingForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CustomCredentialRadioButton = new System.Windows.Forms.RadioButton();
            this.UrlTextBox = new System.Windows.Forms.TextBox();
            this.DefaultCredentialRadioButton = new System.Windows.Forms.RadioButton();
            this.UserTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.UrlLabel = new System.Windows.Forms.Label();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.SharePointRadioButton = new System.Windows.Forms.RadioButton();
            this.FileSystemRadioButton = new System.Windows.Forms.RadioButton();
            this.TypeGroupBox = new System.Windows.Forms.GroupBox();
            this.GMailRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.TypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.Head;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(470, 73);
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // CustomCredentialRadioButton
            // 
            this.CustomCredentialRadioButton.AutoSize = true;
            this.CustomCredentialRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CustomCredentialRadioButton.Location = new System.Drawing.Point(43, 176);
            this.CustomCredentialRadioButton.Name = "CustomCredentialRadioButton";
            this.CustomCredentialRadioButton.Size = new System.Drawing.Size(115, 18);
            this.CustomCredentialRadioButton.TabIndex = 30;
            this.CustomCredentialRadioButton.Text = "Custom credential";
            this.CustomCredentialRadioButton.UseVisualStyleBackColor = true;
            this.CustomCredentialRadioButton.CheckedChanged += new System.EventHandler(this.CustomCredentialRadioButton_CheckedChanged);
            // 
            // UrlTextBox
            // 
            this.UrlTextBox.Location = new System.Drawing.Point(85, 127);
            this.UrlTextBox.Name = "UrlTextBox";
            this.UrlTextBox.Size = new System.Drawing.Size(265, 20);
            this.UrlTextBox.TabIndex = 24;
            // 
            // DefaultCredentialRadioButton
            // 
            this.DefaultCredentialRadioButton.AutoSize = true;
            this.DefaultCredentialRadioButton.Checked = true;
            this.DefaultCredentialRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.DefaultCredentialRadioButton.Location = new System.Drawing.Point(43, 153);
            this.DefaultCredentialRadioButton.Name = "DefaultCredentialRadioButton";
            this.DefaultCredentialRadioButton.Size = new System.Drawing.Size(109, 18);
            this.DefaultCredentialRadioButton.TabIndex = 29;
            this.DefaultCredentialRadioButton.TabStop = true;
            this.DefaultCredentialRadioButton.Text = "My network user";
            this.DefaultCredentialRadioButton.UseVisualStyleBackColor = true;
            this.DefaultCredentialRadioButton.CheckedChanged += new System.EventHandler(this.DefaultCredentialRadioButton_CheckedChanged);
            // 
            // UserTextBox
            // 
            this.UserTextBox.Location = new System.Drawing.Point(138, 199);
            this.UserTextBox.Name = "UserTextBox";
            this.UserTextBox.Size = new System.Drawing.Size(212, 20);
            this.UserTextBox.TabIndex = 26;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(138, 225);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(212, 20);
            this.PasswordTextBox.TabIndex = 28;
            // 
            // UrlLabel
            // 
            this.UrlLabel.AutoSize = true;
            this.UrlLabel.Location = new System.Drawing.Point(40, 130);
            this.UrlLabel.Name = "UrlLabel";
            this.UrlLabel.Size = new System.Drawing.Size(32, 13);
            this.UrlLabel.TabIndex = 23;
            this.UrlLabel.Text = "URL:";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(59, 202);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(73, 13);
            this.UsernameLabel.TabIndex = 25;
            this.UsernameLabel.Text = "Domain\\User:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Password:";
            // 
            // CancelButton
            // 
            this.CancelButton.BackgroundImage = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.CancelButton.Location = new System.Drawing.Point(275, 296);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 32;
            this.CancelButton.Text = "&Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.BackgroundImage = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.OkButton.Location = new System.Drawing.Point(194, 296);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 31;
            this.OkButton.Text = "&Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // SharePointRadioButton
            // 
            this.SharePointRadioButton.AutoSize = true;
            this.SharePointRadioButton.Checked = true;
            this.SharePointRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SharePointRadioButton.Location = new System.Drawing.Point(42, 13);
            this.SharePointRadioButton.Name = "SharePointRadioButton";
            this.SharePointRadioButton.Size = new System.Drawing.Size(83, 18);
            this.SharePointRadioButton.TabIndex = 33;
            this.SharePointRadioButton.TabStop = true;
            this.SharePointRadioButton.Text = "SharePoint";
            this.SharePointRadioButton.UseVisualStyleBackColor = true;
            // 
            // FileSystemRadioButton
            // 
            this.FileSystemRadioButton.AutoSize = true;
            this.FileSystemRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.FileSystemRadioButton.Location = new System.Drawing.Point(194, 13);
            this.FileSystemRadioButton.Name = "FileSystemRadioButton";
            this.FileSystemRadioButton.Size = new System.Drawing.Size(84, 18);
            this.FileSystemRadioButton.TabIndex = 34;
            this.FileSystemRadioButton.Text = "File System";
            this.FileSystemRadioButton.UseVisualStyleBackColor = true;
            // 
            // TypeGroupBox
            // 
            this.TypeGroupBox.Controls.Add(this.GMailRadioButton);
            this.TypeGroupBox.Controls.Add(this.SharePointRadioButton);
            this.TypeGroupBox.Controls.Add(this.FileSystemRadioButton);
            this.TypeGroupBox.Location = new System.Drawing.Point(43, 79);
            this.TypeGroupBox.Name = "TypeGroupBox";
            this.TypeGroupBox.Size = new System.Drawing.Size(307, 37);
            this.TypeGroupBox.TabIndex = 35;
            this.TypeGroupBox.TabStop = false;
            this.TypeGroupBox.Text = "Type";
            // 
            // GMailRadioButton
            // 
            this.GMailRadioButton.AutoSize = true;
            this.GMailRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GMailRadioButton.Location = new System.Drawing.Point(131, 13);
            this.GMailRadioButton.Name = "GMailRadioButton";
            this.GMailRadioButton.Size = new System.Drawing.Size(57, 18);
            this.GMailRadioButton.TabIndex = 35;
            this.GMailRadioButton.Text = "Gmail";
            this.GMailRadioButton.UseVisualStyleBackColor = true;
            this.GMailRadioButton.CheckedChanged += new System.EventHandler(this.GMailRadioButton_CheckedChanged);
            // 
            // SiteSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(465, 380);
            this.Controls.Add(this.TypeGroupBox);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.CustomCredentialRadioButton);
            this.Controls.Add(this.UrlTextBox);
            this.Controls.Add(this.DefaultCredentialRadioButton);
            this.Controls.Add(this.UserTextBox);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.UrlLabel);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "SiteSettingForm";
            this.Text = "SiteSettingForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.TypeGroupBox.ResumeLayout(false);
            this.TypeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton CustomCredentialRadioButton;
        private System.Windows.Forms.TextBox UrlTextBox;
        private System.Windows.Forms.RadioButton DefaultCredentialRadioButton;
        private System.Windows.Forms.TextBox UserTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label UrlLabel;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.RadioButton SharePointRadioButton;
        private System.Windows.Forms.RadioButton FileSystemRadioButton;
        private System.Windows.Forms.GroupBox TypeGroupBox;
        private System.Windows.Forms.RadioButton GMailRadioButton;
    }
}