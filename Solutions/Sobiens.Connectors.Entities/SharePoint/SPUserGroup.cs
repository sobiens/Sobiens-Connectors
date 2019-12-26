
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using System.Xml.Serialization;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.SharePoint
{
#if General
    [Serializable]
#endif
    /// <summary>
    /// SharePoint Foundation Web site
    /// </summary>
    public class SPUserGroup:Folder
    {
        public SPUserGroup() { }
        public SPUserGroup(int id, string title)
        { 
            this.ID = id;
            this.Title = title;
        }

        public int ID  { get; set; }
        public override string GetUrl() { throw new NotImplementedException(); }
        public override string GetPath() { throw new NotImplementedException(); }
        public override string GetListName() { throw new NotImplementedException(); }
        public override string GetRoot() { throw new NotImplementedException(); }
        public override string GetWebUrl() { throw new NotImplementedException(); }
        public override string ToString()
        {
            return this.Title;
        }
    }
}
