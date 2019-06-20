using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using Sobiens.Connectors.Windows.Controls;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.WPF.Controls.Classes;
using Sobiens.Connectors.WPF.Controls.Outlook;
using Sobiens.Connectors.Entities.State;
using Microsoft.Office.Tools;

namespace Sobiens.Connectors.OutlookConnector
{
    public partial class ThisAddIn
    {
        private OutlookConnectorPane taskPaneControl1;
        private Microsoft.Office.Tools.CustomTaskPane taskPaneValue;
        private Dictionary<Outlook.Inspector, InspectorWrapper> inspectorWrappersValue = new Dictionary<Outlook.Inspector, InspectorWrapper>();
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
            try
            {
                taskPaneControl1 = new OutlookConnectorPane();
                IConnectorMainView connectorExplorer = ((System.Windows.Forms.Integration.ElementHost)taskPaneControl1.Controls[0]).Child as IConnectorMainView;

                ApplicationContext.SetApplicationManager(new OutlookConnectorManager(this.Application, connectorExplorer));

                ApplicationContext.Current.ConnectorExplorer.InitializedDate = new DateTime(DateTime.Now.Ticks);
                taskPaneValue = this.CustomTaskPanes.Add(taskPaneControl1, "Sobiens Office Connector");
                taskPaneValue.VisibleChanged += new EventHandler(taskPaneValue_VisibleChanged);
                taskPaneValue.DockPositionChanged += new EventHandler(taskPaneValue_DockPositionChanged);
                taskPaneControl1.SizeChanged += new EventHandler(taskPaneControl1_SizeChanged);
                taskPaneControl1.Resize += new EventHandler(taskPaneControl1_Resize);

                ApplicationBaseState applicationState = StateManager.GetInstance().ConnectorState.GetApplicationState(ApplicationContext.Current.GetApplicationType());
                taskPaneValue.DockPosition = StateManager.GetInstance().GetMsoPaneDockPosition(applicationState.DockPosition);
                taskPaneValue.Visible = applicationState.ConnectorOpen;
                if (applicationState.ConnectorWidth > 0)
                    taskPaneValue.Width = applicationState.ConnectorWidth;
                if (taskPaneValue.DockPosition != Office.MsoCTPDockPosition.msoCTPDockPositionRight && applicationState.ConnectorHeight > 0)
                    taskPaneValue.Height = applicationState.ConnectorHeight;


                inspectors = this.Application.Inspectors;
                inspectors.NewInspector += new Outlook.InspectorsEvents_NewInspectorEventHandler(inspectors_NewInspector);
                this.Application.ItemSend += Application_ItemSend;
                //this.Application.Inspectors.NewInspector += new Microsoft.Office.Interop.Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_NewInspector);
                //SetSPOCButton();

                foreach (Outlook.Inspector inspector in inspectors)
                {
                    inspectors_NewInspector(inspector);
                }

                ConfigurationManager.GetInstance().DownloadAdministrationXml(RefreshControls);
            }
            catch (Exception ex)
            {
                Logger.Info(string.Format("An error occured on outlook startup: {0}", ex.Message), ApplicationContext.Current.GetApplicationType().ToString());
            }
        }

        void Application_ItemSend(object Item, ref bool Cancel)
        {
            Microsoft.Office.Interop.Outlook.MailItem inspector = Item as Microsoft.Office.Interop.Outlook.MailItem;
            if (inspector.Attachments.Count == 0)
                return;

            System.Windows.Forms.MessageBox.Show("testtt");
            Cancel = true;
        }

        void taskPaneControl1_Resize(object sender, EventArgs e)
        {
        }

        void taskPaneControl1_SizeChanged(object sender, EventArgs e)
        {
            if (ApplicationContext.Current.ConnectorExplorer.InitializedDate.AddSeconds(10) < DateTime.Now)
            {
                OutlookConnectorPane pane = (OutlookConnectorPane)sender;
                ApplicationBaseState applicationState = StateManager.GetInstance().ConnectorState.GetApplicationState(ApplicationContext.Current.GetApplicationType());
                applicationState.ConnectorWidth = pane.Width;
                applicationState.ConnectorHeight = pane.Height;
                StateManager.GetInstance().SaveState();
            }
        }

        void taskPaneValue_DockPositionChanged(object sender, EventArgs e)
        {
            CustomTaskPane pane = (CustomTaskPane)sender;
            ApplicationBaseState applicationState = StateManager.GetInstance().ConnectorState.GetApplicationState(ApplicationContext.Current.GetApplicationType());
            applicationState.DockPosition = StateManager.GetInstance().GetConnectorPaneDockPosition(pane.DockPosition);
            StateManager.GetInstance().SaveState();
        }

        void inspectors_NewInspector(Outlook.Inspector Inspector)
        {
            if (Inspector.CurrentItem is Outlook.MailItem)
            {
                inspectorWrappersValue.Add(Inspector, new InspectorWrapper(Inspector));
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            if (inspectors == null)
                return;
            inspectors.NewInspector -= new Outlook.InspectorsEvents_NewInspectorEventHandler(inspectors_NewInspector);
            inspectors = null;
            inspectorWrappersValue = null;
        }

        private void RefreshControls()
        {
            ApplicationContext.Current.RefreshControlsFromConfiguration();
        }

        public Microsoft.Office.Tools.CustomTaskPane TaskPane
        {
            get
            {
                return taskPaneValue;
            }
        }

        private void taskPaneValue_VisibleChanged(object sender, System.EventArgs e)
        {
            int y = 4;
        }

        /*
        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {

            if (this.GetType() == typeof(string))
            {
                return base.CreateRibbonExtensibilityObject();
            }

            return new OutlookConnectorRibbon();
        }
         */ 

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
