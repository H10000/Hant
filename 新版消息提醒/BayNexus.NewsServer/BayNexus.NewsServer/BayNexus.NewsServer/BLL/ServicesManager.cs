using BayNexus.NewsServer;
using BayNexus.NewsServer.BLL.MessageProcess;
using BayNexus.NewsServer.Helper;
using BayNexus.NewsServer.Hubs;
using BayNexus.NewsServer.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
[assembly: OwinStartup(typeof(Startup))]
namespace BayNexus.NewsServer.BLL
{

    class ServicesManager
    {
        private IHubContext HubContext { get; set; }
        private string _servicesUrl;
        public ServicesManager(string ServicesUrl)
        {
            _servicesUrl = ServicesUrl;
        }

        public void Start()
        {
            //生成代理
            WebApp.Start(_servicesUrl);

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    try
                    {
                        ArrayList userIDs = new ArrayList();
                        foreach (KeyValuePair<string, User> p in SignalRHub.Usermananger.users)
                        {
                            userIDs.Add(p.Key);
                        }
                        if (userIDs.Count > 0)
                        {
                            MessageService.MessageServiceSoapClient client = new MessageService.MessageServiceSoapClient();

                            object result = client.GetMessage(string.Join(",", (string[])userIDs.ToArray(typeof(string))), null, 1);
                            DataTable dt = ((DataSet)result).Tables[0];//new DBMessage().GetMessage((string[])userIDs.ToArray(typeof(string)));
                            if (dt.Rows.Count > 0)
                            {
                                ArrayList messageIDs = new ArrayList();
                                HubContext = (GlobalHost.ConnectionManager.GetHubContext<SignalRHub>() as IHubContext);
                                List<Message> ls = StaticMethod<Message>.FillModel(dt);
                                foreach (Message message in ls)
                                {
                                    var messageForwarder = MessageForwarderFactory.CreateMessageForwarder(message.MessageType);
                                    messageForwarder.MessageProcess(message, HubContext);
                                    messageIDs.Add(message.MessageID.ToString());
                                }
                                client.UpdateMessageStatus(2, string.Join(",", (string[])messageIDs.ToArray(typeof(string))));
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }
    }
}
