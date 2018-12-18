using BayNexus.NewsServer.Hubs;
using BayNexus.NewsServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayNexus.NewsServer.BLL.MessageProcess
{
    public class UserMessageForwarder : MessageForwarder
    {
        public override void MessageProcess(Message message, Microsoft.AspNet.SignalR.IHubContext hubcontext)
        {
            if (SignalRHub.Usermananger.users.ContainsKey(message.ReceiveId.ToString()))
            {
                hubcontext.Clients.User(message.ReceiveId.ToString()).receive(new
                {
                    username = message.SendName,
                    type = message.MessageType,
                    id = message.SendId,
                    receiveId = message.ReceiveId,
                    content = message.MessageContent,
                    mine = false,
                    timestamp = DateTime.Now
                });
            }
            else
            {

            }
        }
    }
}
