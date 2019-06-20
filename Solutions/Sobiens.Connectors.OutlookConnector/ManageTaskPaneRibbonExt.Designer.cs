namespace Sobiens.Connectors.OutlookConnector
{
    partial class ManageTaskPaneRibbonExt : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ManageTaskPaneRibbonExt()
            : base(Globals.Factory.GetRibbonFactory())
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
            this.OfficeConnectorMailItemTab = this.Factory.CreateRibbonTab();
            this.SPOC = this.Factory.CreateRibbonGroup();
            this.AttachmentsToSPButton = this.Factory.CreateRibbonToggleButton();
            this.ShowConnectorToggleButton = this.Factory.CreateRibbonToggleButton();
            this.OfficeConnectorMailItemTab.SuspendLayout();
            this.SPOC.SuspendLayout();
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
            // AttachmentsToSPButton
            // 
            this.AttachmentsToSPButton.Image = global::Sobiens.Connectors.OutlookConnector.Properties.Resources.ATTACH;
            this.AttachmentsToSPButton.Label = "Attachments To SP";
            this.AttachmentsToSPButton.Name = "AttachmentsToSPButton";
            this.AttachmentsToSPButton.ShowImage = true;
            this.AttachmentsToSPButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.AttachmentsToSPButton_Click);
            // 
            // ShowConnectorToggleButton
            // 
            this.ShowConnectorToggleButton.Image = global::Sobiens.Connectors.OutlookConnector.Properties.Resources.Sobiens_20x20;
            this.ShowConnectorToggleButton.Label = "Show Connector";
            this.ShowConnectorToggleButton.Name = "ShowConnectorToggleButton";
            this.ShowConnectorToggleButton.ShowImage = true;
            this.ShowConnectorToggleButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ShowConnectorToggleButton_Click);
            // 
            // ManageTaskPaneRibbonExt
            // 
            this.Name = "ManageTaskPaneRibbonExt";
            this.RibbonType = "Microsoft.Outlook.Mail.Read";
            this.Tabs.Add(this.OfficeConnectorMailItemTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ManageTaskPaneRibbonExt_Load);
            this.OfficeConnectorMailItemTab.ResumeLayout(false);
            this.OfficeConnectorMailItemTab.PerformLayout();
            this.SPOC.ResumeLayout(false);
            this.SPOC.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab OfficeConnectorMailItemTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup SPOC;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton AttachmentsToSPButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton ShowConnectorToggleButton;
    }

    partial class ThisRibbonCollection
    {
        internal ManageTaskPaneRibbonExt ManageTaskPaneRibbonExt
        {
            get { return this.GetRibbon<ManageTaskPaneRibbonExt>(); }
        }
    }
}
