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
    public class FSItem : IItem
    {
        public FSItem(Guid siteSettingID, FileInfo fi):this(siteSettingID, Guid.NewGuid().ToString(), fi.Name, fi.FullName)
        {
            this.Properties.Add("ows_Editor", System.IO.File.GetAccessControl(fi.FullName).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString());
        }

        public FSItem(Guid siteSettingID, string uniqueIdentifier, string title, string url)
        {
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            URL = url;
            Properties = new Dictionary<string, string>();
        }
        public string UniqueIdentifier { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public Guid SiteSettingID { get; set; }
        public System.Collections.Generic.Dictionary<string, string> Properties { get; set; }
        public string GetListItemURL()
        {
            throw new Exception("Not implemented yet");
        }
        public int GetMinorVersion()
        {
            throw new Exception("Not implemented yet");
        }

        public int GetMajorVersion()
        {
            throw new Exception("Not implemented yet");
        }

        public string GetID()
        {
            throw new Exception("Not implemented yet");
        }

        public FileInfo GetFileInfo()
        {
            return new FileInfo(this.URL);
        }
        public bool isExtracted()
        {
            throw new Exception("Not implemented yet");
        }

        public bool isFolder()
        {
            throw new Exception("Not implemented yet");
        }
    }
}
