using BayNexus.NewsServer.Models;
using Microsoft.AspNet.SignalR;

namespace BayNexus.NewsServer.BLL.MessageProcess
{
   public abstract class MessageForwarder
   {
       public abstract void MessageProcess(Message message, IHubContext hubcontext);
   }
}
