
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newsDataManage.DAL
{
    public class DBBusiness : BDBase
    {
        #region 保存消息提醒信息
        public virtual void SaveMessageInfo(string NurseID, string NurseName, string NurseCellCode, string NurseCellName, string MessageInfoContent, string alertTime, string alertUrl, string typeID, string applycationType)
        {
            string sql = "";
            sql = " insert into tb_message (c_receive_id,c_receive_name,c_receive_dept,c_receive_zone,c_detail,t_alert_time,c_alert_url,c_sendStatus,c_del,c_type_id,applycationType)";
            sql += string.Format(" values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", NurseID, NurseName, NurseCellCode, NurseCellName, MessageInfoContent, alertTime, alertUrl, "1", "0", typeID, applycationType);
            dbHelper_Frame.MyExecute(sql);
        }
        #endregion

        #region 查询未执行医嘱
        public virtual DataTable Get_unExtOrders()
        {
            string sql = "";
            sql = " SELECT b.NURSE_CELL_CODE as NurseCellCode,b.NURSE_CELL_NAME as NurseCellName,b.PATIENT_ID as PatientID,b.PATIENT_NAME as PatientName,b.BED_NO as BedNo,a.OrderText,a.StartTime,a.UseTime";
            sql += " FROM [Orders] a,V_YDHL_PATIENT b";
            sql += " where a.PatientId=b.PATIENT_ID  and a.EffectFlg!='已执行' and  CONVERT(varchar,a.UseTime,101)=CONVERT(varchar,GetDate(),101)";//and a.VisitId=b.VISIT_ID
            sql += " order by NurseCellCode,INFUSIONCODE";
            DataTable dt = dbHelper_System.MyExecuteQuery(sql);
            return dt;
        }
        #endregion

        #region 查询三天未大便病人
        public virtual List<Model.Patient> Get_unThreeStool()
        {
            List<Model.Patient> listPatient = new List<Model.Patient>();
            string sql = "select Patient_Id as \"PatientID\",VISITID as \"VisitId\",Patient_Name as \"PatientName\",Nurse_Cell_Code as \"NurseCellCode\" from " + Patient + " where 1=1 ";
            //sql += " and  State='在科'";
            sql += " order by Nurse_Cell_Code";
            DataTable dt = dbhelper_Interface.MyExecuteQuery(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Get_isThreeDayUnStool(Convert.ToString(dt.Rows[i]["PatientID"]), Convert.ToString(dt.Rows[i]["VisitId"])))
                {
                    Model.Patient pt = new Model.Patient()
                    {
                        PatientID = Convert.ToString(dt.Rows[i]["PatientID"]),
                        PatientName = Convert.ToString(dt.Rows[i]["PatientName"]),
                        VisitID = new DAL.Common().StrToInt_N(dt.Rows[i]["VisitId"].ToString()),
                        NurseCellCode = Convert.ToString(dt.Rows[i]["NurseCellCode"])
                    };
                    listPatient.Add(pt);
                }
            }
            return listPatient;
        }
        #endregion

        #region 查询新医嘱
        public virtual DataTable Get_newOrders()
        {
            int OrderExtInterval = System.Configuration.ConfigurationManager.AppSettings["OrderExtInterval"] == null ? 0 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["OrderExtInterval"]);
            string sql = "";
            sql = " ";
            sql += " select b.PATIENT_ID as \"PatientID\",b.VISIT_ID as \"VisitId\",b.Patient_Name as \"PatientName\",b.BED_NO as \"BedNo\",b.Department_id as \"NurseCellCode\" ,START_TIME,END_TIME,ORDER_TEXT as \"ORDERTEXT\",DOSE,DOSAGE,SPEC,METHOD,INFUSION_CODE,ORDER_ID,Dose_unit,(case REPEAT_SIGNAL  when '长期医嘱' then 'CZ' when '临时医嘱' then 'LZ' else REPEAT_SIGNAL end) as \"REPEAT_SIGNAL\"";
            sql += " from " + Order + " a," + Patient + " b where a.PATIENT_ID=b.PATIENT_ID and 1=1 ";
            //sql += " and  START_TIME>=dateadd(second," + -OrderExtInterval + ",GetDate())";
            sql += " and TIMESTAMPDIFF(4,CHAR(TIMESTAMP(CURRENT TIMESTAMP)-TIMESTAMP(start_time)))<=5";
            sql += " order by Nurse_Cell_Code,INFUSION_CODE";
            DataTable dt = dbhelper_Interface.MyExecuteQuery(sql);
            return dt;
        }
        #endregion

        #region 查询停止医嘱
        public virtual DataTable Get_stopOrders()
        {
            int OrderExtInterval = System.Configuration.ConfigurationManager.AppSettings["OrderExtInterval"] == null ? 0 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["OrderExtInterval"]);
            string sql = "";
            sql = " ";
            sql += " select b.PATIENT_ID as \"PatientID\",b.VISITID as \"VisitId\",b.Patient_Name as \"PatientName\",b.BED_NO as \"BedNo\",b.Nurse_Cell_Code as \"NurseCellCode\" ,START_TIME,END_TIME,ORDER_TEXT as \"ORDERTEXT\",DOSE,DOSAGE,SPEC,METHOD,INFUSION_CODE,ORDER_ID,Dose_unit,(case REPEAT_SIGNAL  when '长期医嘱' then 'CZ' when '临时医嘱' then 'LZ' else REPEAT_SIGNAL end) as \"REPEAT_SIGNAL\"";
            sql += " from " + Order + " a," + Patient + " b where a.PATIENT_ID=b.PATIENT_ID and 1=1 ";
            sql += " and END_TIME>=dateadd(second," + -OrderExtInterval + ",GetDate())";
            sql += " order by Nurse_Cell_Code,INFUSION_CODE";
            DataTable dt = dbhelper_Interface.MyExecuteQuery(sql);
            return dt;
        }
        #endregion

        #region 检查检验报告提醒(待验证)
        public virtual DataTable Get_inspectionReport()
        {
            int CheckInterval = System.Configuration.ConfigurationManager.AppSettings["CheckInterval"] == null ? 0 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CheckInterval"]);
            string sql = "";
            sql += " select b.PATIENT_ID as \"PatientID\",b.VISITID as \"VisitId\",b.Patient_Name as \"PatientName\",b.BED_NO as \"BedNo\",b.Nurse_Cell_Code as \"NurseCellCode\",SAMPLE_GROUP_NAME as \"CheckName\",CHECK_DATE as \"CheckTime\"";
            sql += " from " + CHECK_VIEW + " a," + Patient + " b where a.PATIENT_ID=b.PATIENT_ID and 1=1 ";
            sql += " and CHECK_DATE>=dateadd(second," + -CheckInterval + ",GetDate())";
            sql += " order by NURSE_CELL_CODE,PATIENT_ID,CHECK_DATE";
            DataTable dt = dbhelper_Interface.MyExecuteQuery(sql);
            return dt;
        }
        #endregion

        #region 手术提醒(待验证)
        public virtual DataTable Get_opetationInfo(string OperationTime)
        {
            string sql = "";
            sql += " select b.PATIENT_ID as \"PatientID\",b.VISITID as \"VisitId\",b.Patient_Name as \"PatientName\",b.BED_NO as \"BedNo\",b.Nurse_Cell_Code as \"NurseCellCode\",a.OPERATION_NAME as \"OperationName\",a.OPERATION_TIME as \"OperationTime\"";
            sql += " from " + OPERATION_REC + " a," + Patient + " b where a.PATIENT_ID=b.PATIENT_ID and 1=1 ";
            sql += " and OPERATION_TIME>'" + OperationTime + " 00:00:00' and OPERATION_TIME<'" + OperationTime + " 23:59:59' ";
            DataTable dt = dbhelper_Interface.MyExecuteQuery(sql);
            return dt;
        }
        #endregion

        #region 转入提醒
        public virtual DataTable zhuanruNews()
        {
            return new DataTable();
        }
        #endregion

        #region 插入消息记录新
        public virtual void SaveMessageInfo_new(string alertTime, string nurseCellCode, string patientID, string newsDetail, string newsType, string applycationType, string Type)
        {
            string sql = "";
            sql = " insert into tb_message_new (alertTime,status,nurseCellCode,patientID,newsDetail,newsType,applycationType,type)";
            sql += string.Format(" values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                alertTime, "1", nurseCellCode, patientID, newsDetail, newsType, applycationType, Type);
            dbHelper_Frame.MyExecute(sql);
        }
        #endregion

        #region 获取医嘱信息
        public virtual DataTable TongBuHisOrder()
        {
            string sql = @"  select b.* from  (SELECT *
	           FROM [IBMDADB2].[YDHIS].[YBJK].[EXEC_ORDERS_SL2017] 
	           where usetime1>=DATEADD(hour,-1,GETDATE()) ) b
	           ,(select Patient_ID as PatientID,Status as Status FROM 
	           [IBMDADB2].[YDHIS].[YBJK].[PATIENT_SL2017]) a
	           where a.PatientID=b.Patient_ID and a.Status='在院' 
	           and b.infusioncode!=''
	           and b.infusioncode not in(select CombNo from Orders where ordertime>=DATEADD(hour,-1,GETDATE()))";
            DataTable dt = dbHelper_System.MyExecuteQuery(sql);
            return dt;
        }
        #endregion

        #region 获取当天医嘱
        public virtual DataTable GetOrderInfo(string type)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" SELECT * FROM Orders where 1=1 ");
            strb.Append(" and usetime>'" + DateTime.Now.ToShortDateString() + " 00:00:00" + "' and usetime<'" + DateTime.Now.ToShortDateString() + " 23:59:59" + "'");
            if (type == "0")
            {
                strb.Append(" and (stage=0 or stage is null)");
            }
            else if (type == "1")
            {
                strb.Append(" and stage=1");
            }
            else if (type == "2")
            {
                strb.Append(" and EffectFlg='未执行'");
            }
            strb.Append(" order by usetime,infusioncode,patientid");
            return dbHelper_System.MyExecuteQuery(strb.ToString());
        }
        #endregion
    }
}
