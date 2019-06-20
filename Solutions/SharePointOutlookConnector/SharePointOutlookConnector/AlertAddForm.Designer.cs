namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class AlertAddForm
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
            this.NextButton = new System.Windows.Forms.Button();
            this.ListsComboBox = new System.Windows.Forms.ComboBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ListLabel = new System.Windows.Forms.Label();
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
            // NextButton
            // 
            this.NextButton.BackColor = System.Drawing.Color.Transparent;
            this.NextButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.NextButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.NextButton.Location = new System.Drawing.Point(191, 154);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(79, 35);
            this.NextButton.TabIndex = 15;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = false;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // ListsComboBox
            // 
            this.ListsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ListsComboBox.FormattingEnabled = true;
            this.ListsComboBox.Location = new System.Drawing.Point(145, 106);
            this.ListsComboBox.Name = "ListsComboBox";
            this.ListsComboBox.Size = new System.Drawing.Size(210, 21);
            this.ListsComboBox.TabIndex = 16;
            // 
            // CancelButton
            // 
            this.CancelButton.BackColor = System.Drawing.Color.Transparent;
            this.CancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.CancelButton.Location = new System.Drawing.Point(276, 154);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(79, 35);
            this.CancelButton.TabIndex = 20;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = false;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ListLabel
            // 
            this.ListLabel.AutoSize = true;
            this.ListLabel.Location = new System.Drawing.Point(44, 109);
            this.ListLabel.Name = "ListLabel";
            this.ListLabel.Size = new System.Drawing.Size(26, 13);
            this.ListLabel.TabIndex = 24;
            this.ListLabel.Text = "List:";
            // 
            // AlertAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(669, 436);
            this.Controls.Add(this.ListLabel);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ListsComboBox);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AlertAddForm";
            this.Text = "Alerts Maintenance";
            this.Load += new System.EventHandler(this.AlertMaintenanceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.ComboBox ListsComboBox;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label ListLabel;
    }
}