using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using System.Xml.Serialization;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;

namespace Sobiens.Connectors.Entities
{
#if General
    [Serializable]
#endif
    public class BasicFolderDefinition
    {
        public BasicFolderDefinition() 
        {
            this.Folders = new List<BasicFolderDefinition>();
        }

        public List<BasicFolderDefinition> Folders { get; set; }
        public Guid SiteSettingID { get; set; }
        public string Title { get; set; }
        public string FolderUrl { get; set; }
        public string FolderType { get; set; }
    }
}
