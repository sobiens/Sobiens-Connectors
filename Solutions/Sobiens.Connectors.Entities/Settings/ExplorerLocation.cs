using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sobiens.Connectors.Entities.Settings
{
    public class ExplorerLocation
    {
        public Guid ID { get; set; }
        public Guid SiteSettingID { get; set; }
        public List<ApplicationTypes> ApplicationTypes = new List<ApplicationTypes>();
        public bool ShowAll { get; set; }
        public BasicFolderDefinition BasicFolderDefinition { get; set; }
        public Folder Folder { get; set; }
        public bool AllowToSelectSubfolders { get; set; }

        public ExplorerLocation() 
        {
        }

        public override string ToString()
        {
            string text = string.Empty;
            foreach (ApplicationTypes at in this.ApplicationTypes)
            {
                text += at.ToString() + ",";
            }

            if (string.IsNullOrEmpty(text) == false)
            {
                text = text.Substring(0, text.Length - 1) + " - ";
            }

            if (this.BasicFolderDefinition != null)
            {
                text += this.BasicFolderDefinition.FolderUrl;
            }

            return text;
        }
    }
}
