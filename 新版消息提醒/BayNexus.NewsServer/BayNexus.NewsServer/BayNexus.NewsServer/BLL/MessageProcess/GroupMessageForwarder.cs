using BayNexus.NewsServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayNexus.NewsServer.BLL.MessageProcess
{
   public class GroupMessageForwarder:MessageForwarder
   {
        public override void MessageProcess(Message message, Microsoft.AspNet.SignalR.IHubContext hubcontext)
        {
            hubcontext.Clients.Group(message.ReceiveId.ToString()).receive(new
            {
                username = message.SendName,
                type =message.MessageType,
                id = message.SendId,
                receiveId = message.ReceiveId,
                content = message.MessageContent,
                mine = false,
                timestamp = DateTime.Now
            });
        }
    }
}
