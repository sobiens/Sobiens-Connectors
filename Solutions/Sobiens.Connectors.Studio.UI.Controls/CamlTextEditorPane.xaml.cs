using Sobiens.Connectors.Common.SQLServer;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Services.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for CamlTextEditorPane.xaml
    /// </summary>
    public partial class CamlTextEditorPane : UserControl
    {
        public CamlTextEditorPane()
        {
            InitializeComponent();
        }

        public void PopulateCamlTextEditor(CamlFilters filters, List<CamlFieldRef> viewFields, List<CamlOrderBy> orderBys, CamlQueryOptions queryOptions)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
            query.InnerXml = SPCamlManager.GetCamlString(filters, orderBys);

            XmlNode viewFieldsNode = SPCamlManager.GetViewFieldsXmlNode(viewFields);
            XmlNode queryOptionsNode = SPCamlManager.GetQueryOptionsXmlNode(queryOptions);

            QueryCamlTextBox.Text = query.OuterXml;
            ViewFieldsCamlTextBox.Text = viewFieldsNode.OuterXml;
            QueryOptionsCamlTextBox.Text = queryOptionsNode.OuterXml;
        }

        public void PopulateSQLServerTextEditor(string tableName, CamlFilters filters, List<CamlFieldRef> viewFields, List<CamlOrderBy> orderBys, CamlQueryOptions queryOptions)
        {
            string sqlString = SQLManager.GetSQLString(tableName, filters, viewFields, orderBys, queryOptions);
            QueryCamlTextBox.Text = sqlString;
            ViewFieldsTabItem.Visibility = Visibility.Hidden;
            QueryOptionsTabItem.Visibility = Visibility.Hidden;
        }


    }
}
