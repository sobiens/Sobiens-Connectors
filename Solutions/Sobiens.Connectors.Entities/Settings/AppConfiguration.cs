using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Documents;

namespace Sobiens.Connectors.Entities.Settings
{
    public class AppConfiguration
    {
        public bool Exist { get; set; }
        public string Name { get; set; }
        public bool DetailedLogMode = true;
        public SiteSettings SiteSettings = new SiteSettings();

        public FolderSettings FolderSettings { get; set; }
        public DocumentTemplates DocumentTemplates { get; set; }
        public DocumentTemplateMappings DocumentTemplateMappings { get; set; }

        public WorkflowConfiguration WorkflowConfiguration
        {
            get;
            set;
        }

        public ExplorerConfiguration ExplorerConfiguration
        {
            get;
            set;
        }

        public ExcelConfigurations ExcelConfigurations
        {
            get;
            set;
        }

        public WordConfigurations WordConfigurations
        {
            get;
            set;
        }

        public OutlookConfigurations OutlookConfigurations
        {
            get;
            set;
        }

        public AppConfiguration()
        {
            this.FolderSettings = new FolderSettings();
            this.ExcelConfigurations = new ExcelConfigurations();
            this.ExplorerConfiguration = new ExplorerConfiguration();
            this.WordConfigurations = new WordConfigurations();
            this.OutlookConfigurations = new OutlookConfigurations();
            this.DocumentTemplates = new DocumentTemplates();
            this.DocumentTemplateMappings = new DocumentTemplateMappings();
            this.WorkflowConfiguration = new WorkflowConfiguration();
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
