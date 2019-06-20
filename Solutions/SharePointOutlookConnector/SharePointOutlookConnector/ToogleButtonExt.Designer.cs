namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class ToogleButtonExt
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
            this.ToogleButtonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ToogleButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ToogleButton
            // 
            this.ToogleButton.Location = new System.Drawing.Point(0, 0);
            this.ToogleButton.Name = "ToogleButton";
            this.ToogleButton.Size = new System.Drawing.Size(20, 20);
            this.ToogleButton.TabIndex = 0;
            this.ToogleButton.UseVisualStyleBackColor = true;
            this.ToogleButton.MouseLeave += new System.EventHandler(this.ToogleButton_MouseLeave);
            this.ToogleButton.Click += new System.EventHandler(this.ToogleButton_Click);
            this.ToogleButton.MouseHover += new System.EventHandler(this.ToogleButton_MouseHover);
            // 
            // ToogleButtonExt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ToogleButton);
            this.Name = "ToogleButtonExt";
            this.Size = new System.Drawing.Size(20, 20);
            this.Load += new System.EventHandler(this.ToogleButtonExt_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip ToogleButtonToolTip;
        private System.Windows.Forms.Button ToogleButton;
    }
}
