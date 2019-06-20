using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmailUploader.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUUploadItem
    {
        public EUEmailUploadFile EmailUploadFile;
        public string DestinationFolderUrl;
        public EUFieldInformations FieldInformations;
        public EUFolder Folder;
        public List<EUField> Fields;
        public SharePointListViewControl SharePointListViewControl;
        public EUUploadItem(EUFolder folder, string destinationFolderUrl, EUEmailUploadFile emailUploadFile, List<EUField> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl)
        {
            EmailUploadFile = emailUploadFile;
            DestinationFolderUrl = destinationFolderUrl;
            FieldInformations = fieldInformations;
            Folder = folder;
            Fields = fields;
            SharePointListViewControl = sharePointListViewControl;
        }
    }
}
