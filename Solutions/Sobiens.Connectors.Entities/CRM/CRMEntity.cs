using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.CRM
{
    [Serializable]
    public class CRMEntity : CRMBaseFolder
    {
        //public FieldCollection Fields = new FieldCollection();

        public CRMEntity() : base() { }
        public CRMEntity(Guid siteSettingID, string uniqueIdentifier, string logicalName, string schemaName, string title)
            : base()
        {
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            LogicalName = logicalName;
            SchemaName = schemaName;
            ContainsItems = true;
            ModifiedByFieldName = "modifiedby";
            ModifiedOnFieldName = "modifiedon";
        }

        public override string GetUrl()
        {
            if (string.IsNullOrEmpty(this.WebUrl) == true)
                return string.Empty;

            string serverRelativeUrl = this.WebUrl.Substring(this.WebUrl.IndexOf('/', 9));
            string folderPath = this.FolderPath;
            if (this.FolderPath.IndexOf(serverRelativeUrl) == 0)
            {
                folderPath = this.FolderPath.Substring(serverRelativeUrl.Length) ;
            }
            //serverRelativeUrl
            return WebUrl.CombineUrl(folderPath).TrimEnd(new char[] { '/' });
        }

        public override string GetPath()
        {
            return SchemaName;
        }

        public override string GetWebUrl()
        {
            return WebUrl;
        }

        public override string IconName
        {
            get
            {
                return "CRMEntity";
            }
        }

        public override string GetRoot()
        {
            // If we try to get the root of an SPWeb, we should fail!
            throw new NotImplementedException();
        }

        public override string GetListName()
        {
            return this.LogicalName;
        }
    }
}
