using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using System.Collections;

namespace Sobiens.Connectors.Entities
{
    public enum SaveFormatOverride { None, Word, Email }
    
    public class UploadItem
    {
        public SaveFormatOverride SaveFormatOverride = SaveFormatOverride.None;
        public Guid UniqueID;
        public string FilePath;
        public Folder Folder;
        //public List<Field> Fields;
        public ContentType ContentType;
        public System.Collections.Generic.Dictionary<string, object> FieldInformations;
        public List<UploadItem> Attachments = new List<UploadItem>();

        //public SharePointListViewControl SharePointListViewControl;
        /*public UploadItem(IFolder folder, string destinationFolderUrl, List<Field> fields, Hashtable fieldInformations, SharePointListViewControl sharePointListViewControl
            )
        {
            EmailUploadFile = emailUploadFile;
            DestinationFolderUrl = destinationFolderUrl;
            FieldInformations = fieldInformations;
            Folder = folder;
            Fields = fields;
            SharePointListViewControl = sharePointListViewControl;
        }*/
    }
}
