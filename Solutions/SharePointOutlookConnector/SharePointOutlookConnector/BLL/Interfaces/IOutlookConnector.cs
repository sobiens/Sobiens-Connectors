using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using System.Windows.Forms;
using EmailUploader.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces
{
    public interface IOutlookConnector
    {
        ISPCFolder GetRootFolder(EUSiteSetting siteSetting);
        List<ISPCFolder> GetSubFolders(ISPCFolder folder);
        List<ISPCView> GetViews(ISPCFolder folder);
        List<ISPCItem> GetListItems(ISPCFolder folder, ISPCView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, EUCamlFilters filters, out string listItemCollectionPositionNext, out int itemCount);
        void BindItemsToListViewControl(ISPCFolder folder, ISPCView view, List<ISPCItem> items, DataGridView LibraryContentDataGridView);
        void UploadFiles(ISPCFolder folder, List<EUEmailUploadFile> uploadFiles, List<EUField> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl);
        // JOEL JEFFERY 20110711
        event EventHandler UploadFailed;
        // JON SILVER JULY 2011
        event EventHandler UploadSucceeded; 
        void DeleteListItem(ISPCItem item);
        bool CheckFileExistency(ISPCFolder folder, ISPCItem item, string newFileName);
        void CopyFile(ISPCFolder folder, ISPCItem item, string newFileName);
        // JOEL JEFFERY 20110710
        bool CheckFolderExists(ISPCFolder folder, string newFolderName);
    }
}
