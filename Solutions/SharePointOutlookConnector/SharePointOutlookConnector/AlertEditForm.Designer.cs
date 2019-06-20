namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class AlertEditForm
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
            this.SelectedListNameLabel = new System.Windows.Forms.Label();
            this.SelectedListLabel = new System.Windows.Forms.Label();
            this.SaveFilterButton = new System.Windows.Forms.Button();
            this.TitleTextBox = new System.Windows.Forms.TextBox();
            this.CancelSaveFilterButton = new System.Windows.Forms.Button();
            this.EventTypeLabel = new System.Windows.Forms.Label();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AllRadioButton = new System.Windows.Forms.RadioButton();
            this.DeleteRadioButton = new System.Windows.Forms.RadioButton();
            this.ModifyRadioButton = new System.Windows.Forms.RadioButton();
            this.AddRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ImmediateRadioButton = new System.Windows.Forms.RadioButton();
            this.DailyRadioButton = new System.Windows.Forms.RadioButton();
            this.WeeklyRadioButton = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.WeekDayComboBox = new System.Windows.Forms.ComboBox();
            this.HoursComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.FiltersTreeView = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
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
            // SelectedListNameLabel
            // 
            this.SelectedListNameLabel.AutoSize = true;
            this.SelectedListNameLabel.Location = new System.Drawing.Point(556, 73);
            this.SelectedListNameLabel.Name = "SelectedListNameLabel";
            this.SelectedListNameLabel.Size = new System.Drawing.Size(35, 13);
            this.SelectedListNameLabel.TabIndex = 12;
            this.SelectedListNameLabel.Text = "label2";
            // 
            // SelectedListLabel
            // 
            this.SelectedListLabel.AutoSize = true;
            this.SelectedListLabel.Location = new System.Drawing.Point(503, 73);
            this.SelectedListLabel.Name = "SelectedListLabel";
            this.SelectedListLabel.Size = new System.Drawing.Size(47, 13);
            this.SelectedListLabel.TabIndex = 11;
            this.SelectedListLabel.Text = "Alert List";
            // 
            // SaveFilterButton
            // 
            this.SaveFilterButton.BackColor = System.Drawing.Color.Transparent;
            this.SaveFilterButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SaveFilterButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.SaveFilterButton.Location = new System.Drawing.Point(493, 425);
            this.SaveFilterButton.Name = "SaveFilterButton";
            this.SaveFilterButton.Size = new System.Drawing.Size(79, 35);
            this.SaveFilterButton.TabIndex = 15;
            this.SaveFilterButton.Text = "Ok";
            this.SaveFilterButton.UseVisualStyleBackColor = false;
            this.SaveFilterButton.Click += new System.EventHandler(this.SaveFilterButton_Click);
            // 
            // TitleTextBox
            // 
            this.TitleTextBox.Location = new System.Drawing.Point(207, 74);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(258, 20);
            this.TitleTextBox.TabIndex = 18;
            // 
            // CancelSaveFilterButton
            // 
            this.CancelSaveFilterButton.BackColor = System.Drawing.Color.Transparent;
            this.CancelSaveFilterButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelSaveFilterButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.CancelSaveFilterButton.Location = new System.Drawing.Point(578, 425);
            this.CancelSaveFilterButton.Name = "CancelSaveFilterButton";
            this.CancelSaveFilterButton.Size = new System.Drawing.Size(79, 35);
            this.CancelSaveFilterButton.TabIndex = 20;
            this.CancelSaveFilterButton.Text = "Cancel";
            this.CancelSaveFilterButton.UseVisualStyleBackColor = false;
            this.CancelSaveFilterButton.Click += new System.EventHandler(this.CancelSaveFilterButton_Click);
            // 
            // EventTypeLabel
            // 
            this.EventTypeLabel.AutoSize = true;
            this.EventTypeLabel.Location = new System.Drawing.Point(12, 215);
            this.EventTypeLabel.Name = "EventTypeLabel";
            this.EventTypeLabel.Size = new System.Drawing.Size(71, 13);
            this.EventTypeLabel.TabIndex = 23;
            this.EventTypeLabel.Text = "Change Type";
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Location = new System.Drawing.Point(12, 77);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(27, 13);
            this.ValueLabel.TabIndex = 26;
            this.ValueLabel.Text = "Title";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Send Alerts for These Changes";
            // 
            // AllRadioButton
            // 
            this.AllRadioButton.AutoSize = true;
            this.AllRadioButton.Checked = true;
            this.AllRadioButton.Location = new System.Drawing.Point(6, 19);
            this.AllRadioButton.Name = "AllRadioButton";
            this.AllRadioButton.Size = new System.Drawing.Size(80, 17);
            this.AllRadioButton.TabIndex = 29;
            this.AllRadioButton.TabStop = true;
            this.AllRadioButton.Text = "All changes";
            this.AllRadioButton.UseVisualStyleBackColor = true;
            // 
            // DeleteRadioButton
            // 
            this.DeleteRadioButton.AutoSize = true;
            this.DeleteRadioButton.Location = new System.Drawing.Point(6, 75);
            this.DeleteRadioButton.Name = "DeleteRadioButton";
            this.DeleteRadioButton.Size = new System.Drawing.Size(106, 17);
            this.DeleteRadioButton.TabIndex = 30;
            this.DeleteRadioButton.Text = "Items are deleted";
            this.DeleteRadioButton.UseVisualStyleBackColor = true;
            // 
            // ModifyRadioButton
            // 
            this.ModifyRadioButton.AutoSize = true;
            this.ModifyRadioButton.Location = new System.Drawing.Point(6, 55);
            this.ModifyRadioButton.Name = "ModifyRadioButton";
            this.ModifyRadioButton.Size = new System.Drawing.Size(148, 17);
            this.ModifyRadioButton.TabIndex = 31;
            this.ModifyRadioButton.Text = "Existing items are modified";
            this.ModifyRadioButton.UseVisualStyleBackColor = true;
            // 
            // AddRadioButton
            // 
            this.AddRadioButton.AutoSize = true;
            this.AddRadioButton.Location = new System.Drawing.Point(6, 37);
            this.AddRadioButton.Name = "AddRadioButton";
            this.AddRadioButton.Size = new System.Drawing.Size(125, 17);
            this.AddRadioButton.TabIndex = 32;
            this.AddRadioButton.Text = "New items are added";
            this.AddRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AllRadioButton);
            this.groupBox1.Controls.Add(this.AddRadioButton);
            this.groupBox1.Controls.Add(this.DeleteRadioButton);
            this.groupBox1.Controls.Add(this.ModifyRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(207, 217);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 100);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Only send me alerts when:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.HoursComboBox);
            this.groupBox2.Controls.Add(this.WeekDayComboBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.ImmediateRadioButton);
            this.groupBox2.Controls.Add(this.DailyRadioButton);
            this.groupBox2.Controls.Add(this.WeeklyRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(207, 321);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(258, 122);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Only send me alerts when:";
            // 
            // ImmediateRadioButton
            // 
            this.ImmediateRadioButton.AutoSize = true;
            this.ImmediateRadioButton.Checked = true;
            this.ImmediateRadioButton.Location = new System.Drawing.Point(6, 19);
            this.ImmediateRadioButton.Name = "ImmediateRadioButton";
            this.ImmediateRadioButton.Size = new System.Drawing.Size(137, 17);
            this.ImmediateRadioButton.TabIndex = 29;
            this.ImmediateRadioButton.TabStop = true;
            this.ImmediateRadioButton.Text = "Send e-mail immediately";
            this.ImmediateRadioButton.UseVisualStyleBackColor = true;
            this.ImmediateRadioButton.CheckedChanged += new System.EventHandler(this.ImmediateRadioButton_CheckedChanged);
            // 
            // DailyRadioButton
            // 
            this.DailyRadioButton.AutoSize = true;
            this.DailyRadioButton.Location = new System.Drawing.Point(6, 37);
            this.DailyRadioButton.Name = "DailyRadioButton";
            this.DailyRadioButton.Size = new System.Drawing.Size(127, 17);
            this.DailyRadioButton.TabIndex = 32;
            this.DailyRadioButton.Text = "Send a daily summary";
            this.DailyRadioButton.UseVisualStyleBackColor = true;
            this.DailyRadioButton.CheckedChanged += new System.EventHandler(this.DailyRadioButton_CheckedChanged);
            // 
            // WeeklyRadioButton
            // 
            this.WeeklyRadioButton.AutoSize = true;
            this.WeeklyRadioButton.Location = new System.Drawing.Point(6, 55);
            this.WeeklyRadioButton.Name = "WeeklyRadioButton";
            this.WeeklyRadioButton.Size = new System.Drawing.Size(139, 17);
            this.WeeklyRadioButton.TabIndex = 31;
            this.WeeklyRadioButton.Text = "Send a weekly summary";
            this.WeeklyRadioButton.UseVisualStyleBackColor = true;
            this.WeeklyRadioButton.CheckedChanged += new System.EventHandler(this.WeeklyRadioButton_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 319);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "When to Send Alerts";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Time:";
            // 
            // WeekDayComboBox
            // 
            this.WeekDayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WeekDayComboBox.FormattingEnabled = true;
            this.WeekDayComboBox.Items.AddRange(new object[] {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"});
            this.WeekDayComboBox.Location = new System.Drawing.Point(6, 95);
            this.WeekDayComboBox.Name = "WeekDayComboBox";
            this.WeekDayComboBox.Size = new System.Drawing.Size(121, 21);
            this.WeekDayComboBox.TabIndex = 36;
            // 
            // HoursComboBox
            // 
            this.HoursComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HoursComboBox.FormattingEnabled = true;
            this.HoursComboBox.Items.AddRange(new object[] {
            "00:00",
            "01:00",
            "02:00",
            "03:00",
            "04:00",
            "05:00",
            "06:00",
            "07:00",
            "08:00",
            "09:00",
            "10:00",
            "11:00",
            "12:00",
            "13:00",
            "14:00",
            "15:00",
            "16:00",
            "17:00",
            "18:00",
            "19:00",
            "20:00",
            "21:00",
            "22:00",
            "23:00"});
            this.HoursComboBox.Location = new System.Drawing.Point(133, 95);
            this.HoursComboBox.Name = "HoursComboBox";
            this.HoursComboBox.Size = new System.Drawing.Size(121, 21);
            this.HoursComboBox.TabIndex = 37;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.button1.Location = new System.Drawing.Point(471, 176);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 35);
            this.button1.TabIndex = 36;
            this.button1.Text = "New Group";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button2.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.button2.Location = new System.Drawing.Point(471, 135);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 35);
            this.button2.TabIndex = 37;
            this.button2.Text = "New Filter";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FiltersTreeView
            // 
            this.FiltersTreeView.Location = new System.Drawing.Point(207, 100);
            this.FiltersTreeView.Name = "FiltersTreeView";
            this.FiltersTreeView.Size = new System.Drawing.Size(258, 111);
            this.FiltersTreeView.TabIndex = 38;
            // 
            // AlertEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(669, 472);
            this.Controls.Add(this.FiltersTreeView);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ValueLabel);
            this.Controls.Add(this.EventTypeLabel);
            this.Controls.Add(this.CancelSaveFilterButton);
            this.Controls.Add(this.TitleTextBox);
            this.Controls.Add(this.SaveFilterButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.SelectedListNameLabel);
            this.Controls.Add(this.SelectedListLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AlertEditForm";
            this.Text = "Alerts Maintenance";
            this.Load += new System.EventHandler(this.AlertMaintenanceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label SelectedListNameLabel;
        private System.Windows.Forms.Label SelectedListLabel;
        private System.Windows.Forms.Button SaveFilterButton;
        private System.Windows.Forms.TextBox TitleTextBox;
        private System.Windows.Forms.Button CancelSaveFilterButton;
        private System.Windows.Forms.Label EventTypeLabel;
        private System.Windows.Forms.Label ValueLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton AllRadioButton;
        private System.Windows.Forms.RadioButton DeleteRadioButton;
        private System.Windows.Forms.RadioButton ModifyRadioButton;
        private System.Windows.Forms.RadioButton AddRadioButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton ImmediateRadioButton;
        private System.Windows.Forms.RadioButton DailyRadioButton;
        private System.Windows.Forms.RadioButton WeeklyRadioButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox HoursComboBox;
        private System.Windows.Forms.ComboBox WeekDayComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TreeView FiltersTreeView;
    }
}