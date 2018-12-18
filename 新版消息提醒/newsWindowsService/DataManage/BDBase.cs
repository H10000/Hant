
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataManage
{
    public class BDBase
    {
        protected DBHelper dbHelper_System = new DBHelper();//护理文书
        protected DBHelper dbhelper_Interface = new DBHelper("MNSConnectString");//His接口
        protected DBHelper dbHelper_Frame = new DBHelper("FrameConnectString");//平台
        protected DBHelper dbHelper_HT = new DBHelper("HTConnectString");//泉州医嘱信息接口
        protected DBHelper dbHelper_CheckView = new DBHelper("CheckViewConnectString");

        protected string Order = System.Configuration.ConfigurationManager.AppSettings["ORDERS"];//医嘱接口
        protected string Patient = System.Configuration.ConfigurationManager.AppSettings["PATIENT"];//患者信息接口
        protected string CHECK_VIEW = System.Configuration.ConfigurationManager.AppSettings["CHECK_VIEW"];//检查检验结果接口
        protected string OPERATION_REC = System.Configuration.ConfigurationManager.AppSettings["OPERATION_REC"];//手术接口
        protected string TRANSFER = System.Configuration.ConfigurationManager.AppSettings["V_YDHL_TRANSFER"];//手术接口

        #region 根据id获取用户信息
        public virtual DataTable getUserList(string strwhere)
        {
            string sqlstr = "SELECT  Id ,Name ,personnel_id as \"UserId\",Sort ,Department_ID,DEPARTMENT_NAME,NURSE_CELL_CODE,NURSE_CELL_NAME,JobType,Sex,CardId,ICCard_Flg,CHARGE_CELL_CODE ";
            sqlstr += " FROM t_user";
            if (strwhere != "")
            {
                sqlstr = sqlstr + "  WHERE " + strwhere;

            }
            DataTable dt = dbHelper_Frame.MyExecuteQuery(sqlstr.ToString());
            return dt;
        }
        #endregion

        #region 获取集成平台同一个病区的护士信息
        public virtual DataTable Get_UserListInfo(string NURSE_CELL_CODE)
        {
            string sql = "";
            sql = " select NAME as 'UserName',personnel_id as 'UserID' from t_user where 1=1 ";
            sql += " and CHARINDEX('" + NURSE_CELL_CODE + "',CHARGE_CELL_CODE)>0 ";
            DataTable dt = dbHelper_Frame.MyExecuteQuery(sql);
            return dt;
        }
        #endregion

        #region 获取体温单数据
        public List<TemperatureModel> Get_TemplateData(string PatientID, string VisitID)
        {
            List<TemperatureModel> lstemp = new List<TemperatureModel>();
            XmlDocument xmlDoc = new XmlDocument();
            string sql = "";
            sql = " select data from t_temperature where PatientID='" + PatientID + "'  order by Updatetime desc";//and Visit_ID='" + VisitID + "'
            DataTable dt = dbHelper_Frame.MyExecuteQuery(sql);
            if (dt.Rows.Count > 0)
            {
                xmlDoc.LoadXml(Convert.ToString(dt.Rows[0]["data"]));
                XmlElement rootElem = xmlDoc.DocumentElement;//获取根节点 
                XmlNode MeasuresElem = rootElem.SelectSingleNode("Measures");
                XmlNodeList MeasureNodes = ((XmlElement)MeasuresElem).GetElementsByTagName("Measure");//获取Measure子节点集合 
                DateTime ValueDate = DateTime.MinValue;
                string P = "";
                string R = "";
                string T = "";
                string BP = "";
                string Stool = "";
                TemperatureModel tmp1 = new TemperatureModel()
                {
                    Temperature = " ",
                    Breath = " ",
                    Pluse = " ",
                    HeartRate = " ",
                    Blood_Pressure = " ",
                    Stool = " "
                };
                TemperatureModel tmp2 = new TemperatureModel()
                {
                    Temperature = " ",
                    Breath = " ",
                    Pluse = " ",
                    HeartRate = " ",
                    Blood_Pressure = " ",
                    Stool = " "
                }; ;
                TemperatureModel tmp3 = new TemperatureModel()
                {
                    Temperature = " ",
                    Breath = " ",
                    Pluse = " ",
                    HeartRate = " ",
                    Blood_Pressure = " ",
                    Stool = " "
                }; ;
                foreach (XmlNode node in MeasureNodes)
                {
                    if (ValueDate < new DAL.Common().StrToDateTime_N(node.SelectSingleNode("ValueDate").InnerText.Substring(0, 10) + " " + node.SelectSingleNode("ValueTime").InnerText + ":00:00"))
                    {
                        ValueDate = new DAL.Common().StrToDateTime_N(node.SelectSingleNode("ValueDate").InnerText.Substring(0, 10) + " " + node.SelectSingleNode("ValueTime").InnerText + ":00:00");
                        T = node.SelectSingleNode("Body_Temperature") == null ? " " : node.SelectSingleNode("Body_Temperature").InnerText;
                        R = node.SelectSingleNode("HeartRate") == null ? " " : node.SelectSingleNode("HeartRate").InnerText;
                        P = node.SelectSingleNode("Pluse") == null ? " " : node.SelectSingleNode("Pluse").InnerText;
                        tmp3 = tmp2;
                        tmp2 = tmp1;
                        tmp1.Temperature = T;
                        tmp1.HeartRate = R;
                        tmp1.Pluse = P;
                    }
                }
                XmlNode FooterMeasuresElem = rootElem.SelectSingleNode("FooterMeasures");
                XmlNodeList FooterMeasureNodes = ((XmlElement)FooterMeasuresElem).GetElementsByTagName("FooterMeasure");//获取Measure子节点集合 
                ValueDate = DateTime.MinValue;
                foreach (XmlNode node in FooterMeasureNodes)
                {
                    if (ValueDate < new DAL.Common().StrToDateTime_N(node.SelectSingleNode("ValueDate").InnerText.Substring(0, 10)))
                    {
                        ValueDate = new DAL.Common().StrToDateTime_N(node.SelectSingleNode("ValueDate").InnerText.Substring(0, 10));
                        BP = node.SelectSingleNode("Blood_Pressure") == null ? " " : node.SelectSingleNode("Blood_Pressure").InnerText.Replace("¤", "/");
                        Stool = node.SelectSingleNode("Stool") == null ? " " : node.SelectSingleNode("Stool").InnerText;
                        tmp1.Blood_Pressure = BP;
                        tmp1.Stool = Stool;
                    }
                }
                lstemp.Add(tmp1);
                lstemp.Add(tmp2);
                lstemp.Add(tmp3);
            }

            return lstemp;
        }
        #endregion

        #region 获取病人信息
        public DataTable dt_pt
        {
            get
            {
                string sql = "";
                sql = "SELECT PATIENT_ID as \"PatientID\",";
                sql += "VISIT_ID as \"VisitId\",";
                sql += "PATIENT_NAME as \"PatientName\",";
                sql += "SEX as \"SEX\",";
                sql += "AGE as \"Age\",";
                sql += "BED_NO as \"BedNo\",";
                sql += "IN_DATE as \"InDate\",";
                sql += "OUT_DATE as \"OutDate\",";
                sql += "PHONE as \"PHONE\",";
                sql += "CONTACT_PHONE as \"ContactPhone\",";
                sql += "NURSE_CELL_CODE as \"NurseCellCode\",";
                sql += "NURSE_CELL_NAME as \"NurseCellName\",";
                sql += "DEPARTMENT_ID as \"DepartmentId\",";
                sql += "DEPARTMENT_NAME as \"DepartmentName\"";
                sql += " FROM " + Patient + " WHERE 1=1 ";
                sql += " and STATUS<>'出院' ";
                DataTable dt_pt = dbHelper_System.MyExecuteQuery(sql);
                return dt_pt;
            }
        }
        #endregion

        #region
        public DataTable dt_res
        {
            get
            {
                DataTable dt = new DataTable("dt_res");
                dt.Columns.Add("PatientID", Type.GetType("System.String"));
                dt.Columns.Add("PatientName", Type.GetType("System.String"));
                dt.Columns.Add("NurseCellCode", Type.GetType("System.String"));
                dt.Columns.Add("BedNo", Type.GetType("System.String"));
                dt.Columns.Add("news", Type.GetType("System.String"));
                return dt;
            }
        }

        #endregion

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
    }
}
