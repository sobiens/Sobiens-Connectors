using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.State
{
    public class ConnectorState
    {
        public ConnectorState()
        {
            this.Outlook = new OutlookState();
            this.Word = new WordState();
            this.Excel = new ExcelState();
            this.General = new GeneralState();
        }

        public OutlookState Outlook { get; set; }
        public WordState Word { get; set; }
        public ExcelState Excel { get; set; }
        public GeneralState General { get; set; }

        public ApplicationBaseState GetApplicationState(ApplicationTypes applicationType)
        {
            switch (applicationType)
            {
                case ApplicationTypes.Excel:
                    return this.Excel;
                case ApplicationTypes.Word:
                    return this.Word;
                case ApplicationTypes.Outlook:
                    return this.Outlook;
                case ApplicationTypes.General:
                    return this.General;
            }

            throw new Exception("Related application state could not be found.");
        }
    }
}
