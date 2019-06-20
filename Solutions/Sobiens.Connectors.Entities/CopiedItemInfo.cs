using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Entities
{
    public class CopiedItemInfo
    {
        public CopiedItemInfo(ISiteSetting siteSetting, IItem item,bool move)
        {
            this.SiteSetting = siteSetting;
            this.Item = item;
            this.Move = move;
        }

        public bool Move = false;
        public ISiteSetting SiteSetting { get; private set; }
        public IItem Item { get; private set; }
    }
}
