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
            try
            {
                Folder selectedObject = ApplicationContext.Current.SPCamlStudio.ServerObjectExplorer.SelectedObject;
                ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(selectedObject.SiteSettingID);
                if (siteSetting.SiteSettingType == SiteSettingTypes.SQLServer && selectedObject as SQLDB == null)
                {
                    MessageBox.Show("You need to select a Database from server explorer");
                    return;
                }

                /*
                Entities.CRM.CRMEntity entity = new Entities.CRM.CRMEntity(siteSetting.ID, Guid.NewGuid().ToString(), "ectp_executivecorrespondance", "ectp_executivecorrespondance", "ectp_executivecorrespondance");
                List<IView> views = (new Services.CRM.CRMService()).GetEntityViews(siteSetting, entity);

                string[] divisionNames = new string[] { "Executive", "PC", "MOI", "Mitigation", "Transparency", "OC", "CA", "ISCP", "CE", "ASHR" };
                foreach (string divisionName in divisionNames)
                {
                    string executiveCorrespondenceAssignedViewName = divisionName + ": Executive correspondence - assigned";
                    string executiveCorrespondenceOverdueViewName = divisionName + ": Executive correspondence - overdue";
                    //string eventsAcceptedViewName = divisionName + ": Events accepted";
                    //string invitationsForInformationViewName = divisionName + ": Invitations for information";
                    //string missionReportsOutstandingViewName = divisionName + ": Mission reports outstanding";
                    //string pendingInvitationsToReviewViewName = divisionName + ": Pending invitations to review";

                    IView executiveCorrespondenceAssignedView = views.Where(t => t.Name.Equals(executiveCorrespondenceAssignedViewName, StringComparison.InvariantCultureIgnoreCase)).Single();
                    IView executiveCorrespondenceOverdueView = views.Where(t => t.Name.Equals(executiveCorrespondenceOverdueViewName, StringComparison.InvariantCultureIgnoreCase)).Single();
                    //IView eventsAcceptedView = views.Where(t => t.Name.Equals(eventsAcceptedViewName, StringComparison.InvariantCultureIgnoreCase)).Single();
                    //IView invitationsForInformationView = views.Where(t => t.Name.Equals(invitationsForInformationViewName, StringComparison.InvariantCultureIgnoreCase)).Single();
                    //IView missionReportsOutstandingView = views.Where(t => t.Name.Equals(missionReportsOutstandingViewName, StringComparison.InvariantCultureIgnoreCase)).Single();
                    //IView pendingInvitationsToReviewView = views.Where(t => t.Name.Equals(pendingInvitationsToReviewViewName, StringComparison.InvariantCultureIgnoreCase)).Single();

                    string formXml = "<form><tabs><tab showlabel=\"false\" verticallayout=\"true\" id=\"{" + (Guid.NewGuid().ToString()) + "}\" labelid=\"{" + (Guid.NewGuid().ToString()) + "}\"><labels><label description=\"Tab\" languagecode=\"1033\" /></labels><columns><column width=\"100%\"><sections><section showlabel=\"false\" showbar=\"false\" columns=\"1111\" id=\"{" + (Guid.NewGuid().ToString()) + "}\" labelid=\"{" + (Guid.NewGuid().ToString()) + "}\"><labels><label description=\"Section\" languagecode=\"1033\" /></labels><rows><row><cell colspan=\"4\" rowspan=\"12\" showlabel=\"false\" id=\"{" + (Guid.NewGuid().ToString()) + "}\" auto=\"false\" labelid=\"{" + (Guid.NewGuid().ToString()) + "}\"><labels><label description=\"ExecCorres\" languagecode=\"1033\" /></labels><control id=\"Component73cf31c\" uniqueid=\"{8667ecba-ce54-d295-7c75-70699b2c7376}\" classid=\"{E7A81278-8635-4d9e-8D4D-59480B391C5B}\" indicationOfSubgrid=\"true\"><parameters><TargetEntityType>ectp_executivecorrespondance</TargetEntityType><ChartGridMode>Grid</ChartGridMode><EnableQuickFind>true</EnableQuickFind><EnableViewPicker>true</EnableViewPicker><EnableJumpBar>true</EnableJumpBar><RecordsPerPage>8</RecordsPerPage>" +
                        "<ViewId>{" + executiveCorrespondenceAssignedView.UniqueIdentifier + "}</ViewId><IsUserView>false</IsUserView><ViewIds>{D50B77EA-568E-4FEF-8CDD-36BD64AE33B1},{683BBAE6-BD30-E711-80F7-005056970C85},{56EFDF10-CA30-E711-80F7-005056970C85}</ViewIds><AutoExpand>Fixed</AutoExpand><VisualizationId /><IsUserChart>false</IsUserChart><EnableChartPicker>false</EnableChartPicker><RelationshipName /></parameters></control></cell></row><row /><row /><row /><row /><row /><row /><row /><row /><row /><row /><row /><row><cell colspan=\"4\" rowspan=\"12\" showlabel=\"false\" id=\"{" + (Guid.NewGuid().ToString()) + "}\" auto=\"false\" labelid=\"{" + (Guid.NewGuid().ToString()) + "}\"><labels><label description=\"ExecCorresOverdue\" languagecode=\"1033\" /></labels><control id=\"Componentcd0a060\" uniqueid=\"{59e6cd2c-e8c9-e329-3e41-ac63a7d33d03}\" classid=\"{E7A81278-8635-4d9e-8D4D-59480B391C5B}\" indicationOfSubgrid=\"true\"><parameters><TargetEntityType>ectp_executivecorrespondance</TargetEntityType><ChartGridMode>Grid</ChartGridMode><EnableQuickFind>true</EnableQuickFind><EnableViewPicker>true</EnableViewPicker><EnableJumpBar>true</EnableJumpBar><RecordsPerPage>8</RecordsPerPage>" +
                        "<ViewId>{" + executiveCorrespondenceOverdueView.UniqueIdentifier + "}</ViewId><IsUserView>false</IsUserView><ViewIds>{D50B77EA-568E-4FEF-8CDD-36BD64AE33B1},{0B64C2A2-CA30-E711-80F7-005056970C85},{5D0E590F-CD30-E711-80F7-005056970C85}</ViewIds><AutoExpand>Fixed</AutoExpand><VisualizationId /><IsUserChart>false</IsUserChart><EnableChartPicker>false</EnableChartPicker><RelationshipName /></parameters></control></cell></row><row /><row /><row /><row /><row /><row /><row /><row /><row /><row /><row /></rows></section></sections></column></columns></tab></tabs><DisplayConditions FallbackForm=\"true\"><Role Id=\"{8BCB89D6-1C87-E611-80CB-00155D010242}\" /><Role Id=\"{02AB56B5-1C87-E611-80CB-00155D010242}\" /></DisplayConditions><controlDescriptions /></form>";
                    (new Services.CRM.CRMService()).CreateDashboard(siteSetting, "ECTP " + divisionName + " Dashboard for Mail Focal Points", formXml);
                }
                */

                CodeWizardForm codeWizardForm = new CodeWizardForm();
                codeWizardForm.Initialize(ApplicationContext.Current.SPCamlStudio.QueriesPanel.QueryPanels, selectedObject);
                if (codeWizardForm.ShowDialog(this.ParentWindow, "Code Wizard") == true)
                {
                }
            }
            catch(Exception ex)
            {
                int g = 3;
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
