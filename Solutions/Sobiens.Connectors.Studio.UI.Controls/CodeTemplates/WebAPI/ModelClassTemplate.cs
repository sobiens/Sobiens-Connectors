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
    
    #line 1 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class ModelClassTemplate : ModelClassTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\r\n");
            this.Write("\r\nusing System.Collections.Generic;\r\nusing System.ComponentModel.DataAnnotations;" +
                    "\r\nusing System.ComponentModel.DataAnnotations.Schema;\r\n\r\n\r\nnamespace SobyGrid_We" +
                    "bAPIExample.Models\r\n{\r\n\t[Table(\"");
            
            #line 18 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("\")]\r\n    public class ");
            
            #line 19 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableName));
            
            #line default
            #line hidden
            this.Write("Record\r\n    {\r\n\t\t");
            
            #line 21 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"

		int keyColumnOrder = 0;
        bool hasPrimaryField = false;
		foreach (Field field in Fields)
        {
        	if (field.IsPrimary == true)
                hasPrimaryField = true;
        }
		foreach (Field field in Fields)
        {
			string fieldTypeString = "string";
			switch (((Sobiens.Connectors.Entities.SQLServer.SQLField)field).SQLFieldTypeName)
            {
                case "int":
			        fieldTypeString = "int";
                    break;
                case "bigint":
			        fieldTypeString = "long";
                    break;
                case "float":
                case "decimal":
                case "numeric":
			        fieldTypeString = "decimal";
                    break;
                case "smallint":
			        fieldTypeString = "Int16";
                    break;
                case "tinyint":
			        fieldTypeString = "Byte";
                    break;
                case "bit":
			        fieldTypeString = "bool";
                    break;
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    fieldTypeString = "System.DateTime";
                    break;
            }


		
            
            #line default
            #line hidden
            this.Write("\t\t    ");
            
            #line 64 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
 if (field.Required == true){ 
            
            #line default
            #line hidden
            this.Write("[Required]");
            
            #line 64 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t    ");
            
            #line 65 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
 if (hasPrimaryField == false && keyColumnOrder == 0){ 
            
            #line default
            #line hidden
            this.Write("[Key][DatabaseGenerated(DatabaseGeneratedOption.None)]");
            
            #line 65 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t    ");
            
            #line 66 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
 if (field.IsPrimary == true){
		    keyColumnOrder++;
		     
            
            #line default
            #line hidden
            this.Write("\t\t    [Key]\r\n\t\t    [Column(Order=");
            
            #line 70 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(keyColumnOrder));
            
            #line default
            #line hidden
            this.Write(")]\r\n\t\t    ");
            
            #line 71 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
 }
            
            #line default
            #line hidden
            this.Write("\r\n            ");
            
            #line 73 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
 if(field.Type == FieldTypes.Lookup){
            
            #line default
            #line hidden
            this.Write("                // Foreign Key\r\n\t\t        [ForeignKey(\"");
            
            #line 75 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.List));
            
            #line default
            #line hidden
            this.Write("Record\")]\r\n\t\t        public ");
            
            #line 76 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(fieldTypeString));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 76 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name));
            
            #line default
            #line hidden
            this.Write(" { get; set; }\r\n\t\t        // Navigation property\r\n\t\t        public ");
            
            #line 78 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.List));
            
            #line default
            #line hidden
            this.Write("Record ");
            
            #line 78 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.List));
            
            #line default
            #line hidden
            this.Write("Record { get; set; }\r\n            ");
            
            #line 79 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"

            }
            else{
            
            
            #line default
            #line hidden
            this.Write("    \t        public ");
            
            #line 83 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(fieldTypeString));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 83 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name));
            
            #line default
            #line hidden
            this.Write("{ get; set; }\r\n            ");
            
            #line 84 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"

            }
            
            
            #line default
            #line hidden
            this.Write("\t\t\r\n        ");
            
            #line 88 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    }\r\n}");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "C:\Projects\GitHub\Sobiens-Connectors\Solutions\Sobiens.Connectors.Studio.UI.Controls\CodeTemplates\WebAPI\ModelClassTemplate.tt"

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
    public class ModelClassTemplateBase
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
