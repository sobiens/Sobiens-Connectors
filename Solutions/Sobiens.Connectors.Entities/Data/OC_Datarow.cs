using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sobiens.Connectors.Entities.Data
{
    [Serializable]
    public class OC_Datarow : DataRow
    {
        public object Tag { get; set; }

        public OC_Datarow()
            : base(null)
        {
        }

        public OC_Datarow(DataRowBuilder builder)
            : base(builder)
        {
        }
    }
}
