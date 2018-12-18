using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerManageRunningService.Base
{
  public class CommonLog
    {
        public static void WriteLog(string FileName, string strLog)
        {
            try
            {
                FileName = "SL" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + FileName;

                String strFolderPath = System.Windows.Forms.Application.StartupPath + @"\log";
                if (!Directory.Exists(strFolderPath))
                    Directory.CreateDirectory(strFolderPath);

                string strPath = strFolderPath + @"\" + FileName;
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_streamWriter = new StreamWriter(fs);
                m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_streamWriter.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "--" + strLog);
                m_streamWriter.Flush();
                m_streamWriter.Close();
                fs.Close();
            }
            catch { }
        }

        public static void WriteLogError(string FileName, string strLog)
        {
            try
            {
                WriteLog(FileName, strLog);
                WriteLog("Error" + FileName, strLog);
            }
            catch { }
        }
    }
}
