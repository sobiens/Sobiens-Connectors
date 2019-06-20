using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.Controls.EditItem;
using System.Collections.Specialized;
using System.Xml;

namespace Sobiens.Office.SharePointOutlookConnector.Forms
{
    public partial class ActivateForm : Form
    {
        private EUFieldCollection Fields { get; set; }
        public EUCamlFilters CustomFilters = new EUCamlFilters();
        public ActivateForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            try
            {
                ActivationService.Activation actiovationService = new Sobiens.Office.SharePointOutlookConnector.ActivationService.Activation();
//                actiovationService.Url = "http://localhost:4701/Sobiens/Activation.asmx";
                actiovationService.Url = "http://www.sobiens.com/Activation.asmx";
                StringCollection clientIDs = ActivationManager.GetCurrentClientIDs();
                StringBuilder clientIDsString = new StringBuilder();
                foreach (string clientID in clientIDs)
                {
                    clientIDsString.Append(clientID + ";#");
                }
                string result = actiovationService.Activate(1, clientIDsString.ToString(), CompanyLicenceKeyTextBox.Text);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(result);
                string errorText = xDoc["Result"]["ErrorText"].InnerText;
                if (errorText != String.Empty)
                {
                    ResultLabel.Text = errorText;
                    ResultLabel.Visible = true;
                }
                else
                {
                    StringCollection generatedKeys = new StringCollection();
                    for (int i = 1; i < 5; i++)
                    {
                        generatedKeys.Add(xDoc["Result"]["ClientLicense"]["GeneratedKey" + i].InnerText);
                    }
                    ActivationManager.SaveGeneratedKeys(generatedKeys);
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = ex.Message;
                ResultLabel.Visible = true;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
