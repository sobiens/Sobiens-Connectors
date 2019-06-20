namespace Sobiens.Office.SharePointOutlookConnector.Forms
{
    partial class EmailMappingFieldSelectionForm
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
            this.EditControlsPanel = new System.Windows.Forms.Panel();
            this.FieldsComboBox = new System.Windows.Forms.ComboBox();
            this.ActionControlsPanel = new System.Windows.Forms.Panel();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.EditControlsPanel.SuspendLayout();
            this.ActionControlsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // EditControlsPanel
            // 
            this.EditControlsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.EditControlsPanel.AutoScroll = true;
            this.EditControlsPanel.Controls.Add(this.FieldsComboBox);
            this.EditControlsPanel.Location = new System.Drawing.Point(0, 73);
            this.EditControlsPanel.Name = "EditControlsPanel";
            this.EditControlsPanel.Size = new System.Drawing.Size(672, 326);
            this.EditControlsPanel.TabIndex = 33;
            // 
            // FieldsComboBox
            // 
            this.FieldsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FieldsComboBox.FormattingEnabled = true;
            this.FieldsComboBox.Location = new System.Drawing.Point(12, 22);
            this.FieldsComboBox.Name = "FieldsComboBox";
            this.FieldsComboBox.Size = new System.Drawing.Size(182, 21);
            this.FieldsComboBox.TabIndex = 34;
            // 
            // ActionControlsPanel
            // 
            this.ActionControlsPanel.Controls.Add(this.CancelButton);
            this.ActionControlsPanel.Controls.Add(this.OkButton);
            this.ActionControlsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ActionControlsPanel.Location = new System.Drawing.Point(0, 402);
            this.ActionControlsPanel.Name = "ActionControlsPanel";
            this.ActionControlsPanel.Size = new System.Drawing.Size(672, 31);
            this.ActionControlsPanel.TabIndex = 34;
            // 
            // CancelButton
            // 
            this.CancelButton.BackgroundImage = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.CancelButton.Location = new System.Drawing.Point(589, 3);
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
            this.OkButton.Location = new System.Drawing.Point(508, 3);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 31;
            this.OkButton.Text = "&Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.Head;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(672, 73);
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // EmailMappingFieldSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(672, 433);
            this.Controls.Add(this.ActionControlsPanel);
            this.Controls.Add(this.EditControlsPanel);
            this.Controls.Add(this.pictureBox1);
            this.Name = "EmailMappingFieldSelectionForm";
            this.Text = "Mapping Field Selection";
            this.EditControlsPanel.ResumeLayout(false);
            this.ActionControlsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Panel EditControlsPanel;
        private System.Windows.Forms.Panel ActionControlsPanel;
        private System.Windows.Forms.ComboBox FieldsComboBox;
    }
}