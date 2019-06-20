namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class ManageTaskPaneRibbon
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
            this.tab1 = new Microsoft.Office.Tools.Ribbon.RibbonTab();
            this.group1 = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.ShowConnectorToggleButton = new Microsoft.Office.Tools.Ribbon.RibbonToggleButton();
            this.AttachmentsToSPButton = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "SP Outlook Connector";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.ShowConnectorToggleButton);
            this.group1.Items.Add(this.AttachmentsToSPButton);
            this.group1.Label = "SharePoint Outlook Connector";
            this.group1.Name = "group1";
            // 
            // ShowConnectorToggleButton
            // 
            this.ShowConnectorToggleButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.Sobiens_20x20;
            this.ShowConnectorToggleButton.Label = "Show Connector";
            this.ShowConnectorToggleButton.Name = "ShowConnectorToggleButton";
            this.ShowConnectorToggleButton.ShowImage = true;
            this.ShowConnectorToggleButton.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.ShowConnectorToggleButton_Click);
            // 
            // AttachmentsToSPButton
            // 
            this.AttachmentsToSPButton.Image = global::Sobiens.Office.SharePointOutlookConnector.Properties.Resources.ATTACH;
            this.AttachmentsToSPButton.Label = "Attachments to SP";
            this.AttachmentsToSPButton.Name = "AttachmentsToSPButton";
            this.AttachmentsToSPButton.ShowImage = true;
            this.AttachmentsToSPButton.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.AttachmentsToSPButton_Click);
            // 
            // ManageTaskPaneRibbon
            // 
            this.Name = "ManageTaskPaneRibbon";
            this.RibbonType = "Microsoft.Outlook.Mail.Compose, Microsoft.Outlook.Mail.Read";
            this.Tabs.Add(this.tab1);
            this.Load += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonUIEventArgs>(this.ManageTaskPaneRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton AttachmentsToSPButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton ShowConnectorToggleButton;
    }

    partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonReadOnlyCollection
    {
        internal ManageTaskPaneRibbon ManageTaskPaneRibbon
        {
            get { return this.GetRibbon<ManageTaskPaneRibbon>(); }
        }
    }
}
