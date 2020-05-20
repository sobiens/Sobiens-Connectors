
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
    public class CRMOptionSet : Folder
    {
        public string Name { get; set; }
        public Dictionary<int, string> Options { get; set; }

        public CRMOptionSet() : base() {
            this.Options = new Dictionary<int, string>();
        }
        public CRMOptionSet(string uniqueIdentifier, string name)
        {
            this.Options = new Dictionary<int, string>();
            UniqueIdentifier = uniqueIdentifier;
            Name = name;
            Title = name;
        }

        public override string GetUrl() { return string.Empty; }
        public override string GetPath() { throw new NotImplementedException(); }
        public override string GetListName() { throw new NotImplementedException(); }
        public override string GetRoot() { throw new NotImplementedException(); }
        public override string GetWebUrl() { throw new NotImplementedException(); }
    }
}
