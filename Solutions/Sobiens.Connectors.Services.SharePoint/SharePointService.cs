using System;
using System.Net;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using Sobiens.Connectors.Entities;
using System.Xml;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.SharePoint;
using System.Collections;
using Sobiens.Connectors.Services.SharePoint.SharePointCopyWS;
using System.Web;
using Microsoft.SharePoint.Client;
using Sobiens.Connectors.Entities.Workflows;
using System.Globalization;
using Microsoft.SharePoint.Client.Taxonomy;
using Microsoft.SharePoint.Client.WorkflowServices;
//using Microsoft.SharePoint.Client.Workflow;

namespace Sobiens.Connectors.Services.SharePoint
{
    public class SharePointService : ISharePointService
    {
        private static readonly object UpdateListItemLocker = new object();

        private static Dictionary<string, CookieContainer> CookieContainers = new Dictionary<string, CookieContainer>();

        public static CookieContainer GetCookieContainer(string url, string userName, string password)
        {
            string key = string.Format("{0}_{1}_{2}", url, userName, password);
            if (CookieContainers.ContainsKey(key) == false)
            {
                MsOnlineClaimsHelper claimsHelper = new MsOnlineClaimsHelper(new Uri(url), userName, password, true);
                using (ClientContext context = new ClientContext(url))
                {
                    context.ExecutingWebRequest += claimsHelper.clientContext_ExecutingWebRequest;

                    context.Load(context.Web);

                    context.ExecuteQuery();

                    CookieContainers.Add(key, claimsHelper.CookieContainer);
                }
            }

            return CookieContainers[key];
        }

        public static ClientContext GetClientContext(ISiteSetting siteSetting)
        {
            return GetClientContext(siteSetting, siteSetting.Url);
        }

        public static ClientContext GetClientContext(ISiteSetting siteSetting, string webUrl)
        {
            ClientContext context = new ClientContext(webUrl);
            if (siteSetting.UseDefaultCredential == true)
            {
                context.Credentials = System.Net.CredentialCache.DefaultCredentials; ;
            }
            else
            {
                string userName = siteSetting.Username;
                string[] userNameStringArray = userName.Split(new char[] { '\\' });
                if (userNameStringArray.Length > 1)//there is domain
                {
                    context.Credentials = new NetworkCredential(userNameStringArray[1], siteSetting.Password, userNameStringArray[0]);
                }
                else
                {
                    System.Security.SecureString passWord = new System.Security.SecureString();
                    foreach (char c in siteSetting.Password.ToCharArray())
                        passWord.AppendChar(c);
                    context.Credentials = new SharePointOnlineCredentials(siteSetting.Username, passWord);
                }
            }

            return context;
        }

        public static void CopyWorkflow(ISiteSetting sourceSiteSetting, Guid sourceListId, Guid sourceWorkflowDefinitionId, ISiteSetting targetSiteSetting, Guid targetListId, Guid taskListId, Guid historyListId) {
            ClientContext sourceContext = GetClientContext(sourceSiteSetting);
            var sourceWorkflowServicesManager = new WorkflowServicesManager(sourceContext, sourceContext.Web);
            var sourceWorkflowDeploymentService = sourceWorkflowServicesManager.GetWorkflowDeploymentService();

            ClientContext targetContext = GetClientContext(targetSiteSetting);
            var targetWorkflowServicesManager = new WorkflowServicesManager(targetContext, targetContext.Web);
            var targetWorkflowDeploymentService = targetWorkflowServicesManager.GetWorkflowDeploymentService();
            WorkflowSubscriptionService targetWFSubscriptionService = targetWorkflowServicesManager.GetWorkflowSubscriptionService();

            // get all installed workflows
            var publishedWorkflowDefinition = sourceWorkflowDeploymentService.GetDefinition(sourceWorkflowDefinitionId);
            sourceContext.Load(publishedWorkflowDefinition);
            sourceContext.ExecuteQuery();

            /*
            List workflowHistoryList = targetContext.Web.Lists.GetByTitle("Workflow Tasks");
            List taskList = targetContext.Web.Lists.GetByTitle("Workflow History");
            List associatedList = targetContext.Web.Lists.GetByTitle(targetListName);
            targetContext.Load(workflowHistoryList);
            targetContext.Load(taskList);
            targetContext.Load(associatedList);
            targetContext.ExecuteQuery();
            */

            WorkflowDefinition wf = CloneWorkflowDefinition(publishedWorkflowDefinition, targetContext, targetListId);
            ClientResult<Guid> result = targetWorkflowDeploymentService.SaveDefinition(wf);
            targetContext.ExecuteQuery();

            targetWorkflowDeploymentService.PublishDefinition(result.Value);
            targetContext.ExecuteQuery();

            WorkflowSubscription wfSubscription = new WorkflowSubscription(targetContext);
            wfSubscription.DefinitionId = result.Value;
            wfSubscription.Enabled = true;
            wfSubscription.Name = wf.DisplayName;
            wfSubscription.EventSourceId = targetListId;

            var startupOptions = new List<string>();
            // automatic start
            //startupOptions.Add("ItemAdded");
            //startupOptions.Add("ItemUpdated");
            // manual start
            startupOptions.Add("WorkflowStart");

            // set the workflow start settings
            wfSubscription.EventTypes = startupOptions;

            // set the associated task and history lists
            wfSubscription.SetProperty("HistoryListId", historyListId.ToString());

            wfSubscription.SetProperty("TaskListId", taskListId.ToString());

            //Create the Association
            ClientResult<Guid> result1 = targetWFSubscriptionService.PublishSubscriptionForList(wfSubscription, targetListId);

            targetContext.ExecuteQuery();
            /*
            // find the first workflow definition
            var firstWorkflowDefinition = publishedWorkflowDefinitions.First();

            // connect to the subscription service
            var workflowSubscriptionService = workflowServicesManager.GetWorkflowSubscriptionService();

            // get all workflow associations
            var workflowAssociations = workflowSubscriptionService.EnumerateSubscriptionsByDefinition(firstWorkflowDefinition.Id);
            sourceContext.Load(workflowAssociations);
            sourceContext.ExecuteQuery();

            foreach (var association in workflowAssociations)
            {
                Console.WriteLine("{0} - {1}", association.Id, association.Name);
            }
            */
            //workflowServicesManager.
            /*
            Web web = sourceContext.Web;
            sourceContext.Load(web);
            List sourceList = web.Lists.GetByTitle(sourceListName);
            //Microsoft.SharePoint.Client.Workflow.WorkflowAssociationCollection workflowAssociations = sourceList.WorkflowAssociations;
            sourceContext.Load(sourceList);
            sourceContext.Load(sourceList.WorkflowAssociations);
            sourceContext.Load(web.WorkflowAssociations);

            sourceContext.ExecuteQuery();

            Microsoft.SharePoint.Client.Workflow.WorkflowAssociation workflowAssociation = web.WorkflowAssociations[0];
            string data = workflowAssociation.AssociationData;
            int x = 3;
            */
            //sourceContext.Load($web.Webs);
        }

        private static WorkflowDefinition CloneWorkflowDefinition(WorkflowDefinition wf, ClientContext context, Guid listId)
        {
            WorkflowDefinition newWFDefinition = new WorkflowDefinition(context);
            newWFDefinition.AssociationUrl = wf.AssociationUrl;
            newWFDefinition.Description = wf.Description;
            newWFDefinition.DisplayName = wf.DisplayName;
            newWFDefinition.DraftVersion = wf.DraftVersion;
            newWFDefinition.FormField = wf.FormField;
            newWFDefinition.Id = wf.Id;
            newWFDefinition.InitiationUrl = wf.InitiationUrl;
            foreach (string key in wf.Properties.Keys)
            {
                newWFDefinition.SetProperty(key, wf.Properties[key]);
            }
            //newWFDefinition.Published = wf.Published;
            newWFDefinition.RequiresAssociationForm = wf.RequiresAssociationForm;
            newWFDefinition.RequiresInitiationForm = wf.RequiresInitiationForm;
            newWFDefinition.RestrictToScope = listId.ToString();
            newWFDefinition.RestrictToType = wf.RestrictToType;
            newWFDefinition.Tag = wf.Tag;
            newWFDefinition.Xaml = wf.Xaml;

            return newWFDefinition;
        }

        public static void DeleteListItem(ISiteSetting siteSetting, string webUrl, string listName, string fileRef, int listItemID)
        {
            try
            {
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);

                /*Create an XmlDocument object and construct a Batch element and its
                attributes. Note that an empty ViewName parameter causes the method to use the default view. */
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                System.Xml.XmlElement batchElement = doc.CreateElement("Batch");
                batchElement.SetAttribute("OnError", "Continue");
                batchElement.SetAttribute("ListVersion", "1");

                fileRef = HttpUtility.UrlDecode(fileRef);
                /*Specify methods for the batch post using CAML. To update or delete, 
                specify the ID of the item, and to update or add, specify 
                the value to place in the specified column.*/
                string xml = "<Method ID='1' Cmd='Delete'>" +
                   "<Field Name='ID'>" + listItemID + "</Field>" +
                   (fileRef != String.Empty ? "<Field Name=\"FileRef\">" + fileRef + "</Field>" : "") +
                   "</Method>";
                batchElement.InnerXml = xml;
                /*Update list items. This example uses the list GUID, which is recommended, 
                but the list display name will also work.*/
                XmlNode resultNode = ws.UpdateListItems(listName, batchElement);
                string message = string.Format("SharePointService DeleteListItem method returned xml:{0}", resultNode.OuterXml);
                Logger.Info(message, "Service");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                throw ex;
            }
        }

        public List<Sobiens.Connectors.Entities.Folder> GetFolders(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder parentFolder)
        {
            return GetFolders(siteSetting, parentFolder, null);
        }

