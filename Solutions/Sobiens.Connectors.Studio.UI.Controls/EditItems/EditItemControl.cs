using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Studio.UI.Controls.EditItems
{
    public class EditItemControl : UserControl
    {
        public Field Field { get; set; }
        public ContentType ContentType { get; set; }
        public string WebURL { get; set; }
        public ISiteSetting SiteSetting { get; set; }
        /// <summary>
        /// Not used or set yet!
        /// </summary>
        public IItem ListItem { get; set; }
        public string RootFolder { get; set; }

        public class BoolStringClass
        {
            public string TheText { get; set; }
            public string TheValue { get; set; }
        }

        public virtual void InitializeControl(string webURL, Field field, IItem listItem, ContentType contentType, ISiteSetting siteSetting, string rootFolder)
        {
            this.WebURL = webURL;
            this.Field = field;
            this.RootFolder = rootFolder;
            this.ListItem = listItem;
            this.ContentType = contentType;
            this.SiteSetting = siteSetting;
        }

        public virtual void SetEmailMappingField()
        {
            throw new Exception("this method is not implemented yet");
        }

        public virtual bool IsValid
        {
            get
            {
                return true;
            }
        }
        
        public virtual object Value
        {
            get
            {
                return String.Empty;
            }
            set
            { }
        }

        public virtual bool hasBeenModified
        {
            get
            {
                return true;
            }
        }

        public virtual double GetHeight()
        {
            return this.Height;
        }

        public virtual void TakeFocus()
        {
        }

    }
}
