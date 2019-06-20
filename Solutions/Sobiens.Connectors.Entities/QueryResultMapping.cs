using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class QueryResultMapping
    {
        public QueryResult QueryResult { get; set; }
        public QueryResultMappingSelectField[] SelectFields { get; set; }
        public string SourceFilterField { get; set; }
        public string DestinationFilterField { get; set; }
        public QueryResultMapping()
        {
            SelectFields = new QueryResultMappingSelectField[] { };
        }
    }
}
