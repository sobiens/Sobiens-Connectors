using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Officex = Microsoft.Office.Core;
using System.Windows.Forms;
using EmailUploader.BLL;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL;
using EmailUploader.BLL.Entities;
using Microsoft.Office.Interop.Outlook;
using Sobiens.Office.SharePointOutlookConnector.Forms;
namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class ThisAddIn
    {
        private SharePointExplorerPane sharePointExplorerPane;
        private Microsoft.Office.Tools.CustomTaskPane _tpMoveToSP;
        Microsoft.Office.Core.CommandBarButton spocButton = null;

        private Dictionary<Outlook.Inspector, InspectorWrapper> inspectorWrappersValue =
            new Dictionary<Outlook.Inspector, InspectorWrapper>();
        private Outlook.Inspectors inspectors;

        public Dictionary<Outlook.Inspector, InspectorWrapper> InspectorWrappers
        {
            get
            {
                return inspectorWrappersValue;
            }
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            LogManager.Log("Entered ThisAddIn_Startup", EULogModes.Normal);

            LogManager.Log("Checking Activation...", EULogModes.Normal);
            /*
            if(ActivationManager.CheckActivation() == false)
            {
                ActivateForm activateForm = new ActivateForm();
                activateForm.ShowDialog();
                if (ActivationManager.CheckActivation() == false)
                {
                    return;
                }
            }
             */ 
            /*
            DateTime expiryDate = new DateTime(2011, 2, 20);
            if (DateTime.Now > expiryDate)
            {
                MessageBox.Show("The trial period has expired", "SharePoint Outlook Connector");
                return;
            }
             */ 

            LogManager.Log("Initializing SharePointExplorerPane", EULogModes.Normal);
            sharePointExplorerPane = new SharePointExplorerPane();
            LogManager.Log("Setting reference of application object", EULogModes.Normal);
            sharePointExplorerPane.Application = Application;
            SystemInformationManager.OutlookVersion = Application.Version;

            LogManager.Log("Adding SharePointExplorerPane", EULogModes.Normal);
            _tpMoveToSP = this.CustomTaskPanes.Add(sharePointExplorerPane, "Sobiens SharePointOutlookConnector");
            this.Application.Inspectors.NewInspector += new Microsoft.Office.Interop.Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_NewInspector);
            LogManager.Log("Making SharePointExplorerPane visible", EULogModes.Normal);
            _tpMoveToSP.Visible = true;
            SetSPOCButton();
            LogManager.Log("Exiting ThisAddIn_Startup", EULogModes.Normal);

            inspectors = this.Application.Inspectors;
            inspectors.NewInspector += new Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_NewInspector);

            foreach (Outlook.Inspector inspector in inspectors)
            {
                Inspectors_NewInspector(inspector);
            }
        }

        void Inspectors_NewInspector(Microsoft.Office.Interop.Outlook.Inspector Inspector)
        {
            /*
            CurrentInspector = Inspector;
            //SetSaveAttachmentsToSharePointButton();
             */
            if (Inspector.CurrentItem is Outlook.MailItem)
            {
                inspectorWrappersValue.Add(Inspector, new InspectorWrapper(Inspector));
            }
        }


        private void SetSPOCButton()
        {
            Microsoft.Office.Core.CommandBar cmdBar = this.Application.ActiveExplorer().CommandBars["Standard"];
            //Try to get the existing Button
            spocButton = null;
            try
            {
                spocButton = (Microsoft.Office.Core.CommandBarButton)cmdBar.Controls["SPOC"];
            }
            catch (System.Exception)
            {
                spocButton = null;
            }

            //If the button doesn't exist, create a new one
            if (spocButton == null)
            {
                spocButton = (Microsoft.Office.Core.CommandBarButton)cmdBar.Controls.Add(1, missing, missing, missing, false);
            }
            spocButton.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonIconAndCaption;
            spocButton.Caption = "SPOC";
            spocButton.Tag = "SPOC";
            spocButton.TooltipText = "SharePoint Outlook Connector";
            ButtonHelper.GetInstance().SetButtonPicture(ref spocButton, ImageManager.GetInstance().GetSobiens20X20Image(), null);

            //Add a click handler for this button

            spocButton.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(spocButton_Click);
        }

        public void spocButton_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            _tpMoveToSP.Visible = !_tpMoveToSP.Visible;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            if (inspectors == null)
                return;
            inspectors.NewInspector -= new Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_NewInspector);
            inspectors = null;
            inspectorWrappersValue = null;
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
