using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface IStatusBar
    {
        void SetStatusBar(string text, int percentage);
    }
}
