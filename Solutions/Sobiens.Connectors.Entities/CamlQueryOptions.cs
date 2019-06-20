using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class CamlQueryOptions
    {
        public CamlQueryOptions()
        {
            this.IncludeMandatoryColumns = true;
            this.DateInUtc = true;
            this.RowLimit = 100;
        }
        public string Folder { get; set; }
        public string Scope { get; set; }
        public int? RowLimit { get; set; }
        public string ListItemCollectionPositionNext { get; set; }
        public bool IncludeMandatoryColumns { get; set; }
        public bool DateInUtc { get; set; }
    }
}
