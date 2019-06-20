using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Documents;
using Google.GData.Client;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities.FileSystem;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities.Gmail;
using System.IO;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public class GMailManager
    {
        private bool loggedIn = false;
        private DocumentsService service = null;

        private static GMailManager _instance = null;
        public static GMailManager GetInstance()
        {
            if (_instance == null)
                _instance = new GMailManager();
            return _instance;
        }

        /// <summary>
        /// Authenticates to Google servers
        /// </summary>
        /// <param name="username">The user's username (e-mail)</param>
        /// <param name="password">The user's password</param>
        /// <exception cref="AuthenticationException">Thrown on invalid credentials.</exception>
        public void Login(string username, string password)
        {
            try
            {
                if (service == null)
                {
                    service = new DocumentsService("DocListUploader");
                    ((GDataRequestFactory)service.RequestFactory).KeepAlive = false;
                    service.setUserCredentials(username, password);
                    //force the service to authenticate
                    DocumentsListQuery query = new DocumentsListQuery();
                    query.NumberToRetrieve = 1;
                    service.Query(query);
                    loggedIn = true;
                }
            }
            catch (AuthenticationException e)
            {
                loggedIn = false;
                service = null;
                throw e;
            }
        }

        public List<ISPCItem> GetItems(EUSiteSetting siteSetting, ISPCFolder parentFolder)
        {
            Login(siteSetting.User, siteSetting.Password);
            List<ISPCItem> items = new List<ISPCItem>();

            DocumentsListQuery query = new DocumentsListQuery();
            if (parentFolder.UniqueIdentifier != String.Empty)
                query = new FolderQuery(parentFolder.UniqueIdentifier);
            DocumentsFeed feed = service.Query(query);
            foreach (DocumentEntry entry in feed.Entries)
            {
                if (entry.IsFolder == false)
                {
                    if (parentFolder == null || parentFolder.UniqueIdentifier == String.Empty)
                    {
                        if (entry.ParentFolders.Count > 0)
                            continue;
                    }
                    ISPCItem item = new GItem(siteSetting, entry.ResourceId, entry.Title.Text, entry.AlternateUri.ToString());
                    items.Add(item);
                }
            }
            return items;
        }
        public List<ISPCFolder> GetFolders(EUSiteSetting siteSetting, ISPCFolder parentFolder)
        {
            Login(siteSetting.User, siteSetting.Password);
            List<ISPCFolder> folders = new List<ISPCFolder>();
            DocumentsListQuery query = new DocumentsListQuery();
            if(parentFolder.UniqueIdentifier != String.Empty)
                query = new FolderQuery(parentFolder.UniqueIdentifier);
            query.ShowFolders = true;
            DocumentsFeed feed = service.Query(query);
            foreach (DocumentEntry entry in feed.Entries)
            {
                if (entry.IsFolder)
                {
                    if (parentFolder == null || parentFolder.UniqueIdentifier == String.Empty)
                    {
                        if (entry.ParentFolders.Count > 0)
                            continue;
                    }
                    ISPCFolder folder = new GFolder(siteSetting, entry.ResourceId, entry.Title.Text, entry.Id.AbsoluteUri);
                    folders.Add(folder);
                }
            }
            return folders;
        }

        public void UploadFile(EUSiteSetting siteSetting, string filePath, string parentResourceId)
        {
            Login(siteSetting.User, siteSetting.Password);
            FileInfo fileInfo = new FileInfo(filePath);
            string fileName = fileInfo.Name;
            string fileExtension = fileInfo.Extension.ToUpper().Substring(1);
            fileName = fileName + "." + fileExtension;
            String contentType = (String)DocumentsService.DocumentTypes[fileExtension];
            string feed = string.Format(DocumentsListQuery.foldersUriTemplate, parentResourceId);

            FileStream stream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var result = service.Insert(new Uri(feed), stream, contentType, fileName);
        }

        public void DeleteFile(EUSiteSetting siteSetting, string fileResourceId)
        {
            Login(siteSetting.User, siteSetting.Password);
            AtomEntry entry = service.Get(DocumentsListQuery.documentsBaseUri + "/" + fileResourceId);
            entry.Delete();
        }
        public bool CheckFileExistency(EUSiteSetting siteSetting, string folderResourceId, string fileName)
        {
            return false;
            /*
            Login(siteSetting.User, siteSetting.Password);
            AtomEntry entry = service.Get(DocumentsListQuery.documentsBaseUri + "/" + fileResourceId);
            if (entry == null)
                return false;
            return true;
             */ 
        }
    }
}
