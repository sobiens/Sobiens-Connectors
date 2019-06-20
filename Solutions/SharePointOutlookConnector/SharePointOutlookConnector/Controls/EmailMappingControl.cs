using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Microsoft.Office.Interop.Outlook;

namespace Sobiens.Office.SharePointOutlookConnector.Controls
{
    public partial class EmailMappingControl : UserControl
    {
        public EmailMappingControl()
        {
            InitializeComponent();
        }

        public List<EUEmailMapping> GetEmailMappings(List<EUEmailMapping> _emailMappings)
        {
            List<EUEmailMapping> emailMappings = new List<EUEmailMapping>();

            foreach (string name in Enum.GetNames(typeof(EUEmailFields)))
            {
                if (name != EUEmailFields.BCC.ToString()) // JOEL JEFFERY 20110714 - filter out pesky BCC fields
                {
                    EUEmailFields euEmailField = (EUEmailFields)Enum.Parse(typeof(EUEmailFields), name);
                    EUEmailMapping emailMapping = new EUEmailMapping();
                    emailMapping.OutlookFieldName = euEmailField.ToString();

                    EUEmailMapping tempEmailMapping = _emailMappings.SingleOrDefault(eMapping => eMapping.OutlookFieldName == emailMapping.OutlookFieldName);
                    if (tempEmailMapping != null)
                        emailMapping.SharePointFieldName = tempEmailMapping.SharePointFieldName;

                    emailMappings.Add(emailMapping);
                }
            }
            return emailMappings;
        }

        public void BindEmailMapping(EUListSetting listSetting)
        {
            listSetting.EmailMappings = GetEmailMappings(listSetting.EmailMappings);
            EmailMappingDataGridView.DataSource = listSetting.EmailMappings;
        }
    }
}
