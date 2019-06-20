using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Sobiens.Office.SharePointOutlookConnector.BLL;
using EmailUploader.BLL.Entities;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools;
namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class ManageTaskPaneRibbon : OfficeRibbon
    {
        public ManageTaskPaneRibbon()
        {
            InitializeComponent();
        }

        private void ManageTaskPaneRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void ShowConnectorToggleButton_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.Inspector inspector = (Outlook.Inspector)e.Control.Context;
            InspectorWrapper inspectorWrapper = Globals.ThisAddIn.InspectorWrappers[inspector];
            CustomTaskPane taskPane = inspectorWrapper.CustomTaskPane;
            if (taskPane != null)
            {
                taskPane.Visible = ((RibbonToggleButton)sender).Checked;
            }
//            Globals.ThisAddIn.TaskPane.Visible = ((RibbonToggleButton)sender).Checked;
        }

        private void AttachmentsToSPButton_Click(object sender, RibbonControlEventArgs e)
        {
            Inspector inspector = this.Context as Inspector;
            if(inspector == null)
                throw new System.Exception("This is not an inspector window.");
            Outlook.MailItem mailItem = inspector.CurrentItem as Outlook.MailItem;
            if (mailItem == null)
                throw new System.Exception("This is not a mail item.");

            if (EUSettingsManager.GetInstance().Settings.EmailAttachmentFolderUrl == String.Empty)
            {
                MessageBox.Show("Please set attachments folder first.");
                return;
            }
            string sourceFolder = EUSettingsManager.GetInstance().CreateATempFolder();
            List<EUEmailUploadFile> emailUploadFiles = new List<EUEmailUploadFile>();
            foreach (Attachment attachment in mailItem.Attachments)
            {
                string fileName = attachment.FileName;
                string extensionName = String.Empty;
                string filenameWithoutExtension = String.Empty;
                if (fileName.LastIndexOf(".") > -1)
                {
                    extensionName = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    filenameWithoutExtension = fileName.Substring(0, fileName.LastIndexOf("."));
                }
                else
                {
                    filenameWithoutExtension = fileName;
                }
                string filePath = SharePointManager.GetUnuqieFileName(sourceFolder, filenameWithoutExtension, extensionName, out fileName);
                attachment.SaveAsFile(filePath);
                emailUploadFiles.Add(new EUEmailUploadFile(filePath, mailItem, null, false));
            }
//            string siteURL = SharePointManager.GetSiteURL(
            UploadAttachmentsProgressForm uploadAttachmentsProgressForm = new UploadAttachmentsProgressForm();
            EUSiteSetting siteSetting = EUSettingsManager.GetInstance().GetSiteSetting(EUSettingsManager.GetInstance().Settings.EmailAttachmentRootWebUrl);
            string siteURL = SharePointManager.GetSiteURL(siteSetting.Url, siteSetting);
            uploadAttachmentsProgressForm.Initialize(EUSettingsManager.GetInstance().Settings.EmailAttachmentWebUrl + "/" + EUSettingsManager.GetInstance().Settings.EmailAttachmentFolderUrl.TrimStart(new char[] { '/' }), sourceFolder, siteSetting, EUSettingsManager.GetInstance().Settings.EmailAttachmentRootFolderUrl, siteURL, EUSettingsManager.GetInstance().Settings.EmailAttachmentWebUrl, EUSettingsManager.GetInstance().Settings.EmailAttachmentListName, emailUploadFiles, mailItem);
            uploadAttachmentsProgressForm.ShowDialog();
//            SetSaveAttachmentsToSharePointButton();
                    }
    }
}
