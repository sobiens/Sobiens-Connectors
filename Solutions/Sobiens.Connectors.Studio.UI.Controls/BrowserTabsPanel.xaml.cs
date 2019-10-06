using Sobiens.Connectors.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for QueriesPanel.xaml
    /// </summary>
    public partial class BrowserTabsPanel : HostControl
    {
        public BrowserTabsPanel()
        {
            InitializeComponent();
        }

        public ClosableBrowserTab AddNewTab(string title, string url)
        {
            ClosableBrowserTab theTabItem = new ClosableBrowserTab();
            BrowserTabsControl.Items.Add(theTabItem);
            theTabItem.Initialize(title, url);
            theTabItem.Focus();
            return theTabItem;
        }

        public ClosableBrowserTab ActiveQueryPanel
        {
            get
            {
                if (BrowserTabsControl.SelectedItem != null)
                    return (ClosableBrowserTab)BrowserTabsControl.SelectedItem;

                return null;
            }
        }

        public List<ClosableBrowserTab> Tabs
        {
            get
            {
                List<ClosableBrowserTab> tabs = new List<ClosableBrowserTab>();
                foreach (object item in BrowserTabsControl.Items)
                {
                    tabs.Add(item as ClosableBrowserTab);
                }

                return tabs;
            }
        }

        public IQueryPanel GetQueryPanel(Guid id) 
        {
            foreach(IQueryPanel queryPanel in this.Tabs)
            {
                if (queryPanel.ID.Equals(id) == true)
                    return queryPanel;
            }

            return null;
        }
    }
}
