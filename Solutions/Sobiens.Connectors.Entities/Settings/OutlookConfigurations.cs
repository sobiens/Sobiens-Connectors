using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sobiens.Connectors.Entities.Settings
{
    public class OutlookConfigurations
    {
        public bool SaveAsWord = false;
        public bool UploadAutomatically = true;

        public BasicFolderDefinition DefaultAttachmentSaveFolder { get; set; }
        public bool DontAskSaveAttachmentLocation = false;
        public OutlookConfigurations()
        {
            DefaultAttachmentSaveFolder = new BasicFolderDefinition();
        }
    }
}
