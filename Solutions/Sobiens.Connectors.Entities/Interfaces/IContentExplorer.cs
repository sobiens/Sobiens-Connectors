using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface IContentExplorer
    {
        bool HasAnythingToDisplay { get; }
        void AddTempItemInGrid(string id, string title);
        void UpdateUploadItemInGrid(string id, IItem item);
        void UpdateUploadItemErrorInGrid(string id, IItem item);
        void UpdateItem(string id, IItem item);
        IConnectorExplorer ConnectorExplorer { get; }
        void SetConnectorExplorer(IConnectorExplorer connectorExplorer);
        void SelectFolder(Folder parentFolder, Folder folder);
        void reloadItemList();
        Folder SelectedFolder { get; set; }
    }
}
