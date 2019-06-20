namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class ListItemPropertyGrid
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
            this.ListItemPropertyGridGrid = new System.Windows.Forms.PropertyGrid();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // ListItemPropertyGridGrid
            // 
            this.ListItemPropertyGridGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListItemPropertyGridGrid.Location = new System.Drawing.Point(0, 0);
            this.ListItemPropertyGridGrid.Name = "ListItemPropertyGridGrid";
            this.ListItemPropertyGridGrid.Size = new System.Drawing.Size(150, 150);
            this.ListItemPropertyGridGrid.TabIndex = 0;
            this.ListItemPropertyGridGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.ListItemPropertyGridGrid_PropertyValueChanged);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // ListItemPropertyGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ListItemPropertyGridGrid);
            this.Name = "ListItemPropertyGrid";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid ListItemPropertyGridGrid;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
