using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using System.Xml.Serialization;
using Sobiens.Connectors.Entities.SharePoint;

namespace Sobiens.Connectors.Entities.Documents
{
#if General
    [Serializable]
#endif
    public class DocumentTemplateMapping
    {
        public DocumentTemplateMapping()
        {
        }

        public Guid DocumentTemplateID { get; set; }
        public BasicFolderDefinition Folder { get; set; }
        public bool AllowToSelectSubFolders { get; set; }
        public string ContentTypeID { get; set; }
        public string ContentTypeName { get; set; }
        public string DocumentTemplateName { get; set; }

        public string ID
        {
            get
            {
                return ContentTypeID + " - " + DocumentTemplateID + " - " + Folder.FolderUrl;
            }
        }

        public string Title
        {
            get
            {
                return DocumentTemplateName + " - " + ContentTypeName + " - " + Folder.FolderUrl; 
            }
        }

    }
}
