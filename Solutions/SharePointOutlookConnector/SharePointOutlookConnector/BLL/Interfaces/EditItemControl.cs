using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using System.Windows.Forms;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces
{
    public class EditItemControl : UserControl
    {
        protected bool _IsLoaded = false;
        public bool IsLoaded
        {
            get
            {
                return _IsLoaded;
            }
        }

        public EUField Field { get; set; }
        public EUFolder RootFolder { get; set; }
        public EUListItem ListItem { get; set; }
        private EUListSetting ListSetting { get; set; }
        public EUEmailFields EmailMappingField { get; set; }

        public virtual void InitializeControl(EUField field, EUFolder rootFolder, EUListSetting listSetting, EUListItem listItem)
        {
            Field = field;
            RootFolder = rootFolder;
            ListItem = listItem;
            ListSetting = listSetting;
            if (EmailMappingPropertyName != null && EmailMappingPropertyName != String.Empty)
            {
                EmailMappingField = CommonManager.GetEUEmailField(EmailMappingPropertyName);
            }
        }

        public virtual bool IsValid
        {
            get
            {
                return true;
            }
        }

        public virtual void SetEmailMappingField()
        {
            throw new Exception("this method is not implemented yet");
        }

        public virtual string GetValueFromListItemOrDefault()
        {
            if (ListItem != null)
            {
                if (ListItem.Properties["ows_" + Field.Name] != null)
                    return ListItem.Properties["ows_" + Field.Name].Value;
                else
                    return String.Empty;
            }
            else
            {
                return this.Field.DefaultValue;
            }
        }

        public virtual string Value
        {
            get
            {
                return String.Empty;
            }
        }

        public string EmailMappingPropertyName
        {
            get
            {
                EUEmailMapping emailMapping = ListSetting.EmailMappings.SingleOrDefault(em => em.SharePointFieldName == Field.Name);
                if(emailMapping != null)
                {
                    return emailMapping.OutlookFieldName.Trim();
                }
                return String.Empty;
            }
        }
    }
}
