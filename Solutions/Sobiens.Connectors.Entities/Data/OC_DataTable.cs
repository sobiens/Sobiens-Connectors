using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;

namespace Sobiens.Connectors.Entities.Data
{
    public class OC_DataTable : DataTable
    {
        public OC_DataTable()
            : base()
        {
        }

        public OC_DataTable(string tableName)
            : base(tableName)
        {
        }

        public OC_DataTable(string tableName, string tableNamespace)
            : base(tableName, tableNamespace)
        {
        }

        /// <summary>
        /// Needs using System.Runtime.Serialization;
        /// </summary>
        public OC_DataTable(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        protected override Type GetRowType()
        {
            return typeof(OC_Datarow);
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new OC_Datarow(builder);
        }
    }
}
