using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using System.IO;

namespace Sobiens.Connectors.Entities.FileSystem
{
#if General
    [Serializable]
#endif
    public class FSFolder : Folder
    {
        public FSFolder():base()
        {
        }

        public FSFolder(Guid siteSettingID, DirectoryInfo di):this(siteSettingID, Guid.NewGuid().ToString(), di.Name, di.FullName)
        {
        }

        public FSFolder(Guid siteSettingID, string uniqueIdentifier, string title, string path)
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
            return this.Path;
        }

        public override string GetUrl()
        {
            return this.Path;
        }

        public override string GetRoot()
        {
            return this.Path;
        }

        public override string GetPath()
        {
            return this.Path;
        }

        public override string GetListName()
        {
            return this.Path;
        }

        public DirectoryInfo GetDirectoryInfo()
        {
            return new DirectoryInfo(this.Path);
        }
    }
}
