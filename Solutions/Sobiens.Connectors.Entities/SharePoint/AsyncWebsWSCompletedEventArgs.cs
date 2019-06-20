using System;
using System.Net;
using System.Collections.Generic;
using Sobiens.Connectors.Entities.SharePoint;

namespace Sobiens.Connectors.Entities.SharePoint
{
    public class AsyncWebsWSCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        public List<SPWeb> Webs { get; set; }
            public AsyncWebsWSCompletedEventArgs(Exception error, bool cancelled, object userState, List<SPWeb> webs)
                : base(error, cancelled, userState)
    {
                Webs = webs;
    }
    }
}
