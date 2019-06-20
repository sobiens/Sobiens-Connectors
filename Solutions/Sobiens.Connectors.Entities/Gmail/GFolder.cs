using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.Gmail
{
#if General
    [Serializable]
#endif
    public class GFolder : Folder
    {
        public GFolder():base()
        {
        }

        public GFolder(Guid siteSettingID, string uniqueIdentifier, string title, string path)
            : base()
        {
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            Path = path;
            ContainsItems = true;
        }
        public string Path { get; set; }

        public override string IconName
        {
            get
            {
                return "FOLDER";
            }
        }

        public override string GetWebUrl()
        {
            throw new NotImplementedException();
        }

        public override string GetUrl()
        {
            return this.UniqueIdentifier;
        }

        public override string GetRoot()
        {
            throw new NotImplementedException();
        }

        public override string GetPath()
        {
            return this.Path;
        }

        public override string GetListName()
        {
            throw new NotImplementedException();
        }
    }
}
