using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities.Interfaces;
using System.Collections;
using Sobiens.Connectors.Entities;
using System.IO;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Studio.UI.Controls;
using Sobiens.Connectors.Entities.SharePoint;
using System.Data;
using Sobiens.Connectors.Entities.Data;
using System.Windows.Threading;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using Sobiens.Connectors.Entities.CRM;
using Sobiens.Connectors.Entities.Workflows;
using Sobiens.Connectors.Entities.SQLServer;

namespace Sobiens.Connectors.UI
{
    public class SPCamlStudioApplicationManager : ApplicationManager
    {
        //public AppConfiguration Configuration = new AppConfiguration();

        public SPCamlStudioApplicationManager(ISPCamlStudio spCamlStudio)
        {
            this.SPCamlStudio = spCamlStudio;
        }

        public override void SetStatusBar(string text, int percentage)
        {
            //this.ConnectorExplorer.StatusBar.SetStatusBar(text, percentage);
        }
        public override IQueryPanel AddNewQueryPanel(Folder folder, ISiteSetting siteSetting)
        {
            return this.SPCamlStudio.QueriesPanel.AddNewQueryPanel(folder, siteSetting);
        }

        public override ISiteSetting GetSiteSetting(Guid siteSettingID)
        {
            return Configuration.SiteSettings[siteSettingID];
        }

        public override void Initialize()
        {
            this.IsInitialized = true;
            ApplicationContext.Current.Configuration = ConfigurationManager.GetInstance().LoadConfiguration();
            if (this.SPCamlStudio != null)
            {
                this.SPCamlStudio.QueryDesignerToolbar.ValidateButtonEnabilities();
                this.SPCamlStudio.ServerObjectExplorer.Initialize();
                //            this.RefreshControlsFromConfiguration();
            }
        }

        public override void RefreshControlsFromConfiguration()
        {
            //this.SPCamlStudio.RefreshControls();
        }

        public override Folder GetFolder(ISiteSetting siteSetting, BasicFolderDefinition folderDefinition)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetFolder(siteSetting, folderDefinition);
        }

