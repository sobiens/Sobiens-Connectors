using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Sobiens.Connectors.Services.SharePoint
{
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ServiceName", Namespace = "http://tempuri.org/")]
    public class WebService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        protected override System.Net.WebRequest GetWebRequest(Uri uri)
        {
            HttpWebRequest request;
            request = (HttpWebRequest)base.GetWebRequest(uri);
            if (PreAuthenticate)
            {
                NetworkCredential networkCredentials = Credentials.GetCredential(uri, "Basic");
                if (networkCredentials != null)
                {
                    byte[] credentialBuffer = new UTF8Encoding().GetBytes(networkCredentials.UserName + ":" + networkCredentials.Password);
                    request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);
                }
                else
                {
                    throw new ApplicationException("No network credentials");
                }
            }
            return request;
        }
    }
}
