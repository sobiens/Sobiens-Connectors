using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Common.SQLServer;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.SQLServer;
using Sobiens.Connectors.Services.SharePoint;
using Sobiens.Connectors.Studio.UI.Controls.CodeTemplates.SharePoint;
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
            ComponentTypeRadioButton_Checked(null, null);
        }

        void ViewRelationForm_OKButtonSelected(object sender, EventArgs e)
        {
            if (ValidateCodeGenerationForm() == false)
                return;
            int componentTypeIndex = 0;
            if (ComponentTypeListingsRadioButton.IsChecked == true)
            {
                componentTypeIndex = 0;
            }
            else if (ComponentTypeCarouselRadioButton.IsChecked == true)
            {
                componentTypeIndex = 1;
            }

            LoadingWindow.ShowDialog("Generating codes...", delegate ()
            {
                ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(_MainObject.SiteSettingID);
                if (siteSetting.SiteSettingType == Entities.Settings.SiteSettingTypes.SQLServer)
                {
                    GenerateSQLListingCode();
                }
                else if (siteSetting.SiteSettingType == Entities.Settings.SiteSettingTypes.SharePoint)
                {
                    if (componentTypeIndex == 0)
                    {
                        GenerateSPListingCode();
                    }
                    else if (componentTypeIndex == 1)
                    {
                        LoadingWindow.SetMessage("Generating carousel code...");
                        GenerateSPCarouselCode();
                    }
                }
                else if (siteSetting.SiteSettingType == Entities.Settings.SiteSettingTypes.CRM)
                {
                    //GenerateSharePointCode();
                }
            });

            MessageBox.Show("Code generation has been completed");
        }

        private bool ValidateCodeGenerationForm() 
        {
            if (_MainObject as SPWeb != null)
            {
                SiteSetting siteSetting = (SiteSetting)ApplicationContext.Current.GetSiteSetting(_MainObject.SiteSettingID);
                if (ComponentTypeListingsRadioButton.IsChecked == true)
                {
                    if (SPLOPageLibraryComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a page library");
                        SPLOPageLibraryComboBox.Focus();
                        return false;
                    }
                    if (SPLOResourceLibraryComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a resource library");
                        SPLOResourceLibraryComboBox.Focus();
                        return false;
                    }
                    if (string.IsNullOrEmpty(SPLOPageNameTextBox.Text) == true)
                    {
                        MessageBox.Show("Please enter a page name");
                        SPLOPageNameTextBox.Focus();
                        return false;
                    }
                    
                    if (SPLOSelectedListingsEntities.SelectedItems.Count ==0)
                    {
                        MessageBox.Show("Please select at least one list");
                        return false;
                    }

                    SPList pageLibrary = pageLibrary = ((SPList)SPLOPageLibraryComboBox.SelectedItem);
                    string pageName = SPLOPageNameTextBox.Text;
                    if((new SharePointService()).CheckFileExistency(siteSetting, _MainObject.GetWebUrl(), pageLibrary.Title,string.Empty, null, pageName + ".aspx") == true)
                    {
                        MessageBox.Show("This page already exists, please select a different page name");
                        SPLOPageNameTextBox.Focus();
                        return false;
                    }

                }
                else if (ComponentTypeCarouselRadioButton.IsChecked == true)
                {
                    if (SPCOPageLibraryComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please select page library");
                        SPCOPageLibraryComboBox.Focus();
                        return false;
                    }
                    if (SPCOResourceLibraryComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please select resource library");
                        SPCOResourceLibraryComboBox.Focus();
                        return false;
                    }
                    if (string.IsNullOrEmpty(SPCOPageNameTextBox.Text) == true)
                    {
                        MessageBox.Show("Please enter a page name");
                        SPCOPageNameTextBox.Focus();
                        return false;
                    }

                    if (SPCOCarouselLibraryComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please select carousel library");
                        SPCOCarouselLibraryComboBox.Focus();
                        return false;
                    }
                    if (SPCOImageFieldComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please select image field");
                        SPCOImageFieldComboBox.Focus();
                        return false;
                    }
                    if (SPCOCaptionFieldComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please select caption field");
                        SPCOCaptionFieldComboBox.Focus();
                        return false;
                    }
                    if (SPCOContentFieldComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please select content field");
                        SPCOContentFieldComboBox.Focus();
                        return false;
                    }
                    SPList pageLibrary = pageLibrary = ((SPList)SPCOPageLibraryComboBox.SelectedItem);
                    string pageName = SPCOPageNameTextBox.Text;
                    if ((new SharePointService()).CheckFileExistency(siteSetting, _MainObject.GetWebUrl(), pageLibrary.Title, string.Empty, null, pageName + ".aspx") == true)
                    {
                        MessageBox.Show("This page already exists, please select a different page name");
                        SPCOPageNameTextBox.Focus();
                        return false;
                    }
                }
                else if (ComponentTypeCalendarRadioButton.IsChecked == true)
                {
                }
            }
            else if (_MainObject as SQLDB != null)
            {
                if (ComponentTypeListingsRadioButton.IsChecked == true)
                {
                    if (SQLLOSelectedListingsEntities.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Please select at least one list");
                        return false;
                    }
                }
                else if (ComponentTypeCarouselRadioButton.IsChecked == true)
                {
                    SQLListingOptionsTabItem.Visibility = Visibility.Visible;
                }
                else if (ComponentTypeCalendarRadioButton.IsChecked == true)
                {
                    SQLListingOptionsTabItem.Visibility = Visibility.Visible;
                }
            }

            return true;
        }

        public void GenerateFileFromCodeTemplate(dynamic template, string path, Dictionary<string, object> parameters)
        {
            string codeContent = TransformCodeTemplate(template, parameters);
            File.WriteAllText(path, codeContent);
        }
        public string TransformCodeTemplate(dynamic template, Dictionary<string, object> parameters)
        {
            try
            {
                template.Session = new System.Collections.Generic.Dictionary<string, object>();
                foreach (string parameterKey in parameters.Keys)
                {
                    template.Session[parameterKey] = parameters[parameterKey];
                }
                template.Initialize();
                return template.TransformText();
            }
            catch(Exception ex)
            {
                int x = 3;
                throw ex;
            }
        }

        public string GetTempPath()
        {
            string folderPath = System.IO.Path.GetTempPath() + "SPCamlStudio";
            if (System.IO.Directory.Exists(folderPath) == false)
                System.IO.Directory.CreateDirectory(folderPath);
            folderPath = folderPath + "\\" + Guid.NewGuid().ToString();
            if (System.IO.Directory.Exists(folderPath) == false)
                System.IO.Directory.CreateDirectory(folderPath);

            return folderPath;
        }

        void GenerateSQLListingCode()
        {
            try
            {
                List<Folder> selectedTables = new List<Folder>();
                foreach (Selectors.CheckBoxList.CheckBoxListItem selectedItem in SQLLOSelectedListingsEntities.SelectedItems)
                {
                    selectedTables.Add((SQLTable)selectedItem.Value);
                }

                //string zipPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Resources\SobyGrid_WebAPIExample.zip";
                string extractPath = GetTempPath();
                string zipPath = extractPath + @"\SobyGrid_WebAPIExample.zip";
                Stream stream = Assembly.GetEntryAssembly().GetManifestResourceStream("Sobiens.Connectors.Studio.UI.Resources.SobyGrid_WebAPIExample.zip");
                FileStream fileStream = new FileStream(zipPath, FileMode.CreateNew);
                for (int i = 0; i < stream.Length; i++)
                    fileStream.WriteByte((byte)stream.ReadByte());
                fileStream.Close();

                //            ZipFile.CreateFromDirectory(startPath, zipPath);

                ZipFile.ExtractToDirectory(zipPath, extractPath);
                File.Delete(zipPath);
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
                GenerateFileFromCodeTemplate(new WebAPIConfigClassTemplate(), appStartPath + "\\WebApiConfig.cs", parameters);
                GenerateFileFromCodeTemplate(new TaskServiceContextClassTemplate(), modelsPath + "\\TaskServiceContext.cs", parameters);

                SiteSetting siteSetting = ApplicationContext.Current.Configuration.SiteSettings[_MainObject.SiteSettingID];

                parameters = new Dictionary<string, object>();
                parameters["Database"] = _MainObject;
                //parameters["SelectedTables"] = selectedTables;

                string gridComponentsCode = string.Empty;
                foreach (Folder folder in _MainObject.Folders)
                {
                    SQLTable table = (SQLTable)folder;
                    string tableName = folder.Title;
                    string schemaName = ((SQLTable)folder).Schema;
                    string fixedTableName = CodeWizardManager.FixTableNameForCode(tableName);
                    FieldCollection fields = table.Fields.GetUsableFields();// ApplicationContext.Current.GetFields(siteSetting, folder);
                                                                            //FieldCollection primaryKeyFields = new FieldCollection();
                                                                            //string[] primaryKeys = table.PrimaryKeys;
                    string gridName = fixedTableName + "Grid";
                    parameters = new Dictionary<string, object>();
                    parameters["Tables"] = _MainObject.Folders;
                    parameters["TableName"] = tableName;
                    parameters["SchemaName"] = schemaName;
                    parameters["Fields"] = fields;
                    parameters["GridName"] = gridName;
                    parameters["GridTitle"] = folder.Title;
                    parameters["GridAltTitle"] = folder.Title;
                    if (selectedTables.Contains(table) == true)
                    {
                        SQLForeignKey[] foreignKeys = table.ForeignKeys;
                        List<SQLForeignKey> relatedForeignKeys = new List<SQLForeignKey>();
                        //if (primaryKeys.Count() == 0)
                        //    continue;

                        //List<Folder> referencedTables = new List<Folder>();
                        foreach (Folder otherTable in _MainObject.Folders)
                        {
                            if (otherTable.Title.Equals(folder.Title, StringComparison.InvariantCultureIgnoreCase) == true)
                                continue;

                            SQLForeignKey[] _foreignKeys = ((SQLTable)otherTable).ForeignKeys.Where(t => t.ReferencedTableName.Equals(folder.Title, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                            if (_foreignKeys.Count() > 0)
                            {
                                //referencedTables.Add(otherTable);
                                relatedForeignKeys.AddRange(_foreignKeys);
                            }
                        }


                        //parameters["ForeignKeys"] = foreignKeys;
                        //parameters["ReferencedTables"] = referencedTables;

                        string gridComponentCode = TransformCodeTemplate(new SobyGridComponentSQLTemplate(), parameters);

                        string showFieldName = string.Empty;
                        List<Field> textFields = fields.Where(t => t.Type == FieldTypes.Text).ToList();
                        if (textFields.Count > 0)
                            showFieldName = textFields[0].Name;
                        else
                            showFieldName = fields[0].Name;
                        foreach (SQLForeignKey referencedForeignKey in relatedForeignKeys)
                        {
                            string referencedGridName = fixedTableName + "_";
                            string referencedTableColumnNames = string.Empty;
                            string tableColumnNames = string.Empty;
                            foreach (string referencedTableColumnName in referencedForeignKey.ReferencedTableColumnNames)
                            {
                                if (string.IsNullOrEmpty(referencedTableColumnNames) == false)
                                    referencedTableColumnNames += ",";

                                referencedTableColumnNames += referencedTableColumnName;
                            }

                            foreach (string tableColumnName in referencedForeignKey.TableColumnNames)
                            {
                                if (string.IsNullOrEmpty(tableColumnNames) == false)
                                    tableColumnNames += ",";

                                tableColumnNames += tableColumnName;
                                referencedGridName += "_" + tableColumnName;
                            }

                            referencedGridName += "_Grid";
                            SQLTable referencedTable = (SQLTable)_MainObject.Folders.Where(t => t.Title.Equals(referencedForeignKey.TableName, StringComparison.InvariantCultureIgnoreCase)).First();
                            FieldCollection referencedFields = referencedTable.Fields.GetUsableFields(); // ApplicationContext.Current.GetFields(siteSetting, referencedTable);
                                                                                                         //FieldCollection referencedPrimaryKeyFields = new FieldCollection();
                                                                                                         //string[] referencedPrimaryKeys = _referencedTable.PrimaryKeys;
                                                                                                         //List<Field> referencedPrimaryKeyFieldObjects = (from x in fields where referencedPrimaryKeys.Contains(x.Name) select x).ToList();

                            Dictionary<string, object> referencedTableParameters = new Dictionary<string, object>();
                            referencedTableParameters["Tables"] = _MainObject.Folders;
                            referencedTableParameters["TableName"] = referencedTable.Title;
                            referencedTableParameters["SchemaName"] = schemaName;
                            referencedTableParameters["Fields"] = referencedFields;
                            referencedTableParameters["GridName"] = referencedGridName;
                            referencedTableParameters["GridTitle"] = referencedTable.Title;
                            referencedTableParameters["GridAltTitle"] = tableColumnNames + " --- " + referencedTableColumnNames;

                            //referencedTableParameters["ForeignKeys"] = foreignKeys;
                            gridComponentCode += TransformCodeTemplate(new SobyGridComponentSQLTemplate(), referencedTableParameters);
                            gridComponentCode += "      " + gridName + ".AddDataRelation(\"" + showFieldName + "\", \"" + referencedTableColumnNames + "\", " + referencedGridName + ".GridID, \"" + tableColumnNames + "\");" + Environment.NewLine;
                        }


                        gridComponentCode = "   function soby_Populate" + fixedTableName + "() {" +
                            Environment.NewLine + gridComponentCode +
                            Environment.NewLine + "     " + gridName + ".Initialize(true);" +
                            Environment.NewLine + " }" + Environment.NewLine + Environment.NewLine;

                        gridComponentsCode += gridComponentCode;
                    }

                    GenerateFileFromCodeTemplate(new ModelClassTemplate(), modelsPath + "\\" + fixedTableName + "Record.cs", parameters);
                    if (fields.Where(t => t.IsPrimary == true).Count() > 0 && table.Fields.HasUnusableFieldsPrimary() == false)
                    {
                        GenerateFileFromCodeTemplate(new ODataControllerClassTemplate(), controllersPath + "\\" + fixedTableName + "ListController.cs", parameters);
                    }
                    else
                    {
                        GenerateFileFromCodeTemplate(new ApiControllerClassTemplate(), controllersPath + "\\" + fixedTableName + "ListController.cs", parameters);
                    }

                    //parameters["GridComponentSyntax"] = gridComponentCode;
                    XmlAttribute includeAttribute;

                    /*
                    GenerateSobyGridPageTemplateCodeFile(rootPath + "\\" + fixedTableName + "Management.html", parameters);

                    XmlElement htmlElement = doc.CreateElement("Content", "http://schemas.microsoft.com/developer/msbuild/2003");
                    includeAttribute = doc.CreateAttribute("Include");
                    includeAttribute.Value = fixedTableName + "Management.html";
                    htmlElement.Attributes.Append(includeAttribute);
                    contentNodeItemGroup.AppendChild(htmlElement);
                    */

                    XmlElement modelElement = doc.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003");
                    includeAttribute = doc.CreateAttribute("Include");
                    includeAttribute.Value = "Models\\" + fixedTableName + "Record.cs";
                    modelElement.Attributes.Append(includeAttribute);
                    nodeItemGroup.AppendChild(modelElement);

                    XmlElement newChild = doc.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003");
                    includeAttribute = doc.CreateAttribute("Include");
                    includeAttribute.Value = "Controllers\\" + fixedTableName + "ListController.cs";
                    newChild.Attributes.Append(includeAttribute);
                    nodeItemGroup.AppendChild(newChild);
                }

                parameters = new Dictionary<string, object>();
                parameters["Database"] = _MainObject;
                parameters["GridComponentsSyntax"] = gridComponentsCode;
                parameters["SelectedTables"] = selectedTables;
                GenerateFileFromCodeTemplate(new IndexSQLPageTemplate(), rootPath + "\\Index.html", parameters);

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

        void GenerateSPListingCode()
        {
            string pageName = "";
            string includeFileName = "sobysplistinginclude_" + DateTime.Now.ToString("yyyyMMddmmss") + ".html";
            SPList resourceLibrary = null; // ((SPList)SPCOResourceLibraryComboBox.SelectedItem);
            SPList pageLibrary = null;// ((SPList)SPCOPageLibraryComboBox.SelectedItem);
            List<Folder> selectedLists = new List<Folder>();
            this.Dispatcher.Invoke((Action)(() =>
            {
                resourceLibrary = ((SPList)SPLOResourceLibraryComboBox.SelectedItem);
                pageLibrary = ((SPList)SPLOPageLibraryComboBox.SelectedItem);
                pageName = SPLOPageNameTextBox.Text;
                foreach (Selectors.CheckBoxList.CheckBoxListItem selectedItem in SPLOSelectedListingsEntities.SelectedItems)
                {
                    selectedLists.Add((SPList)selectedItem.Value);
                }
            }));

            //this.ShowLoadingStatus("SharePoint code is being generated...");
            SiteSetting siteSetting = (SiteSetting)ApplicationContext.Current.GetSiteSetting(_MainObject.SiteSettingID);
            //this.ShowLoadingStatus("Generating page...");
            SharePointService.CreatePage(siteSetting, _MainObject.GetWebUrl(), pageLibrary.Title, pageName + ".aspx");
            //this.ShowLoadingStatus("Modifying page...");
            SharePointService.AddContentEditorWebpartWithContentLink(siteSetting, pageLibrary.ServerRelativePath + "/" + pageName + ".aspx", "Main",1, ((SPWeb)_MainObject).ServerRelativePath + "/Style Library/" + includeFileName);

            //this.ShowLoadingStatus("Generating page components...");
            string componentsCode = GenerateSPGridsScript(selectedLists, siteSetting);
            string filePath = GetTempPath() + "\\" + includeFileName;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["Database"] = _MainObject;
            parameters["Lists"] = selectedLists;
            parameters["GridComponentsSyntax"] = componentsCode;
            GenerateFileFromCodeTemplate(new IndexSPPageTemplate(), filePath, parameters);

            //this.ShowLoadingStatus("Generating page content...");
            UploadItem uploadItem = new UploadItem();
            uploadItem.FilePath = filePath;
            uploadItem.FieldInformations = new Dictionary<object, object>();
            uploadItem.Folder = new SPFolder()
            {
                RootFolderPath = resourceLibrary.RootFolderPath,
                FolderPath = resourceLibrary.FolderPath,
                WebUrl = siteSetting.Url,
                SiteUrl = siteSetting.Url,
                ListName = resourceLibrary.Title,
                BaseType = 1,
            };
            Entities.Interfaces.IItem item = null;
            Logger.Info("Uploading " + uploadItem.FilePath + " ...", "Service");
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            serviceManager.UploadFile(siteSetting, uploadItem, false, out item);
            //this.HideLoadingStatus("Completed.");

        }

        void GenerateSPCarouselCode()
        {
            SiteSetting siteSetting = (SiteSetting)ApplicationContext.Current.GetSiteSetting(_MainObject.SiteSettingID);
            string pageName = "";
            string includeFileName = "sobyspcarouselinclude_" + DateTime.Now.ToString("yyyyMMddmmss") + ".html";
            SPList resourceLibrary = null; // ((SPList)SPCOResourceLibraryComboBox.SelectedItem);
            SPList pageLibrary = null;// ((SPList)SPCOPageLibraryComboBox.SelectedItem);
            SPList carouselList = null;// (SPList)SPCOCarouselLibraryComboBox.SelectedItem;
            Field imageField = null;//(Field)SPCOImageFieldComboBox.SelectedItem;
            Field captionField = null;//(Field)SPCOCaptionFieldComboBox.SelectedItem;
            Field contentField = null;//(Field)SPCOContentFieldComboBox.SelectedItem;
            this.Dispatcher.Invoke((Action)(() =>
            {
                resourceLibrary = ((SPList)SPCOResourceLibraryComboBox.SelectedItem);
                pageLibrary = ((SPList)SPCOPageLibraryComboBox.SelectedItem);
                pageName = SPCOPageNameTextBox.Text;
                carouselList = (SPList)SPCOCarouselLibraryComboBox.SelectedItem;
                imageField = (Field)SPCOImageFieldComboBox.SelectedItem;
                captionField = (Field)SPCOCaptionFieldComboBox.SelectedItem;
                contentField = (Field)SPCOContentFieldComboBox.SelectedItem;
            }));
            FieldCollection fields = ApplicationContext.Current.GetFields(siteSetting, carouselList);

            //this.ShowLoadingStatus("SharePoint code is being generated...");
            //this.ShowLoadingStatus("Generating page...");
            SharePointService.CreatePage(siteSetting, _MainObject.GetWebUrl(), pageLibrary.Title, pageName + ".aspx");
            //this.ShowLoadingStatus("Modifying page...");
            SharePointService.AddContentEditorWebpartWithContentLink(siteSetting, pageLibrary.ServerRelativePath + "/" + pageName + ".aspx", "Main", 1, ((SPWeb)_MainObject).ServerRelativePath + "/Style Library/" + includeFileName);

            //this.ShowLoadingStatus("Generating page components...");


            string filePath = GetTempPath() + "\\" + includeFileName;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["Database"] = _MainObject;
            parameters["List"] = carouselList;
            parameters["ImageField"] = imageField;
            parameters["CaptionField"] = captionField;
            parameters["ContentField"] = contentField;
            parameters["Fields"] = fields;
            GenerateFileFromCodeTemplate(new IndexSPCarouselPageTemplate(), filePath, parameters);

            //this.ShowLoadingStatus("Generating page content...");
            UploadItem uploadItem = new UploadItem();
            uploadItem.FilePath = filePath;
            uploadItem.FieldInformations = new Dictionary<object, object>();
            uploadItem.Folder = new SPFolder()
            {
                RootFolderPath = resourceLibrary.RootFolderPath,
                FolderPath = resourceLibrary.FolderPath,
                WebUrl = siteSetting.Url,
                SiteUrl = siteSetting.Url,
                ListName = resourceLibrary.Title,
                BaseType = 1,
            };
            Entities.Interfaces.IItem item = null;
            //Logger.Info("Uploading " + uploadItem.FilePath + " ...", "Service");
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            serviceManager.UploadFile(siteSetting, uploadItem, false, out item);
            //this.HideLoadingStatus("Completed.");

        }

        public void Initialize(List<IQueryPanel> queryPanels, Folder mainObject)
        {
            _QueryPanels = queryPanels;
            _MainObject = mainObject;
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(_MainObject.SiteSettingID);
            string childFoldersCategoryName = "";
            if (_MainObject as SQLDB != null)
                childFoldersCategoryName = "Tables";
            List<Folder> subFolders = ApplicationContext.Current.GetSubFolders(siteSetting, _MainObject, null, childFoldersCategoryName);


            _MainObject.Folders = subFolders;
            foreach (Folder subFolder in subFolders)
            {
                if (subFolder as SPWeb != null)
                    continue;

                SPLOSelectedListingsEntities.Items.Add(new Selectors.CheckBoxList.CheckBoxListItem() { IsChecked = false, Text = subFolder.Title, Value = subFolder });
                SQLLOSelectedListingsEntities.Items.Add(new Selectors.CheckBoxList.CheckBoxListItem() { IsChecked = true, Text = subFolder.Title, Value = subFolder });
            }

            PopulatePageLibraries(SPLOPageLibraryComboBox, subFolders);
            PopulateResourceLibraries(SPLOResourceLibraryComboBox, subFolders);

            PopulatePageLibraries(SPCOPageLibraryComboBox, subFolders);
            PopulateResourceLibraries(SPCOResourceLibraryComboBox, subFolders);
            PopulateLibraries(SPCOCarouselLibraryComboBox, subFolders);
        }

        private void PopulateImageFields(ComboBox comboBox, SPList list, List<Field> fields)
        {
            List<Field> imageFields = new List<Field>();
            foreach (Field field in fields)
            {
                if (field.Type != FieldTypes.Text && field.Type != FieldTypes.URL && field.Name.Equals("FileLeafRef", StringComparison.InvariantCultureIgnoreCase) == false)
                    continue;

                imageFields.Add(field);
            }

            comboBox.ItemsSource = imageFields;
            comboBox.IsEnabled = true;
            if (list.ServerTemplate == 101 || list.ServerTemplate == 851)
            {
                Field fileLeafRefField = imageFields.Where(t => t.Name.Equals("FileLeafRef", StringComparison.InvariantCultureIgnoreCase) == true).FirstOrDefault();
                if (fileLeafRefField != null)
                {
                    comboBox.SelectedItem = fileLeafRefField;
                    comboBox.IsEnabled = false;
                }
            }
            
            if (comboBox.SelectedItem == null && imageFields.Count > 0)
                comboBox.SelectedItem = imageFields[0];
        }

        private void PopulateTextFields(ComboBox comboBox, List<Field> fields)
        {
            List<Field> textFields = new List<Field>();
            foreach (Field field in fields)
            {
                if (field.Type != FieldTypes.Text && field.Type != FieldTypes.Note)
                    continue;

                textFields.Add(field);
            }

            comboBox.ItemsSource = textFields;
            if (textFields.Count > 0)
                comboBox.SelectedItem = textFields[0];
        }

        private void PopulatePageLibraries(ComboBox comboBox, List<Folder> folders)
        {
            List<Folder> pageLibraries = new List<Folder>();
            Folder defaultPageLibrary = null;
            foreach(Folder folder in folders)
            {
                if (folder as SPList == null)
                    continue;

                if (((SPList)folder).ServerTemplate == 850)
                {
                    if (folder.Title == "Pages" || folder.Title == "Site Pages")
                        defaultPageLibrary = folder;
                    pageLibraries.Add(folder);
                }

            }

            comboBox.ItemsSource = pageLibraries;
            if (defaultPageLibrary == null && pageLibraries.Count > 0)
                defaultPageLibrary = pageLibraries[0];

            if (defaultPageLibrary != null)
                comboBox.SelectedItem = defaultPageLibrary;
        }

        private void PopulateLibraries(ComboBox comboBox, List<Folder> folders)
        {
            List<Folder> libraries = new List<Folder>();
            foreach (Folder folder in folders)
            {
                if (folder as SPList == null)
                    continue;

                libraries.Add(folder);
            }

            comboBox.ItemsSource = libraries;
            if (libraries.Count > 0)
                comboBox.SelectedItem = libraries[0];
        }

        private void PopulateResourceLibraries(ComboBox comboBox, List<Folder> folders)
        {
            List<Folder> resourceLibraries = new List<Folder>();
            Folder defaultResourceLibrary = null;
            foreach (Folder folder in folders)
            {
                if (folder as SPList == null)
                    continue;

                if (((SPList)folder).ServerTemplate == 101)
                {
                    if (folder.Title == "Style Library")
                        defaultResourceLibrary = folder;

                    resourceLibraries.Add(folder);
                }
            }

            comboBox.ItemsSource = resourceLibraries;
            if (defaultResourceLibrary == null && resourceLibraries.Count > 0)
                defaultResourceLibrary = resourceLibraries[0];

            if (defaultResourceLibrary != null)
                comboBox.SelectedItem = defaultResourceLibrary;
        }


        private string GenerateSPGridsScript(List<Folder> lists, SiteSetting siteSetting) 
        {
            StringBuilder sb = new StringBuilder();
            foreach (Selectors.CheckBoxList.CheckBoxListItem selectedItem in SPLOSelectedListingsEntities.SelectedItems)
            {
                SPList spList = (SPList)selectedItem.Value;
                FieldCollection fields = ApplicationContext.Current.GetFields(siteSetting, spList);
                string tableName = spList.Title;
                string fixedTableName = CodeWizardManager.FixTableNameForCode(tableName);
                string gridName = fixedTableName + "Grid";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["Tables"] = lists;
                parameters["TableName"] = tableName;
                parameters["Fields"] = fields;
                parameters["GridName"] = gridName;
                parameters["GridTitle"] = spList.Title;
                parameters["GridAltTitle"] = spList.Title;

                string gridComponent = TransformCodeTemplate(new SobyGridComponentSPTemplate(), parameters);
                gridComponent = "   function soby_Populate" + fixedTableName + "() {" +
                                    Environment.NewLine + gridComponent +
                                    Environment.NewLine + "     " + gridName + ".Initialize(true);" +
                                    Environment.NewLine + " }" + Environment.NewLine + Environment.NewLine;
                                    //Environment.NewLine + "$(function() { SP.SOD.executeFunc('sp.js', 'SP.ClientContext', soby_Populate" + fixedTableName + "); });"; 

                sb.Append(gridComponent);
            }

            return sb.ToString();
        }

        private void ComponentTypeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (ComponentTypeTabControl == null || ComponentTypeTabControl.Items == null)
                return;

            foreach(object tabItem in ComponentTypeTabControl.Items)
            {
                ((TabItem)tabItem).Visibility = Visibility.Hidden;
                ((TabItem)tabItem).Height = 0;
            }

            if (_MainObject as SPWeb != null)
            {
                if (ComponentTypeListingsRadioButton.IsChecked == true)
                {
                    //SPListingOptionsTabItem.Visibility = Visibility.Visible;
                    ComponentTypeTabControl.SelectedItem = SPListingOptionsTabItem;
                }
                else if (ComponentTypeCarouselRadioButton.IsChecked == true)
                {
                    //SPCarouselOptionsTabItem.Visibility = Visibility.Visible;
                    ComponentTypeTabControl.SelectedItem = SPCarouselOptionsTabItem;
                }
                else if (ComponentTypeCalendarRadioButton.IsChecked == true)
                {
                    //SPListingOptionsTabItem.Visibility = Visibility.Visible;
                }
            }
            else if (_MainObject as SQLDB != null)
            {
                if (ComponentTypeListingsRadioButton.IsChecked == true)
                {
                    //SQLListingOptionsTabItem.Visibility = Visibility.Visible;
                }
                else if (ComponentTypeCarouselRadioButton.IsChecked == true)
                {
                    //SQLListingOptionsTabItem.Visibility = Visibility.Visible;
                }
                else if (ComponentTypeCalendarRadioButton.IsChecked == true)
                {
                    //SQLListingOptionsTabItem.Visibility = Visibility.Visible;
                }
            }
        }

        private void SPCOCarouselLibraryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SPList carouselList = (SPList)SPCOCarouselLibraryComboBox.SelectedItem;
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(carouselList.SiteSettingID);
            FieldCollection fields = ApplicationContext.Current.GetFields(siteSetting, carouselList);
            PopulateImageFields(SPCOImageFieldComboBox, carouselList, fields);
            PopulateTextFields(SPCOCaptionFieldComboBox, fields);
            PopulateTextFields(SPCOContentFieldComboBox, fields);

        }
    }
}
