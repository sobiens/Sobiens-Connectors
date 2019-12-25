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
        public string Name { get; set; }
        public int RowLimit { get; set; }
        public List<CamlFieldRef> ViewFields { get; set; }
        public string Schema { get; set; }
        public string Content { get; set; }

        public override bool Equals(object value)
        {
            SQLView view = value as SQLView;

            return (view != null)
                && (Content == view.Content);
        }
    }
}
