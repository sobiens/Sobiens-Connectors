using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Common
{
    public static class tools
    {
        /// <summary>
        /// remove sub string from tag
        /// </summary>
        /// <param name="txt">text to manage</param>
        /// <param name="tag">tag</param>
        /// <returns></returns>
        public static string keepBehind(string txt, string tag)
        {
            int pos = txt.IndexOf(tag) + tag.Length;
            if (pos > 1) txt = txt.Substring(pos, txt.Length - pos);
            return txt;
        }
    }
}
