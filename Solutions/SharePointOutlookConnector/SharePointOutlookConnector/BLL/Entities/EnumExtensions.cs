using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public static class EnumExtensions
    {
        public static string GetTitle(this TitleAttribute enumeration)
        {
            var type = enumeration.GetType();
            var field = type.GetField(enumeration.ToString());
            var enumString = (from attribute in field.GetCustomAttributes(true) where attribute is TitleAttribute select attribute).FirstOrDefault();
            if (enumString != null)
                return enumString.ToString();
            return enumeration.ToString();
        }
        public static string GetEmailFieldTypeName(this TypeAttribute enumeration)
        {
            var type = enumeration.GetType();
            var field = type.GetField(enumeration.ToString());
            var enumString = (from attribute in field.GetCustomAttributes(true) where attribute is TypeAttribute select attribute).FirstOrDefault();
            if (enumString != null)
                return enumString.ToString();
            return enumeration.ToString();
        }
    }
}
