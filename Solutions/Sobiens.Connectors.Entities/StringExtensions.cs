using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sobiens.Connectors.Entities
{
    public static class StringExtensions
    {
        public static string CombineUrl(this string url1, string url2)
        {
            string uri1 = url1.TrimEnd('/');
            string uri2 = url2.TrimStart('/');
            return string.Format("{0}/{1}", uri1, uri2);
        }
        public static string absoluteTorelative(this string URL,string parentURL)
        {
            if (URL.StartsWith(parentURL, StringComparison.OrdinalIgnoreCase))
                return URL.Substring(parentURL.Length);
            else
                return URL;
        }
        public static string cleanSharePointField(this string text)
        {
            string[] txtTab = text.ToString().Split(new string[] { "w|", ";#" }, StringSplitOptions.None);
            return txtTab[txtTab.Length - 1];
        }
        public static string removeTextInsideParenthesis(this string text)
        {
            return Regex.Replace(text, @" ?\(.*?\)", "");
        }
    }
}
