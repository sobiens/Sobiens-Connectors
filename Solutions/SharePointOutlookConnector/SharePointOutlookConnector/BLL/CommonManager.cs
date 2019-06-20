using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using EmailUploader.BLL.Entities;
using System.IO;
using Microsoft.Office.Interop.Outlook;
using System.Windows.Forms;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public enum SaveFormatOverride { None, Word, Email }
    public class CommonManager
    {

        public static void GetFileNameAndExtension(string fileFullPath, out string filenameWithoutExtension, out string fileExtension)
        {
            fileExtension = String.Empty;
            filenameWithoutExtension = String.Empty;
            if (fileFullPath.LastIndexOf(".") > -1)
            {
                fileExtension = fileFullPath.Substring(fileFullPath.LastIndexOf(".") + 1);
                filenameWithoutExtension = fileFullPath.Substring(0, fileFullPath.LastIndexOf("."));
            }
            else
            {
                filenameWithoutExtension = fileFullPath;
            }
        }

        public static EUEmailFields GetEUEmailField(string value)
        {
            if (EUEmailFields.BCC.ToString() == value)
                return EUEmailFields.BCC;
            else if (EUEmailFields.Body.ToString() == value)
                return EUEmailFields.Body;
            else if (EUEmailFields.CC.ToString() == value)
                return EUEmailFields.CC;
            else if (EUEmailFields.ReceivedTime.ToString() == value)
                return EUEmailFields.ReceivedTime;
            else if (EUEmailFields.SenderEmailAddress.ToString() == value)
                return EUEmailFields.SenderEmailAddress;
            else if (EUEmailFields.SentOn.ToString() == value)
                return EUEmailFields.SentOn;
            else if (EUEmailFields.Subject.ToString() == value)
                return EUEmailFields.Subject;
            else if (EUEmailFields.To.ToString() == value)
                return EUEmailFields.To;
            return EUEmailFields.NotSelected;
        }

        public static List<EUEmailUploadFile> GetEmailUploadFiles(List<Microsoft.Office.Interop.Outlook.MailItem> emailItems, DragEventArgs e, bool isListItemAndAttachmentMode, SaveFormatOverride saveFormatOverride) // JOEL JEFFERY 20110708 added SaveFormatOverride format
        {
            List<EUEmailUploadFile> emailUploadFiles = new List<EUEmailUploadFile>();
            string sourceFolder = EUSettingsManager.GetInstance().CreateATempFolder();
            if (e.Data.GetDataPresent("RenPrivateSourceFolder") == false)
            {
                MemoryStream memoryStream = (MemoryStream)e.Data.GetData("FileGroupDescriptor");
                memoryStream.Seek(76, SeekOrigin.Begin);

                byte[] fileName1 = new byte[256];
                memoryStream.Read(fileName1, 0, 256);

                System.Text.Encoding encoding = System.Text.Encoding.ASCII;
                string fileName = encoding.GetString(fileName1);
                fileName = fileName.TrimEnd(new char[] { '\0' });
                string extensionName = String.Empty;
                string filenameWithoutExtension = String.Empty;
                CommonManager.GetFileNameAndExtension(fileName, out filenameWithoutExtension, out extensionName);
                string filePath = SharePointManager.GetUnuqieFileName(sourceFolder, filenameWithoutExtension, extensionName, out fileName);
                memoryStream = (MemoryStream)e.Data.GetData("FileContents");
                FileStream fs = new FileStream(filePath, FileMode.Create);
                memoryStream.WriteTo(fs);
                fs.Close();
                emailUploadFiles.Add(new EUEmailUploadFile(filePath, emailItems[0], null, isListItemAndAttachmentMode));
            }
            else
            {
                for (int i = 0; i < emailItems.Count; i++)
                {
                    Microsoft.Office.Interop.Outlook.MailItem item = emailItems[i] as Microsoft.Office.Interop.Outlook.MailItem;
                    if (item != null)
                    {
                        string subject = (item.Subject == null ? "Email Message" : item.Subject);
                        //if we are to upload the email as a .msg
                        if (saveFormatOverride == SaveFormatOverride.Email || (saveFormatOverride == SaveFormatOverride.None && EUSettingsManager.GetInstance().Settings.SaveAsWord == false && isListItemAndAttachmentMode == false)) // JOEL JEFFERY 20110708 added format == SaveFormatOverride check
                        {
                            string fileName = subject + ".msg";
                            string filePath = SharePointManager.GetUnuqieFileName(sourceFolder, item.Subject, "msg", out fileName);
                            item.SaveAs(filePath, OlSaveAsType.olMSG);
                            emailUploadFiles.Add(new EUEmailUploadFile(filePath, item, null, isListItemAndAttachmentMode));
                        }
                        else //if we are to upload the email as .doc and attachments
                        {
                            string fileName = String.Empty;
                            string filePath = SharePointManager.GetUnuqieFileName(sourceFolder, subject, "doc", out fileName);
                            item.SaveAs(filePath, OlSaveAsType.olDoc);
                            EUEmailUploadFile emailUploadFile = new EUEmailUploadFile(filePath, item, null, isListItemAndAttachmentMode, saveFormatOverride);
                            foreach (Attachment attachment in item.Attachments)
                            {
                                string extensionName = String.Empty;
                                string filenameWithoutExtension = String.Empty;
                                CommonManager.GetFileNameAndExtension(attachment.FileName, out filenameWithoutExtension, out extensionName);
                                filePath = SharePointManager.GetUnuqieFileName(sourceFolder, filenameWithoutExtension, extensionName, out fileName);
                                attachment.SaveAsFile(filePath);
                                emailUploadFile.Attachments.Add(new EUEmailUploadFile(filePath, item, null, isListItemAndAttachmentMode, saveFormatOverride));
                            }
                            emailUploadFiles.Add(emailUploadFile);
                        }
                    }
                }
            }
            return emailUploadFiles;
        }

    }
}
