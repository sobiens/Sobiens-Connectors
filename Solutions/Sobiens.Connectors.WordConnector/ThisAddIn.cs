using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using Microsoft.Office.Tools.Word.Extensions;
using Sobiens.Connectors.Windows.Controls;
using Sobiens.Connectors.Common;
using Sobiens.WPF.Controls.Classes;
using Sobiens.Connectors.WPF.Controls.Word;
using Sobiens.Connectors.Entities;
using Microsoft.Office.Tools;
using System.Runtime.InteropServices;

namespace Sobiens.Connectors.WordConnector
{
    public partial class ThisAddIn
    {
        private bool IsAdministrationXmlLoaded = false;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Application.DocumentOpen += new Word.ApplicationEvents4_DocumentOpenEventHandler(Application_DocumentOpen);
            Application.DocumentChange += new Word.ApplicationEvents4_DocumentChangeEventHandler(Application_DocumentChange);
        }

        private void AddAllTaskPanes()
        {
            foreach (Word.Window window in this.Application.Windows)
            {
                CustomTaskPane ctp = this.CustomTaskPanes.SingleOrDefault(t => t.Window == window);
                if (ctp == null)
                {
                    WordConnectorPane taskPaneControl1 = new WordConnectorPane();

                    IConnectorMainView connectorExplorer = ((System.Windows.Forms.Integration.ElementHost)taskPaneControl1.Controls[0]).Child as IConnectorMainView;
                    ApplicationContext.SetApplicationManager(new WordConnectorManager(Application, connectorExplorer));

                    Microsoft.Office.Tools.CustomTaskPane taskPaneValue = this.CustomTaskPanes.Add(taskPaneControl1, "Sobiens Office Connector", window);
                    taskPaneValue.VisibleChanged += new EventHandler(taskPaneValue_VisibleChanged);
                }
            }
        }

        private void RemoveOrphanedTaskPanes()
        {
            try
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
            catch { }
        }

        void Application_DocumentChange()
        {
            this.RemoveOrphanedTaskPanes();
            this.AddAllTaskPanes();

            if (IsAdministrationXmlLoaded == false)
            {
                ConfigurationManager.GetInstance().DownloadAdministrationXml(RefreshControls);
                IsAdministrationXmlLoaded = true;
            }
        }

        void Application_DocumentOpen(Word.Document Doc)
        {
            this.RemoveOrphanedTaskPanes();
            this.AddAllTaskPanes();
        }

        private void RefreshControls()
        {
            ApplicationContext.Current.RefreshControlsFromConfiguration();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void taskPaneValue_VisibleChanged(object sender, System.EventArgs e)
        {
            
        }

        public Microsoft.Office.Tools.CustomTaskPane TaskPane
        {
            get
            {
                return Globals.ThisAddIn.CustomTaskPanes.Single(t => t.Window == Globals.ThisAddIn.Application.ActiveWindow);
            }
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
