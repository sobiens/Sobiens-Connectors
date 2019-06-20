using Sobiens.Connectors.Entities.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Sobiens.Connectors.Common.EDMX
{
    public class EDMXManager
    {
        public static void Save(string filePath, SQLDB db)
        {
            string edmxSchemaFilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Resources\EDMXSchema.xml";

            XNamespace edmxNamespaceURI = "http://schemas.microsoft.com/ado/2009/11/edmx";
            XNamespace edmNamespaceURI = "http://schemas.microsoft.com/ado/2009/11/edm";
            XNamespace mappingcsNamespaceURI = "http://schemas.microsoft.com/ado/2009/11/mapping/cs";
            XNamespace edmssdlNamespaceURI = "http://schemas.microsoft.com/ado/2009/11/edm/ssdl";
            



            XElement rootElement = XElement.Load(edmxSchemaFilePath);
            XElement conceptualModelsSchemaElement = rootElement.Descendants(edmxNamespaceURI + "ConceptualModels").Descendants(edmNamespaceURI + "Schema").FirstOrDefault();
            XElement entityContainerMappingElement = rootElement.Descendants(edmxNamespaceURI + "Mappings").Descendants(mappingcsNamespaceURI + "EntityContainerMapping").FirstOrDefault();
            XElement storageModelsSchemaElement = rootElement.Descendants(edmxNamespaceURI + "StorageModels").Descendants(edmssdlNamespaceURI + "Schema").FirstOrDefault();
            int y = 5;
            /*
            XDocument doc = new XDocument(
                new XElement("Edmx",
                             new XAttribute("Version", "3.0"),
                             new XElement("Runtime")));
*/

            /*
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlElement edmxNode = doc.CreateElement("Edmx", edmxNamespaceURI);
            XmlAttribute versionAttribute = doc.CreateAttribute("Version");
            versionAttribute.Value = "3.0";
            edmxNode.Attributes.Append(versionAttribute);
            doc.AppendChild(edmxNode);

            XmlElement runtimeNode = doc.CreateElement("Runtime", edmxNamespaceURI);
            edmxNode.AppendChild(runtimeNode);

            XmlElement conceptualModelsNode = doc.CreateElement("ConceptualModels", edmxNamespaceURI);
            runtimeNode.AppendChild(conceptualModelsNode);

            //              = "false" xmlns: annotation = "http://schemas.microsoft.com/ado/2009/02/edm/annotation" xmlns: customannotation = "http://schemas.microsoft.com/ado/2013/11/edm/customannotation" 
            XmlElement conceptualModelsSchemaNode = doc.CreateElement("Schema", edmNamespaceURI);
            conceptualModelsNode.AppendChild(conceptualModelsSchemaNode);

            conceptualModelsSchemaNode.SetAttribute("Namespace", "SobyGrid_WebAPIExample.Models");
            conceptualModelsSchemaNode.SetAttribute("Alias", "Self");
            conceptualModelsSchemaNode.SetAttribute("annotation:UseStrongSpatialTypes", edmNamespaceURI, "false");
//            XmlAttribute schemaUseStrongSpatialTypesAttribute = doc.CreateAttribute("annotation", "UseStrongSpatialTypes");
//            XmlAttribute schemaAnnotationAttribute = doc.CreateAttribute("xmlns", "annotation", edmNamespaceURI);
            //            XmlAttribute schemaAliasAttribute = doc.CreateAttribute("Alias");
            //            XmlAttribute schemaAliasAttribute = doc.CreateAttribute("Alias");
//            schemaUseStrongSpatialTypesAttribute.Value = "false";
//            schemaAnnotationAttribute.Value = "http://schemas.microsoft.com/ado/2009/02/edm/annotation";
//            conceptualModelsSchemaNode.Attributes.Append(schemaUseStrongSpatialTypesAttribute);
//            conceptualModelsSchemaNode.Attributes.Append(schemaAnnotationAttribute);


            foreach (SQLFolder folder in db.Folders)
            {

            }


            XmlNode mappingsNode = doc.CreateElement("Mappings");
            runtimeNode.AppendChild(mappingsNode);

            XmlNode storageModelsNode = doc.CreateElement("StorageModels");
            runtimeNode.AppendChild(storageModelsNode);


            XmlNode designerNode = doc.CreateElement("Designer");
            edmxNode.AppendChild(designerNode);
            designerNode.InnerXml = "<Connection>" +
                                    "<DesignerInfoPropertySet><DesignerProperty Name = \"MetadataArtifactProcessing\" Value =\"EmbedInOutputAssembly\" /></DesignerInfoPropertySet >" +
                                    "</Connection>" +
                                    "<Options>" +
                                        "<DesignerInfoPropertySet>" +
                                            "<DesignerProperty Name = \"ValidateOnBuild\" Value = \"False\" />" +
                                            "<DesignerProperty Name = \"CodeGenerationStrategy\" Value = \"None\" />" +
                                            "<DesignerProperty Name = \"ProcessDependentTemplatesOnSave\" Value = \"False\" />" +
                                            "<DesignerProperty Name = \"UseLegacyProvider\" Value = \"False\" />" +
                                        "</DesignerInfoPropertySet>" +
                                    "</Options>" +
                                    "<Diagrams />";
                                    */

            /*
            XmlNode productNode = doc.CreateElement("product");
            XmlAttribute productAttribute = doc.CreateAttribute("id");
            productAttribute.Value = "01";
            productNode.Attributes.Append(productAttribute);
            productsNode.AppendChild(productNode);

            XmlNode nameNode = doc.CreateElement("Name");
            nameNode.AppendChild(doc.CreateTextNode("Java"));
            productNode.AppendChild(nameNode);
            XmlNode priceNode = doc.CreateElement("Price");
            priceNode.AppendChild(doc.CreateTextNode("Free"));
            productNode.AppendChild(priceNode);

            // Create and add another product node.
            productNode = doc.CreateElement("product");
            productAttribute = doc.CreateAttribute("id");
            productAttribute.Value = "02";
            productNode.Attributes.Append(productAttribute);
            productsNode.AppendChild(productNode);
            nameNode = doc.CreateElement("Name");
            nameNode.AppendChild(doc.CreateTextNode("C#"));
            productNode.AppendChild(nameNode);
            priceNode = doc.CreateElement("Price");
            priceNode.AppendChild(doc.CreateTextNode("Free"));
            productNode.AppendChild(priceNode);
            */
            rootElement.Save(filePath);
        }
    }
}