        public override Folder GetRootFolder(ISiteSetting siteSetting)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetRootFolder(siteSetting);
        }

        public override Folder GetParentFolder(ISiteSetting siteSetting, Folder folder)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetParentFolder(siteSetting, folder);
        }

        public override List<Folder> GetSubFolders(ISiteSetting siteSetting, Folder folder, int[] includedFolderTypes, string childFoldersCategoryName)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetFolders(siteSetting, folder, includedFolderTypes, childFoldersCategoryName);
        }

        public override List<IView> GetViews(ISiteSetting siteSetting, Folder folder)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetViews(siteSetting, folder);
        }



        public override FieldCollection GetFields(ISiteSetting siteSetting, Folder folder)
        {
            /*
            if(folder as CRMEntity != null)
            {
                return ((CRMEntity)folder).Fields;
            }
            */
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetFields(siteSetting, folder);
        }

        public override void CreateFields(ISiteSetting siteSetting, Folder folder, List<Field> fields)
        {
            if (folder as CRMEntity != null)
            {
            }
            else
            {
                IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
                serviceManager.CreateFields(siteSetting, folder, fields);
            }
        }

        public override IView GetView(ISiteSetting siteSetting, Folder folder)
        {
            return null;
        }

        public override List<IItem> GetAuditLogs(ISiteSetting siteSetting, string listName, string itemId)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetAuditLogs(siteSetting, listName, itemId);
        }

        public override List<IItem> GetListItems(ISiteSetting siteSetting, Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetListItems(siteSetting, folder, view, sortField, isAsc, currentPageIndex, currentListItemCollectionPositionNext, filters, isRecursive, out listItemCollectionPositionNext, out itemCount);
        }

        public override List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetListItems(siteSetting, orderBys, filters, viewFields, queryOptions, webUrl, listName, out listItemCollectionPositionNext, out itemCount);
        }

        public override void DeleteUniquePermissions(ISiteSetting siteSetting, Folder folder, bool applyToAllSubItems) {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            serviceManager.DeleteUniquePermissions(siteSetting, folder, applyToAllSubItems);
        }

        public override void Save()
        {
            this.SPCamlStudio.QueriesPanel.Save();
        }

        public override void Load()
        {
            this.SPCamlStudio.QueriesPanel.Load();
        }

        public override void ShowSyncDataWizard()
        {
            SyncDataWizardForm syncDataWizardForm = new SyncDataWizardForm();
            syncDataWizardForm.Initialize();
            if (syncDataWizardForm.ShowDialog(null, "Export Wizard",false, true) == true)
            {
            }
        }

        public override string GetActiveFilePath()
        {
            throw new NotImplementedException();
        }

        public override string EnsureSaved()
        {
            throw new NotImplementedException();
        }

        public override void BindFoldersToListViewControl(ISiteSetting siteSetting, Folder parentFolder, Folder folder, List<Folder> folders, object LibraryContentDataListView, object LibraryContentDataGridView)
        {
            throw new NotImplementedException();
        }

        public override void BindItemsToListViewControl(ISiteSetting siteSetting, Folder parentFolder, Folder folder, IView view, List<Folder> folders, List<IItem> items, object LibraryContentDataListView, object LibraryContentDataGridView)
        {
            throw new NotImplementedException();
        }

        public override void BindSearchResultsToListViewControl(ISiteSetting siteSetting, List<IItem> items, object LibraryContentDataGridView)
        {
            throw new NotImplementedException();
        }

        public override void DeleteListItem(IItem item)
        {
            throw new NotImplementedException();
        }

        public override bool CheckFileExistency(Folder folder, IItem item, string newFileName)
        {
            throw new NotImplementedException();
        }

        public override void CopyFile(Folder folder, IItem item, string newFileName)
        {
            throw new NotImplementedException();
        }

        public override bool CheckFolderExists(Folder folder, string newFolderName)
        {
            throw new NotImplementedException();
        }

        public override void GetApplicationDragDropInformation(IDataObject data, out string[] filenames, out MemoryStream[] filestreams)
        {
            throw new NotImplementedException();
        }

        public override List<ApplicationItemProperty> GetApplicationFields(string filePath)
        {
            throw new NotImplementedException();
        }

        public override void ActivateDocument(object document)
        {
            throw new NotImplementedException();
        }

        public override object OpenFile(ISiteSetting siteSetting, string filePath)
        {
            throw new NotImplementedException();
        }

        public override void OpenNewFile(string templatePath)
        {
            throw new NotImplementedException();
        }

        public override void CreateNewFile(string templatePath, string fileToBeSaved)
        {
            throw new NotImplementedException();
        }

        public override void CloseActiveDocument()
        {
            throw new NotImplementedException();
        }

        public override ApplicationTypes GetApplicationType()
        {
            return ApplicationTypes.Studio;
        }

        public override SC_MenuItems GetTaskMenuItems(ISiteSetting siteSetting, Task task)
        {
            throw new NotImplementedException();
        }

        public override SC_MenuItems GetItemMenuItems(ISiteSetting siteSetting, IItem item)
        {
            throw new NotImplementedException();
        }

        public override SC_MenuItems GetFolderMenuItems(ISiteSetting siteSetting, Folder folder)
        {
            throw new NotImplementedException();
        }

        public override SC_MenuItems GetItemVersionMenuItems(ISiteSetting siteSetting, ItemVersion itemVersion)
        {
            throw new NotImplementedException();
        }

        public override void DoMenuItemAction(ISiteSetting siteSetting, SC_MenuItemTypes menuItemType, object item, object[] args)
        {
            throw new NotImplementedException();
        }

        public override void CheckInFile(ISiteSetting siteSetting, IItem item, string comment, CheckinTypes checkinType)
        {
            throw new NotImplementedException();
        }

        public override void CheckOutFile(ISiteSetting siteSetting, IItem item)
        {
            throw new NotImplementedException();
        }

        public override void UndoCheckOutFile(ISiteSetting siteSetting, IItem item)
        {
            throw new NotImplementedException();
        }

        public override void AttachAsAHyperLink(ISiteSetting siteSetting, IItem item, object _inspector)
        {
            throw new NotImplementedException();
        }

        public override void AttachAsAnAttachment(ISiteSetting siteSetting, IItem item, object _inspector)
        {
            throw new NotImplementedException();
        }

        public override List<ContentType> GetContentTypes(ISiteSetting siteSetting, Folder folder)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            if(folder as SPWeb != null)
                return serviceManager.GetContentTypes(siteSetting, folder, true);
            else if (folder as SPList != null)
                return serviceManager.GetContentTypes(siteSetting, folder.GetListName());
            else
                throw new NotImplementedException();
        }

        public override List<Workflow> GetWorkflows(ISiteSetting siteSetting, Folder folder)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            //if (folder as SPWeb != null)
            //    return serviceManager.GetWorkflows(siteSetting, folder, true);
            //else
            if (folder as SPList != null)
                return serviceManager.GetWorkflows(siteSetting, folder.GetListName());
            else
                throw new NotImplementedException();
        }
        public override List<CompareObjectsResult> GetObjectDifferences(ISiteSetting sourceSiteSetting, Folder sourceFolder, ISiteSetting destinationSiteSetting, Folder destinationFolder, Action<int, string> reportProgressAction)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(sourceSiteSetting.SiteSettingType);
            return serviceManager.GetObjectDifferences(sourceSiteSetting, sourceFolder, destinationSiteSetting, destinationFolder, reportProgressAction);
        }

        public override void ApplyMissingCompareObjectsResult(CompareObjectsResult compareObjectsResult, ISiteSetting sourceSiteSetting, ISiteSetting destinationSiteSetting)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(sourceSiteSetting.SiteSettingType);
            serviceManager.ApplyMissingCompareObjectsResult(compareObjectsResult, sourceSiteSetting, destinationSiteSetting);
        }

        public override void ReportProgress(int? percentage, string message)
        {
            this.SPCamlStudio.ReportProgress(percentage, message);
        }
    }
}
