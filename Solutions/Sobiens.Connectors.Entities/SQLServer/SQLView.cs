using Sobiens.Connectors.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace Sobiens.Connectors.Entities.SQLServer
{
#if General
    [Serializable]
#endif
    public class SQLView: SQLFolder, IView
    {
        public SQLView() : base() { }
        public SQLView(string title, Guid siteSettingID, string uniqueIdentifier)
            : base(siteSettingID, uniqueIdentifier, title)
        {
        }
        public int RowLimit { get; set; }
        public List<CamlFieldRef> ViewFields { get; set; }

        public override bool Equals(object value)
        {
            SQLView view = value as SQLView;

            return (view != null)
                && (ToSQLSyntax() == view.ToSQLSyntax());
        }

        public override string IconName
        {
            get
            {
                return "View";
            }
        }
    }
}
