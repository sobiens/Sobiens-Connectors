using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Settings;
//using Microsoft.Windows.Controls;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface IOutlookConnector
    {
        Folder GetRootFolder();
        List<Folder> GetSubFolders(Folder folder);
        List<IView> GetViews(Folder folder);
        IView GetView(string webUrl, string listName, string viewName);

        //TODO:Implement following ones
        void BindFoldersToListViewControl(Folder parentFolder, Folder folder, List<Folder> folders, object LibraryContentDataGridView);
        void BindItemsToListViewControl(Folder parentFolder, Folder folder, IView view, List<Folder> folders, List<IItem> items, object LibraryContentDataGridView);
        void BindSearchResultsToListViewControl(List<IItem> items, object LibraryContentDataGridView);
        //        void UploadFiles(IFolder folder, List<EUEmailUploadFile> uploadFiles, List<EUField> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl);
        List<IItem> GetListItems(Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount);
        event EventHandler UploadFailed;
        event EventHandler UploadSucceeded;
        void DeleteListItem(IItem item);
        bool CheckFileExistency(Folder folder, IItem item, string newFileName);
        void CopyFile(Folder folder, IItem item, string newFileName);
        void MoveFile(IItem item,Folder folder, string newFileName);
        bool CheckFolderExists(Folder folder, string newFolderName);
    }
}
