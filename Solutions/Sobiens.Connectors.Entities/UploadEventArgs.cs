using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Entities
{
    public class UploadEventArgs : EventArgs
    {
        public UploadItem UploadItem { get; set; }
        public Sobiens.Connectors.Entities.Interfaces.IItem UploadedItem { get; set; }
        public string ErrorMessage { get; set; }
        public IConnectorExplorer ConnectorExplorer { get; set; }
        public bool CanUpdateItemInGrid { get; set; }
    }
}
