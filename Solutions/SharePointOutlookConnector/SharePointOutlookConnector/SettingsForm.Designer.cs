namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class SettingsForm
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
            this.EmailRadioButton = new System.Windows.Forms.RadioButton();
            this.WordPlusAttachmentsRadioButton = new System.Windows.Forms.RadioButton();
            this.SettingsTabControl = new System.Windows.Forms.TabControl();
            this.SitesTabPage = new System.Windows.Forms.TabPage();
            this.ShowLogsButton = new System.Windows.Forms.Button();
            this.DetailedLogCheckBox = new System.Windows.Forms.CheckBox();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.SitesListBox = new System.Windows.Forms.ListBox();
            this.ListSettingTabPage = new System.Windows.Forms.TabPage();
            this.EmailMetaDataSOFieldsLabel = new System.Windows.Forms.Label();
            this.DeleteEmailMappingSettingButton = new System.Windows.Forms.Button();
            this.EmailMappingSettingsComboBox = new System.Windows.Forms.ComboBox();
            this.EmailMappingControl = new Sobiens.Office.SharePointOutlookConnector.Controls.EmailMappingControl();
            this.UploadTabPage = new System.Windows.Forms.TabPage();
            this.UploadAutomaticallyCheckBox = new System.Windows.Forms.CheckBox();
            this.SaveEmailAsLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.SettingsTabControl.SuspendLayout();
            this.SitesTabPage.SuspendLayout();
            this.ListSettingTabPage.SuspendLayout();
            this.UploadTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // EmailRadioButton
            // 
            this.EmailRadioButton.AutoSize = true;
            this.EmailRadioButton.Checked = true;
            this.EmailRadioButton.Location = new System.Drawing.Point(23, 62);
            this.EmailRadioButton.Name = "EmailRadioButton";
            this.EmailRadioButton.Size = new System.Drawing.Size(104, 17);
            this.EmailRadioButton.TabIndex = 16;
            this.EmailRadioButton.TabStop = true;
            this.EmailRadioButton.Text = "Email Item (.msg)";
            this.EmailRadioButton.UseVisualStyleBackColor = true;
            // 
            // WordPlusAttachmentsRadioButton
            // 
            this.WordPlusAttachmentsRadioButton.AutoSize = true;
            this.WordPlusAttachmentsRadioButton.Location = new System.Drawing.Point(23, 39);
            this.WordPlusAttachmentsRadioButton.Name = "WordPlusAttachmentsRadioButton";
            this.WordPlusAttachmentsRadioButton.Size = new System.Drawing.Size(280, 17);
            this.WordPlusAttachmentsRadioButton.TabIndex = 18;
            this.WordPlusAttachmentsRadioButton.Text = "Word document (.doc + attachments as seperate files)";
            this.WordPlusAttachmentsRadioButton.UseVisualStyleBackColor = true;
            // 
            // SettingsTabControl
            // 
            this.SettingsTabControl.Controls.Add(this.SitesTabPage);
            this.SettingsTabControl.Controls.Add(this.ListSettingTabPage);
            this.SettingsTabControl.Controls.Add(this.UploadTabPage);
            this.SettingsTabControl.Location = new System.Drawing.Point(12, 79);
            this.SettingsTabControl.Name = "SettingsTabControl";
            this.SettingsTabControl.SelectedIndex = 0;
            this.SettingsTabControl.Size = new System.Drawing.Size(443, 271);
            this.SettingsTabControl.TabIndex = 20;
            // 
            // SitesTabPage
            // 
            this.SitesTabPage.BackColor = System.Drawing.Color.White;
            this.SitesTabPage.Controls.Add(this.ShowLogsButton);
            this.SitesTabPage.Controls.Add(this.DetailedLogCheckBox);
            this.SitesTabPage.Controls.Add(this.DeleteButton);
            this.SitesTabPage.Controls.Add(this.AddButton);
            this.SitesTabPage.Controls.Add(this.SitesListBox);
            this.SitesTabPage.Location = new System.Drawing.Point(4, 22);
            this.SitesTabPage.Name = "SitesTabPage";
            this.SitesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.SitesTabPage.Size = new System.Drawing.Size(435, 245);
            this.SitesTabPage.TabIndex = 0;
            this.SitesTabPage.Text = "Sites";
            this.SitesTabPage.UseVisualStyleBackColor = true;
            // 
            // ShowLogsButton
            // 
            this.ShowLogsButton.BackgroundImage = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.ShowLogsButton.Location = new System.Drawing.Point(244, 207);
            this.ShowLogsButton.Name = "ShowLogsButton";
            this.ShowLogsButton.Size = new System.Drawing.Size(75, 23);
            this.ShowLogsButton.TabIndex = 24;
            this.ShowLogsButton.Text = "Logs";
            this.ShowLogsButton.UseVisualStyleBackColor = true;
            this.ShowLogsButton.Click += new System.EventHandler(this.ShowLogsButton_Click);
            // 
            // DetailedLogCheckBox
            // 
            this.DetailedLogCheckBox.AutoSize = true;
            this.DetailedLogCheckBox.Location = new System.Drawing.Point(336, 213);
            this.DetailedLogCheckBox.Name = "DetailedLogCheckBox";
            this.DetailedLogCheckBox.Size = new System.Drawing.Size(86, 17);
            this.DetailedLogCheckBox.TabIndex = 23;
            this.DetailedLogCheckBox.Text = "Detailed Log";
            this.DetailedLogCheckBox.UseVisualStyleBackColor = true;
            // 
            // DeleteButton
            // 
            this.DeleteButton.BackgroundImage = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.DeleteButton.Location = new System.Drawing.Point(312, 126);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 22;
            this.DeleteButton.Text = "&Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.BackgroundImage = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.AddButton.Location = new System.Drawing.Point(312, 94);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 21;
            this.AddButton.Text = "&New";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // SitesListBox
            // 
            this.SitesListBox.FormattingEnabled = true;
            this.SitesListBox.Location = new System.Drawing.Point(17, 15);
            this.SitesListBox.Name = "SitesListBox";
            this.SitesListBox.Size = new System.Drawing.Size(289, 134);
            this.SitesListBox.TabIndex = 20;
            this.SitesListBox.DoubleClick += new System.EventHandler(this.SitesListBox_DoubleClick);
            // 
            // ListSettingTabPage
            // 
            this.ListSettingTabPage.BackColor = System.Drawing.Color.White;
            this.ListSettingTabPage.Controls.Add(this.EmailMetaDataSOFieldsLabel);
            this.ListSettingTabPage.Controls.Add(this.DeleteEmailMappingSettingButton);
            this.ListSettingTabPage.Controls.Add(this.EmailMappingSettingsComboBox);
            this.ListSettingTabPage.Controls.Add(this.EmailMappingControl);
            this.ListSettingTabPage.Location = new System.Drawing.Point(4, 22);
            this.ListSettingTabPage.Name = "ListSettingTabPage";
            this.ListSettingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ListSettingTabPage.Size = new System.Drawing.Size(435, 245);
            this.ListSettingTabPage.TabIndex = 2;
            this.ListSettingTabPage.Text = "Lists";
            // 
            // EmailMetaDataSOFieldsLabel
            // 
            this.EmailMetaDataSOFieldsLabel.AutoSize = true;
            this.EmailMetaDataSOFieldsLabel.Location = new System.Drawing.Point(7, 28);
            this.EmailMetaDataSOFieldsLabel.Name = "EmailMetaDataSOFieldsLabel";
            this.EmailMetaDataSOFieldsLabel.Size = new System.Drawing.Size(219, 13);
            this.EmailMetaDataSOFieldsLabel.TabIndex = 21;
            this.EmailMetaDataSOFieldsLabel.Text = "Email Meta Data SharePoint Fields Mappings";
            // 
            // DeleteEmailMappingSettingButton
            // 
            this.DeleteEmailMappingSettingButton.BackgroundImage = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.DeleteEmailMappingSettingButton.Location = new System.Drawing.Point(345, 6);
            this.DeleteEmailMappingSettingButton.Name = "DeleteEmailMappingSettingButton";
            this.DeleteEmailMappingSettingButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteEmailMappingSettingButton.TabIndex = 9;
            this.DeleteEmailMappingSettingButton.Text = "&Delete";
            this.DeleteEmailMappingSettingButton.UseVisualStyleBackColor = true;
            this.DeleteEmailMappingSettingButton.Click += new System.EventHandler(this.DeleteEmailMappingSettingButton_Click);
            // 
            // EmailMappingSettingsComboBox
            // 
            this.EmailMappingSettingsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EmailMappingSettingsComboBox.FormattingEnabled = true;
            this.EmailMappingSettingsComboBox.Location = new System.Drawing.Point(6, 6);
            this.EmailMappingSettingsComboBox.Name = "EmailMappingSettingsComboBox";
            this.EmailMappingSettingsComboBox.Size = new System.Drawing.Size(333, 21);
            this.EmailMappingSettingsComboBox.TabIndex = 1;
            this.EmailMappingSettingsComboBox.SelectedIndexChanged += new System.EventHandler(this.EmailMappingSettingsComboBox_SelectedIndexChanged);
            // 
            // EmailMappingControl
            // 
            this.EmailMappingControl.Location = new System.Drawing.Point(6, 43);
            this.EmailMappingControl.Name = "EmailMappingControl";
            this.EmailMappingControl.Size = new System.Drawing.Size(414, 196);
            this.EmailMappingControl.TabIndex = 0;
            // 
            // UploadTabPage
            // 
            this.UploadTabPage.BackColor = System.Drawing.Color.White;
            this.UploadTabPage.Controls.Add(this.UploadAutomaticallyCheckBox);
            this.UploadTabPage.Controls.Add(this.SaveEmailAsLabel);
            this.UploadTabPage.Controls.Add(this.WordPlusAttachmentsRadioButton);
            this.UploadTabPage.Controls.Add(this.EmailRadioButton);
            this.UploadTabPage.Location = new System.Drawing.Point(4, 22);
            this.UploadTabPage.Name = "UploadTabPage";
            this.UploadTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.UploadTabPage.Size = new System.Drawing.Size(435, 245);
            this.UploadTabPage.TabIndex = 1;
            this.UploadTabPage.Text = "Upload";
            this.UploadTabPage.UseVisualStyleBackColor = true;
            // 
            // UploadAutomaticallyCheckBox
            // 
            this.UploadAutomaticallyCheckBox.AutoSize = true;
            this.UploadAutomaticallyCheckBox.Checked = true;
            this.UploadAutomaticallyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UploadAutomaticallyCheckBox.Location = new System.Drawing.Point(23, 86);
            this.UploadAutomaticallyCheckBox.Name = "UploadAutomaticallyCheckBox";
            this.UploadAutomaticallyCheckBox.Size = new System.Drawing.Size(196, 17);
            this.UploadAutomaticallyCheckBox.TabIndex = 37;
            this.UploadAutomaticallyCheckBox.Text = "Upload automatically on drag && drop";
            this.UploadAutomaticallyCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveEmailAsLabel
            // 
            this.SaveEmailAsLabel.AutoSize = true;
            this.SaveEmailAsLabel.Location = new System.Drawing.Point(12, 20);
            this.SaveEmailAsLabel.Name = "SaveEmailAsLabel";
            this.SaveEmailAsLabel.Size = new System.Drawing.Size(75, 13);
            this.SaveEmailAsLabel.TabIndex = 19;
            this.SaveEmailAsLabel.Text = "Save Email As";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.Head;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(470, 73);
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // CancelButton
            // 
            this.CancelButton.BackgroundImage = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.CancelButton.Location = new System.Drawing.Point(380, 356);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 9;
            this.CancelButton.Text = "&Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.BackgroundImage = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.OkButton.Location = new System.Drawing.Point(299, 356);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 8;
            this.OkButton.Text = "&Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(467, 385);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.SettingsTabControl);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.SettingsTabControl.ResumeLayout(false);
            this.SitesTabPage.ResumeLayout(false);
            this.SitesTabPage.PerformLayout();
            this.ListSettingTabPage.ResumeLayout(false);
            this.ListSettingTabPage.PerformLayout();
            this.UploadTabPage.ResumeLayout(false);
            this.UploadTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.RadioButton EmailRadioButton;
        private System.Windows.Forms.RadioButton WordPlusAttachmentsRadioButton;
        private System.Windows.Forms.TabControl SettingsTabControl;
        private System.Windows.Forms.TabPage SitesTabPage;
        private System.Windows.Forms.TabPage UploadTabPage;
        private System.Windows.Forms.Label SaveEmailAsLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox UploadAutomaticallyCheckBox;
        private System.Windows.Forms.TabPage ListSettingTabPage;
        private System.Windows.Forms.ListBox SitesListBox;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button DeleteEmailMappingSettingButton;
        private System.Windows.Forms.ComboBox EmailMappingSettingsComboBox;
        private Sobiens.Office.SharePointOutlookConnector.Controls.EmailMappingControl EmailMappingControl;
        private System.Windows.Forms.Label EmailMetaDataSOFieldsLabel;
        private System.Windows.Forms.CheckBox DetailedLogCheckBox;
        private System.Windows.Forms.Button ShowLogsButton;
    }
}