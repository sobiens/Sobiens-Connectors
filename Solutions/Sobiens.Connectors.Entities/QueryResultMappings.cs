using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class QueryResultMappings
    {
        public List<QueryResultMapping> Mappings { get; set; }
        public QueryResultMappings()
        {
            Mappings = new List<QueryResultMapping>();
        }

    }
}
