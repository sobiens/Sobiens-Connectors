
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
    public class SPTermStore
    {
        public SPTermStore() { }
        public SPTermStore(Guid id, string title, int lcid)
        {
            this.ID = id;
            this.Title = title;

            this.LCID = lcid;

        }

        public Guid ID  { get; set; }

        public int LCID { get; set; }
        
        public string Title { get; set; }


        public override string ToString()
        {
            return this.Title;
        }
    }
}
