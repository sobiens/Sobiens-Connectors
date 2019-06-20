using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using EmailUploader.BLL.Entities;
using System.IO;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.Forms;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public class BackgroundThreadManager
    {
        // JOEL JEFFERY 20110711
        public event EventHandler UploadFailed;
        // JON SILVER JULY 2011
        public event EventHandler UploadSucceeded;


        private static BackgroundThreadManager instance = null;
        public static BackgroundThreadManager GetInstance()
        {
            if (instance == null)
                instance = new BackgroundThreadManager();
            return instance;
        }
        private BackgroundThreadManager() { }
        public void QueueUploadItems(EUFolder folder, string destinationFolderUrl, List<EUEmailUploadFile> uploadFiles, List<EUField> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl)
        {
            List<EUUploadItem> uploadItems = new List<EUUploadItem>();
            foreach (EUEmailUploadFile emailUploadFile in uploadFiles)
            {
                if (emailUploadFile.SaveFormatOverride == SaveFormatOverride.Word)
                    destinationFolderUrl = ensureFolderExists(folder, destinationFolderUrl, emailUploadFile, fields, fieldInformations, sharePointListViewControl);
                EUUploadItem uploadItem = new EUUploadItem(folder, destinationFolderUrl, emailUploadFile, fields, fieldInformations, sharePointListViewControl);
                uploadItems.Add(uploadItem);
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(UploadItems), uploadItems);
            bool test;
        }

        /// <summary>
        /// Ensures the folder exists.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="destinationFolderUrl">The destination folder URL.</param>
        /// <param name="emailUploadFile">The email upload file.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="fieldInformations">The field informations.</param>
        /// <param name="sharePointListViewControl">The share point list view control.</param>
        /// <returns>The url of the new folder.</returns>
        private string ensureFolderExists(EUFolder folder, string destinationFolderUrl, EUEmailUploadFile emailUploadFile, List<EUField> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl)
        {
            string newFolderName = SharePointManager.MakeFileNameSafe(emailUploadFile.Subject + (emailUploadFile.SentOn.HasValue ? emailUploadFile.SentOn.Value.ToString(" yyyyddMM") : ""));
            string newDestinationUrl = destinationFolderUrl + "/" + newFolderName;
            if (!SharePointManager.CheckFolderExists(folder.SiteSetting, newDestinationUrl, folder.ListName, folder.FolderPath, newFolderName))
            {
                SharePointManager.CreateListFolder(folder.SiteSetting, folder.FolderPath, folder.WebUrl, folder.ListName, newFolderName);
            }
            // JOEL JEFFERY 20110712 - what the hell was I thinking?
            //folder.FolderPath = folder.FolderPath += "/" + newFolderName;
            return newDestinationUrl;
        }

        public void UploadItems(object _uploadItems)
        {
            bool uploadSucceeded = false;
            List<EUUploadItem> uploadItems = (List<EUUploadItem>)_uploadItems;
            FileExistDialogResults lastFileExistDialogResults = FileExistDialogResults.NotSelected;
            bool doThisForNextConflicts = false;
            foreach (EUUploadItem uploadItem in uploadItems)
            {
                string newDestinationUrl = uploadItem.DestinationFolderUrl + "/"; 
                string copySource = new FileInfo(uploadItem.EmailUploadFile.FilePath).Name;
                string[] copyDest = new string[1] { uploadItem.DestinationFolderUrl + "/" + copySource };
                byte[] itemByteArray = SharePointManager.ReadByteArrayFromFile(uploadItem.EmailUploadFile.FilePath);
                
                EUListItem listItem;

                string newFileName = copySource;
                IOutlookConnector connector = OutlookConnector.GetConnector(uploadItem.Folder.SiteSetting);

                if (
                    (doThisForNextConflicts == true && lastFileExistDialogResults == FileExistDialogResults.Skip)
                    ||
                    lastFileExistDialogResults == FileExistDialogResults.Cancel)
                {
                    uploadItem.SharePointListViewControl.DeleteUploadItemInvoke(uploadItem.EmailUploadFile.UniqueID);
                    continue;
                }

                bool isCurrentFileUploadCanceled = false;
                if (
                    (doThisForNextConflicts == false)
                    ||
                    (doThisForNextConflicts == true && lastFileExistDialogResults == FileExistDialogResults.Copy)
                    )
                {
                    while (connector.CheckFileExistency(uploadItem.Folder, null, newFileName) == true)
                    {
                        FileExistConfirmationForm fileExistConfirmationForm = new FileExistConfirmationForm(copyDest[0]);
                        fileExistConfirmationForm.ShowDialog();
                        lastFileExistDialogResults = fileExistConfirmationForm.FileExistDialogResult;
                        doThisForNextConflicts = fileExistConfirmationForm.DoThisForNextConflicts;
                        
                        newFileName = fileExistConfirmationForm.NewFileName;
                        if (lastFileExistDialogResults == FileExistDialogResults.Skip || lastFileExistDialogResults == FileExistDialogResults.Cancel)
                        {
                            uploadItem.SharePointListViewControl.DeleteUploadItemInvoke(uploadItem.EmailUploadFile.UniqueID);
                            isCurrentFileUploadCanceled = true;
                            break;
                        }
                        if (lastFileExistDialogResults == FileExistDialogResults.CopyAndReplace)
                        {
                            break;
                        }
                        string newCopyDest = copyDest[0].Substring(0, copyDest[0].LastIndexOf("/")) + "/" + newFileName;
                        copyDest = new string[] { newCopyDest };
                    }
                }
                if (isCurrentFileUploadCanceled == true)
                    continue;

                if (uploadItem.EmailUploadFile.IsListItemAndAttachment == false)
                {
                    uint? result = SharePointManager.UploadFile(uploadItem.Folder.SiteSetting, uploadItem.Folder.ListName, uploadItem.Folder.RootFolderPath, uploadItem.Folder.SiteUrl, uploadItem.Folder.WebUrl, copySource, copyDest, itemByteArray, uploadItem.Fields, uploadItem.EmailUploadFile.MetaData, uploadItem.EmailUploadFile.FieldInformations, out listItem);
                    if (uploadItem.SharePointListViewControl != null && listItem != null)   // JON SILVER JULY 2011 - Is this success???
                    {
                        uploadItem.SharePointListViewControl.NotifyUploadItemInvoke(uploadItem.EmailUploadFile.UniqueID, listItem);
                    }
                    // JON SILVER JULY 2011 
                    if (result.HasValue && listItem != null)
                        uploadSucceeded = true;
                }
                else
                {
                    int? result = SharePointManager.UploadListItemWithAttachment(uploadItem);
                    uploadSucceeded = result.HasValue;
                }
            }

            // JON SILVER JULY 2011 - RAISE UPLOAD SUCCESS SO WE CAN DELETE IF WE ARE MOVING
            if (!uploadSucceeded && UploadFailed != null) //why is UploadFailed null sometimes? JJ
                UploadFailed(this, new EventArgs());
            else if (UploadSucceeded != null)
                UploadSucceeded(this, new EventArgs());
        }
    }
}
