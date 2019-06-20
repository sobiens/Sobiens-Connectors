namespace Sobiens.Office.SharePointOutlookConnector.Controls
{
    partial class EmailFieldMappingControl
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
            this.components = new System.ComponentModel.Container();
            this.MappingFieldCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // MappingFieldCheckBox
            // 
            this.MappingFieldCheckBox.AutoSize = true;
            this.MappingFieldCheckBox.Location = new System.Drawing.Point(3, 3);
            this.MappingFieldCheckBox.Name = "MappingFieldCheckBox";
            this.MappingFieldCheckBox.Size = new System.Drawing.Size(88, 17);
            this.MappingFieldCheckBox.TabIndex = 0;
            this.MappingFieldCheckBox.Text = "Not Selected";
            this.MappingFieldCheckBox.UseVisualStyleBackColor = true;
            this.MappingFieldCheckBox.CheckedChanged += new System.EventHandler(this.MappingFieldCheckBox_CheckedChanged);
            // 
            // EmailFieldMappingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MappingFieldCheckBox);
            this.Name = "EmailFieldMappingControl";
            this.Size = new System.Drawing.Size(101, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox MappingFieldCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}
