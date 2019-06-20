using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools;
using Sobiens.Connectors.Windows.Controls;

namespace Sobiens.Connectors.OutlookConnector
{
    public class InspectorWrapper
    {
        private Outlook.Inspector inspector;
        private CustomTaskPane taskPane;

        public InspectorWrapper(Outlook.Inspector Inspector)
        {
            inspector = Inspector;
            ((Outlook.InspectorEvents_Event)inspector).Close +=
                new Outlook.InspectorEvents_CloseEventHandler(InspectorWrapper_Close);
            OutlookConnectorPane sharePointExplorerPane = new OutlookConnectorPane();
            sharePointExplorerPane.SetInspector(inspector);
            taskPane = Globals.ThisAddIn.CustomTaskPanes.Add(sharePointExplorerPane, "Sobiens SharePointOutlookConnector", inspector);
            //Globals.Ribbons. = new 
            taskPane.VisibleChanged += new EventHandler(TaskPane_VisibleChanged);
        }

        void TaskPane_VisibleChanged(object sender, EventArgs e)
        {
            Globals.Ribbons[inspector].ManageTaskPaneRibbonExt.ShowConnectorToggleButton.Checked = taskPane.Visible;
        }

        void InspectorWrapper_Close()
        {
            if (taskPane != null)
            {
                Globals.ThisAddIn.CustomTaskPanes.Remove(taskPane);
            }

            taskPane = null;
            Globals.ThisAddIn.InspectorWrappers.Remove(inspector);
            ((Outlook.InspectorEvents_Event)inspector).Close -=
                new Outlook.InspectorEvents_CloseEventHandler(InspectorWrapper_Close);
            inspector = null;
        }

        public CustomTaskPane CustomTaskPane
        {
            get
            {
                return taskPane;
            }
        }
    }
}
