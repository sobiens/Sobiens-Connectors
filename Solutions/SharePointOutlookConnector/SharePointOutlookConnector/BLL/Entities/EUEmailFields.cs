using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public enum EUEmailFields
    {
        [Title("Not Selected")]
        [Type(typeof(string))]
        NotSelected,
        [Title("Subject")]
        [Type(typeof(string))]
        Subject,
        [Title("SenderEmailAddress")]
        [Type(typeof(string))]
        SenderEmailAddress,
        [Title("To")]
        [Type(typeof(string))]
        To,
        [Title("CC")]
        [Type(typeof(string))]
        CC,
        [Title("BCC")]
        [Type(typeof(string))]
        BCC,
        [Title("Body")]
        [Type(typeof(string))]
        Body,
        [Title("SentOn")]
        [Type(typeof(DateTime))]
        SentOn,
        [Title("ReceivedTime")]
        [Type(typeof(DateTime))]
        ReceivedTime
    }

    public class TitleAttribute : Attribute
    {
        private readonly string title;
        public TitleAttribute(string title) { this.title = title; }
        public override string ToString()
        {
            return title;
        }
    }
    public class TypeAttribute : Attribute
    {
        private readonly Type type;
        public TypeAttribute(Type type) { this.type = type; }
        public override string  ToString()
        {
            return type.ToString();
        }
    }
}
