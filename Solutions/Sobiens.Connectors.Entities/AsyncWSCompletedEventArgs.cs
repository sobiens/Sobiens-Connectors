using System;
using System.Xml.Linq;

namespace Sobiens.Connectors.Entities
{
    public class AsyncWSCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        public XDocument XmlResult { get; set; }
        public AsyncWSCompletedEventArgs(Exception error, bool cancelled, object userState, XDocument xmlResult)
            : base(error, cancelled, userState)
        {
            XmlResult = xmlResult;
        }

    }
}
