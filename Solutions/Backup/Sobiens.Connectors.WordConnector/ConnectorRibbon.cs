using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Office = Microsoft.Office.Core;
using System.Windows;
using Sobiens.Connectors.WPF.Controls;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Microsoft.Office.Tools;
using Sobiens.Connectors.Entities.Settings;

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


namespace Sobiens.Connectors.WordConnector
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
            //StateManager.GetInstance().SaveState();
        }

        public void SaveToButton_OnAction(Office.IRibbonControl control)
        {
            UploadFilesForm uploadFilesForm = new UploadFilesForm();
            string activeFilePath = ApplicationContext.Current.EnsureSaved();
            if (string.IsNullOrEmpty(activeFilePath) == false)
            {
                if (activeFilePath.StartsWith("http", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    MessageBox.Show("You can only save local file to SharePoint");
                    return;
                }

                ApplicationContext.Current.CloseActiveDocument();
                IConnectorMainView connectorExplorer = uploadFilesForm as IConnectorMainView;
                uploadFilesForm.Initialize(new string[] { activeFilePath });
                uploadFilesForm.ShowDialog(null, "Send To Office Connector");
                //TODO: Open remotely
                //ApplicationContext.Current.OpenFile(activeFilePath);
            }
        }

        public void StartWorkflowButton_OnAction(Office.IRibbonControl control)
        {
            string activeFilePath = ApplicationContext.Current.GetActiveFilePath();
            if (string.IsNullOrEmpty(activeFilePath) == false)
            {
                if (activeFilePath.StartsWith("http", StringComparison.InvariantCultureIgnoreCase) == false)
                {
                    MessageBox.Show("You can not start workflow on local files");
                    return;
                }

                ApplicationContext.Current.CloseActiveDocument();
                ItemsManager.OpenWorkflowDialog(activeFilePath);
            }
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("Sobiens.Connectors.WordConnector.ConnectorRibbon.xml");
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
