using Sobiens.Connectors.Common.EDMX;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for QueryDesignerToolbar.xaml
    /// </summary>
    public partial class QueryDesignerToolbar : HostControl, IQueryDesignerToolbar
    {
        public QueryDesignerToolbar()
        {
            InitializeComponent();
        }

        public void ValidateButtonEnabilities()
        {
            IQueryPanel activeQueryPanel = ApplicationContext.Current.SPCamlStudio.QueriesPanel.ActiveQueryPanel;
            if (activeQueryPanel == null)
            {
                ShowCriteriaPaneButton.IsEnabled = false;
                ShowCamlTextPaneButton.IsEnabled = false;
                ShowResultsPaneButton.IsEnabled = false;
                ExecuteQueryButton.IsEnabled = false;
                ValidateCamlButton.IsEnabled = false;
            }
            else
            {
                ShowCriteriaPaneButton.IsEnabled = true;
                ShowCamlTextPaneButton.IsEnabled = true;
                ShowResultsPaneButton.IsEnabled = true;
                ExecuteQueryButton.IsEnabled = true;
                ValidateCamlButton.IsEnabled = true;
            }

            Folder selectedObject = ApplicationContext.Current.SPCamlStudio.ServerObjectExplorer.SelectedObject;
            if (selectedObject != null && 
                    (
                        (selectedObject as SPFolder) != null
                        ||
                        (selectedObject as Entities.SQLServer.SQLTable) != null
                        ||
                        (selectedObject as Entities.CRM.CRMEntity) != null
                    )
                )
                NewQueryButton.IsEnabled = true;
            else
                NewQueryButton.IsEnabled = false;
        }
        private void NewQueryButton_Click(object sender, RoutedEventArgs e)
        {
            Folder selectedFolder = ApplicationContext.Current.SPCamlStudio.ServerObjectExplorer.SelectedObject;
            ApplicationContext.Current.AddNewQueryPanel(selectedFolder, null);
            ValidateButtonEnabilities();
        }

        private void ShowCriteriaPaneButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Current.SPCamlStudio.QueriesPanel.ActiveQueryPanel.ChangeCriteriaPaneVisibility();
        }

        private void ShowCamlTextPaneButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Current.SPCamlStudio.QueriesPanel.ActiveQueryPanel.ChangeCamlTextPaneVisibility();
        }

        private void ShowResultsPaneButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Current.SPCamlStudio.QueriesPanel.ActiveQueryPanel.ChangeResultsPaneVisibility();
        }

        private void ExecuteQueryButton_Click(object sender, RoutedEventArgs e)
        {
            Folder selectedFolder = ApplicationContext.Current.SPCamlStudio.QueriesPanel.ActiveQueryPanel.AttachedObject;
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(selectedFolder.SiteSettingID);
            List<CamlFieldRef> viewFields = ApplicationContext.Current.SPCamlStudio.QueriesPanel.ActiveQueryPanel.GetViewFields();
            CamlQueryOptions queryOptions = ApplicationContext.Current.SPCamlStudio.QueriesPanel.ActiveQueryPanel.GetQueryOptions();
            CamlFilters filters = ApplicationContext.Current.SPCamlStudio.QueriesPanel.ActiveQueryPanel.GetFilters();
            List<CamlOrderBy> orderBys = ApplicationContext.Current.SPCamlStudio.QueriesPanel.ActiveQueryPanel.GetOrderBys();
            string folderServerRelativePath = string.Empty;
            if(selectedFolder as SPFolder != null)
            {
                folderServerRelativePath = ((SPFolder)selectedFolder).ServerRelativePath;
            }
            ApplicationContext.Current.SPCamlStudio.QueriesPanel.ActiveQueryPanel.PopulateResults(siteSetting, selectedFolder.GetWebUrl(), selectedFolder.GetListName(), filters, viewFields, orderBys, queryOptions, folderServerRelativePath);
        }

        private void ValidateCamlButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CodeWizardButton_Click(object sender, RoutedEventArgs e)
        {
            Folder selectedObject = ApplicationContext.Current.SPCamlStudio.ServerObjectExplorer.SelectedObject;
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(selectedObject.SiteSettingID);
            if (siteSetting.SiteSettingType == SiteSettingTypes.SQLServer && selectedObject as SQLDB == null)
            {
                MessageBox.Show("You need to select a Database from server explorer");
                return;
            }

            //EDMXManager.Save("c:\\temp\\newedmx.xml", selectedObject as SQLDB);

            CodeWizardForm codeWizardForm = new CodeWizardForm();
            codeWizardForm.Initialize(ApplicationContext.Current.SPCamlStudio.QueriesPanel.QueryPanels, selectedObject);
            if (codeWizardForm.ShowDialog(this.ParentWindow, "Code Wizard") == true)
            {
            }
        }

        private void DataImportWizardButton_Click(object sender, RoutedEventArgs e)
        {
            Folder selectedObject = ApplicationContext.Current.SPCamlStudio.ServerObjectExplorer.SelectedObject;
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(selectedObject.SiteSettingID);
            if (siteSetting.SiteSettingType == SiteSettingTypes.SQLServer && selectedObject as SQLDB == null)
            {
                MessageBox.Show("You need to select a Database from server explorer");
                return;
            }

            //EDMXManager.Save("c:\\temp\\newedmx.xml", selectedObject as SQLDB);

            ImportWizardForm importWizardForm = new ImportWizardForm();
            importWizardForm.Initialize(ApplicationContext.Current.SPCamlStudio.QueriesPanel.QueryPanels, selectedObject);
            if (importWizardForm.ShowDialog(this.ParentWindow, "Data Import Wizard") == true)
            {
            }
        }

        private void CopyListWizardButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            Folder selectedObject = ApplicationContext.Current.SPCamlStudio.ServerObjectExplorer.SelectedObject;
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(selectedObject.SiteSettingID);
            if (siteSetting.SiteSettingType == SiteSettingTypes.SQLServer && selectedObject as SQLDB == null)
            {
                MessageBox.Show("You need to select a Database from server explorer");
                return;
            }
            */

            SyncCopyListWizardForm syncCopyListWizardForm = new SyncCopyListWizardForm();
            //syncCopyListWizardForm.Initialize();
            if (syncCopyListWizardForm.ShowDialog(this.ParentWindow, "Data Import Wizard", false, true) == true)
            {
            }
        }

        private void CopyMMSWizardButton_Click(object sender, RoutedEventArgs e)
        {
            MMSCopyWizardForm mmsCopyWizardForm = new MMSCopyWizardForm();
            //syncCopyListWizardForm.Initialize();
            if (mmsCopyWizardForm.ShowDialog(this.ParentWindow, "MMS Copy Wizard", false, true) == true)
            {
            }
        }

        private void CopySchemaWizardButton_Click(object sender, RoutedEventArgs e)
        {
            SyncSchemaWizardForm syncSchemaWizardForm = new SyncSchemaWizardForm();
            //syncCopyListWizardForm.Initialize();
            if (syncSchemaWizardForm.ShowDialog(this.ParentWindow, "Copy Schema Wizard", false, true) == true)
            {
            }
        }
    }
}
