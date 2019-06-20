using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Outlook;
using EmailUploader.BLL.Entities;
using Outlook = Microsoft.Office.Interop.Outlook;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using System.Windows.Forms;
using System.IO;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities.FileSystem;
using Sobiens.Office.SharePointOutlookConnector.Forms;
using EmailUploader.BLL;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities.Gmail;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public class EUEmailManager
    {
        // JOEL JEFFERY 20110711
        /// <summary>
        /// Occurs when [upload failed].
        /// </summary>
        public static event EventHandler UploadFailed;
        // JON SILVER JULY 2011
        /// <summary>
        /// Occurs when [upload succeeded].
        /// </summary>
        public static event EventHandler UploadSucceeded;

        // JON SILVER JULY 2011
        /// <summary>
        /// Handles the UploadSucceeded event of the EUEmailManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void EUEmailManager_UploadSucceeded(object sender, EventArgs e)
        {
            if (UploadSucceeded != null)
                UploadSucceeded(sender, e);

        }

        // JOEL JEFFERY 20110712
        /// <summary>
        /// Handles the UploadFailed event of the EUEmailManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void EUEmailManager_UploadFailed(object sender, EventArgs e)
        {
            if (UploadFailed != null)
                UploadFailed(sender, e);
        }

        // JOEL JEFFERY 20110712
        private static bool addedEventHandler = false;

        public static void UploadEmail(SharePointListViewControl listviewControl, ISPCFolder dragedFolder, DragEventArgs e, List<EUEmailUploadFile> emailUploadFiles, bool isListItemAndAttachmentMode)
        {
            try
            {
                EUFieldInformations fieldInformations = null;
                EUFieldCollection fields = null;

                UploadProgressForm uploadProgressForm = new UploadProgressForm();
                if (EUSettingsManager.GetInstance().Settings == null)
                {
                    MessageBox.Show("You need to configure settings first.");
                    SettingsForm settingsControl = new SettingsForm();
                    settingsControl.ShowDialog();
                    return;
                }

                if (dragedFolder as EUFolder != null)
                {
                    EUFolder dragedSPFolder = dragedFolder as EUFolder;
                    List<EUContentType> contentTypes = SharePointManager.GetContentTypes(dragedSPFolder.SiteSetting, dragedSPFolder.WebUrl, dragedSPFolder.ListName);
                    fields = SharePointManager.GetFields(dragedSPFolder.SiteSetting, dragedSPFolder.WebUrl, dragedSPFolder.ListName);
                    ListItemEditForm listItemEditForm = new ListItemEditForm();
                    listItemEditForm.InitializeForm(dragedSPFolder, null);
                    listItemEditForm.ShowDialog();

                    if (listItemEditForm.DialogResult != DialogResult.OK)
                    {
                        return;
                    }
                    foreach(EUEmailUploadFile emailUploadFile in emailUploadFiles)
                    {
                        emailUploadFile.FieldInformations = listItemEditForm.FieldInformations;
                    }
                }

                string sourceFolder = EUSettingsManager.GetInstance().CreateATempFolder();

                if (EUSettingsManager.GetInstance().Settings.UploadAutomatically == true || dragedFolder as FSFolder != null || dragedFolder as GFolder != null)
                {
                    if (listviewControl != null)
                    {
                        for (int i = 0; i < emailUploadFiles.Count; i++)
                        {
                            EUEmailUploadFile emailUploadFile = emailUploadFiles[i];
                            listviewControl.LibraryContentDataGridView.Rows.Insert(i, 1);
                            listviewControl.LibraryContentDataGridView.Rows[i].Tag = emailUploadFile.UniqueID.ToString();
                            if (dragedFolder as EUFolder != null)
                            {
                                listviewControl.LibraryContentDataGridView.Rows[i].Cells["ExtensionImageColumn"].Value = Sobiens.Office.SharePointOutlookConnector.Properties.Resources.ajax_loader;
                            }
                            string title = emailUploadFile.FilePath.Split('\\')[emailUploadFile.FilePath.Split('\\').Length - 1];
                            listviewControl.LibraryContentDataGridView.Rows[i].Cells["TitleColumn"].Value = title;
                        }
                    }
                    // JOEL JEFFERY 20110712 Hook up the UploadFailed event
                    // JON SILVER JULY 2011 Hook up the UploadSucceeded event
                    if (!addedEventHandler)
                    {
                        OutlookConnector.GetConnector(dragedFolder.SiteSetting).UploadFailed += new EventHandler(EUEmailManager_UploadFailed);
                        OutlookConnector.GetConnector(dragedFolder.SiteSetting).UploadSucceeded += new EventHandler(EUEmailManager_UploadSucceeded);
                        addedEventHandler = true;
                    }

                    OutlookConnector.GetConnector(dragedFolder.SiteSetting).UploadFiles(dragedFolder, emailUploadFiles, fields, fieldInformations, listviewControl);
                }
                else
                {
                    uploadProgressForm.Initialize(dragedFolder, sourceFolder, emailUploadFiles, isListItemAndAttachmentMode, fieldInformations);
                    uploadProgressForm.ShowDialog();
                }
            }
            catch (System.Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }
    }
}
