using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.SQLServer;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SQLServer;
using Sobiens.Connectors.Studio.UI.Controls.CodeTemplates.WebAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for CodeWizardForm.xaml
    /// </summary>
    public partial class CodeWizardForm : HostControl
    {
        private List<IQueryPanel> _QueryPanels = null;
        private Folder _MainObject = null;
        public CodeWizardForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.InitializeForm();
        }

        private void InitializeForm()
        {
            this.OKButtonSelected += ViewRelationForm_OKButtonSelected;
        }

        void ViewRelationForm_OKButtonSelected(object sender, EventArgs e)
        {
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(_MainObject.SiteSettingID);
            if (siteSetting.SiteSettingType == Entities.Settings.SiteSettingTypes.SQLServer)
            {
                GenerateSQLServerCode();
            }
            else if (siteSetting.SiteSettingType == Entities.Settings.SiteSettingTypes.SharePoint)
            {
                GenerateSharePointCode();
            }
        }

        public void GenerateModelCodeFile(string path, Dictionary<string, object> parameters)
        {
            ModelClassTemplate t = new ModelClassTemplate();
            t.Session = new System.Collections.Generic.Dictionary<string, object>();
            foreach (string parameterKey in parameters.Keys)
            {
                t.Session[parameterKey] = parameters[parameterKey];
            }
            t.Initialize();
            string content = t.TransformText();
            File.WriteAllText(path, content);
        }

        public void GenerateControllerCodeFile(string path, Dictionary<string, object> parameters)
        {
            ControllerClassTemplate t = new ControllerClassTemplate();
            t.Session = new System.Collections.Generic.Dictionary<string, object>();
            foreach (string parameterKey in parameters.Keys)
            {
                t.Session[parameterKey] = parameters[parameterKey];
            }
            t.Initialize();
            string content = t.TransformText();
            File.WriteAllText(path, content);
        }

        public void GenerateWebAPIConfigCodeFile(string path, Dictionary<string, object> parameters)
        {
            WebAPIConfigClassTemplate t = new WebAPIConfigClassTemplate();
            t.Session = new System.Collections.Generic.Dictionary<string, object>();
            foreach (string parameterKey in parameters.Keys)
            {
                t.Session[parameterKey] = parameters[parameterKey];
            }
            t.Initialize();
            string content = t.TransformText();
            File.WriteAllText(path, content);
        }

        public void GenerateTaskServiceContextCodeFile(string path, Dictionary<string, object> parameters)
        {
            TaskServiceContextClassTemplate t = new TaskServiceContextClassTemplate();
            t.Session = new System.Collections.Generic.Dictionary<string, object>();
            foreach (string parameterKey in parameters.Keys)
            {
                t.Session[parameterKey] = parameters[parameterKey];
            }
            t.Initialize();
            string content = t.TransformText();
            File.WriteAllText(path, content);
        }

        public void GenerateSobyGridPageTemplateCodeFile(string path, Dictionary<string, object> parameters)
        {
            SobyGridPageTemplate t = new SobyGridPageTemplate();
            t.Session = new System.Collections.Generic.Dictionary<string, object>();
            foreach (string parameterKey in parameters.Keys)
            {
                t.Session[parameterKey] = parameters[parameterKey];
            }
            t.Initialize();
            string content = t.TransformText();
            File.WriteAllText(path, content);
        }

        public string GetTempPath()
        {
            string folderPath = System.IO.Path.GetTempPath() + "SPCamlStudio";
            if (System.IO.Directory.Exists(folderPath) == false)
                System.IO.Directory.CreateDirectory(folderPath);
//            folderPath = folderPath + "\\SPCamlStudio";
//            if (System.IO.Directory.Exists(folderPath) == false)
//                System.IO.Directory.CreateDirectory(folderPath);
            folderPath = folderPath + "\\" + Guid.NewGuid().ToString();
            if (System.IO.Directory.Exists(folderPath) == false)
                System.IO.Directory.CreateDirectory(folderPath);

            return folderPath;
        }

        void GenerateSQLServerCode()
        {
            try
            {
                //            string startPath = @"c:\project\tests";
                string zipPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Resources\SobyGrid_WebAPIExample.zip";
                string extractPath = GetTempPath();

                //            ZipFile.CreateFromDirectory(startPath, zipPath);

                ZipFile.ExtractToDirectory(zipPath, extractPath);
                string rootPath = extractPath + "\\SobyGrid_WebAPIExample\\SobyGrid_WebAPIExample";
                string modelsPath = rootPath + "\\Models";
                string controllersPath = rootPath + "\\Controllers";
                string appStartPath = rootPath + "\\App_Start";


                XmlDocument doc = new XmlDocument();
                doc.Load(rootPath + "\\SobyGrid_WebAPIExample.csproj");
                XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("msbld", "http://schemas.microsoft.com/developer/msbuild/2003");
                //            XmlNode node = xmldoc.SelectSingleNode("//msbld:Compile", ns);

                XmlNode nodeItemGroup = doc.SelectSingleNode("//msbld:Compile", ns).ParentNode;
                XmlNode contentNodeItemGroup = doc.SelectSingleNode("//msbld:Content", ns).ParentNode;

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["Tables"] = _MainObject.Folders;
                GenerateWebAPIConfigCodeFile(appStartPath + "\\WebApiConfig.cs", parameters);
                GenerateTaskServiceContextCodeFile(modelsPath + "\\TaskServiceContext.cs", parameters);

                SiteSetting siteSetting = ApplicationContext.Current.Configuration.SiteSettings[_MainObject.SiteSettingID];
                foreach (Folder folder in _MainObject.Folders)
                {
                    FieldCollection fields = ApplicationContext.Current.GetFields(siteSetting, folder);
                    FieldCollection primaryKeyFields = new FieldCollection();
                    string[] primaryKeys = ApplicationContext.Current.GetPrimaryKeys(siteSetting, folder);
                    if (primaryKeys.Count() == 0)
                        continue;

                    List<Field> primaryKeyFieldObjects = (from x in fields where primaryKeys.Contains(x.Name) select x).ToList();
                    foreach (Field field in fields)
                    {
                        
                        if (primaryKeys.Contains(field.Name))
                            field.IsPrimary = true;
                    }

                    string tableName = folder.Title;
                    parameters = new Dictionary<string, object>();
                    parameters["TableName"] = tableName;
                    parameters["Fields"] = fields;
                    GenerateModelCodeFile(modelsPath + "\\" + tableName + "Record.cs", parameters);
                    GenerateControllerCodeFile(controllersPath + "\\" + tableName + "ListController.cs", parameters);
                    GenerateSobyGridPageTemplateCodeFile(rootPath + "\\" + tableName + "Management.html", parameters);

                    XmlElement htmlElement = doc.CreateElement("Content", "http://schemas.microsoft.com/developer/msbuild/2003");
                    XmlAttribute includeAttribute = doc.CreateAttribute("Include");
                    includeAttribute.Value = tableName + "Management.html";
                    htmlElement.Attributes.Append(includeAttribute);
                    contentNodeItemGroup.AppendChild(htmlElement);

                    XmlElement modelElement = doc.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003");
                    includeAttribute = doc.CreateAttribute("Include");
                    includeAttribute.Value = "Models\\" + tableName + "Record.cs";
                    modelElement.Attributes.Append(includeAttribute);
                    nodeItemGroup.AppendChild(modelElement);

                    XmlElement newChild = doc.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003");
                    includeAttribute = doc.CreateAttribute("Include");
                    includeAttribute.Value = "Controllers\\" + tableName + "ListController.cs";
                    newChild.Attributes.Append(includeAttribute);
                    nodeItemGroup.AppendChild(newChild);
                }
                string connectionstring = (new SQLServerService()).GetConnectionString(siteSetting, ((SQLDB)_MainObject).Title);
                SaveConnectionStrings(rootPath + "\\web.config", connectionstring);

                doc.Save(rootPath + "\\SobyGrid_WebAPIExample.csproj");
                Process.Start("Explorer.exe", extractPath);
                MessageBox.Show("Code generation completed successfully.");
            }
            catch (Exception ex) {
                MessageBox.Show("Error:" + ex.Message);
            }
        }

        public void SaveConnectionStrings(string webConfigFilePath, string connectionstring)
        {
            XmlDocument xDoc = new XmlDocument();

            xDoc.Load(webConfigFilePath);

            XmlNodeList xList = xDoc.SelectNodes("/configuration/connectionStrings/add");

            foreach (XmlNode xn in xList)
            {
                XmlElement element = xn as XmlElement;

                element.SetAttribute("connectionString", connectionstring);


            }
            xDoc.Save(webConfigFilePath);
        }

        void GenerateSharePointCode()
        {
            XmlDocument xDoc = new XmlDocument();
            XmlNode contentNode = null;
            //var assembly = Assembly.Load("Sobiens.Connectors.UI");
            //var resourceName = "Sobiens.Connectors.UI.Resources.SPOnlineWebpart.dwp";
            string dwpPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Resources\SPOnlineWebpart.dwp";

            //using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            //using (StreamReader reader = new StreamReader(stream))
            //{
                string xmlString = File.ReadAllText(dwpPath);
                xDoc.LoadXml(xmlString);
                foreach (XmlNode node in xDoc.ChildNodes[1].ChildNodes)
                {
                    if (node.Name.Equals("Content", StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        contentNode = node;
                        break;
                    }
                }
            //}

            StringBuilder sb = new StringBuilder();
            foreach (object t in ViewRelationsTreeView.Items)
            {
                TreeViewItem treeviewItem = (TreeViewItem)t;
                ViewRelation viewRelation = (ViewRelation)treeviewItem.Tag;
                string appendix = viewRelation.ID.ToString().Replace('-', '_');
                sb.AppendLine("<div id='soby_GridDiv" + appendix + "'></div>");
            }
            sb.AppendLine("<script src='https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js'></script>");
            sb.AppendLine("<script language='javascript' src='https://cdn.rawgit.com/serkantsamurkas/sobiens/master/soby.spservice.js'></script>");
            sb.AppendLine("<script language='javascript' src='https://cdn.rawgit.com/serkantsamurkas/sobiens/master/soby.ui.components.js'></script>");
            sb.AppendLine("<script language='javascript'>");
            sb.Append(GenerateGridsScript(ViewRelationsTreeView.Items));
            sb.AppendLine("</script>");
            string path = GetTempPath();
            contentNode.InnerText = sb.ToString();

            xDoc.Save(path + "\\soby_test.dwp");
//            File.WriteAllText(path + "\\soby_test.dwp", sb.ToString());
            Process.Start("Explorer.exe", path);
        }

        public void Initialize(List<IQueryPanel> queryPanels, Folder mainObject) 
        {
            _QueryPanels = queryPanels;
            _MainObject = mainObject;
            ViewComboBox.Items.Clear();
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(_MainObject.SiteSettingID);
            string childFoldersCategoryName = "";
            if (_MainObject as SQLDB != null)
                childFoldersCategoryName = "Tables";
            List<Folder> subFolders = ApplicationContext.Current.GetSubFolders(siteSetting, _MainObject, null, childFoldersCategoryName);

            foreach (IQueryPanel queryPanel in _QueryPanels){
                ViewComboBox.Items.Add(new ComboBoxItem()
                {
                    Content = queryPanel.FileName,
                    Tag = queryPanel
                });
            }

            _MainObject.Folders = subFolders;
            foreach (Folder subFolder in subFolders)
            {
                AddNode(ObjectsTreeView.Items, subFolder);
            }
        }

    private void AddNode(ItemCollection itemCollection, Folder folder)
        {
            TreeViewItem rootNode = new TreeViewItem();

            DockPanel treenodeDock = new DockPanel();

            DockPanel folderTitleDock = new DockPanel();
            Image img = new Image();
            Uri uri = new Uri("/Sobiens.Connectors.Studio.UI.Controls;component/Images/" + folder.IconName + ".GIF", UriKind.Relative);
            ImageSource imgSource = new BitmapImage(uri);
            img.Source = imgSource;

            Label lbl = new Label();
            lbl.Content = folder.Title;

            folderTitleDock.Children.Add(img);
            folderTitleDock.Children.Add(lbl);
            treenodeDock.Children.Add(folderTitleDock);

            rootNode.Header = treenodeDock;
            //rootNode.Expanded += new RoutedEventHandler(rootNode_Expanded);
            rootNode.Tag = folder;
            itemCollection.Add(rootNode);

            foreach (Folder subfolder in folder.Folders)
            {
                AddNode(rootNode.Items, subfolder);
            }

            if (folder.Folders.Count > 0)
            {
                rootNode.IsExpanded = true;
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(ViewComboBox.SelectedItem ==null)
                return;

            TreeViewItem selectedTreeViewItem = null;
            IQueryPanel masterQueryPanel = null;
            Guid masterViewRelationID = Guid.Empty;
            if (ViewRelationsTreeView.SelectedItem != null)
            {
                selectedTreeViewItem = (TreeViewItem)ViewRelationsTreeView.SelectedItem;
                Guid detailQueryPanelID = ((ViewRelation)selectedTreeViewItem.Tag).DetailQueryPanelID;
                masterViewRelationID = ((ViewRelation)selectedTreeViewItem.Tag).ID;
                masterQueryPanel = ApplicationContext.Current.SPCamlStudio.QueriesPanel.GetQueryPanel(detailQueryPanelID);
                //masterQueryPanel = (IQueryPanel)selectedTreeViewItem.Tag;
            }
            ComboBoxItem selectedComboBoxItem = (ComboBoxItem)ViewComboBox.SelectedItem;
            IQueryPanel detailQueryPanel = (IQueryPanel)selectedComboBoxItem.Tag;
            ViewRelation viewRelation = ViewRelation.NewViewRelation();
            viewRelation.MasterViewRelationID = masterViewRelationID;
            viewRelation.DetailQueryPanelID = detailQueryPanel.ID;
            viewRelation.Name = detailQueryPanel.FileName;
            viewRelation.DetailSiteUrl = detailQueryPanel.AttachedObject.GetWebUrl();
            viewRelation.DetailListName = detailQueryPanel.AttachedObject.GetListName();
            if (selectedComboBoxItem.Parent == null)
                viewRelation.IsRoot = true;

            TreeViewItem newTreeViewItem = new TreeViewItem()
                {
                    Header = viewRelation.Name,
                    Tag = viewRelation
                };
            if (selectedTreeViewItem == null)
            {
                ViewRelationsTreeView.Items.Add(newTreeViewItem);
            }
            else
            {
                ViewRelationForm viewRelationForm = new ViewRelationForm();
                viewRelationForm.Initialize(masterQueryPanel, detailQueryPanel);
                viewRelationForm.Tag=viewRelation;
                if (viewRelationForm.ShowDialog(this.ParentWindow, "View Relation") == true)
                {
                    selectedTreeViewItem.Items.Add(newTreeViewItem);
                }
            }

        }

        private string GenerateGridsScript(ItemCollection items) 
        {
            StringBuilder sb = new StringBuilder();
            foreach (object t in items)
            {
                TreeViewItem treeviewItem = (TreeViewItem)t;
                ViewRelation viewRelation = (ViewRelation)treeviewItem.Tag;
                IQueryPanel queryPanel = ApplicationContext.Current.SPCamlStudio.QueriesPanel.GetQueryPanel(viewRelation.DetailQueryPanelID);
                string appendix = viewRelation.ID.ToString().Replace('-', '_') ;
                sb.AppendLine("var dataSourceBuilder" + appendix + " = new soby_CamlBuilder('" + viewRelation.DetailListName + "', '', 5, '" + viewRelation.DetailSiteUrl + "');");
                sb.AppendLine("dataSourceBuilder" + appendix + ".Filters = new CamlFilters(false);");
                sb.AppendLine("var spService" + appendix + " = new soby_SharePointService(dataSourceBuilder" + appendix + ");");
                sb.AppendLine("var sobyGrid" + appendix + " = new soby_DataGrid('#soby_GridDiv" + appendix + "', '" + viewRelation.DetailListName + "', spService" + appendix + ", 'There is no record found.')");
                List<CamlFieldRef> viewFields = queryPanel.GetViewFields();
                foreach(CamlFieldRef viewField in viewFields){
                    sb.AppendLine("dataSourceBuilder" + appendix + ".AddViewField('" + viewField.Name + "', '" + viewField.Name + "', CamlFieldTypes.Text);");
                    sb.AppendLine("sobyGrid" + appendix + ".AddColumn('" + viewField.Name + "', '" + viewField.DisplayName + "');");
                }

                foreach (object z in treeviewItem.Items)
                {
                    TreeViewItem treeviewItem1 = (TreeViewItem)z;
                    ViewRelation viewRelation1 = (ViewRelation)treeviewItem1.Tag;
                    if((from h in viewFields
                                where h.Name == viewRelation1.MasterFieldValueName
                                select h).ToList().Count == 0)
                    {
                        sb.AppendLine("dataSourceBuilder" + appendix + ".AddViewField('" + viewRelation1.MasterFieldValueName + "', '" + viewRelation1.MasterFieldValueName + "', CamlFieldTypes.Text);");
                    }
                }

                if(string.IsNullOrEmpty(viewRelation.MasterFieldDisplayName) == false)
                {
                    sb.AppendLine("sobyGrid" + viewRelation.MasterViewRelationID.ToString().Replace('-', '_') + ".AddDataRelation('" + viewRelation.MasterFieldDisplayName + "', '" + viewRelation.MasterFieldValueName + "', sobyGrid" + appendix + ".GridID, '" + viewRelation.DetailFieldName + "')");
                }
                else
                {
                    sb.AppendLine("sobyGrid" + appendix + ".Initialize(true);");
                }

                sb.Append(GenerateGridsScript(treeviewItem.Items));
            }

            return sb.ToString();
        }

    }
}
