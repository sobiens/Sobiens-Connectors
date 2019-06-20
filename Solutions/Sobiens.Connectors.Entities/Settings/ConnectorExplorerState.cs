using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using System.Xml.Serialization;

namespace Sobiens.Connectors.Entities.Settings
{
#if General
    [Serializable]
#endif
    public class ConnectorExplorerState
    {
        public List<Folder> Folders { get; set; }
        public ConnectorExplorerState()
        {
            this.Folders = new List<Folder>();
        }
    }
}
