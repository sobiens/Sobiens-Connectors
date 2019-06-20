using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUEmailMetaData
    {
        public string Subject = string.Empty;
        public string Body = string.Empty;
        public string BCC = string.Empty;
        public string CC = string.Empty;
        public DateTime ReceivedTime ;
        public string SenderEmailAddress = string.Empty;
        public DateTime SentOn ;
        public string To = string.Empty;

        public EUEmailMetaData(Microsoft.Office.Interop.Outlook.MailItem emailItem)
        {
                BCC = emailItem.BCC;
                Body = emailItem.Body;
                CC = emailItem.CC;
                ReceivedTime = emailItem.ReceivedTime;
                SenderEmailAddress = emailItem.SenderEmailAddress;
                SentOn = emailItem.SentOn;
                Subject = emailItem.Subject;
                To = emailItem.To;
        }
    }
}
