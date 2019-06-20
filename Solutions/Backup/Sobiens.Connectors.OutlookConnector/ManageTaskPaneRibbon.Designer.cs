namespace Sobiens.Connectors.OutlookConnector
{
    partial class ManageTaskPaneRibbon : Microsoft.Office.Tools.Ribbon.OfficeRibbon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ManageTaskPaneRibbon()
        {
            InitializeComponent();
        }

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
            this.OfficeConnectorMailItemTab = new Microsoft.Office.Tools.Ribbon.RibbonTab();
            this.SPOC = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.ShowConnectorToggleButton = new Microsoft.Office.Tools.Ribbon.RibbonToggleButton();
            this.AttachmentsToSPButton = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.OfficeConnectorMailItemTab.SuspendLayout();
            this.SPOC.SuspendLayout();
            this.SuspendLayout();
            // 
            // OfficeConnectorMailItemTab
            // 
            this.OfficeConnectorMailItemTab.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.OfficeConnectorMailItemTab.Groups.Add(this.SPOC);
            this.OfficeConnectorMailItemTab.Label = "Office Connector";
            this.OfficeConnectorMailItemTab.Name = "OfficeConnectorMailItemTab";
            // 
            // SPOC
            // 
            this.SPOC.Items.Add(this.ShowConnectorToggleButton);
            this.SPOC.Items.Add(this.AttachmentsToSPButton);
            this.SPOC.Label = "Office Connector";
            this.SPOC.Name = "SPOC";
            // 
            // ShowConnectorToggleButton
            // 
            this.ShowConnectorToggleButton.Image = global::Sobiens.Connectors.OutlookConnector.Properties.Resources.Sobiens_20x20;
            this.ShowConnectorToggleButton.Label = "Show Connector";
            this.ShowConnectorToggleButton.Name = "ShowConnectorToggleButton";
            this.ShowConnectorToggleButton.ShowImage = true;
            this.ShowConnectorToggleButton.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.ShowConnectorToggleButton_Click);
            // 
            // AttachmentsToSPButton
            // 
            this.AttachmentsToSPButton.Image = global::Sobiens.Connectors.OutlookConnector.Properties.Resources.ATTACH;
            this.AttachmentsToSPButton.Label = "Attachments to SP";
            this.AttachmentsToSPButton.Name = "AttachmentsToSPButton";
            this.AttachmentsToSPButton.ShowImage = true;
            this.AttachmentsToSPButton.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.AttachmentsToSPButton_Click);
            // 
            // ManageTaskPaneRibbon
            // 
            this.Name = "ManageTaskPaneRibbon";
            this.RibbonType = "Microsoft.Outlook.Mail.Compose";
            this.Tabs.Add(this.OfficeConnectorMailItemTab);
            this.Load += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonUIEventArgs>(this.ManageTaskPaneRibbon_Load);
            this.OfficeConnectorMailItemTab.ResumeLayout(false);
            this.OfficeConnectorMailItemTab.PerformLayout();
            this.SPOC.ResumeLayout(false);
            this.SPOC.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab OfficeConnectorMailItemTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup SPOC;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton ShowConnectorToggleButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton AttachmentsToSPButton;
    }

    partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonReadOnlyCollection
    {
        internal ManageTaskPaneRibbon ManageTaskPaneRibbon
        {
            get { return this.GetRibbon<ManageTaskPaneRibbon>(); }
        }
    }
}
