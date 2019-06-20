using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Microsoft.Office.Tools.Excel.Extensions;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Windows.Controls;
using Sobiens.Connectors.Entities;
using Sobiens.WPF.Controls.Classes;
using Sobiens.Connectors.WPF.Controls.Excel;
using Microsoft.Office.Tools;
using System.Runtime.InteropServices;

namespace Sobiens.Connectors.ExcelConnector
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Application.WorkbookOpen+=new Excel.AppEvents_WorkbookOpenEventHandler(Application_WorkbookOpen); 
            Application.WorkbookActivate +=new Excel.AppEvents_WorkbookActivateEventHandler(Application_WorkbookActivate);

            ConfigurationManager.GetInstance().DownloadAdministrationXml(RefreshControls);
        }

        private void AddAllTaskPanes()
        {
            foreach (Excel.Window window in this.Application.Windows)
            {
                CustomTaskPane ctp = this.CustomTaskPanes.SingleOrDefault(t => t.Window == window);
                if (ctp == null)
                {
                    ExcelConnectorPane taskPaneControl1 = new ExcelConnectorPane();

                    IConnectorMainView connectorExplorer = ((System.Windows.Forms.Integration.ElementHost)taskPaneControl1.Controls[0]).Child as IConnectorMainView;
                    ApplicationContext.SetApplicationManager(new ExcelConnectorManager(Application, connectorExplorer));

                    Microsoft.Office.Tools.CustomTaskPane taskPaneValue = this.CustomTaskPanes.Add(taskPaneControl1, "Sobiens Office Connector", window);
                    taskPaneValue.VisibleChanged += new EventHandler(taskPaneValue_VisibleChanged);
                }
            }
        }

        private void RemoveOrphanedTaskPanes()
        {
            for (int i = this.CustomTaskPanes.Count; i > 0; i--)
            {
                CustomTaskPane ctp = this.CustomTaskPanes[i - 1];
                if (ctp.Window == null)
                {
                    this.CustomTaskPanes.Remove(ctp);
                }
            }
        }


        void Application_WorkbookActivate(Excel.Workbook Wb)
        {
            this.RemoveOrphanedTaskPanes();
            this.AddAllTaskPanes();
        }

        void Application_WorkbookOpen(Excel.Workbook Wb)
        {
            this.RemoveOrphanedTaskPanes();
            this.AddAllTaskPanes();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void RefreshControls()
        {
            ApplicationContext.Current.RefreshControlsFromConfiguration();
        }

        public Microsoft.Office.Tools.CustomTaskPane TaskPane
        {
            get
            {
                return Globals.ThisAddIn.CustomTaskPanes.Single(t => ((Excel.Window)t.Window).WindowNumber == Globals.ThisAddIn.Application.ActiveWindow.WindowNumber && ((Excel.Window)t.Window).Caption.ToString().Equals(Globals.ThisAddIn.Application.ActiveWindow.Caption.ToString(), StringComparison.InvariantCultureIgnoreCase));
            }
        }

        private void taskPaneValue_VisibleChanged(object sender, System.EventArgs e)
        {

        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new ConnectorRibbon();
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
