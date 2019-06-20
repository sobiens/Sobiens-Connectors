namespace Sobiens.Office.SharePointOutlookConnector.Controls
{
    partial class EmailMappingControl
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
            this.EmailMappingDataGridView = new System.Windows.Forms.DataGridView();
            this.OutlookFieldColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SharePointFieldColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.EmailMappingDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // EmailMappingDataGridView
            // 
            this.EmailMappingDataGridView.AllowUserToAddRows = false;
            this.EmailMappingDataGridView.AllowUserToDeleteRows = false;
            this.EmailMappingDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EmailMappingDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OutlookFieldColumn,
            this.SharePointFieldColumn});
            this.EmailMappingDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmailMappingDataGridView.Location = new System.Drawing.Point(0, 0);
            this.EmailMappingDataGridView.Name = "EmailMappingDataGridView";
            this.EmailMappingDataGridView.RowTemplate.Height = 16;
            this.EmailMappingDataGridView.ShowEditingIcon = false;
            this.EmailMappingDataGridView.Size = new System.Drawing.Size(398, 221);
            this.EmailMappingDataGridView.TabIndex = 0;
            // 
            // OutlookFieldColumn
            // 
            this.OutlookFieldColumn.DataPropertyName = "OutlookFieldName";
            this.OutlookFieldColumn.HeaderText = "Outlook Field";
            this.OutlookFieldColumn.Name = "OutlookFieldColumn";
            this.OutlookFieldColumn.ReadOnly = true;
            this.OutlookFieldColumn.Width = 170;
            // 
            // SharePointFieldColumn
            // 
            this.SharePointFieldColumn.DataPropertyName = "SharePointFieldName";
            this.SharePointFieldColumn.HeaderText = "SharePoint Field";
            this.SharePointFieldColumn.Name = "SharePointFieldColumn";
            this.SharePointFieldColumn.Width = 170;
            // 
            // EmailMappingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EmailMappingDataGridView);
            this.Name = "EmailMappingControl";
            this.Size = new System.Drawing.Size(398, 221);
            ((System.ComponentModel.ISupportInitialize)(this.EmailMappingDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView EmailMappingDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutlookFieldColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SharePointFieldColumn;
    }
}
