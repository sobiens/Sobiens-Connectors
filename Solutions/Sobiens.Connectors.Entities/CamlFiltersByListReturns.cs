using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Search;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Entities
{
    public class CamlFilterByListReturns : CamlFilters
    {
        public bool Processed = false;
        public CamlFilterByListReturns(ISiteSetting siteSetting, string listName, CamlFilters filters)
        {

        }
    }
}
