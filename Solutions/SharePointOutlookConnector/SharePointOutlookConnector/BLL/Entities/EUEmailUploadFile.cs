using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS;
using Sobiens.Office.SharePointOutlookConnector.BLL;

namespace EmailUploader.BLL.Entities
{
    public class EUEmailUploadFile
    {
        public SaveFormatOverride SaveFormatOverride = SaveFormatOverride.None;
        public string FilePath = String.Empty;
        public string Subject = String.Empty;
        public string EntryID = String.Empty;
        public string Body = String.Empty;
        public DateTime? SentOn = null;
        public bool IsEmail = false;
        public EUEmailMetaData MetaData = null;

        
        //public Microsoft.Office.Interop.Outlook.MailItem MailItem = null;
        public Guid UniqueID;
        public EUFieldInformations FieldInformations;
        public List<EUEmailUploadFile> Attachments = new List<EUEmailUploadFile>();
        public bool IsListItemAndAttachment = false;

        public EUEmailUploadFile(string filePath, Microsoft.Office.Interop.Outlook.MailItem mailItem, EUFieldInformations fieldInformations, bool isListItemAndAttachment)
            : this(filePath, mailItem, null, false, SaveFormatOverride.None)
        {

        }
        public EUEmailUploadFile(string filePath, Microsoft.Office.Interop.Outlook.MailItem mailItem, EUFieldInformations fieldInformations, bool isListItemAndAttachment, SaveFormatOverride saveFormatOverride)  // JOEL JEFFERY 20110710 - added safeFormatOverride
        {
            FilePath = filePath;
            MetaData = new EUEmailMetaData(mailItem);
            // JOEL JEFFERY 20110711 -- mailItem == null? surely not?
            if (mailItem == null) 
            {
                //Subject = mailItem.Subject;
                //EntryID = mailItem.EntryID;
                //Body = mailItem.Body;
            }
            else
            {
                // JOEL JEFFERY 20110711 -- moved here
                Subject = mailItem.Subject;
                EntryID = mailItem.EntryID;
                Body = mailItem.Body;
                SentOn = mailItem.SentOn;
                IsEmail = true;
            }
            //MailItem = mailItem;
            UniqueID = Guid.NewGuid();
            FieldInformations = fieldInformations;
            IsListItemAndAttachment = isListItemAndAttachment;
            
            // JOEL JEFFERY 20110710
            SaveFormatOverride = saveFormatOverride; 

        }

        //public FieldInformation[] GetFieldInformations()
        //{
        //    return null;
        //}
    }
}
