using System;
using System.Net;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using Sobiens.Connectors.Entities;
using System.Xml;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.SharePoint;
using System.Collections;
using Sobiens.Connectors.Entities.Workflows;
using System.Globalization;
using Sobiens.Connectors.Entities.CRM;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel.Description;
using Microsoft.Crm.Sdk.Messages;
using System.Text;

namespace Sobiens.Connectors.Services.CRM
{
    public class CRMService : ICRMService
    {
        private static Dictionary<Guid, IOrganizationService> cachedServices = new Dictionary<Guid, IOrganizationService>();

        public static IOrganizationService GetClientContext(ISiteSetting siteSetting)
        {
            try
            {
                if (cachedServices.ContainsKey(siteSetting.ID) == false)
                {
                    IOrganizationService organizationService = null;
                    Uri uri = new Uri(siteSetting.Url + "/XRMServices/2011/Organization.svc");

                    var credentials = new ClientCredentials();
                    if (siteSetting.UseDefaultCredential == true)
                    {
                        credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
                    }
                    else
                    {
                        string[] userNameValues = siteSetting.Username.Split(new string[] { "\\" }, StringSplitOptions.None);
                        string domainName = string.Empty;
                        string userName = string.Empty;
                        if (userNameValues.Length > 1)
                        {
                            domainName = userNameValues[0];
                            userName = userNameValues[1];
                        }
                        else
                        {
                            userName = userNameValues[0];
                        }

                        if (string.IsNullOrEmpty(domainName) == true)
                        {
                            credentials.UserName.UserName = userName;
                            credentials.UserName.Password = siteSetting.Password;
                        }
                        else
                        {
                            credentials.Windows.ClientCredential =
                                new System.Net.NetworkCredential(userName,
                                    siteSetting.Password,
                                    domainName);
                        }
                    }

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (OrganizationServiceProxy organizationServiceProxy = new OrganizationServiceProxy(uri, null, credentials, null))
                    {
                        organizationServiceProxy.Authenticate();
                        organizationService = (IOrganizationService)organizationServiceProxy;
                    }

                    cachedServices.Add(siteSetting.ID, organizationService);
                }

                return cachedServices[siteSetting.ID];
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }



        public List<Sobiens.Connectors.Entities.Folder> GetFolders(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder parentFolder, int[] includedFolderTypes, string childFoldersCategoryName)
        {
            List<Sobiens.Connectors.Entities.Folder> subFolders = new List<Sobiens.Connectors.Entities.Folder>();
            if (parentFolder as CRMWeb != null)
            {
                CRMWeb web = (CRMWeb)parentFolder;

                try
                {
                    if (childFoldersCategoryName.Equals("Entities", StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        List<CRMEntity> lists = this.GetLists(siteSetting);
                        foreach (CRMEntity list in lists)
                        {
                            subFolders.Add(list);
                        }
                    }
                    else if (childFoldersCategoryName.Equals("Teams", StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        subFolders = this.GetTeams(siteSetting).ToList<Folder>();
                    }
                    else if (childFoldersCategoryName.Equals("Business Units", StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        subFolders = this.GetBusinessUnits(siteSetting).ToList<Folder>();
                    }
                    else if (childFoldersCategoryName.Equals("Processes", StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        subFolders = this.GetProcesses(siteSetting).ToList<Folder>();
                    }
                    else if (childFoldersCategoryName.Equals("Option Sets", StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        subFolders = this.GetOptionSets(siteSetting).ToList<Folder>();
                    }
                    else if (childFoldersCategoryName.Equals("Security Roles", StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        subFolders = this.GetSecurityRoles(siteSetting).ToList<Folder>();
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format("CRMService GetLists method returned error:{0}", ex.Message);
                    Logger.Error(message, "Service");
                }
            }
            else if (parentFolder as CRMEntity != null)
            {
                CRMEntity _folder = (CRMEntity)parentFolder;

                //List<Folder> folders = new List<Folder>();
                //subFolders.Add(new CRMViewCollection(_folder, "Views", siteSetting.ID));
                /*
                foreach (SPFolder __folder in folders)
                {
                    //__folder.ParentFolder = _folder;
                    subFolders.Add(__folder);
                }
                */
            }
            /*
            else if (parentFolder as CRMViewCollection != null)
            {
                CRMViewCollection _folder = (CRMViewCollection)parentFolder;

                if (_folder.Title.Equals("Views") == true)
                {
                    subFolders.Add(new CRMViewCollection(_folder.Entity, "Views shared with me", siteSetting.ID));
                    subFolders.Add(new CRMViewCollection(_folder.Entity, "All views", siteSetting.ID));
                }
                else if (_folder.Title.Equals("Views shared with me") == true)
                {
                    subFolders.Add(new CRMViewCollection(_folder.Entity, "Views shared with me", siteSetting.ID));
                    subFolders.Add(new CRMViewCollection(_folder.Entity, "All views", siteSetting.ID));
                }
                else if (_folder.Title.Equals("All views") == true)
                {
                    List<CRMView> views = GetAllViews(siteSetting, _folder.Entity);
                    subFolders.AddRange(views);
                }

            }
            */

            return subFolders;
        }

        public List<Folder> GetFolders(ISiteSetting siteSetting, Folder currentFolder)
        {
            return GetFolders(siteSetting, currentFolder, null, string.Empty);
        }

        public string[] GetCurrentUserDetails(ISiteSetting siteSetting)
        {
            string userId = string.Empty;
            string fullName = string.Empty;
            string email = string.Empty;
            IOrganizationService organizationService = GetClientContext(siteSetting);
            Logger.Warning("Getting current user", "GetCurrentUserDetails");


            // Get a system user to send the email (From: field)
            WhoAmIRequest systemUserRequest = new WhoAmIRequest();
            WhoAmIResponse systemUserResponse = (WhoAmIResponse)organizationService.Execute(systemUserRequest);
            if (systemUserResponse != null)
            {
                Logger.Warning("systemUserResponse is not null", "GetCurrentUserDetails");
                userId = systemUserResponse.UserId.ToString();
                Logger.Warning("GetCurrentUserDetails result for userId:" + userId, "GetCurrentUserDetails");
            }


            /*
              Lookup User
                        var User = service.Retrieve("systemuser", userId, new ColumnSet("fullname"));
                        string fullName = User["fullname"].ToString();


                        QueryExpression query = new QueryExpression("systemuser");
                        CamlFilters filters = new CamlFilters();
                        string userName = siteSetting.Username;
                        if (siteSetting.UseDefaultCredential == true)
                        {
                            userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                            //userName = System.Net.CredentialCache.DefaultNetworkCredentials.Domain + "\\" + System.Net.CredentialCache.DefaultNetworkCredentials.UserName;
                        }
                        Logger.Warning("GetCurrentUserDetails for username:" + userName, "GetCurrentUserDetails");
                        filters.Add(new CamlFilter("domainname", FieldTypes.Text, CamlFilterTypes.Equals, userName));
                        query.Criteria = GetFilterExpression(filters);
                        query.ColumnSet = new ColumnSet(true);
                        EntityCollection items = organizationService.RetrieveMultiple(query);
                        if (items.Entities.Count > 0)
                        {
                            userId = items.Entities[0]["systemuserid"].ToString();
                            fullName = items.Entities[0]["fullname"].ToString();
                            email = items.Entities[0]["internalemailaddress"].ToString();
                        }
                        Logger.Warning("GetCurrentUserDetails items.Entities.Count:" + items.Entities.Count, "GetCurrentUserDetails");
                        Logger.Warning("GetCurrentUserDetails result for userId:" + userId, "GetCurrentUserDetails");
                        Logger.Warning("GetCurrentUserDetails result for fullName:" + fullName, "GetCurrentUserDetails");
                        Logger.Warning("GetCurrentUserDetails result for email:" + email, "GetCurrentUserDetails");
                        */
            return new string[] { userId, email, fullName};
        }

        public int GetUserObjectAccessDetails(ISiteSetting siteSetting, Guid objectId, Guid userId)
        {
            IOrganizationService organizationService = GetClientContext(siteSetting);
            QueryExpression query = new QueryExpression("principalobjectaccess");
            CamlFilters filters = new CamlFilters();
            filters.IsOr = false;
            filters.Add(new CamlFilter("objectid", FieldTypes.Text, CamlFilterTypes.Equals, objectId.ToString()));
            filters.Add(new CamlFilter("principalid", FieldTypes.Text, CamlFilterTypes.Equals, userId.ToString()));
            query.Criteria = GetFilterExpression(filters);
            query.ColumnSet = new ColumnSet(true);
            EntityCollection items = organizationService.RetrieveMultiple(query);
            if (items.Entities.Count > 0)
            {
                string accessrightsmask = items.Entities[0]["accessrightsmask"].ToString();
                string inheritedaccessrightsmask = items.Entities[0]["inheritedaccessrightsmask"].ToString();
            }

            return 0;
        }

        public List<CRMOptionSet> GetOptionSets(ISiteSetting siteSetting)
        {
            List<CRMOptionSet> optionSets = new List<CRMOptionSet>();
            IOrganizationService organizationService = GetClientContext(siteSetting);
            OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)organizationService;
            serviceProxy.EnableProxyTypes();
            RetrieveAllOptionSetsRequest retrieveAllOptionSetsRequest = new RetrieveAllOptionSetsRequest();
            RetrieveAllOptionSetsResponse retrieveAllOptionSetsResponse = (RetrieveAllOptionSetsResponse)serviceProxy.Execute(retrieveAllOptionSetsRequest);
            if (retrieveAllOptionSetsResponse.OptionSetMetadata.Count() > 0)
            {
                foreach (OptionSetMetadataBase optionSetMetadataBase in retrieveAllOptionSetsResponse.OptionSetMetadata)
                {
                    string displayName = (optionSetMetadataBase.DisplayName.LocalizedLabels.Count > 0) ? optionSetMetadataBase.DisplayName.LocalizedLabels[0].Label : String.Empty;
                    CRMOptionSet optionSet = new CRMOptionSet(Guid.NewGuid().ToString(), displayName);
                    if (optionSetMetadataBase as OptionSetMetadata != null)
                    {
                        OptionSetMetadata _optionSetMetadata = (OptionSetMetadata)optionSetMetadataBase;
                        foreach (OptionMetadata _optionMetadata in _optionSetMetadata.Options)
                        {
                            optionSet.Options.Add(_optionMetadata.Value.HasValue == true ? _optionMetadata.Value.Value : -1, _optionMetadata.Label.LocalizedLabels.Count > 0 ? _optionMetadata.Label.LocalizedLabels[0].Label : string.Empty);
                        }
                    }
                    else if (optionSetMetadataBase as BooleanOptionSetMetadata != null)
                    {
                        BooleanOptionSetMetadata _optionSetMetadata = (BooleanOptionSetMetadata)optionSetMetadataBase;
                        
                        optionSet.Options.Add(_optionSetMetadata.TrueOption.Value.HasValue == true ? _optionSetMetadata.TrueOption.Value.Value : -1, _optionSetMetadata.TrueOption.Label.LocalizedLabels.Count > 0 ? _optionSetMetadata.TrueOption.Label.LocalizedLabels[0].Label : string.Empty);
                        optionSet.Options.Add(_optionSetMetadata.FalseOption.Value.HasValue == true ? _optionSetMetadata.FalseOption.Value.Value : -1, _optionSetMetadata.FalseOption.Label.LocalizedLabels.Count > 0 ? _optionSetMetadata.FalseOption.Label.LocalizedLabels[0].Label : string.Empty);
                    }
                    optionSets.Add(optionSet);
                }
            }

            return optionSets;
        }

        public List<CRMProcess> GetProcesses(ISiteSetting siteSetting)
        {
            List<CRMProcess> processes = new List<CRMProcess>();
            IOrganizationService organizationService = GetClientContext(siteSetting);
            QueryExpression query = new QueryExpression("workflow");
            CamlFilters filters = new CamlFilters();
            filters.IsOr = false;
            query.Criteria = GetFilterExpression(filters);
            query.ColumnSet = new ColumnSet(true);
            EntityCollection items = organizationService.RetrieveMultiple(query);
            foreach (Entity _entity in items.Entities)
            {
                processes.Add(new CRMProcess(_entity.Id.ToString(), _entity["name"].ToString()));
            }

            return processes;
        }

        public List<CRMSecurityRole> GetSecurityRoles(ISiteSetting siteSetting)
        {
            List<CRMSecurityRole> securityRoles = new List<CRMSecurityRole>();
            IOrganizationService organizationService = GetClientContext(siteSetting);
            QueryExpression query = new QueryExpression("role");
            CamlFilters filters = new CamlFilters();
            filters.IsOr = false;
            query.Criteria = GetFilterExpression(filters);
            query.ColumnSet = new ColumnSet(true);
            EntityCollection items = organizationService.RetrieveMultiple(query);
            foreach (Entity _entity in items.Entities)
            {
                securityRoles.Add(new CRMSecurityRole(_entity.Id.ToString(), _entity["name"].ToString()));
            }

            return securityRoles;
        }

        public List<CRMTeam> GetTeams(ISiteSetting siteSetting)
        {
            List<CRMTeam> teams = new List<CRMTeam>();
            IOrganizationService organizationService = GetClientContext(siteSetting);
            QueryExpression query = new QueryExpression("team");
            CamlFilters filters = new CamlFilters();
            filters.IsOr = false;
            query.Criteria = GetFilterExpression(filters);
            query.ColumnSet = new ColumnSet(true);
            EntityCollection items = organizationService.RetrieveMultiple(query);
            foreach(Entity _entity in items.Entities)
            {
                teams.Add(new CRMTeam(_entity.Id.ToString(), _entity["name"].ToString()));
            }

            return teams;
        }

        public List<CRMBusinessUnit> GetBusinessUnits(ISiteSetting siteSetting)
        {
            List<CRMBusinessUnit> businessUnits = new List<CRMBusinessUnit>();
            IOrganizationService organizationService = GetClientContext(siteSetting);
            QueryExpression query = new QueryExpression("businessunit");
            CamlFilters filters = new CamlFilters();
            filters.IsOr = false;
            query.Criteria = GetFilterExpression(filters);
            query.ColumnSet = new ColumnSet(true);
            EntityCollection items = organizationService.RetrieveMultiple(query);
            foreach (Entity _entity in items.Entities)
            {
                businessUnits.Add(new CRMBusinessUnit(_entity.Id.ToString(), _entity["name"].ToString()));
            }

            return businessUnits;
        }

        public void GrantUserObjectAccess(ISiteSetting siteSetting, Guid objectId, string objectLogicalName, Guid userId, int accessrightmask)
        {
            IOrganizationService organizationService = GetClientContext(siteSetting);
            //no delete access
            GrantAccessRequest grant = new GrantAccessRequest();
            grant.Target = new EntityReference(objectLogicalName, objectId);

            PrincipalAccess principal = new PrincipalAccess();
            principal.Principal = new EntityReference("systemuser", userId);
            principal.AccessMask = AccessRights.ReadAccess | AccessRights.AppendAccess | AccessRights.WriteAccess | AccessRights.AppendToAccess | AccessRights.ShareAccess | AccessRights.AssignAccess;
            grant.PrincipalAccess = principal;

            try
            {
                GrantAccessResponse grant_response = (GrantAccessResponse)organizationService.Execute(grant);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            /*
            Entity account = new Entity("principalobjectaccess");
            account["objectid"] = objectId;
            account["principalid"] = userId;
            account["accessrightsmask"] = accessrightmask;
            Guid _accountId = organizationService.Create(account);
            */
        }

        public List<IView> GetPersonalViews(ISiteSetting siteSetting, CRMEntity entity)
        {
            Logger.Warning("GetPersonalViews 0", "GetPersonalViews");

            List<IView> views = new List<IView>();
            IOrganizationService organizationService = GetClientContext(siteSetting);

            // Dictionary to contain all the saved views
            Dictionary<Guid, Entity> dictPersonalViews = new Dictionary<Guid, Entity>();

            // retrieve first all the CRM Users
            QueryExpression systemUsers = new QueryExpression("systemuser");
            CamlFilters filters1 = new CamlFilters();
            filters1.Add(new CamlFilter("isdisabled", FieldTypes.Text, CamlFilterTypes.Equals, "FALSE"));
            systemUsers.Criteria = GetFilterExpression(filters1);
            

            systemUsers.ColumnSet = new ColumnSet(true);
            EntityCollection userCollection = organizationService.RetrieveMultiple(systemUsers);
            Logger.Warning("GetPersonalViews 1", "GetPersonalViews");

            Logger.Warning("Count:" + userCollection.Entities.Count, "GetPersonalViews");
            int currentIndex = 0;
            // for each User we launch the query to retrieve the saved views
            foreach (Entity systemUser in userCollection.Entities)
            {
                try
                {
                    currentIndex++;
                    Logger.Warning("Index:" + currentIndex, "GetPersonalViews");

                    QueryExpression personalViews = new QueryExpression("userquery");
                    CamlFilters filters = new CamlFilters();
                    filters.Add(new CamlFilter("returnedtypecode", FieldTypes.Text, CamlFilterTypes.Equals, entity.LogicalName));
                    personalViews.Criteria = GetFilterExpression(filters);
                    Logger.Warning("GetPersonalViews 2" + entity.LogicalName, "GetPersonalViews");

                    personalViews.ColumnSet = new ColumnSet(true);

                    // we set the CallerId property to impersonate the current iteration user
                    //Logger.Warning("GetPersonalViews 3" + entity.LogicalName, "GetPersonalViews");
                    ((OrganizationServiceProxy)organizationService).CallerId = systemUser.Id;
                    //Logger.Warning("GetPersonalViews 4" + entity.LogicalName, "GetPersonalViews");
                    EntityCollection viewCollection = organizationService.RetrieveMultiple(personalViews);
                    //Logger.Warning("GetPersonalViews 5" + entity.LogicalName, "GetPersonalViews");

                    foreach (Entity personalView in viewCollection.Entities)
                    {
                        // we want a list without duplicates (shared views or automatically shared to SYSTEM and INTEGRATION users)
                        if (!dictPersonalViews.ContainsKey(personalView.Id))
                        {
                            dictPersonalViews.Add(personalView.Id, personalView);
                        }
                    }
                }
                catch(Exception ex1)
                {
                    Logger.Error(ex1, "GetPersonalViews");
                }
            }

            Logger.Warning("GetPersonalViews 6" + entity.LogicalName, "GetPersonalViews");

            // we can process the values (Entity objects) of the dictionary
            foreach (Entity personalView in dictPersonalViews.Values)
            {
                string viewName = personalView["name"].ToString();
                string viewFetchXml = personalView["fetchxml"].ToString();
                EntityReference viewOwnerIdRef = (EntityReference)personalView["ownerid"];


                CRMView view = new CRMView(personalView.Id.ToString(), viewName, siteSetting.ID, entity);
                views.Add(view);
            }
            Logger.Warning("GetPersonalViews 7" + entity.LogicalName, "GetPersonalViews");


            return views;
        }

        /*
        public void ShareRecord(IOrganizationService service, Entity TargetEntity, Entity TargetShare)
        {
            Microsoft.Xrm.Sdk.acc
            //no delete access
            GrantAccessRequest grant = new GrantAccessRequest();
            grant.Target = new EntityReference(TargetEntity.LogicalName, TargetEntity.Id);

            PrincipalAccess principal = new PrincipalAccess();
            principal.Principal = new EntityReference(TargetShare.LogicalName, TargetShare.Id);
            principal.AccessMask = AccessRights.ReadAccess | AccessRights.AppendAccess | AccessRights.WriteAccess | AccessRights.AppendToAccess | AccessRights.ShareAccess | AccessRights.AssignAccess;
            grant.PrincipalAccess = principal;

            try
            {
                GrantAccessResponse grant_response = (GrantAccessResponse)service.Execute(grant);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */

        private List<CRMEntity> GetLists(ISiteSetting siteSetting) {
            IOrganizationService organizationService = GetClientContext(siteSetting);
            //Dictionary<string, string> attributesData = new Dictionary<string, string>();
            RetrieveAllEntitiesRequest metaDataRequest = new RetrieveAllEntitiesRequest();
            RetrieveAllEntitiesResponse metaDataResponse = new RetrieveAllEntitiesResponse();
            metaDataRequest.EntityFilters = EntityFilters.Attributes;

            //XmlDictionaryReaderQuotas myReaderQuotas = new XmlDictionaryReaderQuotas();
            //myReaderQuotas.MaxNameTableCharCount = 2147483647;

            // Execute the request.
            
            metaDataResponse = (RetrieveAllEntitiesResponse)organizationService.Execute(metaDataRequest);
            List<CRMEntity> entities = new List<CRMEntity>();
            foreach (EntityMetadata entityMetadata in metaDataResponse.EntityMetadata)
            {
                //if (entityMetadata.IsCustomizable.Value == false)
                //    continue;

                CRMEntity entity = new CRMEntity(siteSetting.ID, entityMetadata.MetadataId.Value.ToString(), entityMetadata.LogicalName, entityMetadata.SchemaName, (entityMetadata.DisplayName.UserLocalizedLabel != null ? entityMetadata.DisplayName.UserLocalizedLabel.Label : entityMetadata.SchemaName));
                entity.Fields = ParseFields(entityMetadata.Attributes);
                entity.PrimaryIdFieldName = entityMetadata.PrimaryIdAttribute;
                entity.PrimaryNameFieldName = entityMetadata.PrimaryNameAttribute;
                Field primaryField = entity.Fields.Where(t => t.Name.Equals(entity.PrimaryIdFieldName, StringComparison.InvariantCultureIgnoreCase) == true).FirstOrDefault();
                if(primaryField != null)
                {
                    primaryField.IsPrimary = true;
                }
                //entity.PrimaryFileReferenceFieldName = entityMetadata.PrimaryImageAttribute;
                entities.Add(entity);
            }

            entities = (from x in entities orderby x.Title select x).ToList();

            return entities;
        }

        private Sobiens.Connectors.Entities.FieldCollection ParseFields(AttributeMetadata[] attributes)
        {
            Sobiens.Connectors.Entities.FieldCollection fields = new FieldCollection();
            foreach (AttributeMetadata attributeMetadata in attributes)
            {
                //if (attributeMetadata.IsCustomAttribute == false)
                //    continue;

                string displayFieldName = attributeMetadata.SchemaName.ToString();
                if (attributeMetadata.DisplayName.LocalizedLabels.Count > 0)
                {
                    displayFieldName = attributeMetadata.DisplayName.LocalizedLabels[0].Label;
                }
                string internalFieldName = attributeMetadata.LogicalName.ToString();
                bool isRetrievable = attributeMetadata.IsRetrievable.HasValue?attributeMetadata.IsRetrievable.Value:true;
                if(attributeMetadata.AttributeType.ToString().ToLower() == "state")
                {
                    isRetrievable = true;
                }

                bool required = (attributeMetadata.RequiredLevel.Value == AttributeRequiredLevel.ApplicationRequired || attributeMetadata.RequiredLevel.Value == AttributeRequiredLevel.SystemRequired) ? true : false;
                int maxLength = 2500;
                FieldTypes fieldType = FieldTypes.Text;
                switch (attributeMetadata.AttributeType.ToString().ToLower())
                {
                    case "string":
                        fieldType = FieldTypes.Text;
                        isRetrievable = true;
                        break;
                    case "money":
                    case "integer":
                    case "int":
                    case "bigint":
                    case "double":
                    case "float":
                    case "decimal":
                    case "numeric":
                    case "smallint":
                    case "tinyint":
                        fieldType = FieldTypes.Number;
                        break;
                    case "boolean":
                    case "bit":
                        fieldType = FieldTypes.Boolean;
                        break;
                    case "date":
                    case "datetime":
                    case "datetime2":
                    case "smalldatetime":
                        fieldType = FieldTypes.DateTime;
                        break;
                    case "lookup":
                        fieldType = FieldTypes.Lookup;
                        break;
                    case "virtual":
                        fieldType = FieldTypes.Virtual;
                        break;
                    case "picklist":
                    case "status":
                        fieldType = FieldTypes.Choice;
                        isRetrievable = true;
                        break;
                        
                }


                Field field = new Field();
                field.Name = internalFieldName;
                field.DisplayName = displayFieldName;
                field.Required = required;
                field.MaxLength = maxLength;
                field.Type = fieldType;
                field.IsRetrievable = isRetrievable;

                if (attributeMetadata as StateAttributeMetadata != null)
                {
                    StateAttributeMetadata stateAttributeMetadata = (StateAttributeMetadata)attributeMetadata;
                    if (stateAttributeMetadata.OptionSet != null && stateAttributeMetadata.OptionSet.Options != null)
                    {
                        field.ChoiceItems = new List<ChoiceDataItem>();
                        foreach (var option in stateAttributeMetadata.OptionSet.Options)
                        {
                            field.ChoiceItems.Add(new ChoiceDataItem(option.Value.ToString(), option.Label.LocalizedLabels[0].Label));
                        }
                    }
                }
                if (attributeMetadata as PicklistAttributeMetadata != null)
                {
                    PicklistAttributeMetadata stateAttributeMetadata = (PicklistAttributeMetadata)attributeMetadata;
                    if (stateAttributeMetadata.OptionSet != null && stateAttributeMetadata.OptionSet.Options != null)
                    {
                        field.ChoiceItems = new List<ChoiceDataItem>();
                        foreach (var option in stateAttributeMetadata.OptionSet.Options)
                        {
                            field.ChoiceItems.Add(new ChoiceDataItem(option.Value.ToString(), option.Label.LocalizedLabels[0].Label));
                        }
                    }
                }

                fields.Add(field);
            }

            List<Field> _fields = (from x in fields orderby x.DisplayName select x).ToList();
            fields.Clear();
            fields.AddRange(_fields);

            return fields;
        }

        public Sobiens.Connectors.Entities.FieldCollection GetFields(ISiteSetting siteSetting, string entityName)
        {
            try
            {
                IOrganizationService organizationService = GetClientContext(siteSetting);
                RetrieveEntityRequest request = new RetrieveEntityRequest { EntityFilters = EntityFilters.Attributes, LogicalName = entityName };
                RetrieveEntityResponse response = (RetrieveEntityResponse)organizationService.Execute(request);
                return ParseFields(response.EntityMetadata.Attributes);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public List<IItem> GetAuditLogs(ISiteSetting siteSetting, string listName, string itemId)
        {
            try
            {
                List<IItem> results = new List<IItem>();
                IOrganizationService organizationService = GetClientContext(siteSetting);
                RetrieveRecordChangeHistoryRequest changeRequest = new RetrieveRecordChangeHistoryRequest();
                changeRequest.Target = new EntityReference(listName, new Guid(itemId));

                RetrieveRecordChangeHistoryResponse changeResponse =
                    (RetrieveRecordChangeHistoryResponse)organizationService.Execute(changeRequest);

                AuditDetailCollection details = changeResponse.AuditDetailCollection;

                foreach (var detail in details.AuditDetails)
                {
                    // Write out some of the change history information in the audit record. 
                    Entity record = (Entity)detail.AuditRecord;
                    CRMRecord crmRecord = new CRMRecord(siteSetting.ID);

                    /*
                    Console.WriteLine("\nAudit record created on: {0}", record.CreatedOn.Value.ToLocalTime());
                    Console.WriteLine("Entity: {0}, Action: {1}, Operation: {2}",
                        record.ObjectId.LogicalName, record.FormattedValues["action"],
                        record.FormattedValues["operation"]);
                    */
                    StringBuilder changedData = new StringBuilder();
                    // Show additional details for certain AuditDetail sub-types.
                    var detailType = detail.GetType();
                    if (detailType == typeof(AttributeAuditDetail))
                    {
                        var attributeDetail = (AttributeAuditDetail)detail;

                        // Display the old and new attribute values.
                        foreach (KeyValuePair<String, object> attribute in attributeDetail.NewValue.Attributes)
                        {
                            String oldValue = "(no value)", newValue = "(no value)";

                            //TODO Display the lookup values of those attributes that do not contain strings.
                            if (attributeDetail.OldValue.Contains(attribute.Key))
                                oldValue = GetEntityFieldValueAsString(attributeDetail.OldValue[attribute.Key]);

                            newValue = GetEntityFieldValueAsString(attributeDetail.NewValue[attribute.Key]);

                            changedData.AppendLine(string.Format("Attribute: {0}, old value: {1}, new value: {2}",
                                attribute.Key, oldValue, newValue));
                        }

                        foreach (KeyValuePair<String, object> attribute in attributeDetail.OldValue.Attributes)
                        {
                            if (!attributeDetail.NewValue.Contains(attribute.Key))
                            {
                                String newValue = "(no value)";

                                //TODO Display the lookup values of those attributes that do not contain strings.
                                String oldValue = GetEntityFieldValueAsString(attributeDetail.OldValue[attribute.Key]);

                                changedData.AppendLine(string.Format("Attribute: {0}, old value: {1}, new value: {2}",
                                    attribute.Key, oldValue, newValue));
                            }
                        }

                        crmRecord.Properties.Add("Createdon", record.FormattedValues["createdon"]);
                        crmRecord.Properties.Add("Action", record.FormattedValues["action"]);
                        crmRecord.Properties.Add("Operation", record.FormattedValues["operation"]);
                        crmRecord.Properties.Add("ChangedData", changedData.ToString());
                        results.Add(crmRecord);
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                string errorMessage = errorMessage = ex.Message;

                errorMessage += Environment.NewLine + ex.StackTrace;
                Logger.Info(errorMessage, "Service");


                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        private string GetEntityFieldValueAsString(object value)
        {
            if (value as EntityReference != null)
                return ((EntityReference)value).Name;
            if (value as OptionSetValue != null)
                return ((OptionSetValue)value).Value.ToString();

            return value.ToString();
        }

        public List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount)
        {
            try
            {
                List<IItem> results = new List<IItem>();
                IOrganizationService organizationService = GetClientContext(siteSetting);

                var query = new QueryExpression(listName);
                query.ColumnSet.AddColumns(viewFields.Select(t=>t.Name).ToArray());
                query.Criteria = GetFilterExpression(filters);
                if (queryOptions.RowLimit.HasValue == true)
                {
                    query.PageInfo.Count = queryOptions.RowLimit.Value;
                    query.PageInfo.PageNumber = 1;
                }

                //query.Criteria.Filters[0].FilterOperator = LogicalOperator.
                EntityCollection entities = organizationService.RetrieveMultiple(query);
                itemCount = entities.TotalRecordCount==-1? entities.Entities.Count:entities.TotalRecordCount;
                
                listItemCollectionPositionNext = string.Empty;

                foreach(Entity entity in entities.Entities)
                {
                    CRMRecord record = new CRMRecord(siteSetting.ID);
                    for(int i= viewFields.Count - 1; i > -1; i--)
                    {
                        if(viewFields.Where(t=>t.Name.Equals(viewFields[i].Name) == true).Count() > 1)
                        {
                            viewFields.RemoveAt(i);
                        }
                    }

                    foreach(CamlFieldRef viewField in viewFields)
                    {
                        string viewFieldName = viewField.Name;
                        if (entity.Attributes.ContainsKey(viewFieldName) == true)
                        {
                            string value = string.Empty;
                            object objValue = entity.Attributes[viewFieldName];
                            if (objValue != null)
                            {
                                if(objValue is EntityReference)
                                {
                                    EntityReference entityReference = (EntityReference)objValue;
                                    value = entityReference.Id.ToString() + ";#" + entityReference.Name;
                                }
                                else if (objValue is EntityReferenceCollection)
                                {
                                    EntityReferenceCollection entityReferences = (EntityReferenceCollection)objValue;
                                    for (int i = 0; i < entityReferences.Count; i++)
                                    {
                                        EntityReference entityReference = entityReferences[i];
                                        if (i > 0)
                                            value += ";#";
                                        value += entityReference.Id.ToString() + ";#" + entityReference.Name;
                                    }
                                }
                                else if (objValue is DateTime)
                                {
                                    value = ((DateTime)objValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
                                }
                                else if (objValue is OptionSetValue)
                                {
                                    value = ((OptionSetValue)objValue).Value.ToString() + ";#" + entity.FormattedValues[viewFieldName];
                                }
                                else
                                {
                                    value = entity.Attributes[viewFieldName].ToString();
                                }
                            }
                            record.Properties.Add(viewFieldName, value);
                        }
                    }

                    results.Add(record);
                }

                return results;
            }
            catch (Exception ex)
            {
                string errorMessage = errorMessage = ex.Message;

                errorMessage += Environment.NewLine + ex.StackTrace;
                Logger.Info(errorMessage, "Service");


                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        private static FilterExpression GetFilterExpression(CamlFilters filters)
        {
            return GetSQLFiltersString(filters);
        }

        private static ConditionExpression GetSQLFilterString(CamlFilter filter)
        {
            ConditionExpression conditionExpression = new ConditionExpression();
            string filterString = string.Empty;
            string fieldValue = "";
            conditionExpression.AttributeName = filter.FieldName;

            switch (filter.FieldType)
            {
                case FieldTypes.Boolean:
                    bool boolValue;
                    if (filter.FilterValue == "0")
                        fieldValue = "0";
                    else
                        fieldValue = "1";
                    if (Boolean.TryParse(filter.FilterValue, out boolValue) == true)
                    {
                        fieldValue = (boolValue ? "1" : "0");
                    }
                    break;
                case FieldTypes.Number:
                    decimal decimalValue;
                    if (Decimal.TryParse(filter.FilterValue, out decimalValue) == true)
                    {
                        fieldValue = decimalValue.ToString();
                    }
                    break;
                default:
                    fieldValue = filter.FilterValue;
                    break;
            }
            conditionExpression.Values.Add(fieldValue);
            switch (filter.FilterType)
            {
                case CamlFilterTypes.BeginsWith:
                    conditionExpression.Operator = ConditionOperator.BeginsWith;
                    break;
                case CamlFilterTypes.Contains:
                    conditionExpression.Operator = ConditionOperator.Contains;
                    break;
                case CamlFilterTypes.Equals:
                    conditionExpression.Operator = ConditionOperator.Equal;
                    break;
                case CamlFilterTypes.EqualsGreater:
                    conditionExpression.Operator = ConditionOperator.GreaterEqual;
                    break;
                case CamlFilterTypes.EqualsLesser:
                    conditionExpression.Operator = ConditionOperator.LessEqual;
                    break;
                case CamlFilterTypes.Greater:
                    conditionExpression.Operator = ConditionOperator.GreaterThan;
                    break;
                case CamlFilterTypes.Lesser:
                    conditionExpression.Operator = ConditionOperator.LessThan;
                    break;
                case CamlFilterTypes.NotEqual:
                    conditionExpression.Operator = ConditionOperator.NotEqual;
                    break;
                default:
                    conditionExpression.Operator = ConditionOperator.Equal;
                    break;
            }

            return conditionExpression;
        }

        private static FilterExpression GetSQLFiltersString(CamlFilters filters)
        {
            FilterExpression filterExpression = new FilterExpression();
            filterExpression.FilterOperator = filters.IsOr == true ? LogicalOperator.Or : LogicalOperator.And;

            string filtersString = string.Empty;
            foreach (CamlFilter filter in filters.Filters)
            {
                filterExpression.Conditions.Add(GetSQLFilterString(filter));
            }

            foreach (CamlFilters filters1 in filters.FilterCollections)
            {
                filterExpression.Filters.Add(GetSQLFiltersString(filters1));
            }

            return filterExpression;
        }


    }
}
