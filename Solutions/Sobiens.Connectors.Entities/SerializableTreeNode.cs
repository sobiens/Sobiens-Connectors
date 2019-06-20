using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
#if General
    [Serializable]
#endif
    public class SerializableTreeNode
    {
        public List<SerializableTreeNode> Nodes = new List<SerializableTreeNode>();
        public object Tag = null;
        public string Text = String.Empty;
        public bool IsExpanded = false;

        public SerializableTreeNode() { }
    }
}
