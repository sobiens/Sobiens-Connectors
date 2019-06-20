using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Office = Microsoft.Office.Core;
using System.Windows;
using Sobiens.Connectors.Common;
using Microsoft.Office.Tools;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new ConnectorRibbon();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace Sobiens.Connectors.ExcelConnector
{
    [ComVisible(true)]
    public class ConnectorRibbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public ConnectorRibbon()
        {
        }

        public void ShowConnectorButton_OnAction(Office.IRibbonControl control)
        {
            CustomTaskPane ctp = Globals.ThisAddIn.TaskPane;
            ctp.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
            ctp.Visible = !ctp.Visible;
            if (ctp.Visible)
            {
                if(control as Microsoft.Office.Core.CommandBarButtonClass !=null)
                    ((Microsoft.Office.Core.CommandBarButtonClass)control).State = Office.MsoButtonState.msoButtonMixed;
            }

            if (ApplicationContext.Current.IsInitialized == false)
            {
                ApplicationContext.Current.Initialize();
            }

            StateManager.GetInstance().ConnectorState.GetApplicationState(ApplicationContext.Current.GetApplicationType()).ConnectorOpen = ctp.Visible;
            StateManager.GetInstance().SaveState();
        }

        public void SaveToButton_OnAction(Office.IRibbonControl control)
        {
            MessageBox.Show("saved.");
        }


        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("Sobiens.Connectors.ExcelConnector.ConnectorRibbon.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, select the Ribbon XML item in Solution Explorer and then press F1

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
