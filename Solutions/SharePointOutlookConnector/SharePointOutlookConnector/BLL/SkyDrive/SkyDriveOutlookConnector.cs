using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities.SkyDrive;
using System.Windows.Forms;
using EmailUploader.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.SkyDrive
{
    public class SkyDriveOutlookConnector : IOutlookConnector
    {
        public ISPCFolder GetRootFolder(EUSiteSetting siteSetting)
        {
            SDFolder rootFolder = new SDFolder(siteSetting, siteSetting.User, siteSetting.User);
            return rootFolder;
        }
        public List<ISPCFolder> GetSubFolders(ISPCFolder folder)
        {
            throw new Exception("Not implemented");
        }
        public List<ISPCView> GetViews(ISPCFolder folder)
        {
            throw new Exception("Not implemented");
        }

        public List<ISPCItem> GetListItems(ISPCFolder folder, ISPCView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, EUCamlFilters filters, out string listItemCollectionPositionNext, out int itemCount)
        {
            throw new Exception("Not implemented");
        }
        public void BindItemsToListViewControl(ISPCFolder folder, ISPCView view, List<ISPCItem> items, DataGridView LibraryContentDataGridView)
        {
            throw new Exception("Not implemented");
        }
        public void UploadFiles(ISPCFolder folder, List<EUEmailUploadFile> uploadFiles, List<EUField> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl)
        {
            throw new Exception("Not implemented");
        }
        public void DeleteListItem(ISPCItem item)
        {
            throw new Exception("Not implemented");
        }
        public bool CheckFileExistency(ISPCFolder folder, ISPCItem item, string newFileName)
        {
            throw new Exception("Not implemented");
        }
        public void CopyFile(ISPCFolder folder, ISPCItem item, string newFileName)
        {
            throw new Exception("Not implemented");
        }


        // JOEL JEFFERY 20110711
        /// <summary>
        /// Occurs when [upload failed].
        /// </summary>
        public event EventHandler UploadFailed;

        // JON SILVER JULY 2011
        /// <summary>
        /// Occurs when [upload succeeded].
        /// </summary>
        public event EventHandler UploadSucceeded;


        // JOEL JEFFERY 20110710
        /// <summary>
        /// Checks the folder exists.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="newFolderName">New name of the folder.</param>
        /// <returns></returns>
        public bool CheckFolderExists(ISPCFolder folder, string newFolderName)
        {
            throw new Exception("Not implemented");
        }
    }
}
