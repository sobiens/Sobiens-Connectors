﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using System.Xml.Serialization;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.CRM
{
#if General
    [Serializable]
#endif
    /// <summary>
    /// SharePoint Foundation Web site
    /// </summary>
    public class CRMView : IView
    {
        public string UniqueIdentifier { get; set; }
        public string Name { get; set; }
        public int RowLimit { get; set; }
        public Guid SiteSettingID { get; set; }
        public List<CamlFieldRef> ViewFields { get; set; }

        public CRMView() : base() { }
        public CRMView(string uniqueIdentifier, string viewName, Guid siteSettingID, CRMEntity entity)
        {
            UniqueIdentifier = uniqueIdentifier;
            Name = viewName;
            SiteSettingID = siteSettingID;
            Entity = entity;
            RowLimit = 100;
            ViewFields = new List<CamlFieldRef>();
        }

        public CRMEntity Entity
        {
            get;set;
        }

    }
}
