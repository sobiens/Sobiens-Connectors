#if General
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Documents;
using Google.GData.Client;
using System.IO;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Gmail;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Common.Interfaces;
using System.Data;

namespace Sobiens.Connectors.Services.Gmail
{
    /*
    public class GMailServiceManager : IServiceManager
    {
        private bool loggedIn = false;
        private DocumentsService service = null;

        private static GMailServiceManager _instance = null;
        public static GMailServiceManager GetInstance()
        {
            if (_instance == null)
                _instance = new GMailServiceManager();
            return _instance;
        }
        public Folder GetFoldersTreeFromURL(ISiteSetting siteSetting, string url)
        {
            return null;
        }

        public void OpenFile(ISiteSetting siteSetting, IItem item)
        {
        }

        public void DeleteFile(ISiteSetting siteSetting, IItem item)
        {
        }

        public bool UploadFile(ISiteSetting siteSetting, UploadItem uploadItem) { return true; }

        public FieldCollection GetFields(ISiteSetting siteSetting, Folder folder) { return null; }

        public List<ContentType> GetContentTypes(ISiteSetting siteSetting, Folder folder, bool includeReadOnly) { return null; }

        public ContentType GetContentType(ISiteSetting siteSetting, Folder folder, string contentTypeID, bool includeReadOnly) { return null; }


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

        public List<IItem> GetItems(ISiteSetting siteSetting, Folder parentFolder)
        {
            Login(siteSetting.Username, siteSetting.Password);
            List<IItem> items = new List<IItem>();

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
                    IItem item = new GItem(siteSetting.ID, entry.ResourceId, entry.Title.Text, entry.AlternateUri.ToString());
                    items.Add(item);
                }
            }
            return items;
        }
        public List<Folder> GetFolders(ISiteSetting siteSetting, Folder parentFolder)
        {
            Login(siteSetting.Username, siteSetting.Password);
            List<Folder> folders = new List<Folder>();
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
                    Folder folder = new GFolder(siteSetting.ID, entry.ResourceId, entry.Title.Text, entry.Id.AbsoluteUri);
                    folders.Add(folder);
                }
            }
            return folders;
        }

        public void UploadFile(ISiteSetting siteSetting, string filePath, string parentResourceId)
        {
            Login(siteSetting.Username, siteSetting.Password);
            FileInfo fileInfo = new FileInfo(filePath);
            string fileName = fileInfo.Name;
            string fileExtension = fileInfo.Extension.ToUpper().Substring(1);
            fileName = fileName + "." + fileExtension;
            String contentType = (String)DocumentsService.DocumentTypes[fileExtension];
            string feed = string.Format(DocumentsListQuery.foldersUriTemplate, parentResourceId);

            FileStream stream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var result = service.Insert(new Uri(feed), stream, contentType, fileName);
        }

        public void DeleteFile(ISiteSetting siteSetting, string fileResourceId)
        {
            Login(siteSetting.Username, siteSetting.Password);
            AtomEntry entry = service.Get(DocumentsListQuery.documentsBaseUri + "/" + fileResourceId);
            entry.Delete();
        }
        public bool CheckFileExistency(ISiteSetting siteSetting, string folderResourceId, string fileName)
        {
            Login(siteSetting.User, siteSetting.Password);
            AtomEntry entry = service.Get(DocumentsListQuery.documentsBaseUri + "/" + fileResourceId);
            if (entry == null)
                return false;
            return true;
        }
    }
*/
}
#endif
