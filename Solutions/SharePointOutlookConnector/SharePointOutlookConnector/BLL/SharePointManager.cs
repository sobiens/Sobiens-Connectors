using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Globalization;
using System.Collections;
using System.Web;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using EmailUploader.BLL;
using EmailUploader.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS;
using System.Diagnostics;
//using Microsoft.Office.Interop.Outlook;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public class SharePointManager
    {
        private static SharePointSiteWS.SiteData GetSiteWebService(EUSiteSetting siteSetting, string webURL)
        {
            SharePointSiteWS.SiteData ws = new Sobiens.Office.SharePointOutlookConnector.SharePointSiteWS.SiteData();
            ws.Credentials = GetCredential(siteSetting);
            ws.Url = webURL + "/_vti_bin/SiteData.asmx";
            return ws;
        }

        private static SharePointWebsWS.Webs GetWebsWebService(EUSiteSetting siteSetting, string webURL)
        {
            SharePointWebsWS.Webs ws = new Sobiens.Office.SharePointOutlookConnector.SharePointWebsWS.Webs();
            ws.Credentials = GetCredential(siteSetting);
            ws.Url = webURL + "/_vti_bin/webs.asmx";
            return ws;
        }
        private static SharePointViewsWS.Views GetViewsWebService(EUSiteSetting siteSetting, string webURL)
        {
            SharePointViewsWS.Views ws = new Sobiens.Office.SharePointOutlookConnector.SharePointViewsWS.Views();
            ws.Credentials = GetCredential(siteSetting);
            ws.Url = webURL + "/_vti_bin/views.asmx";
            return ws;
        }
        private static SharePointVersionsWS.Versions GetVersionsWebService(EUSiteSetting siteSetting, string webURL)
        {
            SharePointVersionsWS.Versions ws = new Sobiens.Office.SharePointOutlookConnector.SharePointVersionsWS.Versions();
            ws.Credentials = GetCredential(siteSetting);
            ws.Url = webURL + "/_vti_bin/versions.asmx";
            return ws;
        }
        private static SharePointListsWS.Lists GetListsWebService(EUSiteSetting siteSetting, string webURL)
        {
            SharePointListsWS.Lists ws = new SharePointListsWS.Lists();
            ws.Credentials = GetCredential(siteSetting);
            ws.Url = webURL + "/_vti_bin/lists.asmx";
            return ws;
        }
        private static SharePointCopyWS.Copy GetCopyWebService(EUSiteSetting siteSetting, string webURL)
        {
            SharePointCopyWS.Copy ws = new SharePointCopyWS.Copy();
            ws.Credentials = GetCredential(siteSetting);
            ws.Url = webURL + "/_vti_bin/copy.asmx";
            return ws;
        }

        public static SharePointSiteWS._sSiteMetadata GetSite(string webUrl, EUSiteSetting siteSetting)
        {
            try
            {
                SharePointSiteWS.SiteData ws = GetSiteWebService(siteSetting, webUrl);
                SharePointSiteWS._sSiteMetadata siteMetaData = null;
                SharePointSiteWS._sWebWithTime[] websArray;
                string users;
                string groups;
                string[] vGroups;
                uint value = ws.GetSite(out siteMetaData, out websArray, out users, out groups, out vGroups);
                return siteMetaData;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static string GetSiteURL(string webUrl, EUSiteSetting siteSetting)
        {
            try
            {
                SharePointSiteWS.SiteData ws = GetSiteWebService(siteSetting, webUrl);
                string siteURL;
                string webURL;
                uint value = ws.GetSiteAndWeb(siteSetting.Url, out siteURL, out webURL);
                return siteURL;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static List<EUWeb> GetWebs(string webUrl, EUSiteSetting siteSetting)
        {
            try
            {
                List<EUWeb> webs = new List<EUWeb>();
                SharePointWebsWS.Webs ws = GetWebsWebService(siteSetting, webUrl);
                XmlNode subWebs = ws.GetWebCollection();


                foreach (XmlElement element in subWebs)
                {
                    string url = element.Attributes["Url"].Value;
                    string title = element.Attributes["Title"].Value;
                    EUWeb web = new EUWeb(url, title, siteSetting, url);
                    webs.Add(web);
                }
                return webs;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }
        public static EUView GetView(string webUrl, string listName, string viewName, EUSiteSetting siteSetting)
        {
            try
            {
                SharePointViewsWS.Views ws = GetViewsWebService(siteSetting, webUrl);
                XmlNode viewNodes = ws.GetView(listName, viewName);
                EUView view = NodeToViewObject(siteSetting, webUrl, listName, viewNodes);
                if (viewNodes["Query"] != null)
                    view.QueryXML = viewNodes["Query"].OuterXml;
                if (viewNodes["ViewFields"] != null)
                {
                    view.ViewFields = new List<EUCamlFieldRef>();
                    foreach (XmlNode node in viewNodes["ViewFields"].ChildNodes)
                    {
                        view.ViewFields.Add(new EUCamlFieldRef(node.Attributes["Name"].Value));
                    }
                }

                if (viewNodes["RowLimit"] != null)
                    view.RowLimit = int.Parse(viewNodes["RowLimit"].InnerText);

                return view;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        private static EUView NodeToViewObject(EUSiteSetting siteSetting, string webUrl, string listName, XmlNode element)
        {
            EUView view = new EUView(siteSetting);
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
        public static List<ISPCView> GetViews(string webUrl, string listName, EUSiteSetting siteSetting)
        {
            try
            {
                List<ISPCView> views = new List<ISPCView>();
                SharePointViewsWS.Views ws = GetViewsWebService(siteSetting, webUrl);
                XmlNode viewNodes = ws.GetViewCollection(listName);

                foreach (XmlNode element in viewNodes)
                {
                    EUView view = NodeToViewObject(siteSetting, webUrl, listName, element);
                    views.Add(view);
                }
                return views;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static ICredentials GetCredential(EUSiteSetting siteSetting)
        {
            //            EUSiteSetting siteSetting = EUSettingsManager.GetInstance().GetSiteSetting(rootWebURL);
            if (siteSetting.UseDefaultCredential == true)
            {
                return System.Net.CredentialCache.DefaultCredentials; ;
            }
            else
            {
                string userName = siteSetting.User;
                string[] userNameStringArray = userName.Split(new char[] { '\\' });
                if (userNameStringArray.Count() > 1)
                {
                    return new NetworkCredential(userNameStringArray[1], siteSetting.Password, userNameStringArray[0]);
                }
                else
                {
                    return new NetworkCredential(siteSetting.User, siteSetting.Password);
                }
            }
        }

        public static List<EUListItemVersion> GetListItemVersionsFromXmlElement(EUSiteSetting siteSetting, string webUrl, XmlNode elements)
        {
            List<EUListItemVersion> versions = new List<EUListItemVersion>();
            foreach (XmlElement element in elements)
            {
                if (element.Name != "result")
                    continue;
                EUListItemVersion version = new EUListItemVersion(siteSetting);
                version.Version = element.Attributes["version"].Value;
                version.SiteSetting = siteSetting;
                version.URL = element.Attributes["url"].Value;
                version.Size = element.Attributes["size"].Value;
                version.CreatedBy = element.Attributes["createdBy"].Value;
                version.Created = element.Attributes["created"].Value;
                version.Comments = element.Attributes["comments"].Value;
                version.WebURL = webUrl;
                versions.Add(version);
            }
            return versions;
        }

        public static List<EUListItemVersion> GetListItemVersions(EUSiteSetting siteSetting, string webUrl, string fileURL)
        {
            try
            {
                List<EUListItemVersion> versions = null;
                SharePointVersionsWS.Versions ws = GetVersionsWebService(siteSetting, webUrl);
                XmlNode elements = ws.GetVersions(fileURL);
                versions = GetListItemVersionsFromXmlElement(siteSetting, webUrl, elements);
                return versions;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static List<EUListItemVersion> RestoreVersion(EUSiteSetting siteSetting, string webUrl, string fileURL, string versionString)
        {
            try
            {
                List<EUListItemVersion> versions = null;
                SharePointVersionsWS.Versions ws = GetVersionsWebService(siteSetting, webUrl);
                XmlNode elements = ws.RestoreVersion(fileURL, versionString);
                versions = GetListItemVersionsFromXmlElement(siteSetting, webUrl, elements); ;
                return versions;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
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

        public static EUContentType GetContentType(EUSiteSetting siteSetting, string webUrl, string listName, string contentTypeID)
        {
            EUContentType contentType = new EUContentType();
            SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
            XmlNode web = ws.GetListContentType(listName, contentTypeID);
            EUFieldCollection fields = XmlNodeToFieldCollection(web["Fields"]);
            contentType.Fields = fields;
            return contentType;
        }
        public static List<EUContentType> GetContentTypes(EUSiteSetting siteSetting, string webUrl, string listName)
        {
            try
            {
                List<EUContentType> contentTypes = new List<EUContentType>();
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                XmlNode web = ws.GetListContentTypes(listName, String.Empty);

                foreach (XmlNode element in web.ChildNodes)
                {
                    EUContentType contentType = new EUContentType();
                    contentType.ID = element.Attributes["ID"].Value;
                    contentType.Name = element.Attributes["Name"].Value;
                    contentType.Description = element.Attributes["Description"].Value;
                    if (element.Attributes["Group"] != null)
                        contentType.Group = element.Attributes["Group"].Value;
                    if (element.Attributes["Version"] != null)
                        contentType.Version = element.Attributes["Version"].Value;
                    contentTypes.Add(contentType);
                }
                return contentTypes;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        private static EUFieldCollection XmlNodeToFieldCollection(XmlElement fieldElement)
        {
            EUFieldCollection fields = new EUFieldCollection();

            foreach (XmlElement element in fieldElement)
            {
                EUField field = new EUField();
                // JOEL JEFFERY 20110713 -- commented out... we might want to map to EmailTo, EmailSender etc columns
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
                else if (element.Attributes["Group"] != null && element.Attributes["Group"].Value.ToLower() == "_hidden")
                    continue;
                else if (element.Attributes["Hidden"] != null && element.Attributes["Hidden"].Value.ToLower() == "true")
                    continue;

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
                string type = element.Attributes["Type"].Value;
                if (element["Default"] != null)
                    field.DefaultValue = element["Default"].InnerText;

                if (element.Attributes["Mult"] == null || element.Attributes["Mult"].Value.ToString() == String.Empty)
                    field.Mult = false;
                else
                    field.Mult = bool.Parse(element.Attributes["Mult"].Value);

                EUFieldTypes fieldType;
                switch (type.ToLower())
                {
                    case "text":
                        fieldType = EUFieldTypes.Text;
                        break;
                    case "note":
                        fieldType = EUFieldTypes.Note;
                        break;
                    case "boolean":
                        fieldType = EUFieldTypes.Boolean;
                        break;
                    case "datetime":
                        fieldType = EUFieldTypes.DateTime;
                        break;
                    case "number":
                        fieldType = EUFieldTypes.Number;
                        break;
                    case "lookup":
                        fieldType = EUFieldTypes.Lookup;
                        field.List = element.Attributes["List"].Value;
                        field.ShowField = element.Attributes["ShowField"].Value;
                        break;
                    case "lookupmulti":
                        fieldType = EUFieldTypes.Lookup;
                        field.List = element.Attributes["List"].Value;
                        field.ShowField = element.Attributes["ShowField"].Value;
                        field.Mult = true;
                        break;
                    case "choice":
                        fieldType = EUFieldTypes.Choice;
                        field.ChoiceItems = GetChoiceDataItems(element);
                        break;
                    case "multichoice":
                        fieldType = EUFieldTypes.Choice;
                        field.ChoiceItems = GetChoiceDataItems(element);
                        field.Mult = true;
                        break;
                    default:
                        fieldType = EUFieldTypes.Unknown;
                        field.ReadOnly = true;
                        break;
                }
                field.Type = fieldType;
                fields.Add(field);
            }
            return fields;
        }

        public static EUFieldCollection GetFields(EUSiteSetting siteSetting, string webUrl, string listName)
        {
            try
            {
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                XmlNode web = ws.GetList(listName);
                EUFieldCollection fields = XmlNodeToFieldCollection(web["Fields"]);

                return fields;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static void DeleteListItem(EUSiteSetting siteSetting, string webUrl, string listName, string fileRef, int listItemID)
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
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }
        public static void UpdateListItem(EUSiteSetting siteSetting, string webUrl, string listName, int listItemID, Hashtable changedProperties)
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
                //            batchElement.SetAttribute("ViewName", strViewID);

                /*Specify methods for the batch post using CAML. To update or delete, 
                specify the ID of the item, and to update or add, specify 
                the value to place in the specified column.*/
                string xml = "<Method ID='1' Cmd='Update'>" +
                   "<Field Name='ID'>" + listItemID + "</Field>";
                foreach (object fieldName in changedProperties.Keys)
                {
                    xml += "<Field Name='" + fieldName.ToString() + "'>" + changedProperties[fieldName].ToString() + "</Field>";
                }
                xml += "</Method>";
                batchElement.InnerXml = xml;
                /*Update list items. This example uses the list GUID, which is recommended, 
                but the list display name will also work.*/
                XmlNode resultNode = ws.UpdateListItems(listName, batchElement);
                if (resultNode["Result"]["ErrorText"] != null && resultNode["Result"]["ErrorText"].InnerText != String.Empty)
                {
                    Exception ex1 = new Exception(resultNode["Result"]["ErrorText"].InnerText);
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogManager.LogException(methodName, ex1);
                    throw ex1;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static List<EUList> GetLists(string webUrl, EUSiteSetting siteSetting)
        {
            try
            {
                List<EUList> lists = new List<EUList>();
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                XmlNode web = ws.GetListCollection();
                string siteURL = GetSiteURL(webUrl, siteSetting);

                foreach (XmlElement element in web)
                {
                    string id = element.Attributes["ID"].Value;
                    string title = element.Attributes["Title"].Value;
                    EUList list = new EUList(siteSetting, id, title);
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
                    list.BaseType = int.Parse(element.Attributes["BaseType"].Value);
                    list.FolderPath = (list.IsDocumentLibrary == true ? list.Title : "Lists/" + list.Title);
                    list.RootFolderPath = list.WebUrl.TrimEnd(new char[] { '/' }) + "/" + list.FolderPath.TrimStart(new char[] { '/' });
                    list.ListName = list.Title;
                    list.SiteUrl = siteURL;
                    list.WebUrl = webUrl;

                    lists.Add(list);
                }
                return lists;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static void CheckInFile(EUSiteSetting siteSetting, string webUrl, string pageURL, string comment, EUCheckinTypes checkinType)
        {
            SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
            ws.UnsafeAuthenticatedConnectionSharing = true;
            bool result = ws.CheckInFile(pageURL, comment, ((int)checkinType).ToString());
        }

        public static void CheckOutFile(EUSiteSetting siteSetting, string webUrl, string pageURL)
        {
            SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
            ws.CheckOutFile(pageURL, "false", String.Empty);
        }

        public static void UndoCheckOutFile(EUSiteSetting siteSetting, string webUrl, string pageURL)
        {
            SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
            ws.UndoCheckOut(pageURL);
        }

        public static List<EUDocument> GetDocuments(EUSiteSetting siteSetting, string webUrl, string listName, string folderName)
        {
            try
            {
                List<EUDocument> documents = new List<EUDocument>();
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);

                XmlDocument xmlDoc = new XmlDocument();

                XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                XmlNode viewFields = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
                XmlNode queryOptions = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

                //<ViewAttributes Scope='RecursiveAll'/>
                queryOptions.InnerXml = @"<IncludeMandatoryColumns>TRUE</IncludeMandatoryColumns>
                          <DateInUtc>TRUE</DateInUtc>
                          <Folder>" + folderName + "</Folder>";
                viewFields.InnerXml = "";
                query.InnerXml = @"<Where>
                           <Eq>
                               <FieldRef Name='FSObjType' />
                               <Value Type='Lookup'>0</Value>
                           </Eq>
                   </Where>";

                XmlNode ndListItems = ws.GetListItems(listName, null, query, viewFields, null, queryOptions, null);

                xmlDoc.LoadXml(ndListItems.OuterXml);
                XmlNodeList _folders = xmlDoc.GetElementsByTagName("z:row");

                foreach (XmlNode folder in _folders)
                {
                    EUDocument document = new EUDocument(siteSetting);
                    document.ID = folder.Attributes["ows_ID"].Value;
                    document.FileName = folder.Attributes["ows_LinkFilename"].Value;
                    document.URL = folder.Attributes["ows_EncodedAbsUrl"].Value;
                    document.ListName = listName;
                    document.WebUrl = webUrl;
                    document.FolderPath = folderName;
                    documents.Add(document);
                }
                return documents;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        // JOEL JEFFERY 20110710
        /// <summary>
        /// Checks the folder exists.
        /// </summary>
        /// <param name="siteSetting">The site setting.</param>
        /// <param name="webUrl">The web URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="parentFolderName">Name of the parent folder.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        public static bool CheckFolderExists(EUSiteSetting siteSetting, string webUrl, string listName, string parentFolderName, string folderName)
        {
            return false; // pretend it doesn't exist...
            try
            {
                List<EUListItem> items = new List<EUListItem>();
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);

                XmlDocument xmlDoc = new XmlDocument();

                XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                XmlNode viewFields = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
                XmlNode queryOptions = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

                queryOptions.InnerXml = @"<IncludeMandatoryColumns>TRUE</IncludeMandatoryColumns>
                          <DateInUtc>TRUE</DateInUtc>
                          <Folder>" + parentFolderName + "</Folder>";
                viewFields.InnerXml = "";
                string queryString = @"<Where>
                                            <And>
                                            <Eq>
                                             <FieldRef Name='FSObjType' />
                                             <Value Type='Lookup'>1</Value>
                                            </Eq>
                                             <Eq>
                                              <FieldRef Name='FileLeafRef' />
                                              <Value Type='Text'>" + folderName + @"</Value>
                                             </Eq>
                                        </And>
                                    </Where>";

                query.InnerXml = queryString;
                XmlNode ndListItems = ws.GetListItems(listName, null, query, viewFields, null, queryOptions, null);

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
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static bool CheckFileExistency(EUSiteSetting siteSetting, string webUrl, string listName, string folderName, int? listItemID, string fileName)
        {
            try
            {
                List<EUListItem> items = new List<EUListItem>();
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);

                XmlDocument xmlDoc = new XmlDocument();

                XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                XmlNode viewFields = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
                XmlNode queryOptions = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

                queryOptions.InnerXml = @"<IncludeMandatoryColumns>TRUE</IncludeMandatoryColumns>
                          <DateInUtc>TRUE</DateInUtc>
                          <Folder>" + folderName + "</Folder>";
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
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static List<ISPCItem> GetListItems(EUSiteSetting siteSetting, EUView view, string sortField, bool isAsc, bool isDocumentLibrary, string webUrl, string listName, string folderName, string currentListItemCollectionPositionNext, EUCamlFilters customFilters, out string listItemCollectionPositionNext, out int itemCount)
        {
            try
            {
                string orderBy = String.Empty;
                if (sortField == String.Empty)
                {
                    orderBy = view.GetOrderByXML();
                }
                else
                {
                    orderBy = "<OrderBy><FieldRef Name=\"" + sortField + "\" " + (isAsc == false ? "Ascending=\"FALSE\"" : "") + " /></OrderBy>";
                }
                List<ISPCItem> items = new List<ISPCItem>();
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
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

                XmlNode folderNode = xmlDoc.CreateNode(XmlNodeType.Element, "Folder", String.Empty);
                folderNode.InnerText = folderName;
                queryOptions.AppendChild(folderNode);

                if (view.RowLimit > 0)
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
                List<string> filters = new List<string>();
                /*
                if (isDocumentLibrary == true)
                {
                 */
                filters.Add(@"<Eq><FieldRef Name='FSObjType' /><Value Type='Lookup'>0</Value></Eq>");
                //}
                if (view.WhereXML != String.Empty)
                {
                    filters.Add(view.WhereXML);
                }
                if (customFilters.Count > 0)
                {
                    customFilters.IsOr = false;
                    string customQuery = SPCamlManager.GetCamlString(customFilters);
                    filters.Add(customQuery);
                }

                query.InnerXml = orderBy + "<Where>" + SPCamlManager.GetCamlString(filters, false) + "</Where>";
                XmlNode ndListItems = ws.GetListItems(listName, null, query, viewFields, null, queryOptions, null);

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
                    EUListItem listItem = new EUListItem(siteSetting);
                    listItem.WebURL = webUrl;
                    listItem.ListName = listName;
                    listItem.FolderPath = folderName;
                    listItem.ID = int.Parse(item.Attributes["ows_ID"].Value);
                    if (isDocumentLibrary == true)
                    {
                        listItem.Title = item.Attributes["ows_" + titleFieldName].Value;
                    }
                    else
                    {
                        listItem.Title = item.Attributes["ows_" + titleFieldName].Value;
                    }
                    if (item.Attributes["ows_CheckoutUser"] != null)
                        listItem.CheckoutUser = item.Attributes["ows_CheckoutUser"].Value;

                    listItem.URL = item.Attributes["ows_EncodedAbsUrl"].Value;
                    listItem.Properties = item.Attributes;
                    //listItem.ListName = listName;
                    //listItem.WebUrl = webUrl;
                    //listItem.FolderPath = folderName;
                    items.Add(listItem);
                }
                return items;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static EUListItem GetFileListItem(EUSiteSetting siteSetting, string siteUrl, string webUrl, string listName, string folderName, string fileURL)
        {
            try
            {
                fileURL = fileURL.Replace(siteUrl, String.Empty);

                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                XmlDocument xmlDoc = new XmlDocument();

                XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                XmlNode viewFields = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
                XmlNode queryOptions = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

                queryOptions.InnerXml = "<ViewAttributes Scope=\"Recursive\" /><IncludeMandatoryColumns>True</IncludeMandatoryColumns>";

                XmlNode dateInUtcNode = xmlDoc.CreateNode(XmlNodeType.Element, "DateInUtc", String.Empty);
                dateInUtcNode.InnerText = "TRUE";
                queryOptions.AppendChild(dateInUtcNode);

                /*
                if (folderName != "/")
                {
                    XmlNode folderNode = xmlDoc.CreateNode(XmlNodeType.Element, "Folder", String.Empty);
                    folderNode.InnerText = folderName;
                    queryOptions.AppendChild(folderNode);
                }
                 */
                viewFields.InnerXml = "";
                List<string> filters = new List<string>();

                filters.Add(@"<Eq>
                                 <FieldRef Name='FileRef' />
                                 <Value Type='Lookup' >" + fileURL.TrimStart(new char[] { '/' }) + @"</Value>
                                </Eq>");

                query.InnerXml = "<Where>" + SPCamlManager.GetCamlString(filters, false) + "</Where>";
                XmlNode ndListItems = ws.GetListItems(listName, null, query, viewFields, null, queryOptions, null);

                xmlDoc.LoadXml(ndListItems.OuterXml);

                XmlNodeList _items = xmlDoc.GetElementsByTagName("z:row");
                string titleFieldName = "LinkFilename";
                foreach (XmlNode item in _items)
                {
                    EUListItem listItem = new EUListItem(siteSetting);
                    listItem.WebURL = webUrl;
                    listItem.ListName = listName;
                    //listItem.FolderPath = folderName;
                    listItem.ID = int.Parse(item.Attributes["ows_ID"].Value);
                    listItem.Title = item.Attributes["ows_" + titleFieldName].Value;
                    if (item.Attributes["ows_CheckoutUser"] != null)
                        listItem.CheckoutUser = item.Attributes["ows_CheckoutUser"].Value;

                    listItem.URL = item.Attributes["ows_EncodedAbsUrl"].Value;
                    listItem.Properties = item.Attributes;
                    return listItem;
                }
                return null;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static List<ChoiceDataItem> GetListItems(EUSiteSetting siteSetting, string showField, string webUrl, string listName)
        {
            try
            {
                string orderBy = "<OrderBy><FieldRef Name=\"" + showField + "\" Ascending=\"TRUE\" /></OrderBy>";
                List<ChoiceDataItem> items = new List<ChoiceDataItem>();
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
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


                viewFields.InnerXml = "";
                List<string> filters = new List<string>();
                filters.Add(@"<Eq>
                                 <FieldRef Name='FSObjType' />
                                 <Value Type='Lookup'>0</Value>
                                </Eq>");

                query.InnerXml = orderBy + SPCamlManager.GetCamlString(filters, false);
                XmlNode ndListItems = ws.GetListItems(listName, null, query, viewFields, "0", queryOptions, null);

                xmlDoc.LoadXml(ndListItems.OuterXml);


                XmlNodeList _items = xmlDoc.GetElementsByTagName("z:row");
                foreach (XmlNode item in _items)
                {
                    string title = item.Attributes["ows_" + showField].Value; ;
                    ChoiceDataItem choiceDataItem = new ChoiceDataItem(item.Attributes["ows_ID"].Value, title);
                    items.Add(choiceDataItem);
                }
                return items;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static IEnumerable<EUFolder> GetFolders(EUFolder currentFolder)
        {
            try
            {
                List<EUFolder> folders = new List<EUFolder>();
                SharePointListsWS.Lists ws = GetListsWebService(currentFolder.SiteSetting, currentFolder.WebUrl);

                XmlDocument xmlDoc = new XmlDocument();

                XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                XmlNode viewFields = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
                XmlNode queryOptions = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

                queryOptions.InnerXml = @"<IncludeMandatoryColumns>TRUE</IncludeMandatoryColumns>
                          <DateInUtc>TRUE</DateInUtc>
                          <Folder>" + currentFolder.FolderPath + "</Folder>";
                viewFields.InnerXml = "";
                query.InnerXml = @"<Where>
                           <Eq>
                               <FieldRef Name='FSObjType' />
                               <Value Type='Lookup'>1</Value>
                           </Eq>
                   </Where>";
                LogManager.Log("[GetFolders] QueryOptionsXML:" + queryOptions.OuterXml, EULogModes.Detailed);
                LogManager.Log("[GetFolders] query:" + query.OuterXml, EULogModes.Detailed);

                XmlNode ndListItems = ws.GetListItems(currentFolder.ListName, null, query, viewFields, null, queryOptions, null);
                LogManager.Log("[GetFolders] GetListItems XML:" + ndListItems.OuterXml, EULogModes.Detailed);

                xmlDoc.LoadXml(ndListItems.OuterXml);
                XmlNodeList _folders = xmlDoc.GetElementsByTagName("z:row");

                string siteURL = GetSiteURL(currentFolder.WebUrl, currentFolder.SiteSetting);

                foreach (XmlNode folder in _folders)
                {
                    string id = folder.Attributes["ows_ID"].Value;
                    string title = folder.Attributes["ows_BaseName"].Value;
                    EUFolder _folder = new EUFolder(currentFolder.SiteSetting, id, title);
                    _folder.ServerTemplate = currentFolder.ServerTemplate;
                    _folder.BaseType = currentFolder.BaseType;
                    _folder.RootFolderPath = currentFolder.RootFolderPath;
                    _folder.ID = id;
                    _folder.Title = title;
                    _folder.ListName = currentFolder.ListName;
                    _folder.SiteUrl = siteURL;
                    _folder.WebUrl = currentFolder.WebUrl;
                    _folder.FolderPath = currentFolder.FolderPath + "/" + _folder.Title;
                    folders.Add(_folder);
                }
                return folders.OrderBy(s => s.Title);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static SharePointCopyWS.FieldInformation GetFieldInformation(List<EUField> fields, string sharePointFieldName, string outlookFieldName, EUEmailMetaData metaData)
        {
            try
            {
                string value = String.Empty;
                if (outlookFieldName == EUEmailFields.BCC.ToString())
                    value = metaData.BCC;
                else if (outlookFieldName == EUEmailFields.Body.ToString())
                    value = metaData.Body;
                else if (outlookFieldName == EUEmailFields.CC.ToString())
                    value = metaData.CC;
                else if (outlookFieldName == EUEmailFields.ReceivedTime.ToString() && metaData.ReceivedTime != null && metaData.ReceivedTime.Year > 1900 && metaData.ReceivedTime.Year < 2100)
                    value = metaData.ReceivedTime.ToString(DateTimeFormatInfo.InvariantInfo);
                else if (outlookFieldName == EUEmailFields.SenderEmailAddress.ToString())
                    value = metaData.SenderEmailAddress;
                else if (outlookFieldName == EUEmailFields.SentOn.ToString() && metaData.SentOn != null && metaData.SentOn.Year > 1900 && metaData.SentOn.Year < 2100)
                    value = metaData.SentOn.ToString(DateTimeFormatInfo.InvariantInfo);
                else if (outlookFieldName == EUEmailFields.Subject.ToString())
                    value = metaData.Subject;
                else if (outlookFieldName == EUEmailFields.To.ToString())
                    value = metaData.To;
                if (value != null && value != String.Empty)
                {
                    EUField field = fields.SingleOrDefault(f => f.Name.ToLower() == sharePointFieldName.ToLower());
                    if (field == null)
                        return null;
                    SharePointCopyWS.FieldInformation myFieldInfo = new SharePointCopyWS.FieldInformation();
                    myFieldInfo.Id = field.ID;
                    myFieldInfo.InternalName = field.Name;
                    myFieldInfo.DisplayName = field.DisplayName;
                    switch (field.Type)
                    {
                        case EUFieldTypes.Note:
                            myFieldInfo.Type = Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.FieldType.Note;
                            break;
                        case EUFieldTypes.Text:
                            myFieldInfo.Type = Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.FieldType.Text;
                            break;
                        case EUFieldTypes.DateTime:
                            myFieldInfo.Type = Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.FieldType.DateTime;
                            break;
                    }
                    myFieldInfo.Value = (value.Length > field.MaxLength ? value.Substring(0, field.MaxLength) : value); ;
                    return myFieldInfo;
                }
                return null;
            }
            catch (System.Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static SharePointCopyWS.FieldInformation[] GetFieldInformations(EUSiteSetting siteSetting, string rootFolderPath, List<EUField> fields, EUEmailMetaData metaData)
        {
            try
            {
                List<SharePointCopyWS.FieldInformation> fieldInformations = new List<Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.FieldInformation>();
                EUListSetting listSetting = EUSettingsManager.GetInstance().GetListSetting(rootFolderPath);
                if (listSetting == null)
                {
                    var query = from emailMappings in listSetting.EmailMappings
                                where emailMappings.SharePointFieldName != null && emailMappings.SharePointFieldName != String.Empty
                                select emailMappings;
                    foreach (EUEmailMapping emailMapping in query)
                    {
                        SharePointCopyWS.FieldInformation titleFieldInformation = GetFieldInformation(fields, emailMapping.SharePointFieldName, emailMapping.OutlookFieldName, metaData);
                        fieldInformations.Add(titleFieldInformation);
                    }
                }
                return fieldInformations.ToArray();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        //public static void UploadFiles(EUSiteSetting siteSetting, string rootFolderPath, string webURL, string listName, string destinationFolderUrl, List<EUEmailUploadFile> uploadFiles)
        //{
        //    List<EUField> fields = SharePointManager.GetFields(siteSetting, webURL, listName);
        //    SharePointCopyWS.CopyResult[] myCopyResultArray = null;
        //    foreach (EUEmailUploadFile uploadFile in uploadFiles)
        //    {
        //        string copySource = new FileInfo(uploadFile.FilePath).Name;
        //        string[] copyDest = new string[1] { destinationFolderUrl + "/" + copySource };
        //        byte[] itemByteArray = SharePointManager.ReadByteArrayFromFile(uploadFile.FilePath);

        //        UploadFile(siteSetting, rootFolderPath, webURL, copySource, copyDest, itemByteArray, fields, uploadFile.MailItem,  out myCopyResultArray);
        //    }
        //}

        private static SharePointCopyWS.FieldInformation[] GetFieldInformations(EUEmailMetaData metaData, List<EUFieldInformation> fieldInfoArray, List<EUField> fields)
        {
            List<SharePointCopyWS.FieldInformation> fieldInformations = new List<Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.FieldInformation>();
            foreach (EUFieldInformation fieldInformation in fieldInfoArray)
            {
                if (fieldInformation.EmailField == EUEmailFields.NotSelected)
                {
                    if (fieldInformation.Type == FieldType.DateTime && string.IsNullOrEmpty(fieldInformation.Value))
                        continue;
                    FieldInformation fieldInformationx = new FieldInformation();
                    fieldInformationx.DisplayName = fieldInformation.DisplayName;
                    fieldInformationx.Id = fieldInformation.Id;
                    fieldInformationx.InternalName = fieldInformation.InternalName;
                    fieldInformationx.Type = fieldInformation.Type;
                    fieldInformationx.Value = fieldInformation.Value;
                    fieldInformations.Add(fieldInformationx);
                }
                else
                {
                    SharePointCopyWS.FieldInformation titleFieldInformation = GetFieldInformation(fields, fieldInformation.InternalName, fieldInformation.EmailField.ToString(), metaData);
                    fieldInformations.Add(titleFieldInformation);
                }
            }
            return fieldInformations.ToArray();
        }

        // JOEL JEFFERY 20110712 - added try catch block
        /// <summary>
        /// Uploads a list item with attachment.
        /// </summary>
        /// <param name="uploadItem">The upload item.</param>
        /// <returns></returns>
        public static int? UploadListItemWithAttachment(EUUploadItem uploadItem)
        {
            //Microsoft.Office.Interop.Outlook.MailItem emailItem = uploadItem.EmailUploadFile.MailItem;
            string log = String.Empty;
            string body = uploadItem.EmailUploadFile.MetaData.Body;
            try
            {
                int id = SharePointManager.CreateListItem(uploadItem.Folder.SiteSetting, uploadItem.Folder.RootFolderPath, uploadItem.Folder.WebUrl, uploadItem.Folder.ListName, uploadItem.EmailUploadFile.MetaData.Subject, body);

                foreach (EUEmailUploadFile emailUploadFile in uploadItem.EmailUploadFile.Attachments)
                {
                    string filePath = emailUploadFile.FilePath;
                    string newFilename = new FileInfo(filePath).Name;
                    byte[] itemByteArray = SharePointManager.ReadByteArrayFromFile(filePath);
                    string result = SharePointManager.AddAttachment(uploadItem.Folder.SiteSetting, uploadItem.Folder.WebUrl, uploadItem.Folder.ListName, id, newFilename, itemByteArray);

                    log += "WebURL:" + uploadItem.Folder.WebUrl + Environment.NewLine + "ListName:" + uploadItem.Folder.ListName + Environment.NewLine + "filePath:" + filePath + Environment.NewLine + "newFilename:" + newFilename + Environment.NewLine + "result:" + result + Environment.NewLine;
                    LogManager.Log(log, EULogModes.Normal);
                }

                return id;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the field infos subset.
        /// </summary>
        /// <param name="fullFieldInfos">The full field infos.</param>
        /// <param name="fieldsToPass">The fields to pass.</param>
        /// <returns></returns>
        private static EUFieldInformations getFieldInfosSubset(EUFieldInformations fullFieldInfos, FieldInformation[] fieldsToPass)
        {
            EUFieldInformations returnCollection = new EUFieldInformations();
            foreach (var fieldInfo in fullFieldInfos)
            {
                foreach (FieldInformation fi in fieldsToPass)
                    if (fieldInfo.InternalName == fi.InternalName)
                        returnCollection.Add(fieldInfo);
            }
            return returnCollection;
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
        /// <param name="metaData">The meta data.</param>
        /// <param name="fieldInfoArray">The field info array.</param>
        /// <param name="listItem">The list item.</param>
        /// <returns>null on failure.</returns>
        public static uint? UploadFile(EUSiteSetting siteSetting, string listName, string rootFolderPath, string siteURL, string webURL, string copySource, string[] copyDest, byte[] myByteArray, List<EUField> fields, EUEmailMetaData metaData, EUFieldInformations fieldInfoArray, out EUListItem listItem)
        {
            //Microsoft.Office.Interop.Outlook.MailItem emailItem,
            try
            {
                LogManager.Log("UploadFile started with;"
                    + Environment.NewLine + " ListName:" + listName
                    + Environment.NewLine + " WebURL:" + webURL
                     + Environment.NewLine + " CopySource:" + copySource
                     + Environment.NewLine + " copyDest:" + copyDest[0]
                    , EULogModes.Detailed);
                listItem = null;
                SharePointCopyWS.CopyResult[] myCopyResultArray = null;

                SharePointCopyWS.Copy myCopyService = new SharePointCopyWS.Copy();
                myCopyService.Credentials = GetCredential(siteSetting);
                myCopyService.Url = webURL + "/_vti_bin/copy.asmx";


                SharePointCopyWS.FieldInformation[] myFieldInfoArray = null;
                if (fieldInfoArray == null)
                {
                    myFieldInfoArray = GetFieldInformations(siteSetting, rootFolderPath, fields, metaData);
                }
                else
                {
                    myFieldInfoArray = GetFieldInformations(metaData, fieldInfoArray, fields);
                }

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
                LogManager.Log("CopyIntoItems for " + copyDest[0] + ":" + Environment.NewLine + resultLog, EULogModes.Normal);

                string temp = copyDest[0].Replace(webURL + rootFolderPath, String.Empty);
                string folderName = temp.Substring(0, temp.LastIndexOf("/"));
                if (folderName.StartsWith("/") == false)
                    folderName = "/" + folderName;
                string path = copyDest[0]; //.Replace(webURL, String.Empty); 
                if (fieldInfoArray != null && myFieldInfoArray != null)
                {
                    if (fieldInfoArray.Count != myFieldInfoArray.Count())
                        fieldInfoArray = getFieldInfosSubset(fieldInfoArray, myFieldInfoArray);
                    bool needUpdate = false;
                    if (fieldInfoArray.ContentType != null && fieldInfoArray.ContentType.Name != "Document" && fieldInfoArray.ContentType.Name != "Item")
                    {
                        needUpdate = true;
                    }
                    else
                    {
                        for (int i = 0; i < myFieldInfoArray.Count(); i++)
                        {
                            if (myFieldInfoArray[i] != null && myFieldInfoArray[i].Type == Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.FieldType.Lookup)
                            {
                                needUpdate = true;
                                break;
                            }
                        }
                    }

                    if (needUpdate == true)
                    {
                        LogManager.Log("Need update for " + copyDest[0], EULogModes.Normal);

                        Hashtable changedProperties = new Hashtable();
                        if (fieldInfoArray.ContentType != null)
                            changedProperties.Add("ContentType", fieldInfoArray.ContentType.Name);
                        for (int i = 0; i < myFieldInfoArray.Count(); i++)
                        {
                            //if (fieldInfoArray[i].InternalName.ToLower().Contains("cc"))
                            //    Debug.WriteLine("here!");
                            if (myFieldInfoArray[i] == null)
                                continue; //changedProperties.Add(fieldInfoArray[i].InternalName, string.Empty);
                            else if (fieldInfoArray[i].Type == FieldType.DateTime)
                            {
                                string date = convertToISO8601(myFieldInfoArray[i].Value);
                                //if(!string.IsNullOrEmpty(date))
                                changedProperties.Add(fieldInfoArray[i].InternalName, date);
                            }
                            else
                                changedProperties.Add(fieldInfoArray[i].InternalName, myFieldInfoArray[i].Value);
                        }
                        listItem = GetFileListItem(siteSetting, siteURL, webURL, listName, folderName, path);
                        if (listItem == null)
                        {
                            LogManager.Log("Could not find Item for " + copyDest[0], EULogModes.Normal);
                            return myCopyUint;
                        }
                        else
                        {
                            LogManager.Log("Found ItemID for " + copyDest[0] + ":" + listItem.ID, EULogModes.Normal);
                            SharePointManager.UpdateListItem(listItem.SiteSetting, listItem.WebURL, listItem.ListName, listItem.ID, changedProperties);
                            LogManager.Log("Updated Item for " + copyDest[0] + ":" + listItem.ID, EULogModes.Normal);
                        }
                    }
                }
                listItem = GetFileListItem(siteSetting, siteURL, webURL, listName, folderName, path);
                return myCopyUint;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                listItem = null;
                throw ex;
                return null;
            }
        }

        /// <summary>
        /// Converts to ISO8601.
        /// </summary>
        /// <param name="dateTimeString">The date time string.</param>
        /// <returns></returns>
        private static string convertToISO8601(string dateTimeString)
        {
            DateTime dateTime;
            if (!DateTime.TryParse(dateTimeString, out dateTime))
                return string.Empty;
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="destinationfolderPath"></param>
        /// <param name="sourceFileSteam"></param>
        /// <param name="fileName"></param>
        /// <param name="fields"></param>
        /// <returns>String with the destination Uri</returns>
        public static uint CopyFile(EUSiteSetting siteSetting, string webURL, string copySource, string copyDest, out SharePointCopyWS.CopyResult[] myCopyResultArray)
        {
            try
            {
                copySource = HttpUtility.UrlDecode(copySource);
                copyDest = HttpUtility.UrlDecode(copyDest);

                SharePointCopyWS.Copy myCopyService = new SharePointCopyWS.Copy();
                myCopyService.Credentials = GetCredential(siteSetting);
                myCopyService.Url = webURL + "/_vti_bin/copy.asmx";
                string[] copyDests = new string[] { copyDest };

                //            SharePointCopyWS.FieldInformation[] myFieldInfoArray = GetFieldInformations(fields, emailItem, settings);

                SharePointCopyWS.CopyResult myCopyResult1 = new SharePointCopyWS.CopyResult();
                SharePointCopyWS.CopyResult myCopyResult2 = new SharePointCopyWS.CopyResult();
                SharePointCopyWS.CopyResult[] myCopyResultArray1 = { myCopyResult1, myCopyResult2 };

                uint myCopyUint = myCopyService.CopyIntoItemsLocal(copySource, copyDests, out myCopyResultArray1);
                myCopyResultArray = myCopyResultArray1;
                return myCopyUint;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public static string AddAttachment(EUSiteSetting siteSetting, string webUrl, string listName, int itemID, string fileName, byte[] attachment)
        {
            try
            {
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);

                return ws.AddAttachment(listName, itemID.ToString(), fileName, attachment);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }


        // JOEL JEFFERY 20110713 - constant hodling the error code if folder creation dies due to folder pre-existing
        private const string FOLDER_EXISTS = "0x8107090d";

        // JOEL JEFFERY - 20110709
        /// <summary>
        /// Creates a list folder.
        /// </summary>
        /// <param name="siteSetting">The site setting.</param>
        /// <param name="rootFolderPath">The root folder path.</param>
        /// <param name="webUrl">The web URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        public static int CreateListFolder(EUSiteSetting siteSetting, string rootFolderPath, string webUrl, string listName, string title)
        {
            try
            {
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                rootFolderPath = rootFolderPath.Substring(listName.Length); // trim name of list away from start of folder path
                if (rootFolderPath.StartsWith("/"))
                    rootFolderPath = rootFolderPath.Substring(1); // trim name of list away from start of folder path
                string xml = string.Format(@"<Method ID='1' Cmd='New'><Field Name='FSObjType'>1</Field><Field Name='BaseName'>{0}/{1}</Field><Field Name='ID'>New</Field></Method>",
                    rootFolderPath,
                    title);
                XmlDocument xmlDoc = new System.Xml.XmlDocument();
                System.Xml.XmlElement elBatch = xmlDoc.CreateElement("Batch");
                elBatch.SetAttribute("OnError", "Continue");
                elBatch.InnerXml = xml;
                XmlNode returnNode = ws.UpdateListItems(listName, elBatch);
                //if the folder exists already... swallow the error
                if (returnNode.FirstChild.FirstChild.InnerText == FOLDER_EXISTS)
                    return 0;
                int id = int.Parse(returnNode.ChildNodes[0].ChildNodes[2].Attributes["ows_ID"].Value);
                return id;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }

        }

        public static int CreateListItem(EUSiteSetting siteSetting, string rootFolderPath, string webUrl, string listName, string title, string body)
        {
            try
            {
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);

                string sharePointFieldName = EUSettingsManager.GetInstance().GetMappedSharePointFieldName(rootFolderPath, EUEmailFields.Body.ToString());

                string xml = @"<Method ID='1' Cmd='New'>
                <Field Name='ID'>New</Field>
                <Field Name='Title'><![CDATA[" + title + @"]]></Field>" +
                    ((sharePointFieldName != String.Empty && body != String.Empty) ? "<Field Name='" + sharePointFieldName + "'><![CDATA[" + body + @"]]></Field>" : String.Empty) +
                    "</Method>";

                XmlDocument xmlDoc = new System.Xml.XmlDocument();
                System.Xml.XmlElement elBatch = xmlDoc.CreateElement("Batch");
                elBatch.SetAttribute("OnError", "Continue");
                elBatch.SetAttribute("ListVersion", "1");
                //elBatch.SetAttribute("ViewName", "0d7fcacd-1d7c-45bc-bcfc-6d7f7d2eeb40");

                XmlNode returnNode = ws.UpdateListItems(listName, elBatch);
                //string IDString = returnNode.ChildNodes[0].Attributes["ID"].Value;
                int id = int.Parse(returnNode.ChildNodes[0].ChildNodes[2].Attributes["ows_ID"].Value);
                return id;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
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
        public static string GetUnuqieFileName(string sourceFolder, string fileNameWithoutExtension, string extensionName, out string fileName)
        {
            fileName = fileNameWithoutExtension.TrimEnd(new char[] { '.' });
            fileName = MakeFileNameSafe(fileName);
            if (fileName.Length > 75)
                fileName = fileName.Substring(0, 75);
            string filePath = sourceFolder + "\\" + fileName + "." + extensionName;
            int i = 1;
            while (File.Exists(filePath) == true)
            {
                filePath = sourceFolder + "\\" + fileName + " (" + i.ToString() + ")." + extensionName;
                i++;
            }
            return filePath;
        }

        // JOEL JEFFERY 20110713
        /// <summary>
        /// Makes the file name safe.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string MakeFileNameSafe(string fileName)
        {
            fileName = fileName
                .Replace("/", "_")
                .Replace("\\", "_")
                .Replace(":", "_")
                .Replace("*", "_")
                .Replace("?", "_")
                .Replace("<", "_")
                .Replace(">", "_")
                .Replace("|", "_")
                .Replace("&", "_")
                .Replace("#", "_")
                .Replace("%", "_")
                .Replace("{", "_")
                .Replace("}", "_")
                .Replace("'", "_")
                ;
            return fileName;
        }

    }
}
