using newsDataManage.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newsDataManage.BLL
{
    public class Base
    {
        DBBusiness db = new DBBusinessfactory().dbbusiness;
        #region 消息提醒
        public void SaveMessageInfo(string alertTime, string MessageInfoContent, string NurseCellCode, string NurseID, string HDIPAdress)
        {
            DataTable dtu = db.getUserList(" personnel_id = '" + NurseID + "'");
            if (dtu.Rows.Count > 0)
            {
                try
                {
                    db.SaveMessageInfo(NurseID, Convert.ToString(dtu.Rows[0]["Name"]), NurseCellCode, null, MessageInfoContent, alertTime, "", "2", "shiling.mvp");
                }
                catch (Exception ex)
                {

                }
            }
        }
        #endregion

        #region 保存消息记录新
        public void SaveMessageInfo_new(string alertTime, string NurseCellCode,string patientID, string MessageInfoContent,string Type)
        {
            db.SaveMessageInfo_new(alertTime,NurseCellCode,patientID, MessageInfoContent,"2","shiling.mvp",Type);
        }
        #endregion
    }
}
