using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Sobiens.Connectors.Common;

namespace Sobiens.Connectors.OutlookConnector
{
    public partial class ConnectorRibbonExt
    {
        private void ConnectorRibbonExt_Load(object sender, RibbonUIEventArgs e)
        {
            if (StateManager.GetInstance().ConnectorState.GetApplicationState(ApplicationContext.Current.GetApplicationType()).ConnectorOpen == true)
            {
                Globals.ThisAddIn.TaskPane.Visible = true;
                ShowConnectorToggleButton.Label = "Hide Connector";
            }
            else
            {
                Globals.ThisAddIn.TaskPane.Visible = false;
                ShowConnectorToggleButton.Label = "Show Connector";
            }
        }

        private void ShowConnectorToggleButton_Click(object sender, RibbonControlEventArgs e)
        {
            //Globals.ThisAddIn.TaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
            if (Globals.ThisAddIn.TaskPane.Visible == true)
            {
                Globals.ThisAddIn.TaskPane.Visible = false;
                ShowConnectorToggleButton.Label = "Show Connector";
            }
            else
            {
                Globals.ThisAddIn.TaskPane.Visible = true;
                ShowConnectorToggleButton.Label = "Hide Connector";
            }

            if (ApplicationContext.Current.IsInitialized == false)
            {
                ApplicationContext.Current.Initialize();
            }
            StateManager.GetInstance().ConnectorState.GetApplicationState(ApplicationContext.Current.GetApplicationType()).ConnectorOpen = Globals.ThisAddIn.TaskPane.Visible;
            StateManager.GetInstance().SaveState();
        }

        private void SaveToButton_Click(object sender, RibbonControlEventArgs e)
        {

        }
    }
}
