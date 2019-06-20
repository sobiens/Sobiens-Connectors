using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface IServerObjectExplorer
    {
        //ItemCollection AllItems { get; }
        List<Folder> SelectedObjects { get; }
        Folder SelectedObject { get; }
        void Initialize();
    }
}

