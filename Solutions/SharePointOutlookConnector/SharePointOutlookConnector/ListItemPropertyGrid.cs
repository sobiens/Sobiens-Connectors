using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using System.Collections;
using System.Xml;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Sobiens.Office.SharePointOutlookConnector.BLL;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector
{
    public delegate void UpdatePropertyGridHandler();

    public partial class ListItemPropertyGrid : UserControl
    {
        private EUListItem SelectedListItem = null;
        public ListItemPropertyGrid()
        {
            InitializeComponent();
        }

        public void UpdatePropertyGrid()
        {
            XmlAttributeCollection attributes = SelectedListItem.Properties;
            if (attributes != null)
            {
                List<EUField> fields = SharePointManager.GetFields(SelectedListItem.SiteSetting, SelectedListItem.WebURL, SelectedListItem.ListName);
                Assembly ass = EvaluateExpression(fields, attributes);
                var r = ass.CreateInstance("TempClass");
                ListItemPropertyGridGrid.SelectedObject = r;
                ListItemPropertyGridGrid.Tag = SelectedListItem;
            }
        }
        public void SetListItem(EUListItem listItem)
        {
            SelectedListItem = listItem;
            backgroundWorker1.RunWorkerAsync();
        }

        private void ListItemPropertyGridGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Hashtable changedProperties = new Hashtable();
            changedProperties.Add(e.ChangedItem.Label, e.ChangedItem.Value.ToString());
            EUListItem listItem = (EUListItem)ListItemPropertyGridGrid.Tag;
            SharePointManager.UpdateListItem(listItem.SiteSetting, listItem.WebURL, listItem.ListName, listItem.ID, changedProperties);
        }

        private static string ReplaceValueString(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string GetPropertyString(string name, string value, Type type, bool isReadOnly)
        {
            string typeName = String.Empty;
            string propertyPrivateObjectString = String.Empty;
            string propertyGetString = " get {return " + " this._" + name + "; }";
            string propertySetString = " set{ this._" + name + " = value; }";
            if (isReadOnly == true)
                propertySetString = String.Empty;
            if (type == typeof(string))
            {
                typeName = "string";
                propertyPrivateObjectString = "private " + typeName + " _" + name + " = \"" + ReplaceValueString(value) + "\";" + Environment.NewLine;
            }
            else
            {
                typeName = "int";
                propertyPrivateObjectString = "private " + typeName + " _" + name + " = " + ReplaceValueString(value) + ";" + Environment.NewLine;
            }
            return propertyPrivateObjectString + "public " + typeName + " " + name + "{ " +
                propertyGetString + Environment.NewLine +
                propertySetString + Environment.NewLine +
                "}";
        }
        public static Assembly EvaluateExpression(List<EUField> fields, XmlAttributeCollection attributes)
        {
            Hashtable h = new Hashtable();
            string code = "public class TempClass { " + Environment.NewLine;
            foreach (EUField field in fields)
            {
                if (field.FromBaseType == true)
                    continue;
                string value = String.Empty;
                XmlAttribute attribute = attributes["ows_" + field.Name];
                if (attribute != null)
                    value = attribute.Value;
                code += GetPropertyString(field.Name, value, typeof(string), field.ReadOnly) + Environment.NewLine;
            }
            code += "}";
            /*
            string code = "public class TempClass { " + Environment.NewLine +
                GetPropertyString("ID", "12", typeof(int)) + Environment.NewLine +
                GetPropertyString("Title", "design.docx", typeof(string)) + Environment.NewLine +
                "}";
             */
            //            code = "public class TempClass { public string Title{get{return \"test\";}} }";

            CompilerResults compilerResults = CompileScript(code);

            if (compilerResults.Errors.HasErrors)
            {
                throw new InvalidOperationException("Expression has a syntax error.");
            }

            Assembly assembly = compilerResults.CompiledAssembly;

            return assembly;
        }

        public static CompilerResults CompileScript(string source)
        {
            CompilerParameters parms = new CompilerParameters();

            parms.GenerateExecutable = false;
            parms.GenerateInMemory = true;
            parms.IncludeDebugInformation = false;

            CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");

            return compiler.CompileAssemblyFromSource(parms, source);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
//            object[] args = new object[0] { SelectedView, items, listItemCollectionPositionNext, itemCount };
            this.Invoke(new UpdatePropertyGridHandler(UpdatePropertyGrid), null);
        }  

    }
}
