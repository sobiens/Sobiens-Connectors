
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
    public class CRMBusinessUnit : Folder
    {
        public string Name { get; set; }

        public CRMBusinessUnit() : base() { }
        public CRMBusinessUnit(string uniqueIdentifier, string name)
        {
            UniqueIdentifier = uniqueIdentifier;
            Name = name;
            Title = name;
        }

        public override string GetUrl() { throw new NotImplementedException(); }
        public override string GetPath() { throw new NotImplementedException(); }
        public override string GetListName() { throw new NotImplementedException(); }
        public override string GetRoot() { throw new NotImplementedException(); }
        public override string GetWebUrl() { throw new NotImplementedException(); }
    }
}
