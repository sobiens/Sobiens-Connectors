﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Sobiens.Connectors.Studio.UI.Controls.CodeTemplates.WebAPI
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using Sobiens.Connectors.Entities;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class SobyGridComponentTemplate : SobyGridComponentTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\r\n\r\n        var ");
            
            #line 11 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("DataSourceBuilder = new soby_WSBuilder();\r\n        ");
            
            #line 12 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("DataSourceBuilder.Filters = new SobyFilters(false);\r\n        var ");
            
            #line 13 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Service = new soby_WebServiceService(");
            
            #line 13 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("DataSourceBuilder);\r\n        var ");
            
            #line 14 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Grid = new soby_WebGrid(\"#soby_TasksDiv\", \"");
            
            #line 14 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("\", ");
            
            #line 14 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Service, \"There is no record found.\");\r\n        ");
            
            #line 15 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Grid.ImagesFolderUrl = \"/media/images\";\r\n\r\n        ");
            
            #line 17 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Service.Transport.Read = new soby_TransportRequest(soby_GetTutorialWebAPIUrl() + " +
                    "\"/");
            
            #line 17 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("List\", \"json\", \"application/json; charset=utf-8\", \"GET\");\r\n        ");
            
            #line 18 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Service.Transport.Add = new soby_TransportRequest(soby_GetTutorialWebAPIUrl() + \"" +
                    "/");
            
            #line 18 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("List\", \"json\", \"application/json; charset=utf-8\", \"POST\");\r\n        ");
            
            #line 19 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Service.Transport.Update = new soby_TransportRequest(soby_GetTutorialWebAPIUrl() " +
                    "+ \"/");
            
            #line 19 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("List(#key)\", \"json\", \"application/json; charset=utf-8\", \"PUT\");\r\n        ");
            
            #line 20 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Service.Transport.Delete = new soby_TransportRequest(soby_GetTutorialWebAPIUrl() " +
                    "+ \"/");
            
            #line 20 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("List(#key)\", \"json\", \"application/json; charset=utf-8\", \"DELETE\");\r\n\r\n\t\t");
            
            #line 22 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"

		foreach(Field field in Fields){
			if(field.IsPrimary == true){
			
            
            #line default
            #line hidden
            this.Write("            ");
            
            #line 26 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Grid.AddKeyField(\"");
            
            #line 26 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name));
            
            #line default
            #line hidden
            this.Write("\");\r\n\t\t\t");
            
            #line 27 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"

			}
		}
		
            
            #line default
            #line hidden
            this.Write("\r\n\r\n\t\t");
            
            #line 33 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"

		foreach(Field field in Fields){
            string fieldTypeString = "SobyFieldTypes.Text";
			if(field.Type == FieldTypes.Number)
                fieldTypeString = "SobyFieldTypes.Number";

			if(field.Type == FieldTypes.Lookup){
			
            
            #line default
            #line hidden
            this.Write("\t\t\t\t");
            
            #line 41 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("DataSourceBuilder.AddSchemaField(\"");
            
            #line 41 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name));
            
            #line default
            #line hidden
            this.Write("\", SobyFieldTypes.Lookup, { ModelName: \"");
            
            #line 41 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.List));
            
            #line default
            #line hidden
            this.Write("Record\", ValueFieldType: SobyFieldTypes.Number, ValueFieldName: \"");
            
            #line 41 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(((Sobiens.Connectors.Entities.SQLServer.SQLField)field).ReferenceFieldName));
            
            #line default
            #line hidden
            this.Write("\", TitleFieldName: \"");
            
            #line 41 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.ShowField));
            
            #line default
            #line hidden
            this.Write("\", ReadTransport: new soby_TransportRequest(soby_GetTutorialWebAPIUrl() + \"/");
            
            #line 41 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.List));
            
            #line default
            #line hidden
            this.Write("List\", \"json\", \"application/json; charset=utf-8\", \"GET\") });\r\n\t\t\t\t");
            
            #line 42 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Grid.AddColumn(\"");
            
            #line 42 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name));
            
            #line default
            #line hidden
            this.Write("\", \"");
            
            #line 42 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name));
            
            #line default
            #line hidden
            this.Write("\", SobyShowFieldsOn.All, function (item) {\r\n\t\t\t        return item.");
            
            #line 43 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.List));
            
            #line default
            #line hidden
            this.Write("Record.");
            
            #line 43 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.ShowField));
            
            #line default
            #line hidden
            this.Write(";\r\n\t\t\t    }, null, true, true, true, null);\r\n\t\t\t");
            
            #line 45 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"

			}
			else{
			
            
            #line default
            #line hidden
            this.Write("\t\t\t\t");
            
            #line 49 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("DataSourceBuilder.AddSchemaField(\"");
            
            #line 49 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name));
            
            #line default
            #line hidden
            this.Write("\", ");
            
            #line 49 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(fieldTypeString));
            
            #line default
            #line hidden
            this.Write(", null);\r\n\t\t\t\t");
            
            #line 50 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Grid.AddColumn(\"");
            
            #line 50 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name));
            
            #line default
            #line hidden
            this.Write("\", \"");
            
            #line 50 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name));
            
            #line default
            #line hidden
            this.Write("\", SobyShowFieldsOn.All, null, null, true, true, true, null);\r\n\t\t");
            
            #line 51 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"

			}
		}
		
            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\SobyGridComponentTemplate.tt"

private string _TableNameField;

/// <summary>
/// Access the TableName parameter of the template.
/// </summary>
private string TableName
{
    get
    {
        return this._TableNameField;
    }
}

private global::Sobiens.Connectors.Entities.FieldCollection _FieldsField;

/// <summary>
/// Access the Fields parameter of the template.
/// </summary>
private global::Sobiens.Connectors.Entities.FieldCollection Fields
{
    get
    {
        return this._FieldsField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool TableNameValueAcquired = false;
if (this.Session.ContainsKey("TableName"))
{
    this._TableNameField = ((string)(this.Session["TableName"]));
    TableNameValueAcquired = true;
}
if ((TableNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("TableName");
    if ((data != null))
    {
        this._TableNameField = ((string)(data));
    }
}
bool FieldsValueAcquired = false;
if (this.Session.ContainsKey("Fields"))
{
    this._FieldsField = ((global::Sobiens.Connectors.Entities.FieldCollection)(this.Session["Fields"]));
    FieldsValueAcquired = true;
}
if ((FieldsValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("Fields");
    if ((data != null))
    {
        this._FieldsField = ((global::Sobiens.Connectors.Entities.FieldCollection)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class SobyGridComponentTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
