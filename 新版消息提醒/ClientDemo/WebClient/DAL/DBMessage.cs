using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebClient.Helper;

namespace WebClient.DAL
{
    public class DBMessage
    {
        DBHelper db = new DBHelper();
        public void SaveMessage(string messageContent, string messageType, string sendID, string sendName, string receiveID, string receiveName, string sendTime, string sendStatus, string createTime)
        {
            string sql = string.Format(@"insert into messageInfo (MessageContent,messageType,sendID,sendName,receiveID,receiveName,sendTime,sendStatus,createTime)
                                      values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", messageContent, messageType, sendID, sendName, receiveID, receiveName, sendTime, sendStatus, createTime);
            db.MyExecuteInt(sql);
        }

        public DataTable GetMessage(string[] ReceiveIDs = null, string MessageType = null, int SendStatus = 1)
        {
            int DelayMin = ConfigHelper.GetMessageDelayMin == null ? 30 : Convert.ToInt32(ConfigHelper.GetMessageDelayMin);
            DataTable dt = new DataTable();
            string sql = "select * from messageInfo where 1=1  ";

            if (ReceiveIDs != null)
            {
                sql += " and ((";
                foreach (string receiveID in ReceiveIDs)
                {
                    sql += "receiveID='" + receiveID + "' or ";
                }
                sql = sql.Remove(sql.Length - 3);
                sql += " ) or MessageType='all' )";
            }

            if (MessageType != null)
            {
                sql += " and MessageType='" + MessageType + "'";
            }
            else
            {
                sql += " and (sendTime>='" + DateTime.Now.AddMinutes(-DelayMin) + "' or MessageType='friend' or MessageType='group') ";
            }
            sql += " and sendStatus=" + SendStatus + "";
            sql += " order by messageID asc";
            return db.MyExecuteTable(sql);
        }

        public DataTable GetMessageCurrent(string ReceiveID, string SendID, string MessageType = null, int SendStatus = 2)
        {
            DataTable dt = new DataTable();
            string sql = "select * from messageInfo where 1=1  ";
            if (MessageType == "friend")
            {
                sql += " and ((SendId='" + SendID + "'";
                sql += " and receiveID='" + ReceiveID + "') ";
                sql += " or ";
                sql += "(SendId='" + ReceiveID + "' ";
                sql += " and receiveID='" + SendID + "'))";
            }
            else
            {
                sql += " and (SendId='" + SendID + "'";
                sql += " and (receiveID='" + ReceiveID + "' or MessageType='all')) ";
            }
            if (MessageType != null)
            {
                sql += " and MessageType='" + MessageType + "'";
            }

            sql += " and sendStatus=" + SendStatus + "";

            sql += " order by SendTime desc";
            return db.MyExecuteTable(sql);
        }
        public DataTable GetMessageHistory(string ReceiveID, string SendID, string MessageType = null, int SendStatus = 2)
        {
            DataTable dt = new DataTable();
            string sql = "select * from messageInfo_history where 1=1  ";
            if (MessageType == "friend")
            {
                sql += " and ((SendId='" + SendID + "'";
                sql += " and receiveID='" + ReceiveID + "') ";
                sql += " or ";
                sql += "(SendId='" + ReceiveID + "' ";
                sql += " and receiveID='" + SendID + "'))";
            }
            else
            {
                sql += " and (SendId='" + SendID + "'";
                sql += " and (receiveID='" + ReceiveID + "' or MessageType='all')) ";
            }
            if (MessageType != null)
            {
                sql += " and MessageType='" + MessageType + "'";
            }

            sql += " and sendStatus=" + SendStatus + "";

            sql += " order by SendTime desc";
            return db.MyExecuteTable(sql);
        }
        public void UpdateMessageStatus(int sendStatus, string[] messageIDs)
        {
            string sql = "";
            foreach (string messageID in messageIDs)
            {
                sql += "update messageInfo set sendStatus=" + sendStatus + " where messageID=" + messageID + ";";
            }
            db.MyExecuteInt(sql);
        }

        public DataTable GetUserInfo()
        {
            string sql = @"select EMP_ID as UserID,EMP_NAME as UserName,WARD_ID as OUID,WARD_NAME as OUName from COMM_EMPLOYEE
                           order by SORT,WARD_ID";
            DataTable dt = db.MyExecuteTable(sql);
            return dt;
        }
    }
}