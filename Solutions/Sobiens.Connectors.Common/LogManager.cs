//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using Sobiens.Connectors.Entities;

//namespace Sobiens.Connectors.Common
//{
//    public class LogManager
//    {
//        public static void Log(string log, LogModes logMode)
//        {
//            //string logFilePath = ConfigurationManager.GetInstance().CreateALogFile();
//            //StreamWriter sw = new StreamWriter(logFilePath, true);
//            //try
//            //{
//            //    if (ConfigurationManager.GetInstance().GetLogMode() == LogModes.Detailed || logMode == LogModes.Normal)
//            //    {
//            //        sw.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + log);
//            //    }
//            //}
//            //catch (Exception)
//            //{
//            //}
//            //finally
//            //{
//            //    if (sw != null)
//            //    {
//            //        sw.Close();
//            //    }
//            //}
//        }

//        public static void LogAndShowException(string methodName, Exception ex)
//        {
//            LogException(methodName, ex, String.Empty);
//            //ErrorForm sobiensAlertErrorForm = new ErrorForm();
//            //sobiensAlertErrorForm.SetErrorMessage(methodName, ex);
//            //sobiensAlertErrorForm.ShowDialog();
//        }

//        public static void LogException(string methodName, Exception ex)
//        {
//            LogException(methodName, ex, String.Empty);
//        }

//        public static void LogException(string methodName, Exception ex, string additionalMessage)
//        {
//            //string log = "An exception occured on Method:" + methodName + Environment.NewLine + " Exception:" + ex.Message + (additionalMessage != "" ? Environment.NewLine + " Additional message:" : "");
//            //string logFilePath = ConfigurationManager.GetInstance().CreateALogFile();
//            //StreamWriter sw = new StreamWriter(logFilePath, true);
//            //try
//            //{
//            //    sw.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + log);
//            //}
//            //catch (Exception)
//            //{
//            //}
//            //finally
//            //{
//            //    if (sw != null)
//            //    {
//            //        sw.Close();
//            //    }
//            //}
//        }
//    }
//}
