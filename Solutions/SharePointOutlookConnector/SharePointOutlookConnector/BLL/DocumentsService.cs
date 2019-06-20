using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Client;
using Google.GData.Documents;
using Google.GData.AccessControl;
using System.IO;
using System.Collections;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public class DocumentsService : Service
    {
        /// <summary>
        /// the documents namespace
        /// </summary>
        public const string DocumentsNamespace = "http://schemas.google.com/docs/2007";

        /// <summary>
        /// revisions prefix
        /// </summary>
        public const string Revisions = DocumentsNamespace + "/revisions";

        /// <summary>
        /// A Hashtable that expresses the allowed content types.
        /// </summary>
        public static Hashtable DocumentTypes;

        /// <summary>
        /// Static constructor used to initialize GDocumentsAllowedTypes.
        /// </summary>
        static DocumentsService()
        {
            DocumentTypes = new Hashtable();
            DocumentTypes.Add("CSV", "text/csv");
            DocumentTypes.Add("TAB", "text/tab-separated-values");
            DocumentTypes.Add("TSV", "text/tab-separated-values");
            DocumentTypes.Add("TXT", "text/plain");
            DocumentTypes.Add("HTML", "text/html");
            DocumentTypes.Add("HTM", "text/html");
            DocumentTypes.Add("DOC", "application/msword");
            DocumentTypes.Add("DOCX", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            DocumentTypes.Add("ODS", "application/x-vnd.oasis.opendocument.spreadsheet");
            DocumentTypes.Add("ODT", "application/vnd.oasis.opendocument.text");
            DocumentTypes.Add("RTF", "application/rtf");
            DocumentTypes.Add("SXW", "application/vnd.sun.xml.writer");
            DocumentTypes.Add("XLSX", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            DocumentTypes.Add("XLS", "application/vnd.ms-excel");
            DocumentTypes.Add("PPT", "application/vnd.ms-powerpoint");
            DocumentTypes.Add("PPS", "application/vnd.ms-powerpoint");
            DocumentTypes.Add("PDF", "application/pdf");
            DocumentTypes.Add("PPTX", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
        }

        /// <summary>
        ///  default constructor
        /// </summary>
        /// <param name="applicationName">the applicationname</param>
        public DocumentsService(string applicationName)
            : base(ServiceNames.Documents, applicationName)
        {
            this.NewFeed += new ServiceEventHandler(this.OnNewFeed);
        }

        /// <summary>
        /// overloaded to create typed version of Query
        /// </summary>
        /// <param name="feedQuery"></param>
        /// <returns>EventFeed</returns>
        public DocumentsFeed Query(DocumentsListQuery feedQuery)
        {
            return base.Query(feedQuery) as DocumentsFeed;
        }


        /// <summary>
        /// Simple method to upload a document, presentation, or spreadsheet
        /// based upon the file extension.
        /// </summary>
        /// <param name="fileName">The full path to the file.</param>
        /// <param name="documentName">The desired name of the document on the server.</param>
        /// <returns>A DocumentEntry describing the created document.</returns>
        public DocumentEntry UploadDocument(string fileName, string documentName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            //convert the extension to caps and strip the "." off the front
            string ext = fileInfo.Extension.ToUpper().Substring(1);

            String contentType = (String)DocumentTypes[ext];
            if (contentType == null)
            {
                throw new ArgumentException("File extension '" + ext + "' could not be matched to a contentType automatically.");
            }

            return this.UploadDocument(fileName, documentName, contentType);
        }

        /// <summary>
        /// Simple method to upload a document, presentation, or spreadsheet
        /// </summary>
        /// <param name="fileName">The full path to the file.</param>
        /// <param name="documentName">The desired name of the document on the server.</param>
        /// <param name="contentType">The mime type of the document</param>
        /// <returns>A DocumentEntry describing the created document.</returns>
        public DocumentEntry UploadDocument(string fileName, string documentName, string contentType)
        {
            return UploadFile(fileName, documentName, contentType, true);
        }

        /// <summary>
        /// Simple method to upload an arbitrary file. 
        /// </summary>
        /// <param name="fileName">The full path to the file.</param>
        /// <param name="documentName">The desired name of the file on the server.</param>
        /// <param name="contentType">The mime type of the file</param>
        /// <param name="convert">Indiates if the document should be converted to a known type on the server</param>
        /// <returns>A DocumentEntry describing the created document.</returns>
        public DocumentEntry UploadFile(string fileName, string documentName, string contentType, bool convert)
        {
            DocumentEntry entry = null;

            FileInfo fileInfo = new FileInfo(fileName);
            FileStream stream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            try
            {
                Uri postUri;

                if (!convert)
                {
                    postUri = new Uri(DocumentsListQuery.documentsBaseUri + "?convert=false");
                }
                else
                {
                    postUri = new Uri(DocumentsListQuery.documentsBaseUri);
                }


                if (documentName == null)
                {
                    documentName = fileInfo.Name;
                }

                if (contentType == null)
                {
                    throw new ArgumentException("You need to specify a content type, like text/html");
                }

                entry = this.Insert(postUri, stream, contentType, documentName) as DocumentEntry;
            }
            finally
            {
                stream.Close();
            }

            return entry;
        }


        /// <summary>
        /// by default all services now use version 1 for the protocol.
        /// this needs to be overridden by a service to specify otherwise. 
        /// Documents uses Version 3
        /// </summary>
        /// <returns></returns>
        protected override void InitVersionInformation()
        {
            this.ProtocolMajor = VersionDefaults.VersionThree;
        }

        /// <summary>
        /// overloaded to create typed version of Query
        /// </summary>
        /// <param name="feedQuery"></param>
        /// <returns>EventFeed</returns>
        public AclFeed Query(AclQuery feedQuery)
        {
            return base.Query(feedQuery) as AclFeed;
        }




        //////////////////////////////////////////////////////////////////////
        /// <summary>eventchaining. We catch this by from the base service, which 
        /// would not by default create an atomFeed</summary> 
        /// <param name="sender"> the object which send the event</param>
        /// <param name="e">FeedParserEventArguments, holds the feedentry</param> 
        /// <returns> </returns>
        //////////////////////////////////////////////////////////////////////
        protected void OnNewFeed(object sender, ServiceEventArgs e)
        {
            Tracing.TraceMsg("Created new Documents Feed");
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            if (e.Uri.AbsoluteUri.IndexOf("/acl") != -1)
            {
                e.Feed = new AclFeed(e.Uri, e.Service);
            }
            else
                e.Feed = new DocumentsFeed(e.Uri, e.Service);
        }
        /////////////////////////////////////////////////////////////////////////////
    }
}
