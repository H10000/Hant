using newsDataManage.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newsDataManage.BLL
{
    public class Business
    {
        DBBusiness db = new DBBusinessfactory().dbbusiness;

        #region 未执行医嘱
        public virtual void UnExtOrdersMethod()
        {
            string NurseCellCode = "";
            DataTable dt_user = null;
            DataTable dt = db.Get_unExtOrders();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (NurseCellCode != Convert.ToString(dt.Rows[i]["NurseCellCode"]))
                {
                    NurseCellCode = Convert.ToString(dt.Rows[i]["NurseCellCode"]);
                    dt_user = db.Get_UserListInfo(NurseCellCode);
                }
                for (int j = 0; j < dt_user.Rows.Count; j++)
                {
                    new Base().SaveMessageInfo(DateTime.Now.ToString(), "【未执行医嘱】" + Convert.ToString(dt.Rows[i]["BedNo"]) + "  " + Convert.ToString(dt.Rows[i]["PatientName"]) + "--" + Convert.ToString(dt.Rows[i]["OrderText"]),
                        NurseCellCode, Convert.ToString(dt_user.Rows[j]["UserID"]), "");
                }
            }
        }
        #endregion

        #region 新医嘱
        public override void newOrdersMethod()
        {
            string NurseCellCode = "";
            DataTable dt_user = null;
            DataTable dt = db.Get_newOrders();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (NurseCellCode != Convert.ToString(dt.Rows[i]["NurseCellCode"]))
                {
                    NurseCellCode = Convert.ToString(dt.Rows[i]["NurseCellCode"]);
                    dt_user = db.Get_UserListInfo(NurseCellCode);
                }
                for (int j = 0; j < dt_user.Rows.Count; j++)
                {
                    new Base().SaveMessageInfo(DateTime.Now.ToString(), "【新医嘱】" + Convert.ToString(dt.Rows[i]["BedNo"]) + "  " + Convert.ToString(dt.Rows[i]["PatientName"]) + "--" + Convert.ToString(dt.Rows[i]["OrderText"]),
                        NurseCellCode, Convert.ToString(dt_user.Rows[j]["UserID"]), "");
                }
            }
        }
        #endregion

        #region 三天未大便
        public override void unThreeStoolMethod()
        {
            string NurseCellCode = "";
            DataTable dt_user = null;
            List<Model.Patient> listPatient = db.Get_unThreeStool();
            for (int i = 0; i < listPatient.Count; i++)
            {
                if (NurseCellCode != listPatient[i].NurseCellCode)
                {
                    NurseCellCode = listPatient[i].NurseCellCode;
                    dt_user = db.Get_UserListInfo(NurseCellCode);
                }
                for (int j = 0; j < dt_user.Rows.Count; j++)
                {
                    new Base().SaveMessageInfo(DateTime.Now.ToString(), "【三天未大便】" + listPatient[i].PatientName,
                        NurseCellCode, Convert.ToString(dt_user.Rows[j]["UserID"]), "");
                }
            }
        }
        #endregion

        #region 停止医嘱
        public virtual void stopOrdersMethod()
        {
            string NurseCellCode = "";
            DataTable dt_user = null;
            DataTable dt = db.Get_stopOrders();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (NurseCellCode != Convert.ToString(dt.Rows[i]["NurseCellCode"]))
                {
                    NurseCellCode = Convert.ToString(dt.Rows[i]["NurseCellCode"]);
                    dt_user = db.Get_UserListInfo(NurseCellCode);
                }
                for (int j = 0; j < dt_user.Rows.Count; j++)
                {
                    new Base().SaveMessageInfo(DateTime.Now.ToString(), "【停止医嘱】" + Convert.ToString(dt.Rows[i]["BedNo"]) + "  " + Convert.ToString(dt.Rows[i]["PatientName"]) + "--" + Convert.ToString(dt.Rows[i]["OrderText"]),
                        NurseCellCode, Convert.ToString(dt_user.Rows[j]["UserID"]), "");
                }
            }
        }
        #endregion

        #region 检查检验报告
        public virtual void inspectionReportMethod()
        {
            string NurseCellCode = "";
            DataTable dt_user = null;
            DataTable dt = db.Get_inspectionReport();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (NurseCellCode != Convert.ToString(dt.Rows[i]["NurseCellCode"]))
                {
                    NurseCellCode = Convert.ToString(dt.Rows[i]["NurseCellCode"]);
                    dt_user = db.Get_UserListInfo(NurseCellCode);
                }
                for (int j = 0; j < dt_user.Rows.Count; j++)
                {
                    new Base().SaveMessageInfo(DateTime.Now.ToString(), "【检查检验结果】" + Convert.ToString(dt.Rows[i]["BedNo"]) + "  " + Convert.ToString(dt.Rows[i]["PatientName"]) + "--" + Convert.ToString(dt.Rows[i]["CheckName"]),
                        NurseCellCode, Convert.ToString(dt_user.Rows[j]["UserID"]), "");
                }
            }
        }
        #endregion

        #region 手术提醒
        public virtual void opetationInfoMethod()
        {
            string NurseCellCode = "";
            DataTable dt_user = null;
            DataTable dt = db.Get_opetationInfo(DateTime.Now.ToShortDateString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (NurseCellCode != Convert.ToString(dt.Rows[i]["NurseCellCode"]))
                {
                    NurseCellCode = Convert.ToString(dt.Rows[i]["NurseCellCode"]);
                    dt_user = db.Get_UserListInfo(NurseCellCode);
                }
                for (int j = 0; j < dt_user.Rows.Count; j++)
                {
                    new Base().SaveMessageInfo(DateTime.Now.ToString(), "【当日手术】" + Convert.ToString(dt.Rows[i]["BedNo"]) + "  " + Convert.ToString(dt.Rows[i]["PatientName"]) + "--" + Convert.ToString(dt.Rows[i]["OperationName"]),
                        NurseCellCode, Convert.ToString(dt_user.Rows[j]["UserID"]), "");
                }
            }
        }
        #endregion

        #region 转科提醒
        public virtual void zhuanruNewsMethod()
        {
            string NurseCellCode = "";
            DataTable dt_user = null;
            DataTable dt = db.zhuanruNews();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (NurseCellCode != Convert.ToString(dt.Rows[i]["NurseCellCode"]))
                {
                    NurseCellCode = Convert.ToString(dt.Rows[i]["NurseCellCode"]);
                    dt_user = db.Get_UserListInfo(NurseCellCode);
                }
                for (int j = 0; j < dt_user.Rows.Count; j++)
                {
                    new Base().SaveMessageInfo(DateTime.Now.ToString(), "【转科提醒】" + Convert.ToString(dt.Rows[i]["BedNo"]) + "  " + Convert.ToString(dt.Rows[i]["PatientName"]) + "--" + Convert.ToString(dt.Rows[i]["CheckName"]),
                        NurseCellCode, Convert.ToString(dt_user.Rows[j]["UserID"]), "");
                }
            }
        }
        #endregion

        #region 插入医嘱信息
        public void TongBuHisOrder()
        {
            DataTable dt1 = db.TongBuHisOrder();
            if (dt1.Rows.Count > 0)
            {
                List<string> infusionCode_list = new List<string>();
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (!infusionCode_list.Contains(Convert.ToString(dt1.Rows[i]["infusionCode"])))
                    {
                        infusionCode_list.Add(Convert.ToString(dt1.Rows[i]["infusionCode"]));
                    }
                }
                foreach (string inf in infusionCode_list)
                {
                    DataRow[] drs = dt1.Select(" infusionCode='" + inf + "'");
                    //总共要分的包数
                    int count = GetFrequencyCode(drs[0]["FREQUENCYCODE"].ToString());
                    string infusionCode = inf;
                    //总存储次数--即存几包
                    for (int c = 0; c < count; c++)
                    {
                        string newInfusionCode = "";
                        newInfusionCode = infusionCode.Replace("@", "") + (c + 1).ToString("d3");

                        //每包要存多少数据
                        List<string> test1 = new List<string>();
                        string sql = "";
                        for (int i = 0; i < drs.Length; i++)
                        {
                            if (test1.Contains(drs[i]["ORDERID"].ToString()))
                            {
                                continue;
                            }
                            test1.Add(drs[i]["ORDERID"].ToString());
                            string orderTime = drs[0]["STARTTIME"].ToString();
                            sql += "INSERT INTO Orders(PatientId,InfusionCode,EffectFlg,OrderId,PrintStatus,OrderTime ";
                            sql += ",Spec,OrderText,Dosage,Dose,method,FrequencyCode,typecode,CombNo,MedicineType,Dose_unit,patientName,OrderStartDoc,OrderStartNurse,UseTime,NUserTime)";
                            sql += string.Format("values('{0}','{1}','未执行','{2}','0','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','药品','{12}','{13}','{14}','{15}','{16}','{17}')"
                                , drs[i]["PATIENT_ID"], newInfusionCode, drs[i]["ORDERID"], orderTime, drs[i]["SPEC"], drs[i]["ORDERTEXT"]
                                , drs[i]["DOSAGE"], drs[i]["DOSE"], drs[i]["METHOD"], drs[i]["FREQUENCYCODE"]
                                , drs[i]["REPEAT_SIGNAL"], infusionCode, drs[i]["DOSE_UNIT"], drs[i]["PATIENT_NAME"]
                                , drs[i]["ORDER_START_DOC"], drs[i]["ORDER_START_NURSE"], drs[i]["UseTime1"], drs[i]["STARTTIME"]);

                        }
                        new DBHelper().MyExecuteQuery(sql);
                    }
                }
            }
        }

        public int GetFrequencyCode(string frequencyCode)
        {
            int count = 1;
            switch (frequencyCode.Trim().ToLower())
            {
                case "半小时一次":
                    count = 24 * 2;
                    break;
                case "每八小时一次":
                case "一日三次":
                    count = 24 / 8;
                    break;
                case "每2小时一次":
                    count = 24 / 2;
                    break;
                case "每天二次":
                    count = 2;//edit by lhl
                    break;
                case "每12小时一次":
                case "一日两次":
                    count = 24 / 12;
                    break;
                case "每小时一次":
                    count = 24 / 1;
                    break;
                case "每天五次":
                case "一日五次":
                    count = 5;
                    break;
                case "每4小时一次":
                case "一日六次":
                    count = 6;//edit by lhl
                    break;
                case "每天六次":
                    count = 6;
                    break;
                case "每3小时一次":
                case "一日八次":
                    count = 8;
                    break;
                case "每天三次":
                    count = 3;//edit by lhl
                    break;
                case "每8小时一次":
                    count = 3;
                    break;
                case "每天四次":
                case "一日四次":
                    count = 4;//edit by lhl
                    break;
                case "每6小时一次":
                    count = 4;
                    break;
                //剂都是中药的
                case "一日一剂一日三次":
                case "一日一次":
                    count = 1;
                    break;
                case "一日一剂早晚两次":
                    count = 1;
                    break;
                case "每48小时一次":
                case "每24小时一次":
                    count = 1;//edit by lhl
                    break;
                case "每天早晨一次":
                    count = 1;//edit by lhl
                    break;
                case "每天清晨":
                    count = 1;//edit by lhl
                    break;
                case "每周一次":
                case "持续给药":
                case "每天晚上一次":
                case "三天一次":
                case "需要时":
                case "不定时":
                case "二日一剂":
                case "三日一剂":
                case "一次性":
                case "持续性":
                case "隔日一次":
                case "必要时":
                case "每周二次":
                case "每天一剂":
                case "立即用药":
                case "每天一次":
                    count = 1;//edit by lhl
                    break;
                case "必要时用药":
                case "每周三次":
                    count = 1;
                    break;
                case "once":
                    break;
                case "bid":
                    break;
                case "q8h":
                    break;
                default:
                    count = 1;
                    break;
            }
            return count;
        }
        #endregion

        #region 未摆液
        public virtual void UnPutMethod()
        {
            string NurseCellCode = "";
            DataTable dt_user = null;
            DataTable dt = db.GetOrderInfo("0");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (NurseCellCode != Convert.ToString(dt.Rows[i]["NurseCellCode"]))
                {
                    NurseCellCode = Convert.ToString(dt.Rows[i]["NurseCellCode"]);
                    dt_user = db.Get_UserListInfo(NurseCellCode);
                }
                for (int j = 0; j < dt_user.Rows.Count; j++)
                {
                    new Base().SaveMessageInfo(DateTime.Now.ToString(), "【未摆液医嘱】" + Convert.ToString(dt.Rows[i]["BedNo"]) + "  " + Convert.ToString(dt.Rows[i]["PatientName"]) + "--" + Convert.ToString(dt.Rows[i]["OrderText"]),
                        NurseCellCode, Convert.ToString(dt_user.Rows[j]["UserID"]), "");
                }
            }
        }
        #endregion

        #region 未配液
        public virtual void UnDispensedMethod()
        {
            string NurseCellCode = "";
            DataTable dt_user = null;
            DataTable dt = db.GetOrderInfo("1");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (NurseCellCode != Convert.ToString(dt.Rows[i]["NurseCellCode"]))
                {
                    NurseCellCode = Convert.ToString(dt.Rows[i]["NurseCellCode"]);
                    dt_user = db.Get_UserListInfo(NurseCellCode);
                }
                for (int j = 0; j < dt_user.Rows.Count; j++)
                {
                    new Base().SaveMessageInfo(DateTime.Now.ToString(), "【未配液医嘱】" + Convert.ToString(dt.Rows[i]["BedNo"]) + "  " + Convert.ToString(dt.Rows[i]["PatientName"]) + "--" + Convert.ToString(dt.Rows[i]["OrderText"]),
                        NurseCellCode, Convert.ToString(dt_user.Rows[j]["UserID"]), "");
                }
            }
        }
        #endregion

        #region 未核对
        public virtual void UnCheckMethod()
        {
            string NurseCellCode = "";
            DataTable dt_user = null;
            DataTable dt = db.GetOrderInfo("2");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (NurseCellCode != Convert.ToString(dt.Rows[i]["NurseCellCode"]))
                {
                    NurseCellCode = Convert.ToString(dt.Rows[i]["NurseCellCode"]);
                    dt_user = db.Get_UserListInfo(NurseCellCode);
                }
                for (int j = 0; j < dt_user.Rows.Count; j++)
                {
                    new Base().SaveMessageInfo(DateTime.Now.ToString(), "【未核对医嘱】" + Convert.ToString(dt.Rows[i]["BedNo"]) + "  " + Convert.ToString(dt.Rows[i]["PatientName"]) + "--" + Convert.ToString(dt.Rows[i]["OrderText"]),
                        NurseCellCode, Convert.ToString(dt_user.Rows[j]["UserID"]), "");
                }
            }
        }
        #endregion
    }
}
