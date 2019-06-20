using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    [Serializable]
    [XmlInclude(typeof(EUList))]
    [XmlInclude(typeof(EUWeb))]
    public class EUTreeNode
    {
        public List<EUTreeNode> Nodes = new List<EUTreeNode>();
        public object Tag = null;
        public string Text = String.Empty;
        public bool IsExpanded = false;

        public EUTreeNode() {} // keeps XmlSeralizer happy
    }
}
