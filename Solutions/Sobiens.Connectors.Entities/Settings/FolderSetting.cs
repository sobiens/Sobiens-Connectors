using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Settings
{
    public class FolderSetting
    {
        public ApplicationTypes ApplicationType { get; set; }
        public BasicFolderDefinition BasicFolderDefinition { get; set; }
        public Folder Folder { get; set; }
        public ItemPropertyMappings ItemPropertyMappings = new ItemPropertyMappings();
        public override string ToString()
        {
            if (Folder == null)
                return Languages.Translate("Default Settings");
            return Folder.GetPath();
        }
        public FolderSetting() { } // keeps XmlSeralizer happy
    }
}