        public List<Sobiens.Connectors.Entities.Folder> GetFolders(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder parentFolder, int[] includedFolderTypes)
        {
            List<Sobiens.Connectors.Entities.Folder> subFolders = new List<Sobiens.Connectors.Entities.Folder>();
            if (parentFolder as SPWeb != null)
            {
                SPWeb web = (SPWeb)parentFolder;
                try
                {
                    List<SPWeb> webs = this.GetWebs(siteSetting, web.Url);
                    foreach (SPWeb _web in webs)
                    {
                        subFolders.Add(_web);
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format("SharePointService GetWebs method returned error:{0}", ex.Message);
                    Logger.Error(message, "Service");
                }

                try
                {
                    List<SPList> lists = this.GetLists(siteSetting, web.Url);
                    foreach (SPList list in lists)
                    {
                        if (list.Hidden == true)
                            continue;
                        if (includedFolderTypes != null && includedFolderTypes.Contains(list.ServerTemplate) == false)
                            continue;
                        /*
                        if (includedFolderTypes == null && list.ServerTemplate != 101 && list.ServerTemplate != 100 && list.BaseType != 1)
                            continue;
                            */
                        subFolders.Add(list);
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format("SharePointService GetLists method returned error:{0}", ex.Message);
                    Logger.Error(message, "Service");
                }
            }
            else if (parentFolder as SPFolder != null)
            {
                SPFolder _folder = (SPFolder)parentFolder;
                List<Sobiens.Connectors.Entities.Folder> folders = this.GetSubFolders(siteSetting, _folder);
                foreach (SPFolder __folder in folders)
                {
                    //__folder.ParentFolder = _folder;
                    subFolders.Add(__folder);
                }
            }
            return subFolders;
        }

        /// <summary>
        /// Returns webparts of the page
        /// </summary>
        /// <param name="siteSetting"></param>
        /// <param name="pageServerRelativeUrl"></param>
        /// <returns></returns>
        public List<SPWebpart> GetWebparts(ISiteSetting siteSetting, string pageServerRelativeUrl)
        {
            try
            {
                List<SPWebpart> webparts = new List<SPWebpart>();
                ClientContext context = GetClientContext(siteSetting);
                var oFile = context.Web.GetFileByServerRelativeUrl(pageServerRelativeUrl);

                Microsoft.SharePoint.Client.WebParts.LimitedWebPartManager wpm = oFile.GetLimitedWebPartManager(Microsoft.SharePoint.Client.WebParts.PersonalizationScope.Shared);
                //Microsoft.SharePoint.Client.WebParts.WebPartDefinitionCollection webParts = limitedWebPartManager.WebParts;

                var query = wpm.WebParts.Include(wp => wp.Id, wp => wp.WebPart, wp => wp.WebPart.Properties);
                var webPartDefenitions = context.LoadQuery(query);

                context.ExecuteQuery();

                foreach(Microsoft.SharePoint.Client.WebParts.WebPartDefinition webPart in webPartDefenitions)
                {
                    SPWebpart spWebpart = new SPWebpart();
                    spWebpart.Properties = new Dictionary<string, object>();
                    spWebpart.ID = webPart.Id.ToString();
                    spWebpart.Title = webPart.WebPart.Title;
                    foreach (string key in webPart.WebPart.Properties.FieldValues.Keys)
                    {
                        spWebpart.Properties.Add(key, webPart.WebPart.Properties.FieldValues[key]);
                    }
                    webparts.Add(spWebpart);
                    /*
                    webPart.WebPart.Properties["Title"] = "Dum tek";
                    webPart.SaveWebPartChanges();
                    context.Load(webPart);
                    context.ExecuteQuery();
                    */
                }

                return webparts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns webparts of the page
        /// </summary>
        /// <param name="siteSetting"></param>
        /// <param name="pageServerRelativeUrl"></param>
        /// <returns></returns>
        public void SaveWebpartProperty(ISiteSetting siteSetting, string pageServerRelativeUrl, string webpartId, string key, string value)
        {
            try
            {
                List<SPWebpart> webparts = new List<SPWebpart>();
                ClientContext context = GetClientContext(siteSetting);
                var oFile = context.Web.GetFileByServerRelativeUrl(pageServerRelativeUrl);

                Microsoft.SharePoint.Client.WebParts.LimitedWebPartManager wpm = oFile.GetLimitedWebPartManager(Microsoft.SharePoint.Client.WebParts.PersonalizationScope.Shared);

                var query = wpm.WebParts.Include(wp => wp.Id, wp => wp.WebPart, wp => wp.WebPart.Properties);
                var webPartDefenitions = context.LoadQuery(query);

                context.ExecuteQuery();

                Microsoft.SharePoint.Client.WebParts.WebPartDefinition webPart = webPartDefenitions.Where(t => t.Id.ToString().Equals(webpartId) == true).FirstOrDefault();
                if (webPart != null)
                {
                    webPart.WebPart.Properties[key] = value;
                    webPart.SaveWebPartChanges();
                    context.Load(webPart);
                    context.ExecuteQuery();
                }
                else
                    throw new Exception("Webpart could not be found");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns properties of a site
        /// </summary>
        /// <param name="siteSetting"></param>
        /// <param name="webUrl"></param>
        /// <returns></returns>
        public SPWeb GetWeb(ISiteSetting siteSetting, string webUrl)
        {
            try
            {
                ClientContext context = GetClientContext(siteSetting);
                Web web = context.Web;
                Site site = context.Site;

                context.Load(site);
                context.Load(web);
                context.ExecuteQuery();

                /*
                SharePointWebsWS.Webs ws = GetWebsWebService(siteSetting, webUrl);
                XmlNode element = ws.GetWeb(webUrl);
                XmlNode element1 = ws.GetWebCollection();
                string message = string.Format("SharePointService GetWeb method returned xml:{0}", element.OuterXml);
                Logger.Info(message, "Service");
                string url = element.Attributes["Url"].Value;
                string title = element.Attributes["Title"].Value;
                */

                string siteUrl = site.Url;
                string title = web.Title;
                return new SPWeb(web.Url, title, siteSetting.ID, web.Id.ToString(), siteUrl, web.Url, web.ServerRelativeUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SPList GetListById(ISiteSetting siteSetting, Guid listId)
        {
            try
            {
                ClientContext context = GetClientContext(siteSetting);
                Web web = context.Web;
                Site site = context.Site;
                List _list = web.Lists.GetById(listId);
                Microsoft.SharePoint.Client.Folder rootFolder = _list.RootFolder;

                context.Load(site);
                context.Load(web);
                context.Load(_list);
                context.Load(rootFolder);
                context.ExecuteQuery();

                string folderPath = rootFolder.ServerRelativeUrl.Replace(site.ServerRelativeUrl, string.Empty).TrimStart(new char[] { '/' });
                string webApplicationUrl = site.Url.Replace(site.ServerRelativeUrl, string.Empty);
                SPList list = ParseSPList(_list, siteSetting.ID, site.Url, web.Url, folderPath, webApplicationUrl);
                list.ServerRelativePath = rootFolder.ServerRelativeUrl;
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public SPList GetListByTitle(ISiteSetting siteSetting, string title)
        {
            try
            {
                ClientContext context = GetClientContext(siteSetting);
                Web web = context.Web;
                Site site = context.Site;
                List _list = web.Lists.GetByTitle(title);
                Microsoft.SharePoint.Client.Folder rootFolder = _list.RootFolder;

                context.Load(site);
                context.Load(web);
                context.Load(_list);
                context.Load(rootFolder);
                context.ExecuteQuery();

                string folderPath = rootFolder.ServerRelativeUrl.Replace(site.ServerRelativeUrl, string.Empty).TrimStart(new char[] { '/' });
                string webApplicationUrl = site.Url.Replace(site.ServerRelativeUrl, string.Empty);
                SPList list = ParseSPList(_list, siteSetting.ID, site.Url, web.Url, folderPath, webApplicationUrl);
                list.ServerRelativePath = rootFolder.ServerRelativeUrl;
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public SPList GetList(ISiteSetting siteSetting, string listRootFolderUrl)
        {
            try
            {
                SPList list = null;
                SPWeb web = null;

                string webUrl = listRootFolderUrl;
                while (web == null)
                {
                    try
                    {
                        web = GetWeb(siteSetting, webUrl);
                    }
                    catch { }

                    if (web == null)
                    {
                        webUrl = webUrl.Substring(0, webUrl.LastIndexOf("/"));
                    }
                    else
                    {
                        string rootFolderPath = listRootFolderUrl.absoluteTorelative(webUrl);
                        if (rootFolderPath.StartsWith("/Lists/") == true)
                        {
                            if (rootFolderPath.IndexOf("/", 7) > -1)
                            {
                                rootFolderPath = rootFolderPath.Substring(0, rootFolderPath.IndexOf("/", 7));
                            }
                        }
                        else
                        {
                            if (rootFolderPath.IndexOf("/", 1) > -1)
                            {
                                rootFolderPath = rootFolderPath.Substring(0, rootFolderPath.IndexOf("/", 1));
                            }
                        }
                        List<SPList> lists = GetLists(siteSetting, webUrl);
                        list = lists.SingleOrDefault(t => t.RootFolderPath.Equals(rootFolderPath, StringComparison.InvariantCultureIgnoreCase));
                    }
                }

                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }



        public SPFolder GetFolder(ISiteSetting siteSetting, string folderUrl)
        {
            SPList list = null;
            SPWeb web = null;

            string webUrl = folderUrl;
            string folderName = folderUrl.Substring(folderUrl.LastIndexOf("/") + 1);
            string parentFolderUrl = folderUrl.Substring(0, folderUrl.LastIndexOf("/"));
            while (web == null)
            {
                try
                {
                    web = GetWeb(siteSetting, webUrl);
                }
                catch { }

                if (web == null)
                {
                    webUrl = webUrl.TrimEnd('/');//.Substring(0, webUrl.LastIndexOf("/"));//JD
                }
                else
                {
                    string rootFolderPath = folderUrl.absoluteTorelative(web.Url);//.Replace(webUrl, string.Empty);//JD
                    if (rootFolderPath.StartsWith("/Lists/") == true)
                    {
                        if (rootFolderPath.IndexOf("/", 7) > -1)
                            rootFolderPath = rootFolderPath.Substring(0, rootFolderPath.IndexOf("/", 7));
                        else
                        {
                            parentFolderUrl = web.Url;
                        }
                    }
                    else if (rootFolderPath.IndexOf("/", 1) > -1)
                    {
                        rootFolderPath = rootFolderPath.Substring(0, rootFolderPath.IndexOf("/", 1));
                    }
                    List<SPList> lists = GetLists(siteSetting, web.Url);
                    list = lists.SingleOrDefault(t => t.RootFolderPath.Equals(rootFolderPath, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            if (parentFolderUrl.Equals(web.Url, StringComparison.InvariantCultureIgnoreCase) == true)
                return list;

            SharePointListsWS.Lists ws = GetListsWebService(siteSetting, web.Url);

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
            XmlNode viewFields = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
            XmlNode queryOptions = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

            queryOptions.InnerXml = @"<IncludeMandatoryColumns>TRUE</IncludeMandatoryColumns>
                          <DateInUtc>TRUE</DateInUtc>
                          <Folder><![CDATA[" + parentFolderUrl + "]]></Folder>";
            viewFields.InnerXml = "";
            query.InnerXml = @"<Where>
                                <And>
                                   <Eq>
                                       <FieldRef Name='FSObjType' />
                                       <Value Type='Lookup'>1</Value>
                                   </Eq>
                                   <Eq>
                                       <FieldRef Name='BaseName' />
                                       <Value Type='Text'><![CDATA[" + folderName + @"]]></Value>
                                   </Eq>
                                </And>
                           </Where>";

            XmlNode ndListItems = ws.GetListItems(list.ListName, null, query, viewFields, null, queryOptions, null);
            string message = string.Format("SharePointService GetFolder method returned ListName:{0} queryOptions:{1} \n query:{2} \n xml:{3}", list.ListName, queryOptions.OuterXml, query.OuterXml, ndListItems.OuterXml);
            //Logger.Info(message, "Service");

            xmlDoc.LoadXml(ndListItems.OuterXml);
            XmlNodeList _folders = xmlDoc.GetElementsByTagName("z:row");

            return ParseSPFolder(_folders[0], siteSetting.ID, list, web.ServerRelativePath);
        }

        private List<SPWeb> GetWebs(ISiteSetting siteSetting, string webUrl)
        {
            try
            {
                List<SPWeb> webs = new List<SPWeb>();
                SharePointWebsWS.Webs ws = GetWebsWebService(siteSetting, webUrl);
                XmlNode subWebs = ws.GetWebCollection();
                string message = string.Format("SharePointService GetWebs method returned xml:{0}", subWebs.OuterXml);
                Logger.Info(message, "Service");

                foreach (XmlElement element in subWebs)
                {
                    string url = element.Attributes["Url"].Value;
                    string title = element.Attributes["Title"].Value;
                    SPWeb web = new SPWeb(url, title, siteSetting.ID, url, webUrl, url, url);
                    webs.Add(web);
                }
                return webs;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                throw ex;
            }
        }

        /*
        private SPList ParseSPList(XmlElement element, Guid siteSettingID, string siteURL, string webUrl)
        {
            string id = element.Attributes["ID"].Value;
            string title = element.Attributes["Title"].Value;
            SPList list = new SPList(siteSettingID, id, title);
            list.ID = id;
            list.ServerTemplate = int.Parse(element.Attributes["ServerTemplate"].Value);
            list.Title = title;
            if (element.Attributes["Hidden"] != null)
                list.Hidden = bool.Parse(element.Attributes["Hidden"].Value);

            if (element.Attributes["AllowDeletion"] != null)
                list.AllowDeletion = bool.Parse(element.Attributes["AllowDeletion"].Value);
            if (element.Attributes["AllowMultiResponses"] != null)
                list.AllowMultiResponses = bool.Parse(element.Attributes["AllowMultiResponses"].Value);
            if (element.Attributes["EnableAttachments"] != null)
                list.EnableAttachments = bool.Parse(element.Attributes["EnableAttachments"].Value);
            if (element.Attributes["EnableModeration"] != null)
                list.EnableModeration = bool.Parse(element.Attributes["EnableModeration"].Value);
            if (element.Attributes["EnableVersioning"] != null)
                list.EnableVersioning = bool.Parse(element.Attributes["EnableVersioning"].Value);
            if (element.Attributes["EnableMinorVersion"] != null)
                list.EnableMinorVersion = bool.Parse(element.Attributes["EnableMinorVersion"].Value);
            if (element.Attributes["RequireCheckout"] != null)
                list.RequireCheckout = bool.Parse(element.Attributes["RequireCheckout"].Value);
            list.WebPath = element.Attributes["WebFullUrl"].Value;
            list.BaseType = int.Parse(element.Attributes["BaseType"].Value);
            string folderPath = string.Empty;
            if (element.Attributes["DefaultViewUrl"] != null && string.IsNullOrEmpty(element.Attributes["DefaultViewUrl"].Value) == false)
            {
                folderPath = element.Attributes["DefaultViewUrl"].Value;
                folderPath = folderPath.Substring(0, folderPath.LastIndexOf("/"));
                if (list.IsDocumentLibrary == true) // Removes Forms folder
                {
                    folderPath = folderPath.Substring(0, folderPath.LastIndexOf("/"));
                }

                if (webUrl.Split(new char[] { '/' }).Length > 3)
                {
                    string webFolderPath = string.Empty;

                    if(webUrl.StartsWith("https")==true)
                        webFolderPath = webUrl.Substring(webUrl.IndexOf('/', 8));
                    else
                        webFolderPath = webUrl.Substring(webUrl.IndexOf('/', 7));

                    if (folderPath.IndexOf(webFolderPath + "/", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        folderPath = folderPath.Substring(webFolderPath.Length + 1);
                    }
                }
            }
            else
            {
                folderPath = (list.IsDocumentLibrary == true ? list.Title : "Lists/" + list.Title);
            }

            list.FolderPath = folderPath;//webUrl + "/" +JD
            list.RootFolderPath = list.WebUrl.TrimEnd(new char[] { '/' }) + "/" + list.FolderPath.TrimStart(new char[] { '/' });
            list.ListName = list.Title;
            list.SiteUrl = siteURL;
            list.WebUrl = webUrl;

            return list;
        }
        */
        private SPList ParseSPList(List _list, Guid siteSettingID, string siteURL, string webUrl, string folderPath, string webApplicationUrl)
        {
            string id = _list.Id.ToString();
            string title = _list.Title;
            SPList list = new SPList(siteSettingID, id, title);
            list.ID = id;
            list.ServerTemplate = _list.BaseTemplate;
            list.Title = title;
            //if (element.Attributes["Hidden"] != null)
            list.Hidden = _list.Hidden;
            //list.AllowDeletion = _list.alo
            //list.AllowMultiResponses = _list.a
            list.EnableAttachments = _list.EnableAttachments;
            list.EnableModeration = _list.EnableModeration;
            list.EnableVersioning = _list.EnableVersioning;
            list.EnableMinorVersion = _list.EnableMinorVersions;
            list.RequireCheckout = _list.ForceCheckout;
            //list.WebPath = element.Attributes["WebFullUrl"].Value;
            list.BaseType = (int)_list.BaseType;
            list.FolderPath = folderPath;//webUrl + "/" +JD
            list.RootFolderPath = list.WebUrl.TrimEnd(new char[] { '/' }) + "/" + list.FolderPath.TrimStart(new char[] { '/' });
            list.ListName = list.Title;
            list.SiteUrl = siteURL;
            list.WebUrl = webUrl;
            list.WebApplicationURL = webApplicationUrl;
            list.Url = list.WebUrl + "/" + list.FolderPath;

            list.PrimaryIdFieldName = "ID";
            list.PrimaryNameFieldName = "Title";
            list.PrimaryFileReferenceFieldName = "FileRef";

            return list;
        }
        /// <summary>
        /// Get lists from sharepoint site
        /// </summary>
        /// <param name="siteSetting">site setting</param>
        /// <param name="webUrl">web site url</param>
        /// <returns></returns>
        private List<SPList> GetLists(ISiteSetting siteSetting, string webUrl)
        {
            try
            {
                List<SPList> lists = new List<SPList>();
                ClientContext context = GetClientContext(siteSetting);
                Web web = context.Web;
                Site site = context.Site;
                ListCollection listCollection = web.Lists;

                context.Load(site);
                context.Load(web);
                context.Load(listCollection);
                context.ExecuteQuery();

                foreach (List _list in listCollection)
                {
                    Microsoft.SharePoint.Client.Folder rootFolder = _list.RootFolder;
                    context.Load(rootFolder);
                    context.ExecuteQuery();

                    string folderPath = rootFolder.ServerRelativeUrl.Replace(site.ServerRelativeUrl, string.Empty).TrimStart(new char[] { '/' });
                    string webApplicationUrl = site.Url.Replace(site.ServerRelativeUrl, string.Empty);
                    SPList list = ParseSPList(_list, siteSetting.ID, site.Url, webUrl, folderPath, webApplicationUrl);
                    list.ServerRelativePath = rootFolder.ServerRelativeUrl;

                    lists.Add(list);
                }

                return lists;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        private SPFolder ParseSPFolder(XmlNode node, Guid siteSettingID, SPFolder parentFolder, string webServerRelativeUrl)
        {
            string id = node.Attributes["ows_ID"].Value;
            string title = node.Attributes["ows_BaseName"].Value;
            string encodedAbsUrl = node.Attributes["ows_EncodedAbsUrl"].Value;
            string absUrl = Uri.UnescapeDataString(encodedAbsUrl);
            string folderPath = absUrl.absoluteTorelative(parentFolder.WebUrl);//absUrl.Replace(parentFolder.WebUrl, string.Empty);
            SPFolder _folder = new SPFolder(siteSettingID, id, title);
            _folder.ServerTemplate = parentFolder.ServerTemplate;
            _folder.BaseType = parentFolder.BaseType;
            _folder.RootFolderPath = parentFolder.RootFolderPath;
            _folder.ID = id;
            _folder.Title = title;
            _folder.ListName = parentFolder.ListName;
            _folder.SiteUrl = parentFolder.SiteUrl;
            _folder.WebUrl = parentFolder.WebUrl;
            //_folder.WebPath = parentFolder.WebPath;
            _folder.FolderPath = folderPath;
            if (_folder.FolderPath.StartsWith(webServerRelativeUrl) == true)
            {
                _folder.FolderPath = _folder.FolderPath.Substring(webServerRelativeUrl.Length + 1);
            }
            foreach (XmlAttribute attribute in node.Attributes)
            {
                _folder.Properties.Add(attribute.Name, attribute.Value);
            }
            return _folder;
        }

        private SPFolder ParseSPFolder(ListItem listItem, Guid siteSettingID, SPFolder parentFolder, string webServerRelativeUrl)
        {
            string id = listItem["ID"].ToString();
            string fileRef = listItem["FileRef"].ToString();
            string fileDirRef = listItem["FileDirRef"].ToString();
            string title = fileRef.Substring(fileRef.LastIndexOf("/") + 1);
            //string encodedAbsUrl = listItem["EncodedAbsUrl"].ToString();
            //string absUrl = Uri.UnescapeDataString(encodedAbsUrl);
            string folderPath = fileDirRef; // absUrl.absoluteTorelative(parentFolder.WebUrl);//absUrl.Replace(parentFolder.WebUrl, string.Empty);
            SPFolder _folder = new SPFolder(siteSettingID, id, title);
            _folder.ServerTemplate = parentFolder.ServerTemplate;
            _folder.BaseType = parentFolder.BaseType;
            _folder.RootFolderPath = parentFolder.RootFolderPath;
            _folder.ID = id;
            _folder.Title = title;
            _folder.ListName = parentFolder.ListName;
            _folder.SiteUrl = parentFolder.SiteUrl;
            _folder.WebUrl = parentFolder.WebUrl;
            _folder.WebApplicationURL = parentFolder.WebApplicationURL;

            //_folder.WebPath = parentFolder.WebPath;
            _folder.FolderPath = folderPath;
            if (_folder.FolderPath.StartsWith(webServerRelativeUrl) == true)
            {
                _folder.FolderPath = _folder.FolderPath.Substring(webServerRelativeUrl.Length + 1);
            }
            _folder.ServerRelativePath = webServerRelativeUrl + "/" + _folder.FolderPath + "/" + _folder.Title;

            foreach (string fieldName in listItem.FieldValues.Keys)
            {
                string value = string.Empty;
                if (listItem.FieldValues[fieldName] != null)
                {
                    object objValue = listItem.FieldValues[fieldName];
                    if (objValue == null)
                    {
                        value = string.Empty;
                    }
                    else if (objValue is FieldUserValue == true)
                    {
                        FieldUserValue userValue = (FieldUserValue)objValue;
                        value = userValue.LookupId + ";#" + userValue.LookupValue;
                    }
                    else if (objValue is DateTime == true)
                    {
                        value = ((DateTime)objValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
                    }
                    else
                    {
                        value = objValue.ToString();
                    }
                }
                _folder.Properties.Add(fieldName, value);
            }

            return _folder;
        }

        private SPListItem ParseSPListItem(ListItem item, ISiteSetting siteSetting, string folderPath, string webUrl, string webApplicationURL, string siteCollectionURL, string listName, string titleFieldName)
        {
            SPListItem listItem = new SPListItem(siteSetting.ID);
            try {
                listItem.HasUniqueRoleAssignments = item.HasUniqueRoleAssignments;
            }
            catch (Exception ec) { }
            listItem.WebURL = webUrl;
            listItem.SiteCollectionURL = siteCollectionURL;
            listItem.ListName = listName;
            listItem.WebApplicationURL = webApplicationURL;
            listItem.ID = item.Id;
            if (string.IsNullOrEmpty(titleFieldName) == true)
            {
                if (item.FieldValues.ContainsKey("Title") == true && item["Title"] != null)
                {
                    listItem.Title = item.FieldValues["Title"].ToString();
                }
                else if (item.FieldValues.ContainsKey("FileRef") == true && item["FileRef"] != null)
                {
                    listItem.Title = item.FieldValues["FileRef"].ToString();
                    listItem.Title = listItem.Title.Substring(listItem.Title.LastIndexOf("/") + 1);
                }
                else
                {
                    listItem.Title = string.Empty;
                }

                /*
                if (item["Title"] != null)
                {
                    listItem.Title = item["Title"].ToString();
                }
                else if (item["FileRef"] != null)
                {
                    listItem.Title = item["FileRef"].ToString();
                    listItem.Title = listItem.Title.Substring(listItem.Title.LastIndexOf("/") + 1);
                }
                */
            }
            else
            {
                listItem.Title = item[titleFieldName].ToString();
            }

            if (item.FieldValues.ContainsKey("CheckoutUser") == true && item["CheckoutUser"] != null)
                listItem.CheckoutUser = item["CheckoutUser"].ToString();

            if (item.FieldValues.ContainsKey("FileRef") == true && item["FileRef"] != null)
                listItem.URL = webApplicationURL + item["FileRef"].ToString();
            //FileDirRef
            if (item.FieldValues.ContainsKey("EncodedAbsUrl") == true && item["EncodedAbsUrl"] != null)
                listItem.URL = item["EncodedAbsUrl"].ToString();

            foreach (string fieldName in item.FieldValues.Keys)
            {
                string value = string.Empty;
                if (item.FieldValues[fieldName] != null)
                {
                    object objValue = item.FieldValues[fieldName];
                    if (objValue == null)
                    {
                        value = string.Empty;
                    }
                    else if (objValue is string[] == true)
                    {
                        value = string.Empty;
                        string[] arrayValues = (string[])objValue;
                        for(int tx = 0;tx< arrayValues.Length; tx++)
                        {
                            if(tx>0)
                                value += ";#";

                            value += arrayValues[tx];
                        }
                    }
                    else if (objValue is FieldUserValue == true)
                    {
                        FieldUserValue userValue = (FieldUserValue)objValue;
                        value = userValue.LookupId + ";#" + userValue.LookupValue;
                    }
                    else if (objValue is DateTime == true)
                    {
                        value = ((DateTime)objValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
                    }
                    else if (objValue is TaxonomyFieldValue == true)
                    {
                        TaxonomyFieldValue taxonomyFieldValue = (TaxonomyFieldValue)objValue;
                        SPTerm term = GetTerm(siteSetting, new Guid(taxonomyFieldValue.TermGuid));
                        value = term.Path;
                    }
                    else if (objValue is TaxonomyFieldValueCollection == true)
                    {
                        TaxonomyFieldValueCollection taxonomyFieldValues = (TaxonomyFieldValueCollection)objValue;
                        value = string.Empty;
                        foreach (TaxonomyFieldValue taxonomyFieldValue in taxonomyFieldValues) {
                            SPTerm term = GetTerm(siteSetting, new Guid(taxonomyFieldValue.TermGuid));
                            if(string.IsNullOrEmpty(value) == false)
                                value += ";#";
                            value += term.Path;
                        }
                    }
                    else
                    {
                        value = objValue.ToString();
                    }
                }
                listItem.Properties.Add(fieldName, value);
            }


            return listItem;
        }

        /*
        private SPListItem ParseSPListItem(XmlNode item, Guid siteSettingID, string folderPath, string webUrl, string webPath, string listName, string titleFieldName)
        {
            SPListItem listItem = new SPListItem(siteSettingID);
            listItem.WebURL = webUrl;
            listItem.WebPath = webPath;
            listItem.ListName = listName;
            listItem.FolderPath = folderPath;
            listItem.ID = int.Parse(item.Attributes["ows_ID"].Value);
            if (string.IsNullOrEmpty(titleFieldName) == true)
            {
                if(item.Attributes["ows_Title"] != null)
                {
                    listItem.Title = item.Attributes["ows_Title"].Value;
                }
                else if (item.Attributes["ows_FileRef"] != null)
                {
                    listItem.Title = item.Attributes["ows_FileRef"].Value;
                    listItem.Title = listItem.Title.Substring(listItem.Title.LastIndexOf("/")+1);
                }

            }
            else
            {
                listItem.Title = item.Attributes["ows_" + titleFieldName].Value;
            }

            if (item.Attributes["ows_CheckoutUser"] != null)
                listItem.CheckoutUser = item.Attributes["ows_CheckoutUser"].Value;

            if(item.Attributes["ows_FileRef"] !=null)
                listItem.URL = item.Attributes["ows_FileRef"].Value;
            if(item.Attributes["ows_EncodedAbsUrl"]!=null)
                listItem.URL = item.Attributes["ows_EncodedAbsUrl"].Value;

            foreach (XmlAttribute attribute in item.Attributes)
            {
                listItem.Properties.Add(attribute.Name, attribute.Value);
            }


            return listItem;
        }
        */

        private List<Sobiens.Connectors.Entities.Folder> GetSubFolders(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder parentFolder)
        {
            try
            {
                SPFolder currentFolder = parentFolder as SPFolder;
                List<Sobiens.Connectors.Entities.Folder> folders = new List<Sobiens.Connectors.Entities.Folder>();

                ClientContext context = GetClientContext(siteSetting, currentFolder.WebUrl);
                List list = context.Web.Lists.GetByTitle(currentFolder.ListName);
                Site site = context.Site;
                Web web = context.Web;
                context.Load(list);
                context.Load(web);
                context.Load(site);

                //SharePointListsWS.Lists ws = GetListsWebService(siteSetting, currentFolder.WebUrl);
                CamlQuery camlQuery = new CamlQuery();

                XmlDocument xmlDoc = new XmlDocument();

                XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                XmlNode viewFields = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
                XmlNode queryOptions = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

                queryOptions.InnerXml = @"<IncludeMandatoryColumns>TRUE</IncludeMandatoryColumns><DateInUtc>TRUE</DateInUtc>";
                if (currentFolder as SPList == null)
                {
                    camlQuery.FolderServerRelativeUrl = currentFolder.ServerRelativePath;
                    //queryOptions.InnerXml += "<Folder><![CDATA[" + currentFolder.GetPath() + "]]></Folder>";
                }

                viewFields.InnerXml = "";
                query.InnerXml = @"<Where>
                           <Eq>
                               <FieldRef Name='FSObjType' />
                               <Value Type='Lookup'>1</Value>
                           </Eq>
                   </Where>";

                //query.InnerXml = orderBy + SPCamlManager.GetCamlString(filters);
                camlQuery.ViewXml = "<View>" + query.OuterXml + queryOptions.OuterXml + "<RowLimit>50000</RowLimit></View>";
                ListItemCollection collListItem = list.GetItems(camlQuery);
                context.Load(collListItem, a => a.IncludeWithDefaultProperties(b => b.HasUniqueRoleAssignments));
                //context.Load(collListItem);
                context.ExecuteQuery();

                string siteCollectionURL = site.Url;
                string webApplicationURL = site.Url.Substring(0, site.Url.IndexOf(site.ServerRelativeUrl));

                //XmlNode ndListItems = ws.GetListItems(currentFolder.ListName, null, query, viewFields, null, queryOptions, null);
                //string message = string.Format("SharePointService GetSubFolders method returned ListName:{0} queryOptions:{1} \n query:{2} \n xml:{3}", currentFolder.ListName, queryOptions.OuterXml, query.OuterXml, ndListItems.OuterXml);
                //Logger.Info(message, "Service");

                //xmlDoc.LoadXml(ndListItems.OuterXml);
                //XmlNodeList _folders = xmlDoc.GetElementsByTagName("z:row");

                string siteURL = GetSiteURL(siteSetting, currentFolder.WebUrl);

                foreach (ListItem oListItem in collListItem)
                {
                    SPFolder _folder = ParseSPFolder(oListItem, siteSetting.ID, currentFolder, web.ServerRelativeUrl);
                    _folder.HasUniqueRoleAssignments = oListItem.HasUniqueRoleAssignments;

                    //SPListItem listItem = ParseSPListItem(oListItem, siteSetting.ID, folderName, webUrl, webApplicationURL, siteCollectionURL, listName, string.Empty);
                    folders.Add(_folder);
                }

                /*
                foreach (XmlNode node in _folders)
                {
                    SPFolder _folder = ParseSPFolder(node, siteSetting.ID, currentFolder);
                    folders.Add(_folder);
                }
                */
                return folders;//.Sort(s => s.Title);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                throw ex;
            }
        }


        public SPListItem GetItem(ISiteSetting siteSetting, string itemUrl, out Sobiens.Connectors.Entities.Folder itemFolder)
        {
            string folderUrl = itemUrl.Substring(0, itemUrl.LastIndexOf("/"));
            string fileName = itemUrl.Substring(itemUrl.LastIndexOf("/") + 1);
            SPFolder folder = this.GetFolder(siteSetting, folderUrl);

            ClientContext context = GetClientContext(siteSetting);
            List list = context.Web.Lists.GetByTitle(folder.ListName);
            Site site = context.Site;
            Web web = context.Web;
            context.Load(list);
            context.Load(web);
            context.Load(site);

            itemFolder = folder;
            //SharePointListsWS.Lists ws = GetListsWebService(siteSetting, folder.WebUrl);
            string fileURL = itemUrl.absoluteTorelative(folder.SiteUrl);//itemUrl.Replace(folder.SiteUrl, String.Empty);//JD
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
            XmlNode viewFields = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
            XmlNode queryOptions = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");
            queryOptions.InnerXml = "<ViewAttributes Scope=\"Recursive\" /><IncludeMandatoryColumns>True</IncludeMandatoryColumns>";
            XmlNode dateInUtcNode = xmlDoc.CreateNode(XmlNodeType.Element, "DateInUtc", String.Empty);
            dateInUtcNode.InnerText = "TRUE";
            queryOptions.AppendChild(dateInUtcNode);
            viewFields.InnerXml = "";
            CamlFilters filters = new CamlFilters();
            filters.Add(@"<Eq>
                                 <FieldRef Name='FileRef' />
                                 <Value Type='Lookup' >" + fileURL.TrimStart(new char[] { '/' }) + @"</Value>
                                </Eq>");

            query.InnerXml = "<Where>" + SPCamlManager.GetCamlString(filters) + "</Where>";
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View>" + query.OuterXml + queryOptions.OuterXml + "<RowLimit>100</RowLimit></View>";
            ListItemCollection collListItem = list.GetItems(camlQuery);
            context.Load(collListItem);
            context.ExecuteQuery();
            string siteCollectionURL = site.Url;
            string webApplicationURL = site.Url.Substring(0, site.Url.IndexOf(site.ServerRelativeUrl));


            if (collListItem.Count == 0)
            {
                throw new Exception(string.Format("Item with url of {0} could not be found in SiteSettingID:{1}", itemUrl, siteSetting.ID));
            }

            string titleFieldName = "LinkFilename";
            SPListItem listItem = ParseSPListItem(collListItem[0], siteSetting, folder.FolderPath, folder.WebUrl,webApplicationURL, siteCollectionURL, folder.ListName, titleFieldName);
            return listItem;

            /*
            XmlNode ndListItems = ws.GetListItems(folder.ListName, null, query, viewFields, null, queryOptions, null);
            xmlDoc.LoadXml(ndListItems.OuterXml);
            XmlNodeList _items = xmlDoc.GetElementsByTagName("z:row");
            string titleFieldName = "LinkFilename";
            string message = string.Format("SharePointService GetListItems method returned ListName:{0} queryOptions:{1} \n query:{2} \n xml:{3}", folder.ListName, queryOptions.OuterXml, query.OuterXml, ndListItems.OuterXml);
            Logger.Info(message, "Service");

            if (_items.Count == 0)
            {
                throw new Exception(string.Format("Item with url of {} could not be found in SiteSettingID:{1}", itemUrl, siteSetting.ID));
            }

            SPListItem listItem = ParseSPListItem(_items[0], siteSetting.ID, folder.FolderPath, folder.WebUrl, folder.WebPath, folder.ListName, titleFieldName);
            return listItem;
            */
        }

        public List<IItem> GetListItems(ISiteSetting siteSetting, IView selectedView, string sortField, bool isAsc, bool isDocumentLibrary, string webUrl, string listName, string folderName, string currentListItemCollectionPositionNext, CamlFilters customFilters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount)
        {
            try
            {
                CamlQuery camlQuery = new CamlQuery();

                ClientContext context = GetClientContext(siteSetting);
                List list = context.Web.Lists.GetByTitle(listName);
                Site site = context.Site;
                Web web = context.Web;
                context.Load(list);
                context.Load(web);
                context.Load(site);
                context.ExecuteQuery();

                SPView view = selectedView as SPView;
                string orderBy = String.Empty;
                if (sortField == String.Empty)
                {
                    if (view != null)
                    {
                        orderBy = view.GetOrderByXML();
                    }
                }
                else
                {
                    orderBy = "<OrderBy><FieldRef Name=\"" + sortField + "\" " + (isAsc == false ? "Ascending=\"FALSE\"" : "") + " /></OrderBy>";
                }
                List<IItem> items = new List<IItem>();
                //SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                XmlDocument xmlDoc = new XmlDocument();

                XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                XmlNode viewFields = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
                XmlNode queryOptions = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

                XmlNode includeMandatoryColumnsNode = xmlDoc.CreateNode(XmlNodeType.Element, "IncludeMandatoryColumns", String.Empty);
                includeMandatoryColumnsNode.InnerText = "TRUE";
                queryOptions.AppendChild(includeMandatoryColumnsNode);

                XmlNode dateInUtcNode = xmlDoc.CreateNode(XmlNodeType.Element, "DateInUtc", String.Empty);
                dateInUtcNode.InnerText = "TRUE";
                queryOptions.AppendChild(dateInUtcNode);

                if (string.IsNullOrEmpty(folderName) == false)
                {
                    camlQuery.FolderServerRelativeUrl = web.ServerRelativeUrl + "/" + folderName;

                    /*
                    XmlNode folderNode = xmlDoc.CreateNode(XmlNodeType.Element, "Folder", String.Empty);
                    folderNode.InnerText = folderName;
                    queryOptions.AppendChild(folderNode);
                    */
                }

                if (isRecursive == true)
                {
                    XmlNode viewAttributesNode = xmlDoc.CreateNode(XmlNodeType.Element, "ViewAttributes", String.Empty);
                    XmlAttribute scopeAttribute = xmlDoc.CreateAttribute(string.Empty, "Scope", String.Empty);
                    scopeAttribute.InnerText = "RecursiveAll";
                    viewAttributesNode.Attributes.Append(scopeAttribute);
                    queryOptions.AppendChild(viewAttributesNode);
                }


                if (view != null && view.RowLimit > 0)
                {
                    XmlNode rowLimitNode = xmlDoc.CreateNode(XmlNodeType.Element, "RowLimit", String.Empty);
                    rowLimitNode.InnerText = view.RowLimit.ToString();
                    queryOptions.AppendChild(rowLimitNode);
                }
                if (currentListItemCollectionPositionNext != String.Empty)
                {
                    XmlNode pagingNode = xmlDoc.CreateNode(XmlNodeType.Element, "Paging", String.Empty);
                    XmlAttribute positionNextAttribute = xmlDoc.CreateAttribute("ListItemCollectionPositionNext");
                    positionNextAttribute.Value = currentListItemCollectionPositionNext;
                    pagingNode.Attributes.Append(positionNextAttribute);
                    queryOptions.AppendChild(pagingNode);
                }
                viewFields.InnerXml = "";
                CamlFilters filters = new CamlFilters();
                filters.IsOr = false;
                filters.Add(@"<Eq><FieldRef Name='FSObjType' /><Value Type='Lookup'>0</Value></Eq>");
                if (view != null && view.WhereXML != String.Empty)
                {
                    filters.Add(view.WhereXML);
                }
                if (customFilters != null && customFilters.Filters.Count > 0)
                {
                    string customQuery = SPCamlManager.GetCamlString(customFilters);
                    filters.Add(customQuery);
                }

                query.InnerXml = orderBy + SPCamlManager.GetCamlString(filters);
                camlQuery.ViewXml = "<View>" + query.OuterXml + queryOptions.OuterXml + (view!=null?"<RowLimit>" + view.RowLimit.ToString() + "</RowLimit>":string.Empty) + "</View>";
                ListItemCollection collListItem = list.GetItems(camlQuery);
                context.Load(collListItem, a => a.IncludeWithDefaultProperties(b => b.HasUniqueRoleAssignments), c=>c.ListItemCollectionPosition);
                //context.Load(collListItem);
                context.ExecuteQuery();

                string siteCollectionURL = site.Url;
                string webApplicationURL = site.Url.Substring(0, site.Url.IndexOf(site.ServerRelativeUrl));

                listItemCollectionPositionNext = String.Empty;
                if (collListItem.ListItemCollectionPosition != null && collListItem.ListItemCollectionPosition.PagingInfo != null)
                    listItemCollectionPositionNext = collListItem.ListItemCollectionPosition.PagingInfo;

                foreach (ListItem oListItem in collListItem)
                {
                    SPListItem listItem = ParseSPListItem(oListItem, siteSetting, folderName, webUrl, webApplicationURL, siteCollectionURL, listName, string.Empty);
                    items.Add(listItem);
                }

                itemCount = collListItem.Count;
                if (collListItem.AreItemsAvailable == true)
                    itemCount = collListItem.Count + 1;

                /*
                XmlNode ndListItems = ws.GetListItems(listName, null, query, viewFields, null, queryOptions, null);
                string message = string.Format("SharePointService GetListItems method returned ListName:{0} queryOptions:{1} \n query:{2} \n xml:{3}", listName, queryOptions.OuterXml, query.OuterXml, ndListItems.OuterXml);
                Logger.Info(message, "Service");

                xmlDoc.LoadXml(ndListItems.OuterXml);

                itemCount = int.Parse(xmlDoc.GetElementsByTagName("rs:data")[0].Attributes["ItemCount"].Value);
                XmlAttribute listItemCollectionPositionNextAttribute = xmlDoc.GetElementsByTagName("rs:data")[0].Attributes["ListItemCollectionPositionNext"];
                listItemCollectionPositionNext = String.Empty;
                if (listItemCollectionPositionNextAttribute != null)
                    listItemCollectionPositionNext = listItemCollectionPositionNextAttribute.Value;
                XmlNodeList _items = xmlDoc.GetElementsByTagName("z:row");
                string titleFieldName = "Title";
                if (isDocumentLibrary == true)
                    titleFieldName = "LinkFilename";
                foreach (XmlNode item in _items)
                {
                    SPListItem listItem = ParseSPListItem(item, siteSetting.ID, folderName, webUrl, webPath, listName, titleFieldName);

                    items.Add(listItem);
                }
                */
                return items;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount)
        {
            try
            {
                ClientContext context = GetClientContext(siteSetting);
                List list = context.Web.Lists.GetByTitle(listName);
                Site site = context.Site;
                Web web = context.Web;
                context.Load(list);
                context.Load(web);
                context.Load(site);

                if (viewFields.Where(t => t.Name.Equals("Editor") == true).Count() == 0)
                    viewFields.Add(new CamlFieldRef("Editor", "Editor"));

                listItemCollectionPositionNext = string.Empty;
                Logger.Info("GetListItems method is started", "Service");
                List<IItem> items = new List<IItem>();

                XmlDocument xmlDoc = new XmlDocument();

                int columnLimitCount = 8;
                for (int i = 0; i < viewFields.Count; i = i + columnLimitCount)
                {
                    List<CamlFieldRef> currentViewFields = new List<CamlFieldRef>();
                    for (int t = 0; t < columnLimitCount; t++)
                    {
                        if (viewFields.Count > i + t)
                            currentViewFields.Add(viewFields[i + t]);
                    }

                    XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                    XmlNode viewFieldsNode = SPCamlManager.GetViewFieldsXmlNode(currentViewFields);
                    XmlNode queryOptionsNode = SPCamlManager.GetQueryOptionsXmlNode(queryOptions);

                    query.InnerXml = SPCamlManager.GetCamlString(filters, orderBys);



                    CamlQuery camlQuery = new CamlQuery();
                    if(string.IsNullOrEmpty(queryOptions.ListItemCollectionPositionNext) == false)
                    {
                        camlQuery.ListItemCollectionPosition = new ListItemCollectionPosition();
                        camlQuery.ListItemCollectionPosition.PagingInfo = queryOptions.ListItemCollectionPositionNext;
                    }

                    camlQuery.ViewXml = "<View>" + query.OuterXml + queryOptionsNode.OuterXml + "<RowLimit>" + queryOptions.RowLimit.ToString() + "</RowLimit>" + viewFieldsNode.OuterXml + "</View>";
                    ListItemCollection collListItem = list.GetItems(camlQuery);
                    context.Load(collListItem);
                    context.ExecuteQuery();

                    string siteCollectionURL = site.Url;
                    string webApplicationURL = site.Url.Substring(0, site.Url.IndexOf(site.ServerRelativeUrl));

                    listItemCollectionPositionNext = String.Empty;
                    if (collListItem.ListItemCollectionPosition != null && collListItem.ListItemCollectionPosition.PagingInfo != null)
                        listItemCollectionPositionNext = collListItem.ListItemCollectionPosition.PagingInfo;

                    foreach (ListItem oListItem in collListItem)
                    {

                        SPListItem listItem = ParseSPListItem(oListItem, siteSetting, queryOptions.Folder, webUrl,webApplicationURL, siteCollectionURL, listName, string.Empty);
                        if (i > 0)
                        {
                            IItem foundListItem = items.Where(t => t.GetID().Equals(listItem.ID.ToString())).FirstOrDefault();
                            for (int t = 0; t < columnLimitCount; t++)
                            {
                                if (viewFields.Count > i + t)
                                {
                                    string viewFieldName = viewFields[i + t].Name;
                                    if (listItem.Properties.ContainsKey(viewFieldName) == true)
                                    {
                                        if (foundListItem.Properties.ContainsKey(viewFieldName) == true)
                                            foundListItem.Properties[viewFieldName] = listItem.Properties[viewFieldName];
                                        else
                                            foundListItem.Properties.Add(viewFieldName, listItem.Properties[viewFieldName]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            items.Add(listItem);
                        }
                    }
                }
                itemCount = items.Count;
                return items;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Empty;
                if (ex as System.Web.Services.Protocols.SoapException != null)
                    errorMessage = ((System.Web.Services.Protocols.SoapException)ex).Detail.InnerText;
                else
                    errorMessage = ex.Message;

                errorMessage += Environment.NewLine + ex.StackTrace;
                Logger.Info(errorMessage, "Service");


                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        /*
                 public List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string webPath, string listName, out string listItemCollectionPositionNext, out int itemCount)
        {
            try
            {
                ClientContext context = GetClientContext(siteSetting);
                List list = context.Web.Lists.GetByTitle(listName);

                if (viewFields.Where(t => t.Name.Equals("Editor") == true).Count() == 0)
                    viewFields.Add(new CamlFieldRef("Editor", "Editor"));

                listItemCollectionPositionNext = string.Empty;
                itemCount = 0;
                Logger.Info("GetListItems method is started", "Service");
                List<IItem> items = new List<IItem>();

                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                XmlDocument xmlDoc = new XmlDocument();

                int columnLimitCount = 8;
                for (int i = 0; i < viewFields.Count; i = i + columnLimitCount)
                {
                    List<CamlFieldRef> currentViewFields = new List<CamlFieldRef>();
                    for (int t = 0; t < columnLimitCount; t++)
                    {
                        if (viewFields.Count > i + t)
                            currentViewFields.Add(viewFields[i + t]);
                    }

                    XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                    XmlNode viewFieldsNode = SPCamlManager.GetViewFieldsXmlNode(currentViewFields);
                    XmlNode queryOptionsNode = SPCamlManager.GetQueryOptionsXmlNode(queryOptions);

                    query.InnerXml = SPCamlManager.GetCamlString(filters, orderBys);
                    Logger.Info("Retriving list items", "Service");
                    Logger.Info("query.InnerXml:", "Service");
                    Logger.Info(query.InnerXml, "Service");
                    Logger.Info("webUrl:", "Service");
                    Logger.Info(webUrl, "Service");
                    Logger.Info("listName:", "Service");
                    Logger.Info(listName, "Service");
                    XmlNode ndListItems = ws.GetListItems(listName, null, query, viewFieldsNode, queryOptions.RowLimit.ToString(), queryOptionsNode, null);
                    Logger.Info("SharePointService GetListItems method returned ListName:" + listName, "Service");
                    if (queryOptionsNode != null)
                        Logger.Info("queryOptions:" + queryOptionsNode.OuterXml, "Service");
                    if (query != null)
                        Logger.Info("query:" + query.OuterXml, "Service");
                    if (ndListItems != null)
                        Logger.Info("ndListItems:" + ndListItems.OuterXml, "Service");
                    else
                        Logger.Info("ndListItems is null", "Service");


                    xmlDoc.LoadXml(ndListItems.OuterXml);

                    itemCount = int.Parse(xmlDoc.GetElementsByTagName("rs:data")[0].Attributes["ItemCount"].Value);
                    XmlAttribute listItemCollectionPositionNextAttribute = xmlDoc.GetElementsByTagName("rs:data")[0].Attributes["ListItemCollectionPositionNext"];
                    listItemCollectionPositionNext = String.Empty;
                    if (listItemCollectionPositionNextAttribute != null)
                        listItemCollectionPositionNext = listItemCollectionPositionNextAttribute.Value;
                    XmlNodeList _items = xmlDoc.GetElementsByTagName("z:row");
                    foreach (XmlNode item in _items)
                    {
                        SPListItem listItem = ParseSPListItem(item, siteSetting.ID, queryOptions.Folder, webUrl, webPath, listName, string.Empty);
                        if (i > 0)
                        {
                            IItem foundListItem = items.Where(t => t.GetID().Equals(listItem.ID.ToString())).FirstOrDefault();
                            for (int t = 0; t < columnLimitCount; t++)
                            {
                                if (viewFields.Count > i + t)
                                {
                                    string viewFieldName = "ows_" + viewFields[i + t].Name;
                                    if (listItem.Properties.ContainsKey(viewFieldName) == true)
                                    {
                                        if (foundListItem.Properties.ContainsKey(viewFieldName) == true)
                                            foundListItem.Properties[viewFieldName] = listItem.Properties[viewFieldName];
                                        else
                                            foundListItem.Properties.Add(viewFieldName, listItem.Properties[viewFieldName]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            items.Add(listItem);
                        }
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Empty;
                if (ex as System.Web.Services.Protocols.SoapException != null)
                    errorMessage = ((System.Web.Services.Protocols.SoapException)ex).Detail.InnerText;
                else
                    errorMessage = ex.Message;

                errorMessage += Environment.NewLine + ex.StackTrace;
                Logger.Info(errorMessage, "Service");


                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

             */
        public static void UpdateListItem(ISiteSetting siteSetting, string webUrl, string listName, int listItemID, Hashtable changedProperties, bool systemUpdate)
        {

            try
            {
                lock (UpdateListItemLocker)
                {
                    ClientContext context = GetClientContext(siteSetting);
                    List list = context.Web.Lists.GetByTitle(listName);
                    Microsoft.SharePoint.Client.FieldCollection fields = list.Fields;
                    ContentTypeCollection contentTypes = list.ContentTypes;
                    ListItem item = list.GetItemById(listItemID);
                    context.Load(fields);
                    context.Load(contentTypes);
                    context.Load(item);
                    context.ExecuteQuery();

                    if (systemUpdate == true)
                    {
                        list.EnableVersioning = false;
                        list.Update();
                        context.ExecuteQuery();
                    }

                    foreach (object fieldName in changedProperties.Keys)
                    {
                        string name = fieldName.ToString();
                        string val = (changedProperties[fieldName] == null) ? string.Empty : changedProperties[fieldName].ToString();
                        if (name == "FileLeafRef") name = "BaseName";

                        if (name == "ContentType")
                        {
                            Microsoft.SharePoint.Client.ContentType ct = contentTypes.Where(t => t.Name.Equals(val, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                            if (ct != null)
                            {
                                item["ContentTypeId"] = ct.Id.ToString();
                            }
                        }
                        else
                        {
                            Microsoft.SharePoint.Client.Field field = fields.Where(t => t.InternalName.Equals(fieldName.ToString(), StringComparison.InvariantCultureIgnoreCase)).First();
                            item[fieldName.ToString()] = val;
                        }
                    }

                    item.Update();
                    context.ExecuteQuery();
                    if (systemUpdate == true)
                    {
                        list.EnableVersioning = true;
                        list.Update();
                    }
                    context.ExecuteQuery();
                }
                /*
                FieldUserValue author = GetUsers(context, "melisa@sobiens12.onmicrosoft.com");
                FieldUserValue editor = GetUsers(context, "melisa@sobiens12.onmicrosoft.com");
                item["Author"] = author;
                item["Editor"] = editor;
                item.Update();
                list.EnableVersioning = true;
                list.Update();
                context.ExecuteQuery();

                return;
                */
                /*
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                System.Xml.XmlElement batchElement = doc.CreateElement("Batch");
                batchElement.SetAttribute("OnError", "Continue");
                batchElement.SetAttribute("ListVersion", "1");
                //            batchElement.SetAttribute("ViewName", strViewID);

                string xml = "<Method ID='1' Cmd='Update'>" +
                   "<Field Name='ID'>" + listItemID + "</Field>";
                foreach (object fieldName in changedProperties.Keys)
                {
                    string name = fieldName.ToString();
                    if (name == "FileLeafRef") name = "BaseName";

                    string val = (changedProperties[fieldName] == null) ? string.Empty : changedProperties[fieldName].ToString();
                    xml += "<Field Name='" + name + "'>" + System.Security.SecurityElement.Escape(val) + "</Field>";
                }
                xml += "</Method>";
                batchElement.InnerXml = xml;

                Logger.Info("Batch:", "Service");
                if (batchElement == null || batchElement.OuterXml == null)
                    Logger.Info("Batch is empty", "Service");
                else
                    Logger.Info(batchElement.OuterXml, "Service");

                XmlNode resultNode = ws.UpdateListItems(listName, batchElement);
                string message = string.Format("SharePointService UpdateListItem method returned ListName:{0} \n batchElement:{1}", listName, batchElement.OuterXml);
                Logger.Info(message, "Service");
                if (resultNode["Result"]["ErrorText"] != null && resultNode["Result"]["ErrorText"].InnerText != String.Empty)
                {
                    Exception ex1 = new Exception(resultNode["Result"]["ErrorText"].InnerText);
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //LogManager.LogException(methodName, ex1);
                    throw ex1;
                }
                */

            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public List<IView> GetViews(ISiteSetting siteSetting, string webUrl, string listName)
        {
            try
            {
                //string webUrl, string listName, 
                List<IView> views = new List<IView>();
                SharePointViewsWS.Views ws = GetViewsWebService(siteSetting, webUrl);
                XmlNode viewNodes = ws.GetViewCollection(listName);
                string message = string.Format("SharePointService GetViews method returned ListName:{0} \n viewNodes:{1}", listName, viewNodes.OuterXml);
                Logger.Info(message, "Service");

                foreach (XmlNode element in viewNodes)
                {
                    IView view = NodeToViewObject(siteSetting, webUrl, listName, element);
                    views.Add(view);
                }
                return views;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        private static SPView NodeToViewObject(ISiteSetting siteSetting, string webUrl, string listName, XmlNode element)
        {
            SPView view = new SPView(siteSetting.ID);
            view.Name = element.Attributes["Name"].Value;
            if (element.Attributes["DefaultView"] != null)
                view.DefaultView = bool.Parse(element.Attributes["DefaultView"].Value);
            if (element.Attributes["MobileView"] != null)
                view.MobileView = bool.Parse(element.Attributes["MobileView"].Value);
            if (element.Attributes["MobileDefaultView"] != null)
                view.MobileDefaultView = bool.Parse(element.Attributes["MobileDefaultView"].Value);
            view.Type = element.Attributes["Type"].Value;
            view.DisplayName = element.Attributes["DisplayName"].Value;
            view.Url = element.Attributes["Url"].Value;
            view.Level = element.Attributes["Level"].Value;
            view.BaseViewID = element.Attributes["BaseViewID"].Value;
            view.ContentTypeID = element.Attributes["ContentTypeID"].Value;
            if (element.Attributes["ImageUrl"] != null)
                view.ImageUrl = element.Attributes["ImageUrl"].Value;
            view.WebURL = webUrl;
            view.ListName = listName;
            return view;
        }

        public static string GetSiteURL(ISiteSetting siteSetting, string webUrl)
        {
            try
            {
                SharePointSiteWS.SiteData ws = GetSiteWebService(siteSetting, webUrl);
                string siteURL;
                string webURL;
                uint value = ws.GetSiteAndWeb(siteSetting.Url, out siteURL, out webURL);
                string message = string.Format("SharePointService GetSiteURL method returned siteSetting.Url:{0} \n siteURL:{1} \n webURL:{2} \n value:{3}", siteSetting.Url, siteURL, webURL, value);
                Logger.Info(message, "Service");
                return siteURL;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        private static SharePointViewsWS.Views GetViewsWebService(ISiteSetting siteSetting, string webURL)
        {
            SharePointViewsWS.Views ws = new SharePointViewsWS.Views();
            SetCredentials(ws, siteSetting);
            ws.Url = webURL + "/_vti_bin/views.asmx";
            return ws;
        }

        private static SharePointSiteWS.SiteData GetSiteWebService(ISiteSetting siteSetting, string webURL)
        {
            SharePointSiteWS.SiteData ws = new SharePointSiteWS.SiteData();
            SetCredentials(ws, siteSetting);
            ws.Url = webURL + "/_vti_bin/SiteData.asmx";
            return ws;
        }

        private static SharePointTaxonomyWS.Taxonomywebservice GetTaxonomyService(ISiteSetting siteSetting, string webURL)
        {
            SharePointTaxonomyWS.Taxonomywebservice ws = new SharePointTaxonomyWS.Taxonomywebservice();
            SetCredentials(ws, siteSetting);
            ws.Url = webURL + "/_vti_bin/TaxonomyClientService.asmx";
            return ws;
        }

        private static SharePointWorkflowWS.Workflow GetWorkflowService(ISiteSetting siteSetting, string webURL)
        {
            SharePointWorkflowWS.Workflow ws = new SharePointWorkflowWS.Workflow();
            SetCredentials(ws, siteSetting);
            ws.Url = webURL + "/_vti_bin/workflow.asmx";
            return ws;
        }

        private SharePointWebsWS.Webs GetWebsWebService(ISiteSetting siteSetting, string webURL)
        {
            SharePointWebsWS.Webs ws = new SharePointWebsWS.Webs();
            SetCredentials(ws, siteSetting);
            ws.Url = webURL + "/_vti_bin/webs.asmx";
            return ws;
        }

        private static SharePointListsWS.Lists GetListsWebService(ISiteSetting siteSetting, string webURL)
        {
            SharePointListsWS.Lists ws = new SharePointListsWS.Lists();
            SetCredentials(ws, siteSetting);
            ws.Url = webURL + "/_vti_bin/lists.asmx";
            return ws;
        }

        private static SharePointVersionsWS.Versions GetVersionsWebService(ISiteSetting siteSetting, string webURL)
        {
            SharePointVersionsWS.Versions ws = new SharePointVersionsWS.Versions();
            SetCredentials(ws, siteSetting);
            ws.Url = webURL + "/_vti_bin/versions.asmx";
            return ws;
        }

        /// <summary>
        /// Converts to ISO8601.
        /// </summary>
        /// <param name="dateTimeString">The date time string.</param>
        /// <returns></returns>
        private static string convertToISO8601(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        /// <summary>
        /// Converts to ISO8601.
        /// </summary>
        /// <param name="dateTimeString">The date time string.</param>
        /// <returns></returns>
        private static DateTime convertISO8601ToDateTime(string dateTime)
        {
            return DateTime.ParseExact(dateTime, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture).ToLocalTime();
        }

        /// <summary>
        /// Converts to ISO8601.
        /// </summary>
        /// <param name="dateTimeString">The date time string.</param>
        /// <returns></returns>
        private static DateTime convertISO8601ToDateTime1(string dateTime)
        {
            return DateTime.ParseExact(dateTime, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        private static void SetCredentials(System.Web.Services.Protocols.SoapHttpClientProtocol webService, ISiteSetting siteSetting)
        {
            if (siteSetting.UseClaimAuthentication == true)
            {
                webService.CookieContainer = GetCookieContainer(siteSetting.Url, siteSetting.Username, siteSetting.Password);
            }
            else
            {
                webService.UserAgent = "";// user agent must be empty if not TMG don't redirect request to asked page
                webService.Credentials = GetCredential(siteSetting);

            }
        }

        public static ICredentials GetCredential(ISiteSetting siteSetting)
        {
            if (siteSetting.UseDefaultCredential == true)
            {
                return System.Net.CredentialCache.DefaultCredentials; ;
            }
            else
            {
                string userName = siteSetting.Username;
                string[] userNameStringArray = userName.Split(new char[] { '\\' });
                if (userNameStringArray.Length > 1)//there is domain
                {
                    return new NetworkCredential(userNameStringArray[1], siteSetting.Password, userNameStringArray[0]);
                }
                else
                {

                    //NetworkCredential myNetworkCredential = new NetworkCredential(siteSetting.Username, siteSetting.Password);
                    //CredentialCache myCredentialCache = new CredentialCache();
                    //myCredentialCache.Add(new Uri(siteSetting.Url), "Basic", myNetworkCredential);
                    //return myCredentialCache;
                    return new NetworkCredential(siteSetting.Username, siteSetting.Password);
                }
            }
        }

        private static Sobiens.Connectors.Entities.FieldCollection ParseToFieldCollection(Microsoft.SharePoint.Client.FieldCollection _fields, ISiteSetting siteSetting, bool includeReadOnly)
        {
            Sobiens.Connectors.Entities.FieldCollection fields = new Sobiens.Connectors.Entities.FieldCollection();
            foreach (Microsoft.SharePoint.Client.Field _field in _fields)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(_field.SchemaXml);
                XmlElement element = xmlDoc.DocumentElement;
                string type = element.Attributes["Type"].Value;
                Sobiens.Connectors.Entities.Field field = new Sobiens.Connectors.Entities.Field();

                if (type.Equals("TaxonomyFieldType", StringComparison.InvariantCultureIgnoreCase) == true
                    || type.Equals("TaxonomyFieldTypeMulti", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    field = new SPTaxonomyField();
                }

                if (element.Attributes["StaticName"].Value.StartsWith("Email") && element.Attributes["Group"] != null && element.Attributes["Group"].Value.ToLower() == "_hidden" &&
                    (element.Attributes["Type"].Value.ToLower() == "note" || element.Attributes["Type"].Value.ToLower() == "text") &&
                    (element.Attributes["StaticName"].Value == "EmailCc" ||
                    element.Attributes["StaticName"].Value == "EmailTo" ||
                    element.Attributes["StaticName"].Value == "EmailSubject" ||
                    element.Attributes["StaticName"].Value == "EmailSender" ||
                    element.Attributes["StaticName"].Value == "EmailFrom")
                    )
                {
                    element.Attributes["Type"].Value = "Text";
                }
                //else if (element.Attributes["Group"] != null && element.Attributes["Group"].Value.ToLower() == "_hidden")
                //    continue;
                else if (element.Attributes["Hidden"] != null && element.Attributes["Hidden"].Value.ToLower() == "true")
                {//JD
                    if (element.Attributes["Type"].Value.ToLower() == "note")//taxonomy field has hidden note field attached with same name
                    {
                        for (int i = 0; i < fields.Count; i++)
                        {
                            if (fields[i].Type == FieldTypes.TaxonomyFieldType &&
                                element.Attributes["DisplayName"].Value.StartsWith(fields[i].DisplayName.Trim()))
                            {
                                fields[i].attachedField = element.Attributes["Name"].Value;
                                break;
                            }
                        }
                    }
                    continue;
                }
                if (element.Attributes["MaxLength"] != null && element.Attributes["MaxLength"].Value != String.Empty)
                {
                    field.MaxLength = int.Parse(element.Attributes["MaxLength"].Value);
                }
                if (element.Attributes["NumLines"] != null && element.Attributes["NumLines"].Value != String.Empty)
                {
                    field.NumLines = int.Parse(element.Attributes["NumLines"].Value);
                }
                if (element.Attributes["AppendOnly"] != null && element.Attributes["AppendOnly"].Value != String.Empty)
                {
                    field.AppendOnly = element.Attributes["AppendOnly"].Value.ToLower() == "true" ? true : false;
                }

                if (element.Attributes["RichText"] != null && element.Attributes["RichText"].Value != String.Empty)
                {
                    field.RichText = bool.Parse(element.Attributes["RichText"].Value);
                }

                if (element.Attributes["RichTextMode"] != null && element.Attributes["RichTextMode"].Value != String.Empty)
                {
                    field.RichTextMode = element.Attributes["RichTextMode"].Value;
                }

                if (element.Attributes["Decimals"] != null && element.Attributes["Decimals"].Value != String.Empty)
                    field.Decimals = int.Parse(element.Attributes["Decimals"].Value);
                if (element.Attributes["Min"] != null && element.Attributes["Min"].Value != String.Empty)
                    field.Min = decimal.Parse(element.Attributes["Min"].Value);
                if (element.Attributes["Max"] != null && element.Attributes["Max"].Value != String.Empty)
                    field.Max = decimal.Parse(element.Attributes["Max"].Value);

                if (element.Attributes["Description"] != null && element.Attributes["Description"].Value != String.Empty)
                    field.Description = element.Attributes["Description"].Value;

                field.ID = new Guid(element.Attributes["ID"].Value);
                string authoringInfo = String.Empty;
                if (element.Attributes["AuthoringInfo"] != null)
                    authoringInfo = element.Attributes["AuthoringInfo"].Value;
                field.DisplayName = element.Attributes["DisplayName"].Value + (string.IsNullOrEmpty(authoringInfo) == false? " " + authoringInfo:"");
                field.Name = element.Attributes["Name"].Value;
                if (element.Attributes["Required"] != null)
                    field.Required = bool.Parse(element.Attributes["Required"].Value);
                if (element.Attributes["FromBaseType"] != null)
                    field.FromBaseType = bool.Parse(element.Attributes["FromBaseType"].Value);
                else
                    field.FromBaseType = false;
                if (element.Attributes["ReadOnly"] != null)
                    field.ReadOnly = bool.Parse(element.Attributes["ReadOnly"].Value);
                else
                    field.ReadOnly = false;
                if (element["Default"] != null)
                    field.DefaultValue = element["Default"].InnerText;

                if (element.Attributes["Mult"] == null || element.Attributes["Mult"].Value.ToString() == String.Empty)
                    field.Mult = false;
                else
                    field.Mult = bool.Parse(element.Attributes["Mult"].Value);
                if (field.Required == true)
                    field.ReadOnly = false;

                FieldTypes fieldType;
                switch (type.ToLower())
                {
                    case "text":
                        fieldType = FieldTypes.Text;
                        break;
                    case "note":
                        fieldType = FieldTypes.Note;
                        break;
                    case "boolean":
                        fieldType = FieldTypes.Boolean;
                        break;
                    case "datetime":
                        fieldType = FieldTypes.DateTime;
                        break;
                    case "number":
                        fieldType = FieldTypes.Number;
                        break;
                    case "taxonomyfieldtype":
                    case "taxonomyfieldtypemulti":
                        fieldType = FieldTypes.TaxonomyFieldType;
                        var xmlNsM = new XmlNamespaceManager(element.OwnerDocument.NameTable);
                        //xmlNsM.AddNamespace("foo", "http://schemas.microsoft.com/sharepoint/soap/");
                        Guid termSetId = new Guid(element.SelectSingleNode("Customization/ArrayOfProperty/Property[Name='TermSetId']", xmlNsM)["Value"].InnerText);
                        Guid sspId = new Guid(element.SelectSingleNode("Customization/ArrayOfProperty/Property[Name='SspId']", xmlNsM)["Value"].InnerText);
                        Guid anchorId = new Guid(element.SelectSingleNode("Customization/ArrayOfProperty/Property[Name='AnchorId']", xmlNsM)["Value"].InnerText);

                        ((SPTaxonomyField)field).TermSetId = termSetId;
                        ((SPTaxonomyField)field).SspId = sspId;
                        ((SPTaxonomyField)field).AnchorId = anchorId;


                        string lcidString = element.Attributes["ShowField"].Value.Replace("Term", string.Empty);
                        int lcid;
                        if (int.TryParse(lcidString, out lcid) == false)
                        {
                            lcid = 1033;
                        }

                        if(anchorId == Guid.Empty)
                        {
                            SPTermSet termSet = new SharePointService().GetTermSet(siteSetting, termSetId);
                            ((SPTaxonomyField)field).Path = termSet.Path;
                        }
                        else
                        {
                            SPTerm term = new SharePointService().GetTerm(siteSetting, anchorId);
                            ((SPTaxonomyField)field).Path = term.Path;
                        }

                        ((SPTaxonomyField)field).LCID = lcid;
                        //field.List = element.Attributes["List"].Value;
                        //field.ShowField = element.Attributes["ShowField"].Value;
                        if(type.ToLower() == "taxonomyfieldtypemulti")
                            field.Mult = true;
                        break;
                    case "lookup":
                        fieldType = FieldTypes.Lookup;
                        field.List = element.Attributes["List"].Value;
                        Guid listId;
                        if(Guid.TryParse(field.List, out listId) == true)
                        {
                            SPList referenceList = new SharePointService().GetListById(siteSetting, listId);
                            field.List = referenceList.Title;
                        }
                        field.ShowField = (element.Attributes["ShowField"] != null ? element.Attributes["ShowField"].Value : "Title");
                        break;
                    case "lookupmulti":
                        fieldType = FieldTypes.Lookup;
                        field.List = element.Attributes["List"].Value;
                        field.ShowField = (element.Attributes["ShowField"] != null ? element.Attributes["ShowField"].Value : "Title");
                        field.Mult = true;
                        break;
                    case "choice":
                        fieldType = FieldTypes.Choice;
                        field.ChoiceItems = GetChoiceDataItems(element);
                        break;
                    case "multichoice":
                        fieldType = FieldTypes.Choice;
                        field.ChoiceItems = GetChoiceDataItems(element);
                        field.Mult = true;
                        break;
                    case "file":
                        fieldType = FieldTypes.File;
                        break;
                    case "url":
                        fieldType = FieldTypes.URL;
                        break;
                    case "user":
                        fieldType = FieldTypes.User;
                        break;
                    case "currency":
                        fieldType = FieldTypes.Currency;
                        break;
                    case "calculated":
                        fieldType = FieldTypes.Calculated;
                        field.Formula = element.InnerXml;
                        break;
                    case "outcomechoice":
                        fieldType = FieldTypes.OutcomeChoice;
                        field.ChoiceItems = GetChoiceDataItems(element);
                        break;
                    default:
                        fieldType = FieldTypes.Unknown;
                        field.ReadOnly = true;
                        break;
                }
                field.Type = fieldType;
                if (field.ReadOnly == false || includeReadOnly == true)
                {
                    fields.Add(field);
                }
            }
            return fields;
        }

        private static Sobiens.Connectors.Entities.FieldCollection XmlNodeToFieldCollection(XmlElement fieldElement, bool includeReadOnly)
        {
            Sobiens.Connectors.Entities.FieldCollection fields = new Sobiens.Connectors.Entities.FieldCollection();

            foreach (XmlElement element in fieldElement)
            {
                string type = element.Attributes["Type"].Value;

                Sobiens.Connectors.Entities.Field field = new Sobiens.Connectors.Entities.Field();
                if (type.Equals("TaxonomyFieldType", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    field = new SPTaxonomyField();
                }

                if (element.Attributes["StaticName"].Value.StartsWith("Email") && element.Attributes["Group"] != null && element.Attributes["Group"].Value.ToLower() == "_hidden" &&
                    (element.Attributes["Type"].Value.ToLower() == "note" || element.Attributes["Type"].Value.ToLower() == "text") &&
                    (element.Attributes["StaticName"].Value == "EmailCc" ||
                    element.Attributes["StaticName"].Value == "EmailTo" ||
                    element.Attributes["StaticName"].Value == "EmailSubject" ||
                    element.Attributes["StaticName"].Value == "EmailSender" ||
                    element.Attributes["StaticName"].Value == "EmailFrom")
                    )
                {
                    element.Attributes["Type"].Value = "Text";
                }
                //else if (element.Attributes["Group"] != null && element.Attributes["Group"].Value.ToLower() == "_hidden")
                //    continue;
                else if (element.Attributes["Hidden"] != null && element.Attributes["Hidden"].Value.ToLower() == "true")
                {//JD
                    if (element.Attributes["Type"].Value.ToLower() == "note")//taxonomy field has hidden note field attached with same name
                    {
                        for (int i = 0; i < fields.Count; i++)
                        {
                            if (fields[i].Type == FieldTypes.TaxonomyFieldType &&
                                element.Attributes["DisplayName"].Value.StartsWith(fields[i].DisplayName.Trim()))
                            {
                                fields[i].attachedField = element.Attributes["Name"].Value;
                                break;
                            }
                        }
                    }
                    continue;
                }
                if (element.Attributes["MaxLength"] != null && element.Attributes["MaxLength"].Value != String.Empty)
                {
                    field.MaxLength = int.Parse(element.Attributes["MaxLength"].Value);
                }
                if (element.Attributes["NumLines"] != null && element.Attributes["NumLines"].Value != String.Empty)
                {
                    field.NumLines = int.Parse(element.Attributes["NumLines"].Value);
                }

                if (element.Attributes["RichText"] != null && element.Attributes["RichText"].Value != String.Empty)
                {
                    field.RichText = bool.Parse(element.Attributes["RichText"].Value);
                }

                if (element.Attributes["RichTextMode"] != null && element.Attributes["RichTextMode"].Value != String.Empty)
                {
                    field.RichTextMode = element.Attributes["RichTextMode"].Value;
                }

                if (element.Attributes["Decimals"] != null && element.Attributes["Decimals"].Value != String.Empty)
                    field.Decimals = int.Parse(element.Attributes["Decimals"].Value);
                if (element.Attributes["Min"] != null && element.Attributes["Min"].Value != String.Empty)
                    field.Min = decimal.Parse(element.Attributes["Min"].Value);
                if (element.Attributes["Max"] != null && element.Attributes["Max"].Value != String.Empty)
                    field.Max = decimal.Parse(element.Attributes["Max"].Value);

                if (element.Attributes["Description"] != null && element.Attributes["Description"].Value != String.Empty)
                    field.Description = element.Attributes["Description"].Value;

                field.ID = new Guid(element.Attributes["ID"].Value);
                string authoringInfo = String.Empty;
                if (element.Attributes["AuthoringInfo"] != null)
                    authoringInfo = element.Attributes["AuthoringInfo"].Value;
                field.DisplayName = element.Attributes["DisplayName"].Value + " " + authoringInfo;
                field.Name = element.Attributes["Name"].Value;
                if (element.Attributes["Required"] != null)
                    field.Required = bool.Parse(element.Attributes["Required"].Value);
                if (element.Attributes["FromBaseType"] != null)
                    field.FromBaseType = bool.Parse(element.Attributes["FromBaseType"].Value);
                else
                    field.FromBaseType = false;
                if (element.Attributes["ReadOnly"] != null)
                    field.ReadOnly = bool.Parse(element.Attributes["ReadOnly"].Value);
                else
                    field.ReadOnly = false;
                if (element["Default"] != null)
                    field.DefaultValue = element["Default"].InnerText;

                if (element.Attributes["Mult"] == null || element.Attributes["Mult"].Value.ToString() == String.Empty)
                    field.Mult = false;
                else
                    field.Mult = bool.Parse(element.Attributes["Mult"].Value);

                FieldTypes fieldType;
                switch (type.ToLower())
                {
                    case "text":
                        fieldType = FieldTypes.Text;
                        break;
                    case "note":
                        fieldType = FieldTypes.Note;
                        break;
                    case "boolean":
                        fieldType = FieldTypes.Boolean;
                        break;
                    case "datetime":
                        fieldType = FieldTypes.DateTime;
                        break;
                    case "number":
                        fieldType = FieldTypes.Number;
                        break;
                    case "taxonomyfieldtype":
                        fieldType = FieldTypes.TaxonomyFieldType;
                        var xmlNsM = new XmlNamespaceManager(element.OwnerDocument.NameTable);
                        xmlNsM.AddNamespace("foo", "http://schemas.microsoft.com/sharepoint/soap/");
                        ((SPTaxonomyField)field).TermSetId = new Guid(element.SelectSingleNode("foo:Customization/foo:ArrayOfProperty/foo:Property[foo:Name='TermSetId']", xmlNsM)["Value"].InnerText);
                        ((SPTaxonomyField)field).SspId = new Guid(element.SelectSingleNode("foo:Customization/foo:ArrayOfProperty/foo:Property[foo:Name='SspId']", xmlNsM)["Value"].InnerText);
                        string lcidString = element.Attributes["ShowField"].Value.Replace("Term", string.Empty);
                        int lcid;
                        if (int.TryParse(lcidString, out lcid) == false)
                        {
                            lcid = 1033;
                        }
                        ((SPTaxonomyField)field).LCID = lcid;
                        //field.List = element.Attributes["List"].Value;
                        //field.ShowField = element.Attributes["ShowField"].Value;
                        break;
                    case "lookup":
                        fieldType = FieldTypes.Lookup;
                        field.List = element.Attributes["List"].Value;
                        field.ShowField = (element.Attributes["ShowField"] != null?element.Attributes["ShowField"].Value:"Title");
                        break;
                    case "lookupmulti":
                        fieldType = FieldTypes.Lookup;
                        field.List = element.Attributes["List"].Value;
                        field.ShowField = (element.Attributes["ShowField"] != null ? element.Attributes["ShowField"].Value : "Title");
                        field.Mult = true;
                        break;
                    case "choice":
                        fieldType = FieldTypes.Choice;
                        field.ChoiceItems = GetChoiceDataItems(element);
                        break;
                    case "multichoice":
                        fieldType = FieldTypes.Choice;
                        field.ChoiceItems = GetChoiceDataItems(element);
                        field.Mult = true;
                        break;
                    case "file":
                        fieldType = FieldTypes.File;
                        break;
                    case "url":
                        fieldType = FieldTypes.URL;
                        break;
                    default:
                        fieldType = FieldTypes.Unknown;
                        field.ReadOnly = true;
                        break;
                }
                field.Type = fieldType;
                if (field.ReadOnly == false || includeReadOnly == true)
                {
                    fields.Add(field);
                }
            }
            return fields;
        }
        

        private static Sobiens.Connectors.Entities.TermSet XmlNodeToTermSet(XmlElement termSetElement, bool includeDisabled, bool includeDeprecated)
        {
            Sobiens.Connectors.Entities.TermSet termSet = new Sobiens.Connectors.Entities.TermSet();
            try
            {
                foreach (XmlElement element in termSetElement.ChildNodes)
                {
                    if (element.Name == "TS") //TermSet
                    {
                        termSet.Name = element.Attributes["a11"].Value;
                        termSet.Id = new Guid(element.Attributes["a9"].Value);
                        termSet.Open = element.Attributes["a16"].Value == "true";
                        termSet.Enabled = element.Attributes["a17"].Value == "true";
                        termSet.Name = element.Attributes["a68"].Value;
                    }
                    else if (element.Name == "T")// Term
                    {
                        Sobiens.Connectors.Entities.Term term = XmlNodeToTerm(element, includeDisabled, includeDeprecated);
                        if (term != null) termSet.Terms.Add(term);
                    }
                }

            }
            catch (Exception e)
            {
                Logger.Error(e.ToString(), "Service");
            }

            return termSet;
        }

        private static Sobiens.Connectors.Entities.Term XmlNodeToTerm(XmlElement termElement, bool includeDisabled, bool includeDeprecated)
        {
            Sobiens.Connectors.Entities.Term term = new Sobiens.Connectors.Entities.Term();
            try
            {
                Logger.Info(termElement.OuterXml.ToString(), "term...");
                term.Id = new Guid(termElement.Attributes["a9"].Value);
                term.Deprecated = termElement.Attributes["a21"].Value == "true";

                foreach (XmlElement element in termElement)
                {

                    switch (element.Name)
                    {
                        case "LS":
                            XmlNode LSTL = element.FirstChild;
                            term.Name = LSTL.Attributes["a32"].Value;
                            term.Deprecated = false;//LSTL.Attributes["a31"].Value == "true";
                            break;
                        case "DS":
                            XmlNode DSTD = element.FirstChild;
                            term.Description = DSTD.Attributes["a11"].Value;
                            break;
                        case "TMS":
                            XmlNode TMSTM = element.FirstChild;
                            term.ParentTermSetID = new Guid(TMSTM.Attributes["a24"].Value);
                            term.ParentTermName = TMSTM.Attributes["a12"].Value;
                            term.ParentTermName = TMSTM.Attributes["a40"].Value;
                            term.Enabled = TMSTM.Attributes["a17"].Value == "true";
                            //term.Name = TMSTM.Attributes["a67"].Value;
                            term.TermPath = TMSTM.Attributes["a45"].Value;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString(), "Service");
            }

            if (!includeDisabled && !term.Enabled) return null;

            if (!includeDeprecated && term.Deprecated) return null;

            return term;

        }

        private static WorkflowData XmlNodeToWorkflowData(XmlNode templateDataElement)
        {
            WorkflowData workflowData = new WorkflowData();
            workflowData.TemplateData = XmlNodeToWorkflowTemplates(templateDataElement["TemplateData"]);
            if (templateDataElement["ActiveWorkflowsData"]["Workflows"] != null)
            {
                workflowData.ActiveWorkflowsData.Workflows = XmlNodeToWorkflows(templateDataElement["ActiveWorkflowsData"]["Workflows"], workflowData.TemplateData.WorkflowTemplates);
            }
            else
            {
                workflowData.ActiveWorkflowsData.Workflows = new Workflows();
            }
            return workflowData;
            //List<WorkflowTemplate> workflows = new List<WorkflowTemplate>();

            //foreach (XmlElement wfElement in templateDataElement["WorkflowTemplates"].ChildNodes)
            //{
            //    SPWorkflowTemplate workflow = new SPWorkflowTemplate();

            //    workflow.Name = wfElement.Attributes["Name"].Value;
            //    workflow.Description = wfElement.Attributes["Description"].Value;
            //    XmlElement workflowTemplateIdSet = wfElement["WorkflowTemplateIdSet"];
            //    workflow.BaseId = workflowTemplateIdSet.Attributes["BaseId"].Value;
            //    workflow.TemplateId = workflowTemplateIdSet.Attributes["TemplateId"].Value;
            //    workflow.AssociationData = wfElement["AssociationData"].InnerText;

            //    workflows.Add(workflow);
            //}
            //return workflows;
        }

        private static Workflows XmlNodeToWorkflows(XmlNode element, List<WorkflowTemplate> workflowTemplates)
        {
            Workflows workflows = new Workflows();

            foreach (XmlElement wfElement in element.ChildNodes)
            {
                Workflow workflow = new Workflow();

                workflow.StatusPageUrl = wfElement.Attributes["StatusPageUrl"].Value;
                workflow.Id = wfElement.Attributes["Id"].Value;
                workflow.TemplateId = wfElement.Attributes["TemplateId"].Value;
                workflow.ListId = wfElement.Attributes["ListId"].Value;
                workflow.SiteId = wfElement.Attributes["SiteId"].Value;
                workflow.WebId = wfElement.Attributes["WebId"].Value;
                workflow.ItemId = wfElement.Attributes["ItemId"].Value;
                workflow.ItemGUID = wfElement.Attributes["ItemGUID"].Value;
                workflow.TaskListId = wfElement.Attributes["TaskListId"].Value;
                workflow.AdminTaskListId = wfElement.Attributes["AdminTaskListId"].Value;
                workflow.Author = wfElement.Attributes["Author"].Value;
                workflow.InternalState = wfElement.Attributes["InternalState"].Value;

                if (wfElement.Attributes["Modified"] != null && string.IsNullOrEmpty(wfElement.Attributes["Modified"].InnerText) == false)
                {
                    workflow.Modified = convertISO8601ToDateTime1(wfElement.Attributes["Modified"].Value);
                }

                if (wfElement.Attributes["Modified"] != null && string.IsNullOrEmpty(wfElement.Attributes["Modified"].InnerText) == false)
                {
                    workflow.Created = convertISO8601ToDateTime1(wfElement.Attributes["Created"].Value);
                }

                WorkflowTemplate workflowTemplate = workflowTemplates.SingleOrDefault(t => t.TemplateId.Equals(workflow.TemplateId, StringComparison.InvariantCultureIgnoreCase));
                if (workflowTemplate != null)
                {
                    workflow.Name = workflowTemplate.Title;
                }

                switch (workflow.InternalState)
                {
                    case "2":
                        workflow.StatusTitle = Languages.Translate("In Progress");
                        workflow.ActivenessGroupName = Languages.Translate("Running Workflows");
                        break;
                    case "8":
                        workflow.StatusTitle = Languages.Translate("Cancelled");
                        workflow.ActivenessGroupName = Languages.Translate("Completed Workflows");
                        break;
                }


                workflows.Add(workflow);
            }
            return workflows;
        }

        private static TemplateData XmlNodeToWorkflowTemplates(XmlNode templateDataElement)
        {
            TemplateData templateData = new TemplateData();
            templateData.WorkflowTemplates = new List<WorkflowTemplate>();

            foreach (XmlElement wfElement in templateDataElement["WorkflowTemplates"].ChildNodes)
            {
                SPWorkflowTemplate workflow = new SPWorkflowTemplate();

                workflow.Name = wfElement.Attributes["Name"].Value;
                workflow.Description = wfElement.Attributes["Description"].Value;
                XmlElement workflowTemplateIdSet = wfElement["WorkflowTemplateIdSet"];
                workflow.BaseId = workflowTemplateIdSet.Attributes["BaseId"].Value;
                workflow.TemplateId = workflowTemplateIdSet.Attributes["TemplateId"].Value;
                workflow.AssociationData = wfElement["AssociationData"].InnerText;

                templateData.WorkflowTemplates.Add(workflow);
            }
            return templateData;
        }

        private static List<ChoiceDataItem> GetChoiceDataItems(XmlElement element)
        {
            List<ChoiceDataItem> choices = new List<ChoiceDataItem>();
            foreach (XmlNode node in element["CHOICES"].ChildNodes)
            {
                string value = node.InnerText;
                ChoiceDataItem choiceDataItem = new ChoiceDataItem(value, value);
                choices.Add(choiceDataItem);
            }
            return choices;
        }

        public string GetTermsByLabel(ISiteSetting siteSetting, string webUrl, string label, int lcid, int resultCollectionSize, string termIds, bool addIfNotFound)
        {
            try
            {
                SharePointTaxonomyWS.Taxonomywebservice ws = GetTaxonomyService(siteSetting, webUrl);
                string termSetIds = "<termId>" + termIds + "</termId>";// JD
                //string termSetIds = termIds;

                string result = ws.GetTermsByLabel(label, lcid, SharePointTaxonomyWS.StringMatchOption.StartsWith, resultCollectionSize, termSetIds, addIfNotFound);
                //result = ws.GetKeywordTermsByGuids(termSetIds, lcid);

                string message = string.Format("SharePointService GetTermsByLabel method returned label:{0} \n result:{1} ", label, result);
                Logger.Info(message, "Service");

                return result;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public string GetKeywordTermsByGuids(ISiteSetting siteSetting, string webUrl, int lcid, string termIds)
        {
            try
            {

                SharePointTaxonomyWS.Taxonomywebservice ws = GetTaxonomyService(siteSetting, webUrl);
                string termSetIds = "<termIds><termId>" + termIds + "</termId></termIds>";
                XNode xnTermSetIds = new XElement("termSetIds", new XElement("termSetId", new Guid(termIds)));
                string ssid = "<sspIds><sspId>fc46a9a8-8165-4fc4-9739-3f6e481252e0</sspId></sspIds>";
                string oldtimestamp = "<dateTimes><dateTime>0</dateTime></dateTimes>";
                //Always set version to 1
                string clientVersion = "<versions><version>1</version></versions>";
                string timeStamp = "";

                string result = ws.GetKeywordTermsByGuids(xnTermSetIds.ToString(SaveOptions.DisableFormatting), lcid);
                //string result = ws.GetTermSets(ssid, termSetIds, lcid, oldtimestamp, clientVersion,out timeStamp);
                //result = ws.GetKeywordTermsByGuids(termSetIds, lcid);

                string message = string.Format("SharePointService GetKeywordTermsByGuids method returned termSetIds:{0} \n lcid:{1} ", termSetIds, lcid);
                Logger.Info(message, "Service");

                return result;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        private TermStore GetDefaultSiteCollectionTermStore(ClientContext context) {
            var taxonomySession = TaxonomySession.GetTaxonomySession(context);
            TermStore termStore = null;
            try
            {
                context.Load(taxonomySession.TermStores);
                context.ExecuteQuery();
                termStore = taxonomySession.TermStores[0];// taxonomySession.GetDefaultSiteCollectionTermStore();
            }
            catch (Exception ex) {
                Logger.Info("Error:" + ex.Message + (ex.InnerException!=null?"-Inner exception:" + ex.InnerException.Message:string.Empty), "GetDefaultSiteCollectionTermStore");
                termStore = taxonomySession.TermStores[0];
            }

            return termStore;
        }

        public SPTermStore GetTermStore(ISiteSetting siteSetting)
        {
            SPTermStore termStore = new SPTermStore();

            ClientContext context = GetClientContext(siteSetting);
            var _termStore = GetDefaultSiteCollectionTermStore(context);
            //context.Load(_termStore);
            //context.ExecuteQuery();
            termStore.Title = _termStore.Name;
            termStore.ID = _termStore.Id;

            return termStore;
        }

        public List<SPTermGroup> GetTermGroups(ISiteSetting siteSetting)
        {
            List<SPTermGroup> _termGroups = new List<SPTermGroup>();

            ClientContext context = GetClientContext(siteSetting);
            var termStore = GetDefaultSiteCollectionTermStore(context);
            TermGroupCollection termGroups = termStore.Groups;
            context.Load(termGroups);
            context.ExecuteQuery();
            foreach (var termGroup in termGroups)
            {
                _termGroups.Add(new SPTermGroup(termGroup.Id, termGroup.Name, termGroup.IsSystemGroup, termGroup.IsSiteCollectionGroup));
            }

            return _termGroups;
        }

        public List<SPTerm> GetTermTerms(ISiteSetting siteSetting, Guid termId)
        {
            List<SPTerm> _terms = new List<SPTerm>();

            ClientContext context = GetClientContext(siteSetting);
            var termStore = GetDefaultSiteCollectionTermStore(context);
            Microsoft.SharePoint.Client.Taxonomy.Term term = termStore.GetTerm(termId);
            Microsoft.SharePoint.Client.Taxonomy.TermCollection terms = term.Terms;
            context.Load(term);
            context.Load(term.TermSet);
            context.Load(terms);
            context.ExecuteQuery();
            foreach (var _term in terms)
            {
                _terms.Add(new SPTerm(_term.Id, _term.Name, term.TermSet.Id, termId, 1033));
            }

            return _terms;
        }

        public List<SPTerm> GetTerms(ISiteSetting siteSetting, Guid termSetId)
        {
            List<SPTerm> _terms = new List<SPTerm>();

            ClientContext context = GetClientContext(siteSetting);
            var termStore = GetDefaultSiteCollectionTermStore(context);
            Microsoft.SharePoint.Client.Taxonomy.TermSet termSet = termStore.GetTermSet(termSetId);
            Microsoft.SharePoint.Client.Taxonomy.TermCollection terms = termSet.Terms;
            context.Load(terms);
            context.ExecuteQuery();
            foreach (var term in terms)
            {
                _terms.Add(new SPTerm(term.Id, term.Name, termSetId, null, 1033));
            }

            return _terms;
        }

        public SPTermGroup CreateTermGroup(ISiteSetting siteSetting, SPTermGroup termGroup)
        {
            ClientContext context = GetClientContext(siteSetting);
            var taxonomySession = TaxonomySession.GetTaxonomySession(context);
            var termStore = GetDefaultSiteCollectionTermStore(context);
            Guid termGroupGuid = termGroup.ID ;
            Microsoft.SharePoint.Client.Taxonomy.TermGroup _termGroup = termStore.CreateGroup(termGroup.Title, termGroupGuid);
            context.ExecuteQuery();
            context.Load(_termGroup );
            context.ExecuteQuery();

            return new SPTermGroup(_termGroup.Id, _termGroup.Name, _termGroup.IsSystemGroup, _termGroup.IsSiteCollectionGroup);
        }

        public SPTermSet CreateTermSet(ISiteSetting siteSetting, SPTermSet termSet)
        {
            ClientContext context = GetClientContext(siteSetting);
            var termStore = GetDefaultSiteCollectionTermStore(context);
            TermGroup termGroup = termStore.GetGroup(termSet.GroupID);
            context.Load(termGroup);
            context.ExecuteQuery();
            Guid termSetGuid = termSet.ID;
            Microsoft.SharePoint.Client.Taxonomy.TermSet _termSet = termGroup.CreateTermSet(termSet.Title, termSetGuid, termSet.LCID);
            context.ExecuteQuery();
            context.Load(_termSet);
            context.ExecuteQuery();


            return new SPTermSet(_termSet.Id, _termSet.Name, termSet.GroupID , termSet.LCID, _termSet.Names);
        }

        public SPTerm CreateTerm(ISiteSetting siteSetting, SPTerm term)
        {
            ClientContext context = GetClientContext(siteSetting);
            var termStore = GetDefaultSiteCollectionTermStore(context);
            Microsoft.SharePoint.Client.Taxonomy.Term _term = null;
            Guid termGuid = term.ID ;
            if (term.ParentTermID != null) {
                Microsoft.SharePoint.Client.Taxonomy.Term parentTerm = termStore.GetTerm(term.ParentTermID.Value);
                _term = parentTerm.CreateTerm(term.Title, term.LCID, termGuid);
            }
            else
            {
                Microsoft.SharePoint.Client.Taxonomy.TermSet termSet = termStore.GetTermSet(term.TermSetID);
                _term = termSet.CreateTerm(term.Title, term.LCID, termGuid);
            }

            context.ExecuteQuery();
            context.Load(_term);
            context.ExecuteQuery();

            return new SPTerm(_term.Id, _term.Name, term.TermSetID, term.ParentTermID, term.LCID);
        }

        public SPFolder CreateList(ISiteSetting siteSetting, string title, int templateType)
        {
            ClientContext clientContext = GetClientContext(siteSetting);
            ListCreationInformation creationInfo = new ListCreationInformation();
            creationInfo.Title = title;
            creationInfo.Description = title;
            creationInfo.TemplateType = (int)templateType;// ListTemplateType.GenericList;
            // Create a new custom list    

            List newList = clientContext.Web.Lists.Add(creationInfo);
            clientContext.Load(newList);
            clientContext.ExecuteQuery();

            SPFolder folder = new SPFolder(siteSetting.ID, newList.Id.ToString(), title);
            folder.ListName = title;
            return folder;
        }

        public List<SPTermSet> GetTermSets(ISiteSetting siteSetting, Guid termGroupId)
        {
            List<SPTermSet> _termSets = new List<SPTermSet>();

            ClientContext context = GetClientContext(siteSetting);
            var termStore = GetDefaultSiteCollectionTermStore(context);
            TermGroup termGroup = termStore.GetGroup(termGroupId);
            TermSetCollection termSets = termGroup.TermSets;

            context.Load(termGroup);
            context.Load(termSets);
            context.ExecuteQuery();
            foreach (var termSet in termSets)
            {
                _termSets.Add(new SPTermSet(termSet.Id, termSet.Name, termGroupId, 1033, termSet.Names));
            }

            return _termSets;
        }

        public SPTerm GetTerm(ISiteSetting siteSetting, Guid termId)
        {
            try
            {
                ClientContext context = GetClientContext(siteSetting);
                var termStore = GetDefaultSiteCollectionTermStore(context);
                Microsoft.SharePoint.Client.Taxonomy.Term term = termStore.GetTerm(termId);
                ClientResult<string> path = term.GetPath(1033);

                context.Load(term);
                context.Load(term.TermSet);
                context.Load(term.TermSet.Group);
                context.ExecuteQuery();
                string pathString = term.TermSet.Group.Name + ";" + term.TermSet.Name + ";" + path.Value;

                SPTerm _term = new SPTerm(term.Id, term.Name, term.TermSet.Id, null, 1033);
                _term.Path = pathString;
                return _term;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public void GetTermValuesByPath(ISiteSetting siteSetting, string path, out Guid termStoreId, out Guid groupId, out Guid termSetId, out Guid termId)
        {
            groupId = Guid.Empty;
            termSetId = Guid.Empty;
            termId = Guid.Empty;
            termStoreId = Guid.Empty;
            if (string.IsNullOrEmpty(path) == true)
                return;

            string[] pathValues = path.Split(new char[] { ';' });

            ClientContext context = GetClientContext(siteSetting);
            var termStore = GetDefaultSiteCollectionTermStore(context);
            termStoreId = termStore.Id;
            Microsoft.SharePoint.Client.Taxonomy.TermGroupCollection termGroups = termStore.Groups;
            context.Load(termGroups);
            context.ExecuteQuery();
            Microsoft.SharePoint.Client.Taxonomy.TermGroup termGroup = termGroups.Where(t=>t.Name.Equals(pathValues[0], StringComparison.InvariantCulture)).FirstOrDefault();
            if (termGroup != null)
            {
                groupId = termGroup.Id;
                if (pathValues.Length > 1) {
                    Microsoft.SharePoint.Client.Taxonomy.TermSetCollection termSets = termGroup.TermSets;
                    context.Load(termSets);
                    context.ExecuteQuery();
                    Microsoft.SharePoint.Client.Taxonomy.TermSet termSet = termSets.Where(t => t.Name.Equals(pathValues[1], StringComparison.InvariantCulture)).FirstOrDefault();
                    if (termSet != null)
                    {
                        termSetId = termSet.Id;
                        if (pathValues.Length > 2)
                        {
                            Microsoft.SharePoint.Client.Taxonomy.TermCollection terms = termSet.Terms;
                            context.Load(terms);
                            context.ExecuteQuery();
                            Microsoft.SharePoint.Client.Taxonomy.Term rootTerm = terms.Where(t => t.Name.Equals(pathValues[2], StringComparison.InvariantCulture)).FirstOrDefault();
                            if (rootTerm != null)
                            {
                                termId = rootTerm.Id;
                                Microsoft.SharePoint.Client.Taxonomy.Term tempTerm = rootTerm;

                                for (int i = 3; i < pathValues.Length; i++)
                                {
                                    Microsoft.SharePoint.Client.Taxonomy.TermCollection _terms = tempTerm.Terms;
                                    context.Load(_terms);
                                    context.ExecuteQuery();
                                    Microsoft.SharePoint.Client.Taxonomy.Term _tempTerm = _terms.Where(t => t.Name.Equals(pathValues[i], StringComparison.InvariantCulture)).FirstOrDefault();
                                    if(_tempTerm != null)
                                    {
                                        termId = _tempTerm.Id;
                                        tempTerm = _tempTerm;
                                    }
                                    else
                                    {
                                        termId = Guid.Empty;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public SPTermSet GetTermSet(ISiteSetting siteSetting, Guid termSetId)
        {
            try
            {
                ClientContext context = GetClientContext(siteSetting);
                var termStore = GetDefaultSiteCollectionTermStore(context);
                Microsoft.SharePoint.Client.Taxonomy.TermSet termSet = termStore.GetTermSet(termSetId);
                //ClientResult<string> path = termSet.p

                context.Load(termSet);
                context.Load(termSet.Group);
                context.ExecuteQuery();
                SPTermSet _termSet = new SPTermSet(termSet.Id, termSet.Name, termSet.Group.Id, 1033, termSet.Names);
                string pathString = termSet.Group.Name + ";" + termSet.Name;

                _termSet.Path = pathString;
                return _termSet;
/*
                SharePointTaxonomyWS.Taxonomywebservice ws = GetTaxonomyService(siteSetting, webUrl);
                string termSetIds = "<termIds><termId>" + termIds + "</termId></termIds>";
                string ssid = "<sspIds><sspId>" + sspIds + "</sspId></sspIds>";
                string oldtimestamp = "<dateTimes><dateTime>0</dateTime></dateTimes>";
                string clientVersion = "<versions><version>0</version></versions>";
                string timeStamp = "";

                string result = ws.GetTermSets(ssid, termSetIds, lcid, oldtimestamp, clientVersion, out timeStamp);
                //result = ws.GetKeywordTermsByGuids(termSetIds, lcid);

                string message = string.Format("SharePointService GetTermSets method returned\n sspIds:{2}\n termSetIds:{0} \n lcid:{1} ", termSetIds, lcid, sspIds);
                Logger.Info(message, "Service");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);

                return XmlNodeToTermSet((XmlElement)xmlDoc.SelectSingleNode("/Container/TermStore"), false, false);
                */
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static WorkflowData GetWorkflowDataForItem(ISiteSetting siteSetting, string webUrl, string itemUrl)
        {
            try
            {
                SharePointWorkflowWS.Workflow ws = GetWorkflowService(siteSetting, webUrl);
                XmlNode templateDataElement = ws.GetWorkflowDataForItem(itemUrl);
                string message = string.Format("SharePointService GetWorkflowDataForItem method returned itemUrl:{0} \n templateDataElement:{1} ", itemUrl, templateDataElement.OuterXml);
                Logger.Info(message, "Service");

                WorkflowData workflowData = XmlNodeToWorkflowData(templateDataElement);
                return workflowData;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static TemplateData GetTemplatesForItem(ISiteSetting siteSetting, string webUrl, string itemUrl)
        {
            try
            {
                SharePointWorkflowWS.Workflow ws = GetWorkflowService(siteSetting, webUrl);
                XmlNode templateDataElement = ws.GetTemplatesForItem(itemUrl);
                string message = string.Format("SharePointService GetTemplatesForItem method returned itemUrl:{0} \n templateDataElement:{1} ", itemUrl, templateDataElement.OuterXml);
                Logger.Info(message, "Service");

                TemplateData templateData = XmlNodeToWorkflowTemplates(templateDataElement);
                return templateData;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static void StartWorkflow(ISiteSetting siteSetting, string webUrl, string itemUrl, WorkflowTemplate workflow)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(workflow.AssociationData);
                SharePointWorkflowWS.Workflow ws = GetWorkflowService(siteSetting, webUrl);
                XmlNode workflowNode = ws.StartWorkflow(itemUrl, new Guid(workflow.TemplateId), xmlDoc.DocumentElement);
                string message = string.Format("SharePointService StartWorkflow method returned itemUrl:{0} \n workflowNode:{1} ", itemUrl, workflowNode != null ? workflowNode.OuterXml : string.Empty);
                Logger.Info(message, "Service");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                throw ex;
            }
        }


        public static void CreateFields(ISiteSetting siteSetting, string webUrl, string listName, List<Entities.Field> fields)
        {
            try
            {
                ClientContext context = GetClientContext(siteSetting);
                List list = context.Web.Lists.GetByTitle(listName);
                context.Load(list);
                context.ExecuteQuery();

                foreach (Entities.Field _field in fields)
                {
                    string fieldTypeString = "Text";
                    string defaultValueString = "";
                    if (string.IsNullOrEmpty(_field.DefaultValue) == false)
                        defaultValueString = "<Default>" + _field.DefaultValue + "</Default>";
                    string minString = "";
                    string maxString = "";
                    string choicesString = "";
                    string richTextString = "";
                    string richTextModeString = "";
                    string requiredString = " Required = '" + _field.Required.ToString().ToUpper() + "'";
                    string readOnlyString = " ReadOnly = '" + _field.Required.ToString().ToUpper() + "'";
                    string multiString = " Mult = '" + _field.Mult.ToString().ToUpper() + "'";
                    string formulaString = _field.Formula;
                    string resultTypeString = "";
                    string lookupListString = "";
                    string showFieldString = "";
                    string appendOnlyString = "";
                    string numLinesString = "";
                    if (_field.ChoiceItems != null && _field.ChoiceItems.Count > 0)
                    {
                        choicesString = "<CHOICES>";
                        foreach(ChoiceDataItem choiceDataItem in _field.ChoiceItems)
                        {
                            choicesString += "<CHOICE>" + choiceDataItem.Value + "</CHOICE>";
                        }
                        choicesString += "</CHOICES>";
                    }
                    switch (_field.Type)
                    {
                        case FieldTypes.Boolean:
                            fieldTypeString = "Boolean";
                            break;
                        case FieldTypes.Choice:
                            fieldTypeString = "Choice";
                            break;
                        case FieldTypes.Computed:
                            fieldTypeString = "";
                            break;
                        case FieldTypes.DateTime:
                            fieldTypeString = "DateTime";
                            break;
                        case FieldTypes.User:
                            fieldTypeString = "User";
                            break;
                        case FieldTypes.Currency:
                            fieldTypeString = "Currency";
                            break;
                        case FieldTypes.Calculated:
                            fieldTypeString = "Calculated";
                            resultTypeString = " ResultType = 'Text'";
                            break;
                        case FieldTypes.OutcomeChoice:
                            fieldTypeString = "OutcomeChoice";
                            break;
                        case FieldTypes.TaxonomyFieldType:
                            if (_field.Mult == true)
                                fieldTypeString = "TaxonomyFieldTypeMulti";
                            else
                                fieldTypeString = "TaxonomyFieldType";
                            break;
                        case FieldTypes.Lookup:
                            fieldTypeString = "Lookup";
                            SPList referenceList = new SharePointService().GetListByTitle(siteSetting, _field.List);
                            lookupListString = " List='" + referenceList.ID + "'";
                            showFieldString = " ShowField='" + _field.ShowField + "'";
                            break;
                        case FieldTypes.URL:
                            fieldTypeString = "URL";
                            break;
                        case FieldTypes.Note:
                            fieldTypeString = "Note";
                            richTextString = " RichText='" + (_field.RichText == true ? "TRUE" : "FALSE") + "'";
                            //richTextModeString = (_field.RichText == true ? " RichTextMode='" + _field.RichTextMode + "'":"");
                            richTextModeString = " RichTextMode='" + _field.RichTextMode + "'";
                            appendOnlyString = " AppendOnly = '" + _field.AppendOnly.ToString().ToUpper() + "'";
                            numLinesString = " NumLines = '" + _field.NumLines.ToString().ToUpper() + "'";
                            break;
                        case FieldTypes.Number:
                            fieldTypeString = "Number";
                            if (_field.Min != null)
                                minString = " Min='" + _field.Min + "'";
                            if (_field.Max != null)
                                maxString = " Max='" + _field.Max + "'";
                            break;
                    }
                    string schemaTextField = "<Field ID='" + Guid.NewGuid().ToString() + "' " +
                        " Type='" + fieldTypeString + "' Name='" + _field.Name + "' StaticName='" + _field.Name + "'" +
                        " DisplayName='" + _field.DisplayName + "' " + minString + " " + maxString + " " + richTextString +
                        " " + richTextModeString + " " + requiredString + " " + readOnlyString + resultTypeString + multiString +
                        lookupListString + showFieldString + numLinesString + appendOnlyString + ">" +
                        defaultValueString +
                        choicesString +
                        formulaString +
                        "</Field>";
                    Microsoft.SharePoint.Client.Field simpleField = list.Fields.AddFieldAsXml(schemaTextField, true, AddFieldOptions.AddToDefaultContentType);
                    if(_field.Type == FieldTypes.TaxonomyFieldType)
                    {
                        //var termStore = new SharePointService().GetDefaultSiteCollectionTermStore(context);
                        SPTaxonomyField _taxonomyField = (SPTaxonomyField)_field;
                        Guid termStoreId, groupId, termSetId, termId;

                        new SharePointService().GetTermValuesByPath(siteSetting, _taxonomyField.Path, out termStoreId, out groupId, out termSetId, out termId);

                        TaxonomyField taxonomyField = context.CastTo<TaxonomyField>(simpleField);
                        taxonomyField.SspId = termStoreId;
                        taxonomyField.TermSetId = termSetId;
                        taxonomyField.TargetTemplate = String.Empty;
                        taxonomyField.AnchorId = termId;
                        taxonomyField.Update();
                    }

                    context.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static Sobiens.Connectors.Entities.FieldCollection GetFields(ISiteSetting siteSetting, string webUrl, string listName)
        {
            try
            {
                ClientContext context = GetClientContext(siteSetting);
                Microsoft.SharePoint.Client.FieldCollection _fields = context.Web.Lists.GetByTitle(listName).Fields;
                context.Load(_fields);
                context.ExecuteQuery();
                Sobiens.Connectors.Entities.FieldCollection fields = ParseToFieldCollection(_fields, siteSetting, true);


                Logger.Info("webUrl:" + webUrl, "Service");
                Logger.Info("listName:" + webUrl, "listName");
//                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
  //              XmlNode web = ws.GetList(listName);
                //string message = string.Format("SharePointService GetFields method returned listName:{0} \n web:{1} ", listName, web.OuterXml);
//                Logger.Info(message, "Service");
    //            Sobiens.Connectors.Entities.FieldCollection fields = ParseToFieldCollection(web["Fields"], true);

                return fields;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Service");
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="siteSetting">The site setting.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rootFolderPath">The root folder path.</param>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="webURL">The web URL.</param>
        /// <param name="copySource">The copy source.</param>
        /// <param name="copyDest">The copy dest.</param>
        /// <param name="myByteArray">My byte array.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="listItem">The list item.</param>
        /// <returns>null on failure.</returns>
        public static uint? UploadFile(ISiteSetting siteSetting, string listName, string rootFolderPath, string siteURL, string webURL, string copySource, string[] copyDest, byte[] myByteArray, System.Collections.Generic.Dictionary<object, object> fields, Sobiens.Connectors.Entities.ContentType contentType, out SPListItem listItem)
        {
            try
            {
                /*LogManager.Log("UploadFile started with;"
                    + Environment.NewLine + " ListName:" + listName
                    + Environment.NewLine + " WebURL:" + webURL
                     + Environment.NewLine + " CopySource:" + copySource
                     + Environment.NewLine + " copyDest:" + copyDest[0]
                    , EULogModes.Detailed);*/
                listItem = null;
                SharePointCopyWS.CopyResult[] myCopyResultArray = null;

                SharePointCopyWS.Copy myCopyService = new SharePointCopyWS.Copy();
                SetCredentials(myCopyService, siteSetting);
                myCopyService.Url = webURL + "/_vti_bin/copy.asmx";

                SharePointCopyWS.FieldInformation[] myFieldInfoArray = null;
                myFieldInfoArray = GetFieldInformations(fields);

                SharePointCopyWS.CopyResult myCopyResult1 = new SharePointCopyWS.CopyResult();
                SharePointCopyWS.CopyResult myCopyResult2 = new SharePointCopyWS.CopyResult();
                SharePointCopyWS.CopyResult[] myCopyResultArray1 = { myCopyResult1, myCopyResult2 };

                uint myCopyUint = myCopyService.CopyIntoItems(copySource, copyDest, myFieldInfoArray, myByteArray, out myCopyResultArray1);
                myCopyResultArray = myCopyResultArray1;
                string resultLog = String.Empty;
                foreach (CopyResult copyResult in myCopyResultArray1)
                {
                    resultLog += " DestinationUrl:" + copyResult.DestinationUrl
                        + Environment.NewLine + " ErrorCode:" + copyResult.ErrorCode
                        + Environment.NewLine + " ErrorMessage:" + copyResult.ErrorMessage;
                }
                string message = string.Format("SharePointService UploadFile method returned resultLog:{0} ", resultLog);
                Logger.Info(message, "Service");
                //LogManager.Log("CopyIntoItems for " + copyDest[0] + ":" + Environment.NewLine + resultLog, EULogModes.Normal);

                string temp1 = webURL.CombineUrl(rootFolderPath).TrimEnd(new char[] { '/' });
                string temp = copyDest[0].absoluteTorelative(temp1);//.Replace(webURL + rootFolderPath, String.Empty);//JD
                string folderName = temp.Substring(0, temp.LastIndexOf("/"));
                if (folderName.StartsWith("/") == false)
                    folderName = "/" + folderName;
                string path = copyDest[0]; //.Replace(webURL, String.Empty); 
                if (fields != null && myFieldInfoArray != null)
                {
                    if (fields.Count != myFieldInfoArray.Count())
                        fields = getFieldInfosSubset(fields, myFieldInfoArray);
                    bool needUpdate = false;
                    if (contentType != null && contentType.Name != "Document" && contentType.Name != "Item")
                    {
                        needUpdate = true;
                    }
                    else
                    {
                        for (int i = 0; i < myFieldInfoArray.Count(); i++)
                        {
                            if (myFieldInfoArray[i] != null && (myFieldInfoArray[i].Type == SharePointCopyWS.FieldType.Choice || myFieldInfoArray[i].Type == SharePointCopyWS.FieldType.Lookup))
                            {
                                needUpdate = true;
                                break;
                            }
                        }
                    }

                    if (needUpdate == true)
                    {
                        //LogManager.Log("Need update for " + copyDest[0], EULogModes.Normal);

                        Hashtable changedProperties = getChangedProperties(contentType, fields);

                        listItem = SharePointService.GetFileListItem(siteSetting, siteURL, webURL, listName, folderName, path);
                        if (listItem == null)
                        {
                            //LogManager.Log("Could not find Item for " + copyDest[0], EULogModes.Normal);
                            return myCopyUint;
                        }
                        else
                        {
                            //LogManager.Log("Found ItemID for " + copyDest[0] + ":" + listItem.ID, EULogModes.Normal);
                            SharePointService.UpdateListItem(siteSetting, listItem.WebURL, listItem.ListName, listItem.ID, changedProperties, false);
                            //LogManager.Log("Updated Item for " + copyDest[0] + ":" + listItem.ID, EULogModes.Normal);
                        }
                    }
                }
                listItem = GetFileListItem(siteSetting, siteURL, webURL, listName, folderName, path);
                return myCopyUint;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                listItem = null;
                throw ex;
            }
        }

        public static Hashtable getChangedProperties(Sobiens.Connectors.Entities.ContentType contentType, System.Collections.Generic.Dictionary<object, object> fields)
        {
            Hashtable changedProperties = new Hashtable();

            if (contentType != null)
                changedProperties.Add("ContentType", contentType.Name);

            foreach (object entry in fields.Keys)
            {
                Sobiens.Connectors.Entities.Field field = (Sobiens.Connectors.Entities.Field)entry;
                Sobiens.Connectors.Services.SharePoint.SharePointCopyWS.FieldType fieldType = getFieldTypesForWS(field.Type);
                object value = fields[entry];

                if (field == null)
                {
                    continue;
                }
                else if (fieldType == Sobiens.Connectors.Services.SharePoint.SharePointCopyWS.FieldType.DateTime)
                {
                    string date = null;
                    if (value != null)
                    {
                        date = convertToISO8601((DateTime)value);
                    }
                    changedProperties.Add(field.Name, date);
                }
                else if (fieldType == Sobiens.Connectors.Services.SharePoint.SharePointCopyWS.FieldType.Boolean)
                {
                    changedProperties.Add(field.Name, ((bool)value) == true ? "True" : "False");
                }
                else
                {
                    changedProperties.Add(//JD
                        field.Type == FieldTypes.TaxonomyFieldType ? field.attachedField : field.Name,
                        value);
                }
            }
            return changedProperties;
        }

        public static int? UploadListItemWithAttachment(ISiteSetting siteSetting, string listName, string rootFolderPath, UploadItem uploadItem, string webURL)
        {
            string log = String.Empty;
            System.Collections.Generic.Dictionary<object, object> fields = uploadItem.FieldInformations;
            try
            {
                int id = SharePointService.CreateListItem(siteSetting, rootFolderPath, webURL, listName, fields);

                foreach (UploadItem emailUploadFile in uploadItem.Attachments)
                {
                    string filePath = emailUploadFile.FilePath;
                    string newFilename = new FileInfo(filePath).Name;
                    byte[] itemByteArray = SharePointService.ReadByteArrayFromFile(filePath);
                    string result = SharePointService.AddAttachment(siteSetting, webURL, listName, id, newFilename, itemByteArray);

                    //log += "WebURL:" + uploadItem.Folder.WebUrl + Environment.NewLine + "ListName:" + uploadItem.Folder.ListName + Environment.NewLine + "filePath:" + filePath + Environment.NewLine + "newFilename:" + newFilename + Environment.NewLine + "result:" + result + Environment.NewLine;
                    //LogManager.Log(log, EULogModes.Normal);
                }

                return id;
            }
            catch (Exception ex)
            {
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static int CreateListItem(ISiteSetting siteSetting, string rootFolderPath, string webUrl, string listName, System.Collections.Generic.Dictionary<object, object> fields)
        {
            try
            {
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                FieldInformation[] fieldInformations = GetFieldInformations(fields);

                string xml = @"<Method ID='1' Cmd='New'><Field Name='ID'>New</Field>";

                foreach (FieldInformation field in fieldInformations)
                {
                    string name = field.InternalName;
                    string value = field.Value;
                    xml += "<Field Name='" + (string)name + "'><![CDATA[" + value + @"]]></Field>";
                }
                xml += "</Method>";

                XmlDocument xmlDoc = new System.Xml.XmlDocument();
                System.Xml.XmlElement elBatch = xmlDoc.CreateElement("Batch");
                elBatch.SetAttribute("OnError", "Continue");
                elBatch.SetAttribute("ListVersion", "1");
                //elBatch.SetAttribute("ViewName", "0d7fcacd-1d7c-45bc-bcfc-6d7f7d2eeb40");
                elBatch.InnerXml = xml;

                XmlNode returnNode = ws.UpdateListItems(listName, elBatch);
                string message = string.Format("SharePointService UploadFile method returned listName:{0} \n elBatch:{1} \n returnNode:{2} ", listName, elBatch.OuterXml, returnNode.OuterXml);
                Logger.Info(message, "Service");
                //string IDString = returnNode.ChildNodes[0].Attributes["ID"].Value;
                int id = int.Parse(returnNode.ChildNodes[0].ChildNodes[2].Attributes["ows_ID"].Value);
                return id;
            }
            catch (Exception ex)
            {
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        private static FieldInformation[] GetFieldInformations(System.Collections.Generic.Dictionary<object, object> fields)
        {
            List<SharePointCopyWS.FieldInformation> fieldInformations = new List<SharePointCopyWS.FieldInformation>();
            try
            {
                Hashtable returnHashTable = new Hashtable();
                foreach (object entry in fields.Keys)
                {
                    Sobiens.Connectors.Entities.Field field = (Sobiens.Connectors.Entities.Field)entry;
                    object value = fields[entry];

                    if (field == null || value == null)
                        continue;

                    SharePointCopyWS.FieldInformation myFieldInfo = new SharePointCopyWS.FieldInformation();
                    myFieldInfo.Id = field.ID;
                    myFieldInfo.InternalName = field.Name;
                    myFieldInfo.DisplayName = field.DisplayName;
                    string stringValue = string.Empty;
                    switch (field.Type)
                    {
                        case FieldTypes.Note:
                            myFieldInfo.Type = SharePointCopyWS.FieldType.Note;
                            stringValue = value.ToString();
                            value = (stringValue.Length > field.MaxLength ? stringValue.Substring(0, field.MaxLength) : stringValue);
                            break;
                        case FieldTypes.Text:
                            myFieldInfo.Type = SharePointCopyWS.FieldType.Text;
                            stringValue = value.ToString();
                            value = (stringValue.Length > field.MaxLength ? stringValue.Substring(0, field.MaxLength) : stringValue);
                            break;
                        case FieldTypes.DateTime:
                            myFieldInfo.Type = SharePointCopyWS.FieldType.DateTime;
                            stringValue = convertToISO8601((DateTime)value);
                            break;
                        case FieldTypes.Boolean:
                            myFieldInfo.Type = SharePointCopyWS.FieldType.Boolean;
                            stringValue = ((bool)value) ? "True" : "False";
                            break;
                        case FieldTypes.Choice:
                            myFieldInfo.Type = SharePointCopyWS.FieldType.Choice;
                            stringValue = value.ToString();
                            break;
                    }

                    myFieldInfo.Value = stringValue;

                    fieldInformations.Add(myFieldInfo);
                }
            }
            catch (Exception ex)
            {
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
            return fieldInformations.ToArray();
        }

        private static Sobiens.Connectors.Services.SharePoint.SharePointCopyWS.FieldType getFieldTypesForWS(FieldTypes fieldType)
        {
            switch (fieldType)
            {
                case FieldTypes.Note:
                    return SharePointCopyWS.FieldType.Note;
                case FieldTypes.Text:
                    return SharePointCopyWS.FieldType.Text;
                case FieldTypes.DateTime:
                    return SharePointCopyWS.FieldType.DateTime;
                case FieldTypes.Boolean:
                    return SharePointCopyWS.FieldType.Boolean;
                case FieldTypes.Choice:
                    return SharePointCopyWS.FieldType.Choice;
                case FieldTypes.Number:
                    return SharePointCopyWS.FieldType.Number;
                case FieldTypes.Lookup:
                    return SharePointCopyWS.FieldType.Lookup;
                case FieldTypes.File:
                    return SharePointCopyWS.FieldType.File;
                case FieldTypes.TaxonomyFieldType://JD
                    return SharePointCopyWS.FieldType.Guid;
                default:
                    throw new Exception("Unknown Type in getFieldTypesForWS");
            }
        }

        private static System.Collections.Generic.Dictionary<object, object> getFieldInfosSubset(System.Collections.Generic.Dictionary<object, object> fields, FieldInformation[] myFieldInfoArray)
        {
            System.Collections.Generic.Dictionary<object, object> returnHashTable = new System.Collections.Generic.Dictionary<object, object>();
            foreach (object entry in fields.Keys)
            {
                Sobiens.Connectors.Entities.Field field = (Sobiens.Connectors.Entities.Field)entry;
                object value = fields[entry];
                foreach (FieldInformation myFieldInfo in myFieldInfoArray)
                {
                    if (field.Name == myFieldInfo.InternalName)
                        returnHashTable.Add(field, value);
                }
            }
            return returnHashTable;
        }

        public static SPListItem GetFileListItem(ISiteSetting siteSetting, string siteUrl, string webUrl, string listName, string folderName, string fileURL)
        {
            try
            {
                fileURL = fileURL.absoluteTorelative(siteUrl);//.Replace(siteUrl, String.Empty);//JD
                /*
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                XmlDocument xmlDoc = new XmlDocument();

                XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                XmlNode viewFields = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
                XmlNode queryOptions = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

                queryOptions.InnerXml = "<ViewAttributes Scope=\"Recursive\" /><IncludeMandatoryColumns>True</IncludeMandatoryColumns>";

                XmlNode dateInUtcNode = xmlDoc.CreateNode(XmlNodeType.Element, "DateInUtc", String.Empty);
                dateInUtcNode.InnerText = "TRUE";
                queryOptions.AppendChild(dateInUtcNode);
                */
                /*
                if (folderName != "/")
                {
                    XmlNode folderNode = xmlDoc.CreateNode(XmlNodeType.Element, "Folder", String.Empty);
                    folderNode.InnerText = folderName;
                    queryOptions.AppendChild(folderNode);
                }
                 */
                //viewFields.InnerXml = "";
                CamlFilters filters = new CamlFilters();
                filters.Add(@"<Contains>
                                 <FieldRef Name='FileRef' />
                                 <Value Type='Lookup' >" + fileURL.TrimStart(new char[] { '/' }) + @"</Value>
                                </Contains>");

//                query.InnerXml = "<Where>" + SPCamlManager.GetCamlString(filters) + "</Where>";
//                query.InnerXml = SPCamlManager.GetCamlString(filters);

                string listItemCollectionPositionNext = string.Empty;
                int itemCount = 0;
                List<IItem> _items = new SharePointService().GetListItems(siteSetting, new List<CamlOrderBy>(), filters, new List<CamlFieldRef>(), new CamlQueryOptions(), webUrl, listName, out listItemCollectionPositionNext, out itemCount);
                if (_items.Count > 0)
                    return (SPListItem)_items[0];

                return null;
                /*
                    XmlNode ndListItems = ws.GetListItems(listName, null, query, viewFields, null, queryOptions, null);
                string message = string.Format("SharePointService GetFileListItem method returned ListName:{0} queryOptions:{1} \n query:{2} \n xml:{3}", listName, queryOptions.OuterXml, query.OuterXml, ndListItems.OuterXml);
                Logger.Info(message, "Service");

                xmlDoc.LoadXml(ndListItems.OuterXml);

                XmlNodeList _items = xmlDoc.GetElementsByTagName("z:row");
                string titleFieldName = "LinkFilename";
                foreach (XmlNode item in _items)
                {
                    SPListItem listItem = new SPListItem(siteSetting.ID);
                    listItem.WebURL = webUrl;
                    listItem.ListName = listName;
                    //listItem.FolderPath = folderName;
                    listItem.ID = int.Parse(item.Attributes["ows_ID"].Value);
                    listItem.Title = item.Attributes["ows_" + titleFieldName].Value;
                    if (item.Attributes["ows_CheckoutUser"] != null)
                        listItem.CheckoutUser = item.Attributes["ows_CheckoutUser"].Value;

                    listItem.URL = item.Attributes["ows_EncodedAbsUrl"].Value;
                    foreach (XmlAttribute attribute in item.Attributes)
                    {
                        listItem.Properties.Add(attribute.Name, attribute.Value);
                    }
                    return listItem;
                }
                return null;
                */
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        /*
        private static Sobiens.Connectors.Entities.FieldCollection getContentTypeFieldCollection(ClientContext context, string listName, string contentTypeID, bool includeReadOnly)
        {
            Microsoft.SharePoint.Client.ContentTypeCollection _contentTypes = context.Web.Lists.GetByTitle(listName).ContentTypes;
            context.Load(_contentTypes);
            context.ExecuteQuery();

            XmlNode web = ws.GetListContentType(listName, contentTypeID);
            return XmlNodeToFieldCollection(web["Fields"], includeReadOnly);
        }
        */
        public static List<Workflow> GetWorkflows(ISiteSetting siteSetting, string listName)
        {
            try
            {
                List<Workflow> workflows = new List<Workflow>();
                ClientContext sourceContext = GetClientContext(siteSetting);
                var sourceWorkflowServicesManager = new WorkflowServicesManager(sourceContext, sourceContext.Web);
                List list = sourceContext.Web.Lists.GetByTitle(listName);
                sourceContext.Load(list);
                sourceContext.ExecuteQuery();

                var workflowSubscriptionService = sourceWorkflowServicesManager.GetWorkflowSubscriptionService();

                // get all workflow associations
                var workflowAssociations = workflowSubscriptionService.EnumerateSubscriptionsByList(list.Id);
                sourceContext.Load(workflowAssociations);
                sourceContext.ExecuteQuery();

                foreach(WorkflowSubscription subscription in workflowAssociations)
                {
                    Workflow wf = new Workflow();
                    wf.Id = subscription.DefinitionId.ToString();
                    wf.Name = subscription.Name;
                    wf.SiteSettingID = siteSetting.ID;
                    workflows.Add(wf);
                }

                return workflows;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static List<Sobiens.Connectors.Entities.ContentType> GetContentTypes(ISiteSetting siteSetting, string webUrl, string rootFolderPath, string listName, bool includeReadOnly)
        {
            try
            {
                List<Sobiens.Connectors.Entities.ContentType> contentTypes = new List<Sobiens.Connectors.Entities.ContentType>();
                ClientContext context = GetClientContext(siteSetting);
                Microsoft.SharePoint.Client.ContentTypeCollection _contentTypes = context.Web.Lists.GetByTitle(listName).ContentTypes;
                context.Load(_contentTypes);
                context.ExecuteQuery();
                foreach (Microsoft.SharePoint.Client.ContentType _contentType in _contentTypes)
                {
                    Sobiens.Connectors.Entities.ContentType contentType = ParseContentType(_contentType, siteSetting, webUrl, rootFolderPath, listName, includeReadOnly);
                    context.Load(_contentType.Fields);
                    context.ExecuteQuery();
                    contentType.Fields = ParseToFieldCollection(_contentType.Fields, siteSetting, includeReadOnly);
                    if (contentType.Fields != null && contentType.Fields.Count > 0)
                    {
                        contentTypes.Add(contentType);
                    }
                }

                return contentTypes;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }
        public static List<Sobiens.Connectors.Entities.ContentType> GetContentTypes(ISiteSetting siteSetting, string webUrl, string rootFolderPath, bool includeReadOnly)
        {
            try
            {
                List<Sobiens.Connectors.Entities.ContentType> contentTypes = new List<Sobiens.Connectors.Entities.ContentType>();
                ClientContext context = GetClientContext(siteSetting);
                Microsoft.SharePoint.Client.ContentTypeCollection _contentTypes = context.Web.ContentTypes;
                context.Load(_contentTypes);
                context.ExecuteQuery();
                foreach (Microsoft.SharePoint.Client.ContentType _contentType in _contentTypes)
                {
                    Sobiens.Connectors.Entities.ContentType contentType = ParseContentType(_contentType, siteSetting, webUrl, rootFolderPath, string.Empty, includeReadOnly);
                    context.Load(_contentType.Fields);
                    context.ExecuteQuery();
                    contentType.Fields = ParseToFieldCollection(_contentType.Fields, siteSetting, includeReadOnly);
                    if (contentType.Fields != null && contentType.Fields.Count > 0)
                    {
                        contentTypes.Add(contentType);
                    }
                }

                return contentTypes;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static Sobiens.Connectors.Entities.ContentType GetContentType(ISiteSetting siteSetting, string webUrl, string rootFolderPath, string listName, string contentTypeID, bool includeReadOnly)
        {
            try
            {
                List<Sobiens.Connectors.Entities.ContentType> contentTypes = new List<Sobiens.Connectors.Entities.ContentType>();
                ClientContext context = GetClientContext(siteSetting);
                Microsoft.SharePoint.Client.ContentType _contentType = context.Web.Lists.GetByTitle(listName).ContentTypes.GetById(contentTypeID);
                context.Load(_contentType);
                context.ExecuteQuery();

                Sobiens.Connectors.Entities.ContentType contentType = ParseContentType(_contentType, siteSetting, webUrl, rootFolderPath, string.Empty, includeReadOnly);
                context.Load(_contentType.Fields);
                context.ExecuteQuery();
                contentType.Fields = ParseToFieldCollection(_contentType.Fields, siteSetting, includeReadOnly);
                return contentType;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }
        public static Sobiens.Connectors.Entities.ContentType ParseContentType(Microsoft.SharePoint.Client.ContentType _contentType, ISiteSetting siteSetting, string webUrl, string rootFolderPath, string listName, bool includeReadOnly)
        {
            Sobiens.Connectors.Entities.ContentType contentType = new Sobiens.Connectors.Entities.ContentType();
            contentType.ID = _contentType.Id.StringValue;// element.Attributes["ID"].Value;
            contentType.Name = _contentType.Name;
            contentType.Description = _contentType.Description;
            contentType.Group = _contentType.Group;

            //contentType.Version = _contentType.
            //<DocumentTemplate TargetName="Forms/Memo/TS101992348.dotx"/>
            //if (element["DocumentTemplate"] != null)
            //{
            //    if (element["DocumentTemplate"].Attributes["TargetName"] != null)
            //    {
            contentType.TemplateURL = _contentType.DocumentTemplateUrl; // webUrl + rootFolderPath + "/" + element["DocumentTemplate"].Attributes["TargetName"].Value;
            //    }
            //}
            contentType.Fields = ParseToFieldCollection(_contentType.Fields, siteSetting, includeReadOnly);//

            return contentType;
        }

        /*
        public static Sobiens.Connectors.Entities.ContentType ParseContentType(XmlNode element, ISiteSetting siteSetting, string webUrl, string rootFolderPath, string listName, bool includeReadOnly)
        {
            SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);

            Sobiens.Connectors.Entities.ContentType contentType = new Sobiens.Connectors.Entities.ContentType();
            contentType.ID = element.Attributes["ID"].Value;
            contentType.Name = element.Attributes["Name"].Value;
            if (element.Attributes["Description"] != null)
            {
                contentType.Description = element.Attributes["Description"].Value;
            }
            if (element.Attributes["Group"] != null)
            {
                contentType.Group = element.Attributes["Group"].Value;
            }
            if (element.Attributes["Version"] != null)
            {
                contentType.Version = element.Attributes["Version"].Value;
            }
            //<DocumentTemplate TargetName="Forms/Memo/TS101992348.dotx"/>
            if (element["DocumentTemplate"] != null)
            {
                if (element["DocumentTemplate"].Attributes["TargetName"] != null)
                {
                    contentType.TemplateURL = webUrl + rootFolderPath + "/" + element["DocumentTemplate"].Attributes["TargetName"].Value;
                }
            }
            contentType.Fields = getContentTypeFieldCollection(ws, listName, contentType.ID, includeReadOnly);//

            return contentType;
        }
        */

        public string GetUrlContent(ISiteSetting siteSetting, string url)
        {
            HttpWebRequest request;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                if (siteSetting.UseDefaultCredential == true)
                {
                    request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                }
                else
                {
                    string domainName = siteSetting.Username.Split(new char[] { '\\' })[0];
                    string userName = siteSetting.Username.Split(new char[] { '\\' })[1];

                    request.Credentials = new System.Net.NetworkCredential(userName, siteSetting.Password, domainName);
                }
                request.Timeout = 10000;
                request.AllowWriteStreamBuffering = false;
                response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                string content = sr.ReadToEnd();

                s.Close();
                response.Close();

                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public void DownLoadFile(ISiteSetting siteSetting, string fileUrl, string saveFilePath)
        {
            HttpWebRequest request;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(fileUrl);
                if (siteSetting.UseClaimAuthentication == true)
                {
                    request.CookieContainer = GetCookieContainer(siteSetting.Url, siteSetting.Username, siteSetting.Password);
                }
                else if (siteSetting.UseDefaultCredential == true)
                {
                    request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                }
                else
                {
                    string domainName = siteSetting.Username.Split(new char[] { '\\' })[0];
                    string userName = siteSetting.Username.Split(new char[] { '\\' })[1];
                    request.Credentials = new System.Net.NetworkCredential(userName, siteSetting.Password, domainName);

                }
                //request.ContentType = "application/json";// "application/x-www-form-urlencoded";//"text/xml";// "application/x-www-form-urlencoded";
                request.ContentType = "application/x-www-form-urlencoded";//"text/xml";// "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                //request.ContentLength = byteArray.Length;
                request.ProtocolVersion = HttpVersion.Version11;
                request.Timeout = 10000;
                request.KeepAlive = false;
                request.AllowWriteStreamBuffering = false;
                response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();

                //Write to disk
                FileStream fs = new FileStream(saveFilePath, FileMode.Create);
                byte[] read = new byte[256];
                int count = s.Read(read, 0, read.Length);
                while (count > 0)
                {
                    fs.Write(read, 0, count);
                    count = s.Read(read, 0, read.Length);
                }

                //Close everything
                fs.Close();
                s.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private List<ItemVersion> GetListItemVersionsFromXmlElement(ISiteSetting siteSetting, string webUrl, XmlNode elements)
        {
            List<ItemVersion> versions = new List<ItemVersion>();
            foreach (XmlElement element in elements)
            {
                if (element.Name != "result")
                    continue;
                ItemVersion version = new ItemVersion();
                version.Version = element.Attributes["version"].Value;
                version.SiteSettingID = siteSetting.ID;
                version.URL = element.Attributes["url"].Value;
                version.Size = element.Attributes["size"].Value;
                version.CreatedBy = element.Attributes["createdBy"].Value;
                version.Created = element.Attributes["created"].Value;
                version.Comments = element.Attributes["comments"].Value;
                version.CreatedByName = element.Attributes["createdByName"].Value;
                version.CreatedRaw = element.Attributes["createdRaw"].Value;
                version.WebURL = webUrl;
                versions.Add(version);
            }
            return versions;
        }

        public List<ItemVersion> GetListItemVersions(ISiteSetting siteSetting, string webUrl, string fileURL)
        {
            try
            {
                List<ItemVersion> versions = null;
                SharePointVersionsWS.Versions ws = GetVersionsWebService(siteSetting, webUrl);
                XmlNode elements = ws.GetVersions(fileURL);
                string message = string.Format("SharePointService GetListItemVersions method returned fileURL:{0} \n elements:{1} ", fileURL, elements.OuterXml);
                Logger.Info(message, "Service");
                versions = GetListItemVersionsFromXmlElement(siteSetting, webUrl, elements);
                return versions;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                //throw ex;
                return new List<ItemVersion>();
            }
        }

        public List<ItemVersion> RestoreVersion(ISiteSetting siteSetting, ItemVersion itemVersion)
        {
            try
            {
                string webUrl = itemVersion.WebURL;
                string fileURL = itemVersion.URL;
                string versionString = itemVersion.Version;

                SharePointVersionsWS.Versions ws = GetVersionsWebService(siteSetting, webUrl);
                XmlNode elements = ws.RestoreVersion(fileURL, versionString);
                string message = string.Format("SharePointService RestoreVersion method returned fileURL:{0} \n versionString:{1} elements:{2} ", fileURL, versionString, elements.OuterXml);
                Logger.Info(message, "Service");
                return GetListItemVersionsFromXmlElement(siteSetting, webUrl, elements);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public Result CheckInFile(ISiteSetting siteSetting, string webUrl, string pageURL, string comment, CheckinTypes checkinType)
        {
            try
            {
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                ws.UnsafeAuthenticatedConnectionSharing = true;
                bool result = ws.CheckInFile(pageURL, comment, ((int)checkinType).ToString());
                string message = string.Format("SharePointService CheckInFile method returned pageURL:{0} \n comment:{1} \n checkinType:{2} \n result:{3}", pageURL, comment, checkinType, result);
                Logger.Info(message, "Service");

                return new Result(result);
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                return new SPResult(e);
            }
        }

        public Result CheckOutFile(ISiteSetting siteSetting, string webUrl, string pageURL)
        {
            try
            {
            SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
            bool result = ws.CheckOutFile(pageURL, "false", String.Empty);
            string message = string.Format("SharePointService CheckOutFile method returned pageURL:{0} \n result:{1}", pageURL, result);
            Logger.Info(message, "Service");
            return new Result(result);
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                return new SPResult(ex);
            }
        }

        public Result UndoCheckOutFile(ISiteSetting siteSetting, string webUrl, string pageURL)
        {
            try
            {
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                bool result = ws.UndoCheckOut(pageURL);
                string message = string.Format("SharePointService UndoCheckOutFile method returned pageURL:{0} \n result:{1}", pageURL, result);
                Logger.Info(message, "Service");
                return new Result(result);
            }

            catch (System.Web.Services.Protocols.SoapException Ex)
            {
                return new SPResult(Ex);
            }

        }

        public bool CheckFileExistency(ISiteSetting siteSetting, string webUrl, string listName, string folderName, int? listItemID, string fileName)
        {
            try
            {
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);

                XmlDocument xmlDoc = new XmlDocument();

                XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                XmlNode viewFields = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
                XmlNode queryOptions = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

                queryOptions.InnerXml = @"<IncludeMandatoryColumns>TRUE</IncludeMandatoryColumns>
                          <DateInUtc>TRUE</DateInUtc>
                          <Folder><![CDATA[" + folderName + "]]></Folder>";
                viewFields.InnerXml = "";
                string queryString = @"<And>
                                    <Eq>
                                     <FieldRef Name='FSObjType' />
                                     <Value Type='Lookup'>0</Value>
                                    </Eq>
                                     <Eq>
                                      <FieldRef Name='FileLeafRef' />
                                      <Value Type='Text'>" + fileName + @"</Value>
                                     </Eq>
                                    </And>";
                if (listItemID.HasValue == true)
                {
                    queryString = @"<And>
                                 <Neq>
                                  <FieldRef Name='ID' />
                                  <Value Type='Number'>" + listItemID.Value.ToString() + @"</Value>
                                 </Neq>" + queryString + @"
                                </And>
                                ";
                }
                queryString = "<Where>" + queryString + "</Where>";
                query.InnerXml = queryString;
                XmlNode ndListItems = ws.GetListItems(listName, null, query, viewFields, null, queryOptions, null);
                string message = string.Format("SharePointService CheckFileExistency method returned ListName:{0} queryOptions:{1} \n query:{2} \n xml:{3}", listName, queryOptions.OuterXml, query.OuterXml, ndListItems.OuterXml);
                Logger.Info(message, "Service");

                xmlDoc.LoadXml(ndListItems.OuterXml);
                XmlNodeList _folders = xmlDoc.GetElementsByTagName("z:row");
                if (_folders.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                throw ex;
            }
        }

        public uint AddFolder(ISiteSetting siteSetting, string webUrl, string folderName, string FolderPath, string listName)
        {
            SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
            string folderPath = FolderPath.TrimStart('/');
            //string listName = folderPath.Split('/')[0];

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlElement batchElement = doc.CreateElement("Batch");
            batchElement.SetAttribute("OnError", "Continue");
            batchElement.SetAttribute("ListVersion", "1");
            string xmlCommand = "<Method ID='1' Cmd='New'>" +
               "<Field Name='FSObjType'>1</Field>" +
               "<Field Name='ID'>New</Field>" +
            "<Field Name='BaseName'>" + folderName + "</Field></Method>";

            XmlElement batch = doc.CreateElement("Batch");
            batch.SetAttribute("PreCalc", "TRUE");
            batch.SetAttribute("OnError", "Continue");
            batch.SetAttribute("RootFolder", webUrl+"/" + folderPath);
            batch.InnerXml = xmlCommand;

            XmlNode resultNode = ws.UpdateListItems(listName, batch);

            return 1;
        }

        public Result CopyFile(ISiteSetting siteSetting, string webURL, string copySource, string copyDest)
        {
            try
            {
                copySource = HttpUtility.UrlDecode(copySource);
                copyDest = HttpUtility.UrlDecode(copyDest);

                SharePointCopyWS.Copy myCopyService = new SharePointCopyWS.Copy();
                SetCredentials(myCopyService, siteSetting);
                //myCopyService.Credentials = GetCredential(siteSetting);
                myCopyService.Url = webURL + "/_vti_bin/copy.asmx";
                string[] copyDests = new string[] { copyDest };

                //            SharePointCopyWS.FieldInformation[] myFieldInfoArray = GetFieldInformations(fields, emailItem, settings);

                SharePointCopyWS.CopyResult myCopyResult1 = new SharePointCopyWS.CopyResult();
                SharePointCopyWS.CopyResult myCopyResult2 = new SharePointCopyWS.CopyResult();
                SharePointCopyWS.CopyResult[] myCopyResultArray1 = { myCopyResult1, myCopyResult2 };

                uint myCopyUint = myCopyService.CopyIntoItemsLocal(copySource, copyDests, out myCopyResultArray1);
                string message = string.Format("SharePointService CopyIntoItemsLocal method returned copySource:{0} copyDests:{1} \n myCopyUint:{2}", copySource, copyDests, myCopyUint);
                Logger.Info(message, "Service");

                return new SPResult(myCopyResultArray1[0]);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                throw ex;
            }
        }


        public static byte[] ReadByteArrayFromFile(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }

        public static string AddAttachment(ISiteSetting siteSetting, string webUrl, string listName, int itemID, string fileName, byte[] attachment)
        {
            try
            {
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);

                string result = ws.AddAttachment(listName, itemID.ToString(), fileName, attachment);
                string message = string.Format("SharePointService AddAttachment method returned listName:{0} itemID:{1} \n fileName:{2} \n result:{3}", listName, itemID.ToString(), fileName, result);
                Logger.Info(message, "Service");
                return result;
            }
            catch (Exception ex)
            {
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public string GetUser(ISiteSetting siteSetting, string UserName)
        {
            FieldUserValue userValue = new FieldUserValue();
            ClientContext clientContext = GetClientContext(siteSetting);
            User newUser = clientContext.Web.EnsureUser(UserName);
            clientContext.Load(newUser);
            clientContext.ExecuteQuery();
            userValue.LookupId = newUser.Id;
            return newUser.Id + ";#" + newUser.Title;
        }


        public void DeleteUniquePermissions(ISiteSetting siteSetting, Entities.Folder folder, bool applyToAllSubItems)
        {
            Logger.Info(folder.GetPath() + " UniquePermissions has been deleted.", "Service");

            if (((SPFolder)folder).HasUniqueRoleAssignments == true)
            {
                DeleteUniquePermissions(siteSetting, folder.GetListName(), int.Parse(((SPFolder)folder).ID));
            }

            if (applyToAllSubItems == true)
            {
                string listItemCollectionPositionNext = string.Empty;
                int itemCount = 50000;
                IView view = new SPView(siteSetting.ID);
                view.RowLimit = 50000;
                List<IItem> items = GetListItems(siteSetting, view, string.Empty, true, false, siteSetting.Url, folder.GetListName(), folder.GetPath(), listItemCollectionPositionNext, new CamlFilters(), false, out listItemCollectionPositionNext, out itemCount);
                foreach (IItem item in items)
                {
                    if (((SPListItem)item).HasUniqueRoleAssignments == true)
                    {
                        DeleteUniquePermissions(siteSetting, folder.GetListName(), int.Parse(item.GetID()));
                        Logger.Info("Item with ID" + item.GetID() + " on " + folder.GetPath() + " UniquePermissions has been deleted.", "Service");
                    }
                }

                List<Entities.Folder> subFolders = GetSubFolders(siteSetting, folder);
                foreach (Entities.Folder subFolder in subFolders)
                {
                    DeleteUniquePermissions(siteSetting, subFolder, applyToAllSubItems);
                }

            }
        }

        public void DeleteUniquePermissions(ISiteSetting siteSetting, string listName, int listItemId)
        {
            ClientContext context = GetClientContext(siteSetting);
            List list = context.Web.Lists.GetByTitle(listName);
            ListItem item = list.GetItemById(listItemId);
            context.ExecuteQuery();
            item.ResetRoleInheritance();
            context.ExecuteQuery();
        }

    }
}
