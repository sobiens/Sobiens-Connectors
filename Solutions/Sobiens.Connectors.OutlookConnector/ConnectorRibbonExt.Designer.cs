namespace Sobiens.Connectors.OutlookConnector
{
    partial class ConnectorRibbonExt : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ConnectorRibbonExt()
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
            this.OfficeConnectorExplorerTab = this.Factory.CreateRibbonTab();
            this.MailItemC = this.Factory.CreateRibbonGroup();
            this.ShowConnectorToggleButton = this.Factory.CreateRibbonToggleButton();
            this.SaveToButton = this.Factory.CreateRibbonToggleButton();
            this.OfficeConnectorExplorerTab.SuspendLayout();
            this.MailItemC.SuspendLayout();
            // 
            // OfficeConnectorExplorerTab
            // 
            this.OfficeConnectorExplorerTab.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.OfficeConnectorExplorerTab.Groups.Add(this.MailItemC);
            this.OfficeConnectorExplorerTab.Label = "Office Connector";
            this.OfficeConnectorExplorerTab.Name = "OfficeConnectorExplorerTab";
            // 
            // MailItemC
            // 
            this.MailItemC.Items.Add(this.ShowConnectorToggleButton);
            this.MailItemC.Items.Add(this.SaveToButton);
            this.MailItemC.Label = "Office Connector";
            this.MailItemC.Name = "MailItemC";
            // 
            // ShowConnectorToggleButton
            // 
            this.ShowConnectorToggleButton.Image = global::Sobiens.Connectors.OutlookConnector.Properties.Resources.Sobiens_20x20;
            this.ShowConnectorToggleButton.Label = "Show Connector";
            this.ShowConnectorToggleButton.Name = "ShowConnectorToggleButton";
            this.ShowConnectorToggleButton.ShowImage = true;
            this.ShowConnectorToggleButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ShowConnectorToggleButton_Click);
            // 
            // SaveToButton
            // 
            this.SaveToButton.Image = global::Sobiens.Connectors.OutlookConnector.Properties.Resources.Sobiens_20x20;
            this.SaveToButton.Label = "Save To";
            this.SaveToButton.Name = "SaveToButton";
            this.SaveToButton.ShowImage = true;
            this.SaveToButton.Visible = false;
            this.SaveToButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SaveToButton_Click);
            // 
            // ConnectorRibbonExt
            // 
            this.Name = "ConnectorRibbonExt";
            this.RibbonType = "Microsoft.Outlook.Explorer, Microsoft.Outlook.Mail.Read";
            this.Tabs.Add(this.OfficeConnectorExplorerTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ConnectorRibbonExt_Load);
            this.OfficeConnectorExplorerTab.ResumeLayout(false);
            this.OfficeConnectorExplorerTab.PerformLayout();
            this.MailItemC.ResumeLayout(false);
            this.MailItemC.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab OfficeConnectorExplorerTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup MailItemC;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton ShowConnectorToggleButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton SaveToButton;
    }

    partial class ThisRibbonCollection
    {
        internal ConnectorRibbonExt ConnectorRibbonExt
        {
            get { return this.GetRibbon<ConnectorRibbonExt>(); }
        }
    }
}
