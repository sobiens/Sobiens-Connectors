namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class CheckInForm
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
            this.DraftRadioButton = new System.Windows.Forms.RadioButton();
            this.OverwriteRadioButton = new System.Windows.Forms.RadioButton();
            this.PublishRadioButton = new System.Windows.Forms.RadioButton();
            this.YesRadioButton = new System.Windows.Forms.RadioButton();
            this.NoRadioButton = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.CommentsTextBox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DraftRadioButton
            // 
            this.DraftRadioButton.AutoSize = true;
            this.DraftRadioButton.Checked = true;
            this.DraftRadioButton.Location = new System.Drawing.Point(6, 21);
            this.DraftRadioButton.Name = "DraftRadioButton";
            this.DraftRadioButton.Size = new System.Drawing.Size(136, 17);
            this.DraftRadioButton.TabIndex = 0;
            this.DraftRadioButton.TabStop = true;
            this.DraftRadioButton.Text = "0.4 Minor version (draft)";
            this.DraftRadioButton.UseVisualStyleBackColor = true;
            this.DraftRadioButton.CheckedChanged += new System.EventHandler(this.DraftRadioButton_CheckedChanged);
            // 
            // OverwriteRadioButton
            // 
            this.OverwriteRadioButton.AutoSize = true;
            this.OverwriteRadioButton.Location = new System.Drawing.Point(6, 67);
            this.OverwriteRadioButton.Name = "OverwriteRadioButton";
            this.OverwriteRadioButton.Size = new System.Drawing.Size(207, 17);
            this.OverwriteRadioButton.TabIndex = 1;
            this.OverwriteRadioButton.Text = "0.3 Overwrite the current minor version";
            this.OverwriteRadioButton.UseVisualStyleBackColor = true;
            this.OverwriteRadioButton.CheckedChanged += new System.EventHandler(this.OverwriteRadioButton_CheckedChanged);
            // 
            // PublishRadioButton
            // 
            this.PublishRadioButton.AutoSize = true;
            this.PublishRadioButton.Location = new System.Drawing.Point(6, 44);
            this.PublishRadioButton.Name = "PublishRadioButton";
            this.PublishRadioButton.Size = new System.Drawing.Size(148, 17);
            this.PublishRadioButton.TabIndex = 2;
            this.PublishRadioButton.Text = "1.0 Major version (publish)";
            this.PublishRadioButton.UseVisualStyleBackColor = true;
            this.PublishRadioButton.CheckedChanged += new System.EventHandler(this.PublishRadioButton_CheckedChanged);
            // 
            // YesRadioButton
            // 
            this.YesRadioButton.AutoSize = true;
            this.YesRadioButton.Location = new System.Drawing.Point(7, 30);
            this.YesRadioButton.Name = "YesRadioButton";
            this.YesRadioButton.Size = new System.Drawing.Size(43, 17);
            this.YesRadioButton.TabIndex = 4;
            this.YesRadioButton.Text = "Yes";
            this.YesRadioButton.UseVisualStyleBackColor = true;
            // 
            // NoRadioButton
            // 
            this.NoRadioButton.AutoSize = true;
            this.NoRadioButton.Checked = true;
            this.NoRadioButton.Location = new System.Drawing.Point(56, 30);
            this.NoRadioButton.Name = "NoRadioButton";
            this.NoRadioButton.Size = new System.Drawing.Size(39, 17);
            this.NoRadioButton.TabIndex = 5;
            this.NoRadioButton.TabStop = true;
            this.NoRadioButton.Text = "No";
            this.NoRadioButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Comments";
            // 
            // CommentsTextBox
            // 
            this.CommentsTextBox.Location = new System.Drawing.Point(12, 188);
            this.CommentsTextBox.Multiline = true;
            this.CommentsTextBox.Name = "CommentsTextBox";
            this.CommentsTextBox.Size = new System.Drawing.Size(347, 86);
            this.CommentsTextBox.TabIndex = 7;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(197, 287);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 8;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(284, 286);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 9;
            this.CancelButton.Text = "&Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OverwriteRadioButton);
            this.groupBox1.Controls.Add(this.DraftRadioButton);
            this.groupBox1.Controls.Add(this.PublishRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 90);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "What kind of version would you like to check in?";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.NoRadioButton);
            this.groupBox2.Controls.Add(this.YesRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 104);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(347, 59);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Keep the document checked out after checking in this version?";
            // 
            // CheckInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 320);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CommentsTextBox);
            this.Controls.Add(this.label2);
            this.Name = "CheckInForm";
            this.Text = "Check In";
            this.Load += new System.EventHandler(this.CheckInForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton DraftRadioButton;
        private System.Windows.Forms.RadioButton OverwriteRadioButton;
        private System.Windows.Forms.RadioButton PublishRadioButton;
        private System.Windows.Forms.RadioButton YesRadioButton;
        private System.Windows.Forms.RadioButton NoRadioButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CommentsTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}