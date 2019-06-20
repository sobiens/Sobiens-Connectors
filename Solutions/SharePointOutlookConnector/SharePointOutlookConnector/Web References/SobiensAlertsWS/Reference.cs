﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.239.
// 
#pragma warning disable 1591

namespace Sobiens.Office.SharePointOutlookConnector.SobiensAlertsWS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="AlertsWebServiceSoap", Namespace="http://tempuri.org/")]
    public partial class AlertsWebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CheckServiceExistencyOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetMyAlertsOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeleteAlertOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateAlertOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public AlertsWebService() {
            this.Url = global::Sobiens.Office.SharePointOutlookConnector.Properties.Settings.Default.Sobiens_Office_SharePointOutlookConnector_SobiensAlertsWS_AlertsWebService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event CheckServiceExistencyCompletedEventHandler CheckServiceExistencyCompleted;
        
        /// <remarks/>
        public event GetMyAlertsCompletedEventHandler GetMyAlertsCompleted;
        
        /// <remarks/>
        public event DeleteAlertCompletedEventHandler DeleteAlertCompleted;
        
        /// <remarks/>
        public event UpdateAlertCompletedEventHandler UpdateAlertCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CheckServiceExistency", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool CheckServiceExistency() {
            object[] results = this.Invoke("CheckServiceExistency", new object[0]);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void CheckServiceExistencyAsync() {
            this.CheckServiceExistencyAsync(null);
        }
        
        /// <remarks/>
        public void CheckServiceExistencyAsync(object userState) {
            if ((this.CheckServiceExistencyOperationCompleted == null)) {
                this.CheckServiceExistencyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCheckServiceExistencyOperationCompleted);
            }
            this.InvokeAsync("CheckServiceExistency", new object[0], this.CheckServiceExistencyOperationCompleted, userState);
        }
        
        private void OnCheckServiceExistencyOperationCompleted(object arg) {
            if ((this.CheckServiceExistencyCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CheckServiceExistencyCompleted(this, new CheckServiceExistencyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetMyAlerts", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlElement GetMyAlerts() {
            object[] results = this.Invoke("GetMyAlerts", new object[0]);
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public void GetMyAlertsAsync() {
            this.GetMyAlertsAsync(null);
        }
        
        /// <remarks/>
        public void GetMyAlertsAsync(object userState) {
            if ((this.GetMyAlertsOperationCompleted == null)) {
                this.GetMyAlertsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetMyAlertsOperationCompleted);
            }
            this.InvokeAsync("GetMyAlerts", new object[0], this.GetMyAlertsOperationCompleted, userState);
        }
        
        private void OnGetMyAlertsOperationCompleted(object arg) {
            if ((this.GetMyAlertsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetMyAlertsCompleted(this, new GetMyAlertsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/DeleteAlert", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string DeleteAlert(string[] ids) {
            object[] results = this.Invoke("DeleteAlert", new object[] {
                        ids});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void DeleteAlertAsync(string[] ids) {
            this.DeleteAlertAsync(ids, null);
        }
        
        /// <remarks/>
        public void DeleteAlertAsync(string[] ids, object userState) {
            if ((this.DeleteAlertOperationCompleted == null)) {
                this.DeleteAlertOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteAlertOperationCompleted);
            }
            this.InvokeAsync("DeleteAlert", new object[] {
                        ids}, this.DeleteAlertOperationCompleted, userState);
        }
        
        private void OnDeleteAlertOperationCompleted(object arg) {
            if ((this.DeleteAlertCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteAlertCompleted(this, new DeleteAlertCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UpdateAlert", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UpdateAlert(string id, string title, string listID, string alertTime, string eventType, string frequency, string filterXML) {
            object[] results = this.Invoke("UpdateAlert", new object[] {
                        id,
                        title,
                        listID,
                        alertTime,
                        eventType,
                        frequency,
                        filterXML});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateAlertAsync(string id, string title, string listID, string alertTime, string eventType, string frequency, string filterXML) {
            this.UpdateAlertAsync(id, title, listID, alertTime, eventType, frequency, filterXML, null);
        }
        
        /// <remarks/>
        public void UpdateAlertAsync(string id, string title, string listID, string alertTime, string eventType, string frequency, string filterXML, object userState) {
            if ((this.UpdateAlertOperationCompleted == null)) {
                this.UpdateAlertOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateAlertOperationCompleted);
            }
            this.InvokeAsync("UpdateAlert", new object[] {
                        id,
                        title,
                        listID,
                        alertTime,
                        eventType,
                        frequency,
                        filterXML}, this.UpdateAlertOperationCompleted, userState);
        }
        
        private void OnUpdateAlertOperationCompleted(object arg) {
            if ((this.UpdateAlertCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateAlertCompleted(this, new UpdateAlertCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void CheckServiceExistencyCompletedEventHandler(object sender, CheckServiceExistencyCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CheckServiceExistencyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CheckServiceExistencyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetMyAlertsCompletedEventHandler(object sender, GetMyAlertsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetMyAlertsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetMyAlertsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlElement Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlElement)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void DeleteAlertCompletedEventHandler(object sender, DeleteAlertCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteAlertCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DeleteAlertCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void UpdateAlertCompletedEventHandler(object sender, UpdateAlertCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateAlertCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateAlertCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591