using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.Services.SharePoint
{
    public class SPResult : Result
    {
        public SPResult(SharePointCopyWS.CopyResult result)
        {
            this.codeResult = result.ErrorCode.ToString();
            this.messageResult = result.ErrorMessage;
            this.detailResult = "";
        }

        public SPResult(System.Web.Services.Protocols.SoapException ex)
        {
            if (ex.Detail.ChildNodes.Count > 1)
                this.codeResult = ex.Detail.ChildNodes[1].InnerText;
            this.messageResult = "Error";
            this.detailResult = ex.Detail.InnerText;
        }

    }
}
