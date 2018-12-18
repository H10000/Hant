using BayNexus.NewsServer.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayNexus.NewsServer.BLL.MessageProcess
{
    public class AllMessageForwarder : MessageForwarder
    {
        public override void MessageProcess(Models.Message message, Microsoft.AspNet.SignalR.IHubContext hubcontext)
        {
            hubcontext.Clients.All.receive(new
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
    }
}
