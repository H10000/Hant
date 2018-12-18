using BayNexus.NewsServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using BayNexus.NewsServer.BLL.MessageProcess;
using BayNexus.NewsServer.BLL;
using System.Data;
using BayNexus.NewsServer.Helper;
using System.Collections;
using Newtonsoft.Json;

namespace BayNexus.NewsServer.Hubs
{
    [HubName("SignalRHub")]
    public class SignalRHub : Hub
    {
        private Dictionary<string, Session> SessionPool = new Dictionary<string, Session>();
        public static UserManager Usermananger = new UserManager();

        public override Task OnConnected()
        {
            var user = new User() { UserID = Context.QueryString["UserID"], UserName = System.Web.HttpUtility.UrlDecode(Context.QueryString["UserName"]), ConnectionId = Context.ConnectionId, DepartmentID = Context.QueryString["DepartmentID"], DeviceType = Context.QueryString["DeviceType"] };
            if (!Usermananger.Contains(user.UserID))
            {
                Program.from.AddItems(user);
            }
            if (user.DepartmentID != "-1")
                Groups.Add(Context.ConnectionId, user.DepartmentID.ToString());

            Usermananger.Add(user);

            return base.OnConnected();
        }
        public override Task OnReconnected()
        {
            var user = new User() { UserID = Context.QueryString["UserID"], UserName = System.Web.HttpUtility.UrlDecode(Context.QueryString["UserName"]), ConnectionId = Context.ConnectionId, DepartmentID = Context.QueryString["DepartmentID"], DeviceType = Context.QueryString["DeviceType"] };
            if (!Usermananger.Contains(user.UserID))
            {
                Program.from.AddItems(user);
            }
            if (user.DepartmentID != "-1")
                Groups.Add(Context.ConnectionId, user.DepartmentID.ToString());

            Usermananger.Add(user);
            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var user = new User() { UserID = Context.QueryString["UserID"], UserName = System.Web.HttpUtility.UrlDecode(Context.QueryString["UserName"]), ConnectionId = Context.ConnectionId, DepartmentID = Context.QueryString["DepartmentID"], DeviceType = Context.QueryString["DeviceType"] };
            Usermananger.Remove(user.UserID, user.ConnectionId);
            if (!Usermananger.Contains(user.UserID))
            {
                Program.from.RemoveItems(user);
            }
            if (user.DepartmentID != "-1")
                Groups.Remove(Context.ConnectionId, user.DepartmentID.ToString());
            return base.OnDisconnected(stopCalled);
        }

        private IHubContext HubContext { get; set; }
        [HubMethodName("ClientSendMessage")]
        public void ClientSendMessage(Message message)
        {
            //保存数据库消息
            MessageService.MessageServiceSoapClient client = new MessageService.MessageServiceSoapClient();
            client.SaveMessage(message.MessageContent, message.MessageType, message.SendId, message.SendName, message.ReceiveId, "", DateTime.Now.ToString(), "1", DateTime.Now.ToString());

        }

    }
}
