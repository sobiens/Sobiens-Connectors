using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for QueryResultControl.xaml
    /// </summary>
    public partial class QueryGeneratorControl : UserControl
    {
        private QueryResultMappings InitialQueryResultMappings = null;
        public List<QueryResult> QueryResults
        {
            get
            {
                List<QueryResult> queryResults = new List<QueryResult>();
                queryResults.Add(SourceQueryFilterGeneratorControl.QueryResult);
                return queryResults;
            }
        }

        public List<QueryResultMappingSelectField[]> QueryResultMappingSelectFields
        {
            get
            {
                List<QueryResultMappingSelectField[]> array = new List<QueryResultMappingSelectField[]>();
                array.Add(SourceQueryFilterGeneratorControl.QueryResultMappingSelectFields);
                return array;
            }
        }

        //public Folder SourceFolder = null;
        public QueryGeneratorControl()
        {
            InitializeComponent();
        }

        public void Initialize(QueryResultMappings queryResultMappings)
        {
            InitialQueryResultMappings = queryResultMappings;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(InitialQueryResultMappings != null)
                SourceQueryFilterGeneratorControl.Initialize(InitialQueryResultMappings.Mappings[0].QueryResult, InitialQueryResultMappings.Mappings[0].SelectFields);
        }
        /*
private void SourceSelectButton_Click(object sender, RoutedEventArgs e)
{
   SelectEntityForm selectEntityForm = new SelectEntityForm();
   selectEntityForm.Initialize(new Type[] { typeof(SPList) });
   HostControl hc = ((HostControl)this.Parent);

   if (selectEntityForm.ShowDialog(hc.ParentWindow, "Select an object to sync from") == true)
   {
       SourceFolder = selectEntityForm.SelectedObject;
       SourceSelectButton.Content = SourceFolder.GetPath();
   }
}
*/
    }
}
