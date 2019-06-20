using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using Sobiens.Connectors.Entities.Documents;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Common
{
    public class DocumentTemplateManager
    {

        private static DocumentTemplateManager _instance = null;
        public static DocumentTemplateManager GetInstance()
        {
            if (_instance == null)
                _instance = new DocumentTemplateManager();
            return _instance;
        }

        public DocumentTemplates DocumentTemplates { get; set; }
        public DocumentTemplates AdministrativeDocumentTemplates { get; set; }

        public DocumentTemplateManager()
        {
        }

        public DocumentTemplate GetDocumentTemplateByID(Guid templateID)
        {
            DocumentTemplate documentTemplate = this.DocumentTemplates[templateID];
            if (documentTemplate == null)
            {
                documentTemplate = this.AdministrativeDocumentTemplates[templateID];
            }

            return documentTemplate;
        }

        public void UpdateTemplateImages(DocumentTemplates documentTemplates)
        {
            foreach (DocumentTemplate documentTemplate in documentTemplates)
            {
                SC_Image image = ImageManager.GetInstance().Images.GetImageByID(documentTemplate.ImageID);
                documentTemplate.ImagePath = image.ImagePath;                
            }
        }

        public void CopyTemplateDocumentsIntoTemplatesFolder(AppConfiguration configuration)
        {
            string configurationFolder = ConfigurationManager.GetInstance().GetConfigurationsFolder();
            foreach (DocumentTemplate documentTemplate in configuration.DocumentTemplates)
            {
                if (documentTemplate.TemplatePath.IndexOf(configurationFolder) == -1)
                {
                    FileInfo fileInfo = new FileInfo(documentTemplate.TemplatePath);
                    string newTemplateName = "template_" + Guid.NewGuid().ToString().Replace("-", string.Empty) + fileInfo.Extension;
                    string newTemplatePath = GetDocumentTemplatesFolder() + "\\" + newTemplateName;
                    File.Copy(documentTemplate.TemplatePath, newTemplatePath);
                    documentTemplate.TemplatePath = newTemplatePath;
                }
            }
        }

        private string GetDocumentTemplatesFolder()
        {
            string folder = ConfigurationManager.GetInstance().GetConfigurationsFolder() + "\\DocumentTemplates";
            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        public bool AddDocumentTemplate(string templatePath, ApplicationTypes applicationType, string title, string description, Guid imageID, out string errorMessage)
        {
            try
            {
                string folder = GetDocumentTemplatesFolder();
                FileInfo fileInfo = new FileInfo(templatePath);
                string newTemplateName = "template_" + Guid.NewGuid().ToString().Replace("-", string.Empty) + fileInfo.Extension;
                string newTemplatePath = folder + "\\" + newTemplateName;
                File.Copy(templatePath, newTemplatePath);
                DocumentTemplate template = new DocumentTemplate();
                template.ID = Guid.NewGuid();
                template.ApplicationType = applicationType;
                template.Title = title;
                template.Description = description;
                lock (this.DocumentTemplates)
                {
                    this.DocumentTemplates.Add(template);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

    }
}
