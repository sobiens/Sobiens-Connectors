using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.SharePoint;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for QueriesPanel.xaml
    /// </summary>
    public partial class QueriesPanel : UserControl, IQueriesPanel
    {
        public QueriesPanel()
        {
            InitializeComponent();
        }

        private IQueryPanel AddNewQueryPanel(Folder folder, ISiteSetting siteSetting, QueryPanelObject queryPanelObject)
        {
            if(this.QueryProject == null)
            {
                this.QueryProject = new QueryProjectObject();
                this.QueryProject.FolderName = Guid.NewGuid().ToString();
                this.QueryProject.Name = "New Project";
            }

            ClosableTab theTabItem = new ClosableTab();
            QueriesTabControl.Items.Add(theTabItem);
            if (queryPanelObject != null)
            {
                theTabItem.Load(queryPanelObject);
            }
            else
            {
                theTabItem.ID = Guid.NewGuid();
                string folderName = folder.Title;
                if(folder as SPFolder != null)
                {
                    string folderPath = ((SPFolder)folder).FolderPath;
                    string[] values = folderPath.Split(new char[] { '/' }, StringSplitOptions.None);
                    folderName = values[values.Length - 1];
                }
                string fileName = "CAMLQuery" + QueriesTabControl.Items.Count + " " + folderName + ".xml";
                theTabItem.Title = fileName;
                theTabItem.Initialize(fileName, siteSetting, folder);
            }
            theTabItem.Focus();
            return theTabItem;
        }

        public IQueryPanel AddNewQueryPanel(QueryPanelObject queryPanelObject) {
            return this.AddNewQueryPanel(null, null, queryPanelObject);
        }
        public IQueryPanel AddNewQueryPanel(Folder folder, ISiteSetting siteSetting)
        {
            return this.AddNewQueryPanel(folder, siteSetting, null);
        }

        public QueryProjectObject QueryProject { get; set; }

        public IQueryPanel ActiveQueryPanel
        {
            get
            {
                if (QueriesTabControl.SelectedItem != null)
                    return (IQueryPanel)QueriesTabControl.SelectedItem;

                return null;
            }
        }

        public List<IQueryPanel> QueryPanels
        {
            get
            {
                List<IQueryPanel> queryPanels = new List<IQueryPanel>();
                foreach (object item in QueriesTabControl.Items)
                {
                    queryPanels.Add(item as IQueryPanel);
                }

                return queryPanels;
            }
        }

        public IQueryPanel GetQueryPanel(Guid id) 
        {
            foreach(IQueryPanel queryPanel in this.QueryPanels)
            {
                if (queryPanel.ID.Equals(id) == true)
                    return queryPanel;
            }

            return null;
        }

        public void Save()
        {
            this.QueryProject.QueryPanels = new List<QueryPanelObject>();
            foreach (IQueryPanel queryPanel in this.QueryPanels)
            {
                this.QueryProject.QueryPanels.Add(queryPanel.GetQueryPanel());
            }

            ConfigurationManager.GetInstance().SaveProject(this.QueryProject);
        }

        public void Load()
        {
            ProjectLoadForm projectLoadForm = new ProjectLoadForm();
            //projectLoadForm.Tag = viewRelation;
//            if (projectLoadForm.ShowDialog(this.ParentWindow, "View Relation") == true)
            if (projectLoadForm.ShowDialog(null, "View Relation") == true)
            {
                this.QueryProject = projectLoadForm.Tag as QueryProjectObject;
                foreach(QueryPanelObject queryPanelObject in this.QueryProject.QueryPanels){
                    this.AddNewQueryPanel(queryPanelObject.Folder, queryPanelObject.SiteSetting, queryPanelObject);
                }
                int q = 4;
                //                    selectedTreeViewItem.Items.Add(newTreeViewItem);
            }
            //_FileName = filename;
        }

    }
}
