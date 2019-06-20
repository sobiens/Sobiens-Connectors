namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class UploadProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadProgressForm));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.UploadButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.DestionationLabel = new System.Windows.Forms.Label();
            this.DestinationValueLabel = new System.Windows.Forms.Label();
            this.ProgressValueLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.EmailsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.LogButton = new System.Windows.Forms.Button();
            this.ErrorLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(17, 396);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(394, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // UploadButton
            // 
            this.UploadButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("UploadButton.BackgroundImage")));
            this.UploadButton.Location = new System.Drawing.Point(417, 396);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(75, 23);
            this.UploadButton.TabIndex = 1;
            this.UploadButton.Text = "&Upload";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CancelButton.BackgroundImage")));
            this.CancelButton.Enabled = false;
            this.CancelButton.Location = new System.Drawing.Point(498, 396);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "&Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // DestionationLabel
            // 
            this.DestionationLabel.AutoSize = true;
            this.DestionationLabel.Location = new System.Drawing.Point(12, 78);
            this.DestionationLabel.Name = "DestionationLabel";
            this.DestionationLabel.Size = new System.Drawing.Size(63, 13);
            this.DestionationLabel.TabIndex = 4;
            this.DestionationLabel.Text = "Destination:";
            // 
            // DestinationValueLabel
            // 
            this.DestinationValueLabel.AutoSize = true;
            this.DestinationValueLabel.Location = new System.Drawing.Point(81, 78);
            this.DestinationValueLabel.Name = "DestinationValueLabel";
            this.DestinationValueLabel.Size = new System.Drawing.Size(66, 13);
            this.DestinationValueLabel.TabIndex = 5;
            this.DestinationValueLabel.Text = "[Destination]";
            // 
            // ProgressValueLabel
            // 
            this.ProgressValueLabel.AutoSize = true;
            this.ProgressValueLabel.Location = new System.Drawing.Point(17, 376);
            this.ProgressValueLabel.Name = "ProgressValueLabel";
            this.ProgressValueLabel.Size = new System.Drawing.Size(61, 13);
            this.ProgressValueLabel.TabIndex = 6;
            this.ProgressValueLabel.Text = "Not Started";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.Head;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(594, 72);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // EmailsListView
            // 
            this.EmailsListView.CheckBoxes = true;
            this.EmailsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.EmailsListView.LabelEdit = true;
            this.EmailsListView.Location = new System.Drawing.Point(15, 105);
            this.EmailsListView.Name = "EmailsListView";
            this.EmailsListView.Size = new System.Drawing.Size(553, 269);
            this.EmailsListView.TabIndex = 8;
            this.EmailsListView.UseCompatibleStateImageBehavior = false;
            this.EmailsListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Filename";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Filename";
            this.columnHeader3.Width = 0;
            // 
            // LogButton
            // 
            this.LogButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LogButton.BackgroundImage")));
            this.LogButton.Location = new System.Drawing.Point(417, 425);
            this.LogButton.Name = "LogButton";
            this.LogButton.Size = new System.Drawing.Size(75, 23);
            this.LogButton.TabIndex = 9;
            this.LogButton.Text = "Show &Log";
            this.LogButton.UseVisualStyleBackColor = true;
            this.LogButton.Click += new System.EventHandler(this.LogButton_Click);
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.ErrorLabel.Location = new System.Drawing.Point(140, 430);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(271, 13);
            this.ErrorLabel.TabIndex = 10;
            this.ErrorLabel.Text = "An error occured while uploading. Please check the log.";
            this.ErrorLabel.Visible = false;
            // 
            // UploadProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(593, 447);
            this.Controls.Add(this.ErrorLabel);
            this.Controls.Add(this.LogButton);
            this.Controls.Add(this.EmailsListView);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ProgressValueLabel);
            this.Controls.Add(this.DestinationValueLabel);
            this.Controls.Add(this.DestionationLabel);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.UploadButton);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "UploadProgressForm";
            this.Text = "Email Upload";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label DestionationLabel;
        private System.Windows.Forms.Label DestinationValueLabel;
        private System.Windows.Forms.Label ProgressValueLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListView EmailsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button LogButton;
        private System.Windows.Forms.Label ErrorLabel;
    }
}