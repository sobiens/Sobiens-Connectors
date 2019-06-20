using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public enum FileExistDialogResults
    {
        NotSelected = -1,
        CopyAndReplace = 0,
        Cancel =1,
        Copy = 2,
        Skip =3,
    }
}
