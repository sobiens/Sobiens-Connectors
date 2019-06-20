using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools;
using Sobiens.Connectors.Common;
using System.Windows.Forms;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.WPF.Controls;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.OutlookConnector
{
    public partial class ManageTaskPaneRibbon
    {
        private void ManageTaskPaneRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            this.AttachmentsToSPButton.Label = Sobiens.Connectors.Entities.Languages.Translate(this.AttachmentsToSPButton.Label);
            this.ShowConnectorToggleButton.Label = Sobiens.Connectors.Entities.Languages.Translate(this.ShowConnectorToggleButton.Label);
            this.SPOC.Label = Sobiens.Connectors.Entities.Languages.Translate(this.SPOC.Label);
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
        }

        private void AttachmentsToSPButton_Click(object sender, RibbonControlEventArgs e)
        {
            Inspector inspector = this.Context as Inspector;
            if (inspector == null)
                throw new System.Exception("This is not an inspector window.");
            Outlook.MailItem mailItem = inspector.CurrentItem as Outlook.MailItem;
            if (mailItem == null)
                throw new System.Exception("This is not a mail item.");
            Sobiens.Connectors.Entities.Folder defaultAttachmentSaveFolder = ConfigurationManager.GetInstance().GetDefaultAttachmentSaveFolder();

            if (defaultAttachmentSaveFolder == null || ConfigurationManager.GetInstance().DontAskSaveAttachmentLocation == false)
            {
                SiteContentSelections siteContentSelections = new SiteContentSelections();
                SiteSettings siteSettings = ConfigurationManager.GetInstance().GetSiteSettings();
                List<ExplorerLocation> locations = ConfigurationManager.GetInstance().GetExplorerLocations(Sobiens.Connectors.Common.ApplicationContext.Current.GetApplicationType());
                List<Sobiens.Connectors.Entities.Folder> folders = ConfigurationManager.GetInstance().GetFoldersByExplorerLocations(locations, false);
                siteContentSelections.InitializeForm(siteSettings, folders, true, null);
                if (defaultAttachmentSaveFolder != null)
                {
                    siteContentSelections.SetSelectedFolder(defaultAttachmentSaveFolder);
                }

                if (siteContentSelections.ShowDialog(null, Languages.Translate("Select attachment upload location")) == false)
                {
                    return;
                }

                if (siteContentSelections.SelectedFolder == null)
                {
                    return;
                }

                defaultAttachmentSaveFolder = siteContentSelections.SelectedFolder;
            }

            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(defaultAttachmentSaveFolder.SiteSettingID);
            IServiceManager serviceManagerFactory = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            
            string sourceFolder = ConfigurationManager.GetInstance().CreateATempFolder();
            List<Sobiens.Connectors.Entities.UploadItem> emailUploadFiles = new List<Sobiens.Connectors.Entities.UploadItem>();
            for (int i = mailItem.Attachments.Count; i > 0; i--)
            {
                Attachment attachment = mailItem.Attachments[i];
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

                string newFileName = string.Format("{0}_{1}.{2}", filenameWithoutExtension, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), extensionName);
                string filePath = string.Format("{0}\\{1}", sourceFolder, newFileName);
                attachment.SaveAsFile(filePath);
                Sobiens.Connectors.Entities.UploadItem uploadItem = new Entities.UploadItem();
                uploadItem.FilePath = filePath;
                uploadItem.Folder = defaultAttachmentSaveFolder;
                uploadItem.FieldInformations = new Dictionary<object, object>();

                Sobiens.Connectors.Common.ApplicationContext.Current.UploadFile(siteSetting, uploadItem, null, false, false, Upload_Success, Upload_Failed);

                string fileUrl = defaultAttachmentSaveFolder.GetUrl() + "/" + newFileName;
                if (mailItem.BodyFormat == OlBodyFormat.olFormatHTML)
                {
                    mailItem.HTMLBody = "<a href='" + fileUrl + "'>" + fileUrl + "</a><br>" + mailItem.HTMLBody;
                }
                else
                {
                    mailItem.Body = fileUrl + Environment.NewLine + mailItem.Body;
                }
                mailItem.Attachments[i].Delete();
            }

        }

        public void Upload_Success(object sender, EventArgs e)
        {

        }

        public void Upload_Failed(object sender, EventArgs e)
        {
        }

    }
}
