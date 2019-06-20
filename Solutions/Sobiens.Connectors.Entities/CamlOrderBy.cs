using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class CamlOrderBy
    {
        public CamlOrderBy() { }

        public string FieldName { get; set; }
        public bool IsAsc { get; set; }
    }
}
