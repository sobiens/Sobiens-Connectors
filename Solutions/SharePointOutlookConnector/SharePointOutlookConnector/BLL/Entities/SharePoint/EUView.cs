using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    [Serializable]
    public class EUView:ISPCView
    {
        public EUView(EUSiteSetting siteSetting)
        {
            SiteSetting = siteSetting;
            RowLimit = 100;
            ViewFields = new List<EUCamlFieldRef>();
        }
        public string UniqueIdentifier { get; set; }
        public string Name { get; set; }
        public int RowLimit { get; set; }
        public bool DefaultView = false;
        public bool MobileView = false;
        public bool MobileDefaultView = false;
        public string Type = String.Empty;
        public string DisplayName = String.Empty;
        public string Url = String.Empty;
        public EUSiteSetting SiteSetting { get; set; }
        public string Level = String.Empty;
        public string BaseViewID = String.Empty;
        public string ContentTypeID = String.Empty;
        public string ImageUrl = String.Empty;
        public string ListName = String.Empty;
        public string WebURL = String.Empty;
        public string _QueryXML = String.Empty;
        public string WhereXML = String.Empty;
        public string QueryXML
        {
            get
            {
                return _QueryXML;
            }
            set
            {
                /*
                 <Query xmlns="http://schemas.microsoft.com/sharepoint/soap/">
                   <OrderBy><FieldRef Name="ID" /></OrderBy>
                   <Where><Eq><FieldRef Name="Title" /><Value Type="Text">UK</Value></Eq></Where>
                 </Query>
                */ 
                _QueryXML = value;
                if (_QueryXML != String.Empty)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(_QueryXML);
                    XmlElement queryElement = xDoc["Query"];
                    if (queryElement["OrderBy"] != null)
                    {
                        this.OrderBy = new List<EUCamlFieldRef>();
                        foreach (XmlNode node in queryElement["OrderBy"].ChildNodes)
                        {
                            this.OrderBy.Add(new EUCamlFieldRef(node.Attributes["Name"].Value));
                        }
                    }
                    if (queryElement["Where"] != null)
                    {
                        this.WhereXML = queryElement["Where"].InnerXml;
                    }
                }
            }
        }
        public List<EUCamlFieldRef> ViewFields { get; set; }
        public List<EUCamlFieldRef> OrderBy = new List<EUCamlFieldRef>();

        public string GetOrderByXML()
        {
            string orderByXML = "<OrderBy>";
            foreach (EUCamlFieldRef fieldRef in this.OrderBy)
            {
                orderByXML += "<FieldRef Name=\"" + fieldRef.Name + "\" />";
            }
            orderByXML += "</OrderBy>";
            return orderByXML;
        }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
