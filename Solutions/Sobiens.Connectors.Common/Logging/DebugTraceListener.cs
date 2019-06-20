using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Sobiens.Connectors.Common.Logging
{
    public class DebugTraceListener : TextWriterTraceListener
    {
        public DebugTraceListener(string fileName) : base(fileName) { }

        public override void Write(string message)
        {
            base.Write(message);
        }

        public override void WriteLine(object o)
        {
            base.WriteLine(o);
        }

        public override void WriteLine(string message)
        {
            base.WriteLine(message);
        }

        public override void WriteLine(object o, string category)
        {
            if (category.Equals("info", StringComparison.InvariantCultureIgnoreCase) == false || ConfigurationManager.GetInstance().GetLogMode() == Entities.LogModes.Detailed)
            {
                base.WriteLine(o, category);
            }
        }

        public override void WriteLine(string message, string category)
        {
            if (category.Equals("info", StringComparison.InvariantCultureIgnoreCase) == false || ConfigurationManager.GetInstance().GetLogMode() == Entities.LogModes.Detailed)
            {
                base.WriteLine(message, category);
            }
        }

    }
}
