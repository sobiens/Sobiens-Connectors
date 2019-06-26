using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using System.Xml.Serialization;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.SharePoint
{
#if General
    [Serializable]
#endif
    public class SPFolder : SPBaseFolder
    {
        public SPFolder() : base() { }
        public SPFolder(Guid siteSettingID, string uniqueIdentifier, string title)
            : base()
        {
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            ContainsItems = true;
            Selected = true;
            Properties = new Dictionary<string, string>();
            PrimaryIdFieldName = "ID";
            PrimaryNameFieldName = "Title";
            PrimaryFileReferenceFieldName = "FileRef";
            ModifiedByFieldName = "Editor";
            ModifiedOnFieldName = "Modified";
    }
        
        public int ServerTemplate = int.MinValue;
        public int BaseType = int.MinValue;
        public string ID = String.Empty;
        public string ListName = String.Empty;
        public string WebApplicationURL = String.Empty;
        public string SiteCollectionURL = String.Empty;
        public string RootFolderPath = String.Empty;
        public string WebRelativePath
        {
            get
            {
                return this.Url.Substring(this.WebUrl.Length);
            }
        }
        public bool HasUniqueRoleAssignments = false;
        public bool AllowDeletion = false;
        public bool AllowMultiResponses = false;
        public bool EnableAttachments = false;
        public bool EnableModeration = false;
        public bool EnableVersioning = false;
        public bool EnableMinorVersion = false;
        public bool RequireCheckout = false;
        [XmlIgnore]
        public System.Collections.Generic.Dictionary<string, string> Properties { get; set; }

        public bool IsDocumentLibrary
        {
            get{
                if (BaseType == 1)
                    return true;
                else
                    return false;
            }
        }

        public override string IconName
        {
            get
            {
                return "SPFolder";
            }
        }

        public override bool CanUpload()
        {
            return IsDocumentLibrary == true || EnableAttachments == true;
        }

        public override string GetRoot()
        {
            return RootFolderPath;
        }

        public override string GetListName()
        {
            return ListName;
        }
    }
}
