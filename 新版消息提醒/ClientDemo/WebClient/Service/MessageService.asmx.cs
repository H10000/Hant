using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebClient.DAL;
using WebClient.Helper;
using WebClient.Models;

namespace WebClient.Service
{
    /// <summary>
    /// MessageService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class MessageService : System.Web.Services.WebService
    {
        DBMessage db = new DBMessage();
        [WebMethod]
        public string GetCurrentMessage(string receiveID, string sendID, string messageType, int sendStatus)
        {
            DataTable dt = new DataTable();
            dt.TableName = "TableName";
            dt = db.GetMessageCurrent(receiveID, sendID, messageType, sendStatus);
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod]
        public void SaveMessage(string messageContent, string messageType, string sendID, string sendName, string receiveID, string receiveName, string sendTime, string sendStatus, string createTime)
        {
            db.SaveMessage(messageContent, messageType, sendID, sendName, receiveID, receiveName, sendTime, sendStatus, createTime);
        }

        [WebMethod]
        public DataSet GetMessage(string ReceiveIDs, string MessageType, int SendStatus)
        {
            DataTable dt = new DataTable();
            dt.TableName = "TableName";
            dt = db.GetMessage(ReceiveIDs.Split(','), MessageType, SendStatus);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        [WebMethod]
        public void UpdateMessageStatus(int sendStatus, string messageIDs)
        {
            db.UpdateMessageStatus(sendStatus, messageIDs.Split(','));
        }
    }
}
