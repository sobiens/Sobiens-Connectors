namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class AlertFilterEditForm
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
            this.FieldsComboBox = new System.Windows.Forms.ComboBox();
            this.OperationComboBox = new System.Windows.Forms.ComboBox();
            this.FilterValueTextBox = new System.Windows.Forms.TextBox();
            this.CancelSaveFilterButton = new System.Windows.Forms.Button();
            this.FieldsLabel = new System.Windows.Forms.Label();
            this.OperationTypeLabel = new System.Windows.Forms.Label();
            this.ValueLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.SelectedListNameLabel.Location = new System.Drawing.Point(110, 71);
            this.SelectedListNameLabel.Name = "SelectedListNameLabel";
            this.SelectedListNameLabel.Size = new System.Drawing.Size(35, 13);
            this.SelectedListNameLabel.TabIndex = 12;
            this.SelectedListNameLabel.Text = "label2";
            // 
            // SelectedListLabel
            // 
            this.SelectedListLabel.AutoSize = true;
            this.SelectedListLabel.Location = new System.Drawing.Point(8, 71);
            this.SelectedListLabel.Name = "SelectedListLabel";
            this.SelectedListLabel.Size = new System.Drawing.Size(96, 13);
            this.SelectedListLabel.TabIndex = 11;
            this.SelectedListLabel.Text = "Selected Location:";
            // 
            // SaveFilterButton
            // 
            this.SaveFilterButton.BackColor = System.Drawing.Color.Transparent;
            this.SaveFilterButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SaveFilterButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.SaveFilterButton.Location = new System.Drawing.Point(159, 239);
            this.SaveFilterButton.Name = "SaveFilterButton";
            this.SaveFilterButton.Size = new System.Drawing.Size(79, 35);
            this.SaveFilterButton.TabIndex = 15;
            this.SaveFilterButton.Text = "Ok";
            this.SaveFilterButton.UseVisualStyleBackColor = false;
            this.SaveFilterButton.Click += new System.EventHandler(this.SaveFilterButton_Click);
            // 
            // FieldsComboBox
            // 
            this.FieldsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FieldsComboBox.FormattingEnabled = true;
            this.FieldsComboBox.Location = new System.Drawing.Point(113, 159);
            this.FieldsComboBox.Name = "FieldsComboBox";
            this.FieldsComboBox.Size = new System.Drawing.Size(210, 21);
            this.FieldsComboBox.TabIndex = 16;
            this.FieldsComboBox.SelectedIndexChanged += new System.EventHandler(this.FieldsComboBox_SelectedIndexChanged);
            // 
            // OperationComboBox
            // 
            this.OperationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OperationComboBox.FormattingEnabled = true;
            this.OperationComboBox.Location = new System.Drawing.Point(113, 186);
            this.OperationComboBox.Name = "OperationComboBox";
            this.OperationComboBox.Size = new System.Drawing.Size(210, 21);
            this.OperationComboBox.TabIndex = 17;
            // 
            // FilterValueTextBox
            // 
            this.FilterValueTextBox.Location = new System.Drawing.Point(113, 213);
            this.FilterValueTextBox.Name = "FilterValueTextBox";
            this.FilterValueTextBox.Size = new System.Drawing.Size(210, 20);
            this.FilterValueTextBox.TabIndex = 18;
            // 
            // CancelSaveFilterButton
            // 
            this.CancelSaveFilterButton.BackColor = System.Drawing.Color.Transparent;
            this.CancelSaveFilterButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelSaveFilterButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.CancelSaveFilterButton.Location = new System.Drawing.Point(244, 239);
            this.CancelSaveFilterButton.Name = "CancelSaveFilterButton";
            this.CancelSaveFilterButton.Size = new System.Drawing.Size(79, 35);
            this.CancelSaveFilterButton.TabIndex = 20;
            this.CancelSaveFilterButton.Text = "Cancel";
            this.CancelSaveFilterButton.UseVisualStyleBackColor = false;
            this.CancelSaveFilterButton.Click += new System.EventHandler(this.CancelSaveFilterButton_Click);
            // 
            // FieldsLabel
            // 
            this.FieldsLabel.AutoSize = true;
            this.FieldsLabel.Location = new System.Drawing.Point(12, 162);
            this.FieldsLabel.Name = "FieldsLabel";
            this.FieldsLabel.Size = new System.Drawing.Size(37, 13);
            this.FieldsLabel.TabIndex = 24;
            this.FieldsLabel.Text = "Fields:";
            // 
            // OperationTypeLabel
            // 
            this.OperationTypeLabel.AutoSize = true;
            this.OperationTypeLabel.Location = new System.Drawing.Point(12, 189);
            this.OperationTypeLabel.Name = "OperationTypeLabel";
            this.OperationTypeLabel.Size = new System.Drawing.Size(83, 13);
            this.OperationTypeLabel.TabIndex = 25;
            this.OperationTypeLabel.Text = "Operation Type:";
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Location = new System.Drawing.Point(12, 216);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(37, 13);
            this.ValueLabel.TabIndex = 26;
            this.ValueLabel.Text = "Value:";
            // 
            // AlertFilterEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(669, 436);
            this.Controls.Add(this.ValueLabel);
            this.Controls.Add(this.OperationTypeLabel);
            this.Controls.Add(this.FieldsLabel);
            this.Controls.Add(this.CancelSaveFilterButton);
            this.Controls.Add(this.FilterValueTextBox);
            this.Controls.Add(this.OperationComboBox);
            this.Controls.Add(this.FieldsComboBox);
            this.Controls.Add(this.SaveFilterButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.SelectedListNameLabel);
            this.Controls.Add(this.SelectedListLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AlertFilterEditForm";
            this.Text = "Alerts Maintenance";
            this.Load += new System.EventHandler(this.AlertMaintenanceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label SelectedListNameLabel;
        private System.Windows.Forms.Label SelectedListLabel;
        private System.Windows.Forms.Button SaveFilterButton;
        private System.Windows.Forms.ComboBox FieldsComboBox;
        private System.Windows.Forms.ComboBox OperationComboBox;
        private System.Windows.Forms.TextBox FilterValueTextBox;
        private System.Windows.Forms.Button CancelSaveFilterButton;
        private System.Windows.Forms.Label FieldsLabel;
        private System.Windows.Forms.Label OperationTypeLabel;
        private System.Windows.Forms.Label ValueLabel;
    }
}