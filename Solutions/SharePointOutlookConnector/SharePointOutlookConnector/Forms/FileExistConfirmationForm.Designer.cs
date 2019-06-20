namespace Sobiens.Office.SharePointOutlookConnector.Forms
{
    partial class FileExistConfirmationForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.SkipButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.DoThisForNextConflictsCheckBox = new System.Windows.Forms.CheckBox();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ReplaceButton = new System.Windows.Forms.Button();
            this.DontCopyButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Replace_FileLocationLabel = new System.Windows.Forms.Label();
            this.Replace_FileNameLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DontCopy_FileLocationLabel = new System.Windows.Forms.Label();
            this.DontCopy_FileNameLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CopyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.Head;
            this.pictureBox1.Location = new System.Drawing.Point(-1, -4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(668, 72);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.SkipButton);
            this.panel1.Controls.Add(this.CancelButton);
            this.panel1.Controls.Add(this.DoThisForNextConflictsCheckBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 379);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(667, 52);
            this.panel1.TabIndex = 16;
            // 
            // SkipButton
            // 
            this.SkipButton.BackColor = System.Drawing.Color.Transparent;
            this.SkipButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SkipButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.SkipButton.Location = new System.Drawing.Point(500, 9);
            this.SkipButton.Name = "SkipButton";
            this.SkipButton.Size = new System.Drawing.Size(79, 35);
            this.SkipButton.TabIndex = 21;
            this.SkipButton.Text = "Skip";
            this.SkipButton.UseVisualStyleBackColor = false;
            this.SkipButton.Click += new System.EventHandler(this.SkipButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.BackColor = System.Drawing.Color.Transparent;
            this.CancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.CancelButton.Location = new System.Drawing.Point(585, 9);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(79, 35);
            this.CancelButton.TabIndex = 21;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = false;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // DoThisForNextConflictsCheckBox
            // 
            this.DoThisForNextConflictsCheckBox.AutoSize = true;
            this.DoThisForNextConflictsCheckBox.Location = new System.Drawing.Point(12, 19);
            this.DoThisForNextConflictsCheckBox.Name = "DoThisForNextConflictsCheckBox";
            this.DoThisForNextConflictsCheckBox.Size = new System.Drawing.Size(139, 17);
            this.DoThisForNextConflictsCheckBox.TabIndex = 0;
            this.DoThisForNextConflictsCheckBox.Text = "Do this for next conflicts";
            this.DoThisForNextConflictsCheckBox.UseVisualStyleBackColor = true;
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Location = new System.Drawing.Point(6, 47);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(232, 24);
            this.FileNameTextBox.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(26, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(466, 20);
            this.label1.TabIndex = 19;
            this.label1.Text = "There is already a file with the same name in this location";
            // 
            // ReplaceButton
            // 
            this.ReplaceButton.BackColor = System.Drawing.Color.Transparent;
            this.ReplaceButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ReplaceButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReplaceButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.ReplaceButton.Location = new System.Drawing.Point(375, 10);
            this.ReplaceButton.Name = "ReplaceButton";
            this.ReplaceButton.Size = new System.Drawing.Size(79, 35);
            this.ReplaceButton.TabIndex = 22;
            this.ReplaceButton.Text = "Replace";
            this.ReplaceButton.UseVisualStyleBackColor = false;
            this.ReplaceButton.Click += new System.EventHandler(this.ReplaceButton_Click);
            // 
            // DontCopyButton
            // 
            this.DontCopyButton.BackColor = System.Drawing.Color.Transparent;
            this.DontCopyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.DontCopyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DontCopyButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.DontCopyButton.Location = new System.Drawing.Point(375, 11);
            this.DontCopyButton.Name = "DontCopyButton";
            this.DontCopyButton.Size = new System.Drawing.Size(79, 35);
            this.DontCopyButton.TabIndex = 23;
            this.DontCopyButton.Text = "Don\'t Copy";
            this.DontCopyButton.UseVisualStyleBackColor = false;
            this.DontCopyButton.Click += new System.EventHandler(this.DontCopyButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Click the file you want to keep";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DarkBlue;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(330, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Replace the file in the destination folder with the file you are copying:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Replace_FileLocationLabel);
            this.groupBox1.Controls.Add(this.Replace_FileNameLabel);
            this.groupBox1.Controls.Add(this.ReplaceButton);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.DarkBlue;
            this.groupBox1.Location = new System.Drawing.Point(33, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 80);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Copy and Replace";
            // 
            // Replace_FileLocationLabel
            // 
            this.Replace_FileLocationLabel.AutoSize = true;
            this.Replace_FileLocationLabel.Location = new System.Drawing.Point(6, 57);
            this.Replace_FileLocationLabel.Name = "Replace_FileLocationLabel";
            this.Replace_FileLocationLabel.Size = new System.Drawing.Size(52, 17);
            this.Replace_FileLocationLabel.TabIndex = 29;
            this.Replace_FileLocationLabel.Text = "label6";
            // 
            // Replace_FileNameLabel
            // 
            this.Replace_FileNameLabel.AutoSize = true;
            this.Replace_FileNameLabel.Location = new System.Drawing.Point(6, 39);
            this.Replace_FileNameLabel.Name = "Replace_FileNameLabel";
            this.Replace_FileNameLabel.Size = new System.Drawing.Size(52, 17);
            this.Replace_FileNameLabel.TabIndex = 28;
            this.Replace_FileNameLabel.Text = "label6";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DontCopy_FileLocationLabel);
            this.groupBox2.Controls.Add(this.DontCopy_FileNameLabel);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.DontCopyButton);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.DarkBlue;
            this.groupBox2.Location = new System.Drawing.Point(33, 202);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(460, 80);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Don\'t Copy";
            // 
            // DontCopy_FileLocationLabel
            // 
            this.DontCopy_FileLocationLabel.AutoSize = true;
            this.DontCopy_FileLocationLabel.Location = new System.Drawing.Point(6, 56);
            this.DontCopy_FileLocationLabel.Name = "DontCopy_FileLocationLabel";
            this.DontCopy_FileLocationLabel.Size = new System.Drawing.Size(52, 17);
            this.DontCopy_FileLocationLabel.TabIndex = 29;
            this.DontCopy_FileLocationLabel.Text = "label8";
            // 
            // DontCopy_FileNameLabel
            // 
            this.DontCopy_FileNameLabel.AutoSize = true;
            this.DontCopy_FileNameLabel.Location = new System.Drawing.Point(6, 39);
            this.DontCopy_FileNameLabel.Name = "DontCopy_FileNameLabel";
            this.DontCopy_FileNameLabel.Size = new System.Drawing.Size(52, 17);
            this.DontCopy_FileNameLabel.TabIndex = 28;
            this.DontCopy_FileNameLabel.Text = "label7";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkBlue;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(305, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "No files will be changed. Leave this file in the destination folder:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.CopyButton);
            this.groupBox3.Controls.Add(this.FileNameTextBox);
            this.groupBox3.Font = new System.Drawing.Font("MS Reference Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.DarkBlue;
            this.groupBox3.Location = new System.Drawing.Point(33, 288);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(460, 80);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Copy, but keep both files";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DarkBlue;
            this.label5.Location = new System.Drawing.Point(6, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(298, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "The file you are copying will be renamed with your below input";
            // 
            // CopyButton
            // 
            this.CopyButton.BackColor = System.Drawing.Color.Transparent;
            this.CopyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CopyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CopyButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.btn;
            this.CopyButton.Location = new System.Drawing.Point(375, 39);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(79, 35);
            this.CopyButton.TabIndex = 23;
            this.CopyButton.Text = "Copy";
            this.CopyButton.UseVisualStyleBackColor = false;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // FileExistConfirmationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(667, 431);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileExistConfirmationForm";
            this.Text = "Upload File";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FileExistConfirmationForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox DoThisForNextConflictsCheckBox;
        private System.Windows.Forms.Button SkipButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ReplaceButton;
        private System.Windows.Forms.Button DontCopyButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button CopyButton;
        private System.Windows.Forms.Label Replace_FileLocationLabel;
        private System.Windows.Forms.Label Replace_FileNameLabel;
        private System.Windows.Forms.Label DontCopy_FileLocationLabel;
        private System.Windows.Forms.Label DontCopy_FileNameLabel;
    }
}