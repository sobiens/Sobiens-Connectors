using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using EmailUploader.BLL;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL;

namespace Sobiens.Office.SharePointOutlookConnector
{
    partial class SobiensAlertErrorForm : Form
    {
        private string methodName = "";
        private Exception exception = null;
        private string descriptionText = "Please explain how this error occured step by step.";
        public SobiensAlertErrorForm()
        {
            InitializeComponent();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string subject = "An error occured on Method:" + methodName;
            string exceptionMessage = GetExceptionMessage(methodName, exception);
            System.Diagnostics.Process.Start("IExplore", "mailto:serkant.samurkas@sobiens.com?Subject=" + subject + "&Body=" + exceptionMessage);
        }

        private string GetSystemInformation()
        {
            return "SPOC Version:" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine +
                   "Outlook version:" + SystemInformationManager.OutlookVersion;
        }
        private string GetExceptionMessage(string methodName, Exception ex)
        {
            return "Method Name:" + methodName + Environment.NewLine +
                "Exception Message:" + ex.Message + Environment.NewLine +
                "Stack Trace:" + ex.StackTrace;
        }

        public void SetErrorMessage(string _methodName, Exception _exception)
        {
            methodName = _methodName;
            exception = _exception;
            LogManager.LogException(_methodName, _exception);
            ErrorMessageTextBox.Text = GetExceptionMessage(_methodName, _exception);
            DescriptionTextBox.Text = descriptionText;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (EmailTextBox.Text == String.Empty || EmailTextBox.Text.IndexOf("@") == -1 || EmailTextBox.Text.IndexOf(".") ==-1)
            {
                MessageBox.Show("Please enter an email.");
                EmailTextBox.Focus();
                return;
            }
            if (DescriptionTextBox.Text == String.Empty || DescriptionTextBox.Text == descriptionText)
            {
                MessageBox.Show("Please enter a description.");
                DescriptionTextBox.Focus();
                return;
            }
            WebClient myClient = new WebClient();
            myClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            try
            {
                string subject = "[SharePoint Outlook Connector] An error occured on Method:" + methodName;
                string email = EmailTextBox.Text;
                string exceptionMessage = "Email:" + email + Environment.NewLine + "Description:" + DescriptionTextBox.Text + Environment.NewLine + GetSystemInformation() + Environment.NewLine + GetExceptionMessage(methodName, exception);

                NameValueCollection keyvaluepairs = new NameValueCollection();
                keyvaluepairs.Add("Subject", subject);
                keyvaluepairs.Add("Body", exceptionMessage);
                byte[] responseArray = myClient.UploadValues("http://www.sobiens.com/SendEmail.aspx", "POST", keyvaluepairs);
                string response = Encoding.ASCII.GetString(responseArray);
                if (response == "Completed")
                {
                    MessageBox.Show("Your message has been sent completely.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("An error occured while sending the message.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occcured while sending email. Error:" + ex.Message);
            }
        }
    }
}
