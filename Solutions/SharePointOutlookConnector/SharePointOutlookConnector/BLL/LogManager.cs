using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sobiens.Office.SharePointOutlookConnector.BLL;
using Sobiens.Office.SharePointOutlookConnector;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;

namespace EmailUploader.BLL
{
    public class LogManager
    {
        public static void Log(string log, EULogModes logMode)
        {
            string logFilePath = EUSettingsManager.GetInstance().CreateALogFile();
            StreamWriter sw = new StreamWriter(logFilePath, true);
            try
            {
                if (EUSettingsManager.GetInstance().Settings.DetailedLogMode == true || logMode == EULogModes.Normal)
                {
                    sw.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + log);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }
        public static void LogAndShowException(string methodName, Exception ex)
        {
            LogException(methodName, ex, String.Empty);
            SobiensAlertErrorForm sobiensAlertErrorForm = new SobiensAlertErrorForm();
            sobiensAlertErrorForm.SetErrorMessage(methodName, ex);
            sobiensAlertErrorForm.ShowDialog();
        }

        public static void LogException(string methodName, Exception ex)
        {
            LogException(methodName, ex, String.Empty);
        }
        public static void LogException(string methodName, Exception ex, string additionalMessage)
        {
//            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string log = "An exception occured on Method:" + methodName + Environment.NewLine + " Exception:" + ex.Message + (additionalMessage!=""?Environment.NewLine + " Additional message:":"");
            string logFilePath = EUSettingsManager.GetInstance().CreateALogFile();
            StreamWriter sw = new StreamWriter(logFilePath, true);
            try
            {
                sw.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + log);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }
    }
}
