
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
    public class SPTerm
    {
        public SPTerm() { }
        public SPTerm(Guid id, string title, Guid termSetId, Guid? parentTermID, int lcid)
        {
            this.ID = id;
            this.Title = title;
            this.TermSetID = termSetId;
            this.ParentTermID = parentTermID;
            this.LCID = lcid;
        }

        public Guid ID  { get; set; }
        public Guid TermSetID { get; set; }
        public Guid? ParentTermID { get; set; }
        public int LCID { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
