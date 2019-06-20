using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Sobiens.Connectors.Common;

namespace Sobiens.Connectors.OutlookConnector
{
    public partial class ConnectorRibbon
    {
        private void ConnectorRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void ShowConnectorToggleButton_Click(object sender, RibbonControlEventArgs e)
        {
            //Globals.ThisAddIn.TaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
            Globals.ThisAddIn.TaskPane.Visible = !Globals.ThisAddIn.TaskPane.Visible;

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
