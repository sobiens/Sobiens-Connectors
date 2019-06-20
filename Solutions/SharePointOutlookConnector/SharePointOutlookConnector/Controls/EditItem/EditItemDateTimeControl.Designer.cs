namespace Sobiens.Office.SharePointOutlookConnector.Controls.EditItem
{
    partial class EditItemDateTimeControl
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
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.emailFieldMappingControl1 = new Sobiens.Office.SharePointOutlookConnector.Controls.EmailFieldMappingControl();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(0, 0);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowCheckBox = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(131, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // emailFieldMappingControl1
            // 
            this.emailFieldMappingControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.emailFieldMappingControl1.Location = new System.Drawing.Point(260, 0);
            this.emailFieldMappingControl1.Name = "emailFieldMappingControl1";
            this.emailFieldMappingControl1.RootFolder = null;
            this.emailFieldMappingControl1.SelectedEmailMappingField = Sobiens.Office.SharePointOutlookConnector.BLL.Entities.EUEmailFields.NotSelected;
            this.emailFieldMappingControl1.Size = new System.Drawing.Size(125, 28);
            this.emailFieldMappingControl1.TabIndex = 1;
            // 
            // EditItemDateTimeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.emailFieldMappingControl1);
            this.Controls.Add(this.dateTimePicker1);
            this.Name = "EditItemDateTimeControl";
            this.Size = new System.Drawing.Size(385, 20);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private EmailFieldMappingControl emailFieldMappingControl1;

    }
}
