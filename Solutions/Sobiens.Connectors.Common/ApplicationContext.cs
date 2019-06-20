using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities;
using System.Collections;
using System.IO;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Common.Logging;
using System.Diagnostics;

namespace Sobiens.Connectors.Common
{
    public class ApplicationContext
    {
        public static ApplicationManager Current
        { 
            get; 
            private set;
        }

        public static void SetApplicationManager(ApplicationManager applicationManager)
        {
            Current = applicationManager;

            string logFilePath = ConfigurationManager.GetInstance().CreateALogFile();
            DebugTraceListener listener = new DebugTraceListener(logFilePath);
            Trace.AutoFlush = true;
            Trace.Listeners.Add(listener);
        }

    }
}
